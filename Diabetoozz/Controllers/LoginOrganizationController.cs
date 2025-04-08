using Diabetes.Repository.Data;
using Diabetes.Services.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Diabetoozz.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginOrganizationController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginOrganizationController(StoreContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // أولًا نحاول نسجل كـ Medical Syndicate
            var syndicate = await _context.MedicalSyndicates
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (syndicate != null)
            {
                // إنشاء الـ Token للمستخدم
                var token = _jwtTokenService.CreateToken(syndicate.Email, "MedicalSyndicate", syndicate.ID);
                return Ok(new { Token = token });
            }

            // إذا لم يكن المستخدم Medical Syndicate، نحاول Organization
            var organization = await _context.Organizations
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (organization != null)
            {
                // إنشاء الـ Token للمستخدم
                var token = _jwtTokenService.CreateToken(organization.Email, "Organization", organization.ID);
                return Ok(new { Token = token });
            }

            // إذا لم تتطابق البيانات مع أي من الـ MedicalSyndicate أو Organization
            return Unauthorized("البريد الإلكتروني أو كلمة المرور غير صحيحة");
        }

        // DTO داخلي
        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}