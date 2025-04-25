using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Core.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
