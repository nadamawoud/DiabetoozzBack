using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Diabetes.Repository.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            // 1. استخدام سلسلة اتصال مباشرة (بدون ملف إعدادات)
            var connectionString = "Server=NADAPC\\SQLEXPRESS;Database=Diabetoozz;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False";

            // 2. إعداد خيارات DbContext
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            // 3. طباعة المسار للتحقق (لأغراض التصحيح)
            Console.WriteLine($"Using connection string: {connectionString}");

            // 4. إرجاع السياق الجديد
            return new StoreContext(optionsBuilder.Options);
        }
    }
}