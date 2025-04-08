using System.Text.Json;

namespace Diabetes.Services
{
    public class JsonSerializerService : IJsonSerializerService
    {
        public string SerializeObject(object obj)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // تخصيص تسميات الحقول
                WriteIndented = true  // تنسيق JSON بحيث يكون قابلًا للقراءة
            };
            return JsonSerializer.Serialize(obj, options);  // تحويل الكائن إلى JSON
        }
    }
}
