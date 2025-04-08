using Diabetes.Core.Entities;
using Diabetes.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // التأكد من أن المستخدم مسجل الدخول
    public class PostPublishedController : ControllerBase
    {
        private readonly StoreContext _context;

        public PostPublishedController(StoreContext context)
        {
            _context = context;
        }

        [HttpPost("publish")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PublishPost([FromForm] PostRequestDto request)
        {
            try
            {
                // مسار حفظ الملفات
                string imagePath = null;
                string videoPath = null;
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // التعامل مع الصورة إذا كانت موجودة
                if (request.Image != null)
                {
                    var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName); // إنشاء اسم فريد للصورة
                    imagePath = Path.Combine("/uploads", imageFileName); // حفظ الرابط فقط
                    using (var stream = new FileStream(Path.Combine(uploadsFolder, imageFileName), FileMode.Create))
                    {
                        await request.Image.CopyToAsync(stream);
                    }
                }

                // التعامل مع الفيديو إذا كان موجود
                if (request.Video != null)
                {
                    var videoFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Video.FileName); // إنشاء اسم فريد للفيديو
                    videoPath = Path.Combine("/uploads", videoFileName); // حفظ الرابط فقط
                    using (var stream = new FileStream(Path.Combine(uploadsFolder, videoFileName), FileMode.Create))
                    {
                        await request.Video.CopyToAsync(stream);
                    }
                }

                // استخراج الـ UserId و Role من التوكن
                var userId = int.Parse(User.FindFirst("UserId")?.Value);  // استخراج الـ UserId من التوكن
                var role = User.FindFirst("Role")?.Value;  // استخراج الـ Role من التوكن

                // التحقق من الدور وتحديد الـ ID الصحيح (MedicalSyndicateID أو OrganizationID)
                if (role == "MedicalSyndicate" || role == "Organization")
                {
                    var newPost = new NewsFeedPost
                    {
                        Title = request.Title,
                        Content = request.Content,
                        ImageURL = imagePath,
                        VideoURL = videoPath,
                        CreatedAt = DateTime.UtcNow,
                        // حفظ الـ UserId بناءً على الدور في العمود الصحيح:
                        MedicalSyndicateID = role == "MedicalSyndicate" ? userId : (int?)null,
                        OrganizationID = role == "Organization" ? userId : (int?)null
                    };

                    // إضافة المنشور في قاعدة البيانات
                    _context.NewsFeedPosts.Add(newPost);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "تم نشر المنشور بنجاح", newPost });
                }

                return BadRequest("الدور غير مدعوم.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "حدث خطأ أثناء حفظ البيانات",
                    InnerError = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }

    // DTO التي تستقبل البيانات
    public class PostRequestDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }  // صورة المنشور
        public IFormFile Video { get; set; }  // فيديو المنشور
    }
}