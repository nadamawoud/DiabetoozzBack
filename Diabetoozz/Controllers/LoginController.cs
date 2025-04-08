using Diabetes.Repository.Data;
using Diabetes.Services.Login;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Diabetoozz.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginController(StoreContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestInline request)
        {
            object user = null;
            string role = null;
            int userId = 0;
            string passwordHash = null;

            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == request.Email);
            if (doctor != null)
            {
                user = doctor;
                role = "Doctor";
                userId = doctor.ID;
                passwordHash = doctor.PasswordHash;
            }
            else
            {
                var clerk = _context.Clerks.FirstOrDefault(c => c.Email == request.Email);
                if (clerk != null)
                {
                    user = clerk;
                    role = "Clerk";
                    userId = clerk.ID;
                    passwordHash = clerk.PasswordHash;
                }
                else
                {
                    var casualUser = _context.CasualUsers.FirstOrDefault(c => c.Email == request.Email);
                    if (casualUser != null)
                    {
                        user = casualUser;
                        role = "CasualUser";
                        userId = casualUser.ID;
                        passwordHash = casualUser.PasswordHash;
                    }
                }
            }

            // هنا تم إزالة استخدام BCrypt والتحقق عن طريق المقارنة المباشرة
            if (user == null || passwordHash != request.Password)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtTokenService.CreateToken(request.Email, role, userId);
            return Ok(new { Token = token, Role = role });
        }

        // كلاس داخلي لطلب الدخول
        public class LoginRequestInline
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}