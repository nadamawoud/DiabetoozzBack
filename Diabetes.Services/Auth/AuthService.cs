using Diabetes.Core.DTOs;
using Diabetes.Core.Entities;
using Diabetes.Repository.Data;
using Diabetes.Services.Auth;
using Diabetes.Services;
using Microsoft.EntityFrameworkCore;

public class AuthService : IAuthService
{
    private readonly StoreContext _context;
    private readonly IEmailService _emailService;

    public AuthService(StoreContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<ServiceResponse> RegisterCasualUser(CasualUserRegisterDto dto)
    {
        var userExists = await _context.CasualUsers.AnyAsync(x => x.Email == dto.Email);
        if (userExists)
            return new ServiceResponse(false, "هذا البريد مستخدم من قبل.");

        var casualUser = new CasualUser
        {
            Username = dto.Username,
            Email = dto.Email,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            VerificationCode = GenerateVerificationCode(),
            EmailVerified = false
        };

        _context.CasualUsers.Add(casualUser);
        await _context.SaveChangesAsync();

        await _emailService.SendVerificationEmail(dto.Email, casualUser.VerificationCode);

        return new ServiceResponse(true, "تم التسجيل بنجاح، تحقق من بريدك الإلكتروني.");
    }

    public async Task<ServiceResponse> RegisterClerk(ClerkRegisterDto dto)
    {
        var exists = await _context.Clerks.AnyAsync(x => x.Email == dto.Email);
        if (exists)
            return new ServiceResponse(false, "البريد الإلكتروني مستخدم من قبل.");

        var clerk = new Clerk
        {
            Name = dto.Name,
            Email = dto.Email,
            BirthDate = dto.BirthDate,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber,
            LicenseCode = dto.LicenseCode,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            VerificationCode = GenerateVerificationCode(),
            CreatedAt = DateTime.UtcNow,
            IsEmailVerified = false
        };

        _context.Clerks.Add(clerk);
        await _context.SaveChangesAsync();

        await _emailService.SendVerificationEmail(clerk.Email, clerk.VerificationCode);

        return new ServiceResponse(true, "تم التسجيل بنجاح، تحقق من بريدك الإلكتروني.");
    }

    public async Task<ServiceResponse> RegisterDoctor(DoctorRegisterDto dto)
    {
        var exists = await _context.Doctors.AnyAsync(x => x.Email == dto.Email);
        if (exists)
            return new ServiceResponse(false, "البريد الإلكتروني مستخدم من قبل.");

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

        return new ServiceResponse(true, "تم تسجيل الطبيب بنجاح بانتظار موافقة النقابة.");
    }

    public async Task<ServiceResponse> VerifyCasualUserEmail(VerifyEmailDto dto)
    {
        var user = await _context.CasualUsers.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
            return new ServiceResponse(false, "البريد غير مسجل.");

        if (user.VerificationCode != dto.Code)
            return new ServiceResponse(false, "رمز التحقق غير صحيح.");

        user.EmailVerified = true;
        user.VerificationCode = null;
        await _context.SaveChangesAsync();

        return new ServiceResponse(true, "تم التحقق من البريد الإلكتروني بنجاح.");
    }

    public async Task<ServiceResponse> VerifyClerkEmail(VerifyEmailDto dto)
    {
        var clerk = await _context.Clerks.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (clerk == null)
            return new ServiceResponse(false, "البريد غير مسجل.");

        if (clerk.VerificationCode != dto.Code)
            return new ServiceResponse(false, "رمز التحقق غير صحيح.");

        clerk.IsEmailVerified = true;
        clerk.VerificationCode = null;
        await _context.SaveChangesAsync();

        return new ServiceResponse(true, "تم تفعيل البريد الإلكتروني للكاتب.");
    }

    private string GenerateVerificationCode()
    {
        return new Random().Next(100000, 999999).ToString();
    }
}