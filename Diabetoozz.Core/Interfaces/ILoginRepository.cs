using Diabetes.Core.DTOs;
using Diabetes.Core.Entities;
using System.Threading.Tasks;

namespace Diabetes.Core.Interfaces
{
    public interface ILoginRepository
    {
        Task<Clerk> GetUserByEmailAsync(string email);
        Task<bool> VerifyPasswordAsync(Clerk user, string password);
    }
}

