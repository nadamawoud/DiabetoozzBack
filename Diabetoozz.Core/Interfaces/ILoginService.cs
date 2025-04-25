using Diabetes.Core.DTOs;
using System.Threading.Tasks;

namespace Diabetes.Core.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
    }
}