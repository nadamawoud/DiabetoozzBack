using Diabetes.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Diabetes.Services.Login;

namespace Diabetoozz.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Clerk")]
    public class GetPatientsController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly ILogger<GetPatientsController> _logger;

        public GetPatientsController(StoreContext context, ILogger<GetPatientsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetPatients([FromQuery] string? search)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            _logger.LogInformation($"Authorization Header: {authHeader}");

            // استخراج الـ ClerkID من التوكن
            var clerkId = JwtHelper.GetUserIdFromToken(User);
            _logger.LogInformation($"Clerk ID from token: {clerkId}");

            // بناء الاستعلام لتصفية المرضى بناءً على الـ ClerkID
            var query = _context.Patients.Where(p => p.ClerkID == clerkId);

            // إضافة التصفية في حال وجود قيمة بحث (search)
            if (!string.IsNullOrWhiteSpace(search))
            {
                _logger.LogInformation($"Searching for: {search}");

                if (int.TryParse(search, out int parsedId))
                {
                    // لو البحث رقم، اعمل مقارنة دقيقة مع ID + باقي الشروط
                    query = query.Where(p =>
                        p.ID == parsedId ||
                        p.Name.Contains(search) ||
                        p.Gender.Contains(search) ||
                        p.PhoneNumber.Contains(search)
                    );
                }
                else
                {
                    // غير كده، استخدم contains مع باقي الحقول
                    query = query.Where(p =>
                        p.Name.Contains(search) ||
                        p.Gender.Contains(search) ||
                        p.PhoneNumber.Contains(search) ||
                        p.ID.ToString().Contains(search)  // لو فيه جزء من الرقم في ID
                    );
                }
            }

            // تنفيذ الاستعلام وترتيب النتائج
            var result = query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.Gender,
                    p.Age,
                    p.PhoneNumber,
                    p.CreatedAt
                })
                .ToList();

            _logger.LogInformation($"Total patients returned: {result.Count}");

            if (!result.Any())
            {
                return NotFound("No patients found for this clerk.");
            }

            return Ok(result);
        }
    }
}
        