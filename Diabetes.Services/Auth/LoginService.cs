using Diabetes.Core.DTOs;
using Diabetes.Core.Entities;
using Diabetes.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Services.Auth
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;

        public LoginService(ILoginRepository loginRepository, IConfiguration configuration)
        {
            _loginRepository = loginRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            var response = new LoginResponseDto();

            var user = await _loginRepository.GetUserByEmailAsync(loginRequest.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid email or password";
                return response;
            }

            if (!user.IsEmailVerified)
            {
                response.Success = false;
                response.Message = "Email not verified";
                return response;
            }

            var isPasswordValid = await _loginRepository.VerifyPasswordAsync(user, loginRequest.Password);

            if (!isPasswordValid)
            {
                response.Success = false;
                response.Message = "Invalid email or password";
                return response;
            }

            response.Token = GenerateJwtToken(user);
            response.Role = user.AdminID.HasValue ? "Admin" : "Clerk";
            response.Success = true;
            response.Message = "Login successful";

            return response;
        }

        private string GenerateJwtToken(Clerk user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.AdminID.HasValue ? "Admin" : "Clerk")
                }),
                Expires = DateTime.UtcNow.AddMinutes(
                    _configuration.GetValue<double>("JwtSettings:AccessTokenDurationInMinutes")),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}