using global::Diabetes.Core.DTOs;
using global::Diabetes.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Diabetoozz.APIs.Controllers
{      
        [Route("api/auth")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpPost("casual/register")]
            public async Task<IActionResult> RegisterCasualUser([FromBody] CasualUserRegisterDto dto)
            {
                var result = await _authService.RegisterCasualUser(dto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }

            [HttpPost("casual/verify")]
            public async Task<IActionResult> VerifyCasualUserEmail([FromBody] VerifyEmailDto dto)
            {
                var result = await _authService.VerifyCasualUserEmail(dto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }

            [HttpPost("doctor/register")]
            public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDto dto)
            {
                var result = await _authService.RegisterDoctor(dto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }

            [HttpPost("clerk/register")]
            public async Task<IActionResult> RegisterClerk([FromBody] ClerkRegisterDto dto)
            {
                var result = await _authService.RegisterClerk(dto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }

            [HttpPost("clerk/verify")]
            public async Task<IActionResult> VerifyClerkEmail([FromBody] VerifyEmailDto dto)
            {
                var result = await _authService.VerifyClerkEmail(dto);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
        }   
}
