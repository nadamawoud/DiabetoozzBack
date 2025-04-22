using DiabetesApp.Core.DTOs;
using System.Threading.Tasks;
using Diabetes.Core.Entities;

namespace DiabetesApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterClerkAsync(ClerkRegisterDto clerkDto);
        Task<AuthResult> LoginAsync(LoginDto loginDto);
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
        public Clerk Clerk { get; set; }
    }
}