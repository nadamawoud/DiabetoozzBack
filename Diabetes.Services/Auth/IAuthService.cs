using Diabetes.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diabetes.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResponse> RegisterCasualUser(CasualUserRegisterDto dto);
        Task<ServiceResponse> VerifyCasualUserEmail(VerifyEmailDto dto);
        Task<ServiceResponse> RegisterDoctor(DoctorRegisterDto dto);
        Task<ServiceResponse> RegisterClerk(ClerkRegisterDto dto);
        Task<ServiceResponse> VerifyClerkEmail(VerifyEmailDto dto);
    }
}
