using Diabetes.Core.Repositories;
using Diabetes.Repository;
using Diabetes.Repository.Data;
using Diabetes.Services;
using Diabetes.Services.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Diabetes.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IJsonSerializerService, JsonSerializerService>();
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                    };
                });
            #endregion

            var app = builder.Build();

            #region Update-Database
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = services.GetRequiredService<StoreContext>();

                // تأكد من تطبيق الترحيل أولاً
                await DbContext.Database.MigrateAsync();

                // بعد الترحيل، قم بملء البيانات أو تحديثها تلقائيًا
                await StoreContextSeed.SeedAsnc(DbContext); // تأكد من تحميل البيانات من JSON

                // إذا كنت تريد حفظ التعديلات التلقائية بعد إضافة البيانات
                await DbContext.SaveChangesAsync();  // هذه الخطوة تحفظ التعديلات في قاعدة البيانات
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred during migration or seeding");
            }
            #endregion

            #region Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();  // تمكين المصادقة باستخدام JWT
            app.UseAuthorization();
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}