using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsMedicalSyndicate { get; set; }

        // Navigation properties
        public int AdminID { get; set; }
        public Admin Admin { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<DoctorApproval> DoctorApprovals { get; set; }
    }
    
}
