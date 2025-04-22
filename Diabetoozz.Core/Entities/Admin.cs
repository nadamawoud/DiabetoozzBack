using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class Admin : BaseEntity
    {        
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation properties
        public int? ManagerID { get; set; }
        public Manager Manager { get; set; }
        public ICollection<Clerk> Clerks { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Organization> Organizations { get; set; }
        public ICollection<CasualUser> CasualUsers { get; set; }
        public ICollection<ChatbotQuestionDoctor> ChatbotQuestionDoctors { get; set; }
        public ICollection<ChatbotQuestionCasualUser> ChatbotQuestionCasualUsers { get; set; }
    }
}
