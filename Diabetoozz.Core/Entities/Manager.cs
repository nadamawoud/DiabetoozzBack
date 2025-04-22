using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.Entities
{
    public class Manager : BaseEntity
    {       
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation Property for Admin
        public int? AdminID { get; set; }
        public Admin Admin { get; set; }
    }
}
