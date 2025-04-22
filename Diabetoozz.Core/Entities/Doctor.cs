using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class Doctor : BaseEntity
    {
       
        
        public string DoctorSpecialization { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalSyndicateCardNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation properties
        public int AdminID { get; set; }
        public Admin Admin { get; set; }
        
        public DoctorApproval DoctorApproval { get; set; }

        //many to many relation
        public ICollection<MedicalHistory> MedicalHistories { get; set; }
        public ICollection<SuggestedFood> SuggestedFoods { get; set; }
        public ICollection<ChatbotAnswerDoctor> ChatbotAnswerDoctors { get; set; }
        public ICollection<Patient> Patients { get; set; }
        
    }
}



