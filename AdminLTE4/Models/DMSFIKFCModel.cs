using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSFIKFC")]
    public class DMSFIKFCModel
    {
        public int id { get; set; }
        public int? Doc_Number { get; set; }
        public string? KFC_Number { get; set; }
        public string? Customer { get; set; }
        public string? Part_Number { get; set; }
        public string? Description { get; set; }
        public string? Issue { get; set; }
        public DateTime? Issue_Date { get; set; }
        public string? Batch_Number { get; set; }
        public string? Rejection_Ref { get; set; }
        public string? Originator { get; set; }
        public string? Status { get; set; }
        public DateTime? Closed_Date { get; set; }
        public string? Remarks { get; set; }
    
    }
}

