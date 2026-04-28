using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http; // Add this at the top

namespace OfficeApp.Models
{
    [Table("LRForm")]
    public class LRForm
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public string Consignor_Name { get; set; }
        public int Consignor { get; set; }
        [NotMapped]
        public string Consignee_Name { get; set; }
        public int Consignee { get; set; }
        [NotMapped]
        public string VehicleNo_Name { get; set; }
        public int VehicleNo { get; set; }
        public bool GstPaidByConsignor { get; set; }
        public bool GstPaidByConsignee { get; set; }
        public string? To { get; set; }
        public string? From { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? ContactNo { get; set; }
        
        [Required]
        public string LRNo { get; set; }
        public DateTime LRDate { get; set; }
        public string? InvoiceNo { get; set; }
        public string? Value { get; set; }
        public string? EWayBillNo { get; set; }
        public decimal Freight { get; set; }
        public decimal Charges { get; set; }
        public decimal StCh { get; set; }
        public decimal GST { get; set; }
        public decimal Other { get; set; }
        public decimal Advance { get; set; }
        public string? Remarks { get; set; }

        [Required(ErrorMessage = "Please select a payment type.")]
        public string? PaymentType { get; set; }

        [NotMapped]
        public List<ItemEntry> Items { get; set; } = new List<ItemEntry>();

        [NotMapped]
        public List<ItemCharges> ItemCharges { get; set; } = new List<ItemCharges>();

        public int TransportID { get; set; }

        public int UserID { get; set; }

        public DateTime? createdate { get; set; }

        public DateTime? Modifydate { get; set; }

        public virtual ICollection<LRDocumentUpload> Documents { get; set; }

    }
}
