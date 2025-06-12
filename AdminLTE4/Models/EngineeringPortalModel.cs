using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("EngineeringPortal_PartNumber")]
    public class EngineeringPortalModel
    {
        [Key]
        public string PartNumber  { get; set; }
        public string? PartName { get; set; }
        public string? DrawingID { get; set; }
        public string? FamilyPart { get; set; }
        public string? Customer_ID { get; set; }
        public string? Cutomer2_ID { get; set; }
        public string? Customer3_ID { get; set; }
        public string? ProcessPlanNo { get; set; }
        public string? part_image { get; set; }
        public string? PartType { get; set; }
        public string? cost { get; set; }

    }
}
