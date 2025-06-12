using System.ComponentModel.DataAnnotations.Schema;

namespace FILog.Models
{
    [Table("DMSMaterialMaster")]
    public class DMSMaterialMasterModel
    {
        public int id { get; set; }
        public string? Part_Number { get; set; }
        public string? Description { get; set; }
        public decimal? CoS { get; set; }
        public decimal? Price { get; set; }
        public decimal? Weight { get; set; }
        public string? Shipping_Point { get; set; }
        public string? Bin_Number { get; set; }
        public string? Customer_Name { get; set; }
        public string? Serialize { get; set; }
        public string? Batch_Size { get; set; }
        public decimal? OuterContent { get; set; }
        public string? Cell { get; set; }
        public decimal? FI_Std_Labor { get; set; }
        public string? FI_Tag { get; set; }
        public string? Box_Type { get; set; }
        public string? Rec_Item_Check { get; set; }
        public string? Rev_Check { get; set; }
        public decimal? FG_CT { get; set; }
        public int? Alert_Number { get; set; }


    }
}
