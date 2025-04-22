using System.ComponentModel.DataAnnotations;

namespace DiabetesApp.Core.DTOs
{
    public class ClerkRegisterDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required, MaxLength(10)]
        public string Gender { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(20)]
        public string LicenseCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}