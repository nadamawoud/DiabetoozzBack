using Microsoft.AspNetCore.Identity;

namespace Diabetes.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // يمكنك إضافة خصائص مخصصة هنا
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
