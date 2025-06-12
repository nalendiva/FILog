using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSProductionPermit")]
    public class DMSProductionPermitModel
    {
        public int id { get; set; }
        public string? Part_Number { get; set; }
        public string? Description { get; set; }
        public string? Production_Permit { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string? Remarks { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Remaining_Qty { get; set; }
        public string? Status { get; set; }
        public decimal? Alert_Qty { get; set; }
        public string? Alert_Email { get; set; }
    }
}
