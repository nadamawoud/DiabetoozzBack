namespace Diabetes.Services.Login
{
    public interface IJwtTokenService
    {
        string CreateToken(string email, string role, int userId);
    }
}