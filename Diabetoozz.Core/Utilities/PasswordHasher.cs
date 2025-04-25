using BCrypt.Net;

namespace Diabetes.Core.Utilities
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // تأكد من استخدام Salt تلقائي
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA256);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA256);
        }
    }
}

