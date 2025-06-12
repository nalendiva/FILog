using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSFILog")]
    public class DMSFILogModel
    {
        public int id { get; set; }
        public int? Doc_Number { get; set; }
        public string? Order_Number { get; set; }
        public string? Part_Number { get; set; }
        public string? Description { get; set; }
        public string? Drawing_Rev { get; set; }
        public string? Batch_Number { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Acc { get; set; }
        public decimal? Rejected { get; set; }
        public string? Job_Type { get; set; }
        public string? Operation { get; set; }
        public DateTime? Start_Date { get; set; }
        public TimeSpan? Start_Time { get; set; }
        public DateTime? End_Date { get; set; }
        public TimeSpan? End_Time { get; set; }
        public decimal? Labor { get; set; }
        public string? Week { get; set; }
        public string? EID { get; set; }
        public string? Name { get; set; }
        public decimal? Std_Price { get; set; }
        public string? Cell { get; set; }
        public string? QN { get; set; }
        public string? KFR { get; set; }
        public string? Concession { get; set; }
        public string? Production_Permit { get; set; }
        public string? Serial_Number { get; set; }
        public string? Remarks { get; set; }
        public string? Status { get; set; }
    }
}

