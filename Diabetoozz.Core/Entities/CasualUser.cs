using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class CasualUser : BaseEntity
    {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Gender { get; set; }
            public DateTime BirthDate { get; set; }
            public string PasswordHash { get; set; }
            public string PhoneNumber { get; set; }
            public DateTime? CreatedAt { get; set; }




        // Navigation properties
        public int AdminID { get; set; }
        public Admin Admin { get; set; }
            public ICollection<BloodSugar> BloodSugars { get; set; }
            public ICollection<Alarm> Alarms { get; set; }
            public ICollection<ChatbotResultCasualUser> ChatbotResultCasualUsers { get; set; }
            public ICollection<ChatbotAnswerCasualUser> ChatbotAnswerCasualUsers { get; set; }
        
    }
}
