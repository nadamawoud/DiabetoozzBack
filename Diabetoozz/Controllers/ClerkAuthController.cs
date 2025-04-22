using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Diabetes.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Diabetes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Clerk> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Clerk> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // التحقق من وجود البريد الإلكتروني وكلمة المرور
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "البريد الإلكتروني وكلمة المرور مطلوبان" });
            }

            // البحث عن المستخدم
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "بيانات الدخول غير صحيحة" });
            }

            // التحقق من تأكيد البريد الإلكتروني
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized(new { Message = "يجب تأكيد البريد الإلكتروني أولاً" });
            }

            // التحقق من كلمة المرور
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { Message = "بيانات الدخول غير صحيحة" });
            }

            // إنشاء التوكن
            var token = GenerateJwtToken(user);

            // الحصول على دور المستخدم
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Clerk";

            return Ok(new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = role,
                
                Email = user.Email,
                Name = user.Name
            });
        }

        private JwtSecurityToken GenerateJwtToken(Clerk user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("name", user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            return new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}