using OfficeApp.Models;

namespace OfficeApp.DTOs
{
    public class LRFormDto
    {
        public int Id { get; set; }
        public string Consignor_Name { get; set; } = string.Empty;
        public int Consignor { get; set; }
        public string Consignee_Name { get; set; } = string.Empty;
        public int Consignee { get; set; }
        public string VehicleNo_Name { get; set; } = string.Empty;
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
        public string LRNo { get; set; } = string.Empty;
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
        public string? PaymentType { get; set; }

        public List<ItemEntry> Items { get; set; } = new();
        public List<ItemCharges> ItemCharges { get; set; } = new();
        public List<LRDocumentUploadDto> Documents { get; set; } = new();
    }

    public class LRDocumentUploadDto
    {
        public int Id { get; set; }
        public int LRFormsId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
    }
}
