using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("eLOGQualityAlert")]
    public class eLOGQualityAlertModel
    {
        [Key]
        public int id { get; set; }
        public string? Part_Number { get; set; }
        public string? Image {  get; set; }

    }
}
