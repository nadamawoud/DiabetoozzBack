using Diabetes.Core.Entities;
using System.Threading.Tasks;

namespace DiabetesApp.Core.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(Clerk user);
    }
}
