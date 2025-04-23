using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.DTOs
{
    public class DoctorRegisterDto
    {
        public string Name { get; set; }
        public string DoctorSpecialization { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalSyndicateCardNumber { get; set; }
        public string Password { get; set; }
    }
}
