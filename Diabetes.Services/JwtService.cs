using Diabetes.Core.Entities;
using DiabetesApp.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Diabetes.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Clerk> _userManager;

        public JwtService(IConfiguration configuration, UserManager<Clerk> userManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<string> GenerateToken(Clerk user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // التحقق من إعدادات JWT
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in configuration");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing in configuration");
            var jwtAudience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing in configuration");
            var jwtExpiry = _configuration["Jwt:ExpiryInMinutes"] ?? throw new ArgumentNullException("Jwt:ExpiryInMinutes is missing in configuration");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), // استخدام Id بدلاً من ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id),
                new Claim("name", user.Name ?? string.Empty) // تجنب null
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));

            var allClaims = claims.Concat(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (!double.TryParse(jwtExpiry, out var expiryMinutes))
            {
                expiryMinutes = 60; // قيمة افتراضية إذا كانت القيمة غير صالحة
            }
            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: allClaims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}