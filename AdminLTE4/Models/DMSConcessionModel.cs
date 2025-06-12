using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSConcession")]
    public class DMSConcessionModel
    {
        public int id { get; set; }
        public string? Order_Number { get; set; }
        public string? Batch_Number { get; set; }
        public string? Part_Number { get; set; }
        public string? Description { get; set; }
        public string? Concession { get; set; }
        public string? Remarks { get; set; }
        public string? LabelForm { get; set; }
    }
}
