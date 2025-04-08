using Diabetes.Core.Entities;
using Diabetes.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Diabetes.Services.Login;
using Diabetes.Services; // لإضافة IJsonSerializerService

namespace Diabetoozz.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Clerk")]
    public class AddPatientController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly IJsonSerializerService _jsonSerializerService; // حقن IJsonSerializerService

        // تعديل الكونستركتور ليشمل IJsonSerializerService
        public AddPatientController(StoreContext context, IJsonSerializerService jsonSerializerService)
        {
            _context = context;
            _jsonSerializerService = jsonSerializerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] AddPatientDto patientDto)
        {
            try
            {
                var clerkId = JwtHelper.GetUserIdFromToken(User);

                // تحقق من وجود ClerkID في جدول Clerks
                var clerk = await _context.Clerks.FindAsync(clerkId);
                if (clerk == null)
                {
                    return BadRequest("Invalid Clerk ID.");
                }

                // تحقق من وجود DoctorID في جدول Doctors
                var doctor = await _context.Doctors.FindAsync(patientDto.DoctorID);
                if (doctor == null)
                {
                    return BadRequest("Invalid Doctor ID.");
                }

                // إنشاء مريض جديد
                var patient = new Patient
                {
                    Name = patientDto.Name,
                    Gender = patientDto.Gender,
                    Age = patientDto.Age,
                    PhoneNumber = patientDto.PhoneNumber,
                    DoctorID = patientDto.DoctorID, // تعيين DoctorID المدخل
                    ClerkID = clerkId,
                    CreatedAt = DateTime.UtcNow
                };

                // إضافة المريض إلى قاعدة البيانات
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                // استخدام IJsonSerializerService لتحويل الكائن إلى JSON
                var jsonResponse = _jsonSerializerService.SerializeObject(new { message = "Patient added successfully", patient });

                // إرجاع الاستجابة بعد تحويل الكائن إلى JSON
                return Ok(new { JsonResponse = jsonResponse });
            }
            catch (Exception ex)
            {
                // في حالة حدوث خطأ، يمكن إضافة Logging هنا
                return StatusCode(500, new { message = "An error occurred", details = ex.Message });
            }
        }

        // DTO للتعامل مع بيانات المريض
        public class AddPatientDto
        {
            public string Name { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
            public string PhoneNumber { get; set; }
            public int DoctorID { get; set; }
        }
    }
}