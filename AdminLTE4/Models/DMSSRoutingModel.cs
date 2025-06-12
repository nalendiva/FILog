using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSSRouting")]
    public class DMSSRoutingModel
    {
        public int id { get; set; }
        public string? Part_Number { get; set; }
        public string? Wordk_Center { get; set; }
        public string? Operation { get; set; }
    }
}
