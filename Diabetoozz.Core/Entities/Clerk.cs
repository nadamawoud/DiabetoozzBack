using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class Clerk : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseCode { get; set; }
        public string PasswordHash { get; set; }
        public string? VerificationCode { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } 

        // Foreign Key

        public int AdminID { get; set; }

        // Navigation Property
        public Admin Admin { get; set; } // العلاقة مع Admin
        
        //Relation many
        public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
