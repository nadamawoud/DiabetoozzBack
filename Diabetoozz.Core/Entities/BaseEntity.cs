using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diabetes.Core.Entities
{
    public class BaseEntity
    {
        [Key]  // تحديد ID كمفتاح أساسي
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // يضمن Auto-Increment
        public int ID { get; set; }
    }
}