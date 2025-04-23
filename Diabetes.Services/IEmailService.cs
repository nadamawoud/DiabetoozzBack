using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string verificationCode);
    }
}
