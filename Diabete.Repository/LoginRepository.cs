using Diabetes.Core.Entities;
using Diabetes.Core.Interfaces;
using Diabetes.Core.Utilities;
using Diabetes.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Diabetes.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly StoreContext _context;
        private readonly ILogger<LoginRepository> _logger;

        public LoginRepository(StoreContext context, ILogger<LoginRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Clerk> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Clerks
                    .AsNoTracking()
                    .Include(c => c.Admin) // تضمين بيانات المدير إذا لزم الأمر
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    _logger.LogWarning($"User with email {email} not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email");
                throw new Exception("حدث خطأ أثناء استعادة بيانات المستخدم", ex);
            }
        }

        public async Task<bool> VerifyPasswordAsync(Clerk user, string password)
        {
            try
            {
                if (user == null)
                {
                    _logger.LogWarning("Null user provided for password verification");
                    return false;
                }

                if (string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Empty password provided for verification");
                    return false;
                }

                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    _logger.LogError($"User {user.Email} has no password hash");
                    return false;
                }

                var isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);

                if (!isValid)
                {
                    _logger.LogWarning($"Failed login attempt for user: {user.Email}");
                }
                else
                {
                    _logger.LogInformation($"Successful password verification for user: {user.Email}");
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying password for user {user?.Email}");
                throw new Exception("حدث خطأ أثناء التحقق من كلمة المرور", ex);
            }
        }
    }
}