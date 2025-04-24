using Diabetes.Core.DTOs;
using Diabetes.Core.Entities;
using Diabetes.Repository.Data;
using Diabetes.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Diabetes.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly StoreContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            StoreContext context,
            IEmailService emailService,
            ILogger<AuthService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ServiceResponse> RegisterCasualUser(CasualUserRegisterDto dto)
        {
            try
            {
                var userExists = await _context.CasualUsers.AnyAsync(x => x.Email == dto.Email);
                if (userExists)
                {
                    _logger.LogWarning($"Registration attempt with existing email: {dto.Email}");
                    return new ServiceResponse(false, "هذا البريد مستخدم من قبل.");
                }

                var verificationCode = GenerateVerificationCode();
                var casualUser = new CasualUser
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    BirthDate = dto.BirthDate,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    CreatedAt = DateTime.UtcNow,
                    VerificationCode = verificationCode,
                    EmailVerified = false
                };

                _context.CasualUsers.Add(casualUser);
                await _context.SaveChangesAsync();

                try
                {
                    await _emailService.SendVerificationEmail(dto.Email, verificationCode);
                    _logger.LogInformation($"User registered successfully: {dto.Email}");
                    return new ServiceResponse(true, "تم التسجيل بنجاح، تحقق من بريدك الإلكتروني.");
                }
                catch (EmailSendException ex)
                {
                    _logger.LogError(ex, $"Failed to send verification email to {dto.Email}");
                    casualUser.EmailVerified = false;
                    await _context.SaveChangesAsync();
                    return new ServiceResponse(false, "تم التسجيل ولكن فشل إرسال بريد التحقق. يرجى المحاولة لاحقاً.");
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error during registration");
                return new ServiceResponse(false, "حدث خطأ في قاعدة البيانات أثناء التسجيل.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration");
                return new ServiceResponse(false, "حدث خطأ غير متوقع أثناء التسجيل.");
            }
        }

        public async Task<ServiceResponse> RegisterClerk(ClerkRegisterDto dto)
        {
            try
            {
                var clerkExists = await _context.Clerks.AnyAsync(x => x.Email == dto.Email);
                if (clerkExists)
                {
                    _logger.LogWarning($"Registration attempt with existing clerk email: {dto.Email}");
                    return new ServiceResponse(false, "هذا البريد مستخدم من قبل.");
                }

                var verificationCode = GenerateVerificationCode();
                var clerk = new Clerk
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    BirthDate = dto.BirthDate,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    LicenseCode = dto.LicenseCode,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    VerificationCode = verificationCode,
                    CreatedAt = DateTime.UtcNow,
                    IsEmailVerified = false
                };

                _context.Clerks.Add(clerk);
                await _context.SaveChangesAsync();

                try
                {
                    await _emailService.SendVerificationEmail(dto.Email, verificationCode);
                    _logger.LogInformation($"Clerk registered successfully: {dto.Email}");
                    return new ServiceResponse(true, "تم تسجيل الموظف بنجاح، تحقق من بريدك الإلكتروني.");
                }
                catch (EmailSendException ex)
                {
                    _logger.LogError(ex, $"Failed to send verification email to {dto.Email}");
                    clerk.IsEmailVerified = false;
                    await _context.SaveChangesAsync();
                    return new ServiceResponse(false, "تم التسجيل ولكن فشل إرسال بريد التحقق. يرجى المحاولة لاحقاً.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during clerk registration");
                return new ServiceResponse(false, "حدث خطأ أثناء تسجيل الموظف.");
            }
        }

        public async Task<ServiceResponse> RegisterDoctor(DoctorRegisterDto dto)
        {
            try
            {
                var doctorExists = await _context.Doctors.AnyAsync(x => x.Email == dto.Email);
                if (doctorExists)
                {
                    _logger.LogWarning($"Registration attempt with existing doctor email: {dto.Email}");
                    return new ServiceResponse(false, "هذا البريد مستخدم من قبل.");
                }

                var doctor = new Doctor
                {
                    Name = dto.Name,
                    DoctorSpecialization = dto.DoctorSpecialization,
                    Email = dto.Email,
                    BirthDate = dto.BirthDate,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    MedicalSyndicateCardNumber = dto.MedicalSyndicateCardNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    CreatedAt = DateTime.UtcNow,
                    IsApproved = false
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Doctor registered successfully: {dto.Email}");
                return new ServiceResponse(true, "تم تسجيل الطبيب بنجاح بانتظار موافقة النقابة.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during doctor registration");
                return new ServiceResponse(false, "حدث خطأ أثناء تسجيل الطبيب.");
            }
        }

        public async Task<ServiceResponse> VerifyCasualUserEmail(VerifyEmailDto dto)
        {
            try
            {
                var user = await _context.CasualUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);
                if (user == null)
                {
                    _logger.LogWarning($"Verification attempt for non-existent email: {dto.Email}");
                    return new ServiceResponse(false, "البريد غير مسجل.");
                }

                if (user.VerificationCode != dto.Code)
                {
                    _logger.LogWarning($"Invalid verification code for email: {dto.Email}");
                    return new ServiceResponse(false, "رمز التحقق غير صحيح.");
                }

                user.EmailVerified = true;
                user.VerificationCode = null;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Email verified successfully: {dto.Email}");
                return new ServiceResponse(true, "تم التحقق من البريد الإلكتروني بنجاح.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during email verification for {dto.Email}");
                return new ServiceResponse(false, "حدث خطأ أثناء التحقق من البريد الإلكتروني.");
            }
        }

        public async Task<ServiceResponse> VerifyClerkEmail(VerifyEmailDto dto)
        {
            try
            {
                var clerk = await _context.Clerks.FirstOrDefaultAsync(x => x.Email == dto.Email);
                if (clerk == null)
                {
                    _logger.LogWarning($"Verification attempt for non-existent clerk email: {dto.Email}");
                    return new ServiceResponse(false, "البريد غير مسجل.");
                }

                if (clerk.VerificationCode != dto.Code)
                {
                    _logger.LogWarning($"Invalid verification code for clerk email: {dto.Email}");
                    return new ServiceResponse(false, "رمز التحقق غير صحيح.");
                }

                clerk.IsEmailVerified = true;
                clerk.VerificationCode = null;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Clerk email verified successfully: {dto.Email}");
                return new ServiceResponse(true, "تم التحقق من بريد الموظف الإلكتروني بنجاح.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during clerk email verification for {dto.Email}");
                return new ServiceResponse(false, "حدث خطأ أثناء التحقق من بريد الموظف الإلكتروني.");
            }
        }

        private string GenerateVerificationCode()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var byteArray = new byte[4];
            rng.GetBytes(byteArray);
            var number = BitConverter.ToUInt32(byteArray, 0) % 1000000;
            return number.ToString("D6");
        }
    }
}