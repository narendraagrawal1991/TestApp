namespace OfficeApp.Models
{
    public class VehicleOwner
    {
        public int Id { get; set; }

        public string VehicleNo { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerMobileNo1 { get; set; }
        public string? OwnerMobileNo2 { get; set; }

        public string? DriverName { get; set; }
        public string? DriverMobileNo1 { get; set; }
        public string? DriverMobileNo2 { get; set; }

        public string? RCNo { get; set; }
        public string? PanCardNo { get; set; }
        public string? AadharCardNo { get; set; }

        public int TransportID { get; set; }

        public int UserID { get; set; }

        public DateTime? createdate { get; set; }

        public DateTime? Modifydate { get; set; }
        public virtual ICollection<DocumentUpload> Documents { get; set; }
    }
}
