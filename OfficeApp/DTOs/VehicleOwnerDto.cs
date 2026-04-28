namespace OfficeApp.DTOs
{
    public class VehicleOwnerDto
    {
        public int Id { get; set; }
        public string VehicleNo { get; set; } = string.Empty;
        public string? OwnerName { get; set; }
        public string? OwnerMobileNo1 { get; set; }
        public string? OwnerMobileNo2 { get; set; }
        public string? DriverName { get; set; }
        public string? DriverMobileNo1 { get; set; }
        public string? DriverMobileNo2 { get; set; }
        public string? RCNo { get; set; }
        public string? PanCardNo { get; set; }
        public string? AadharCardNo { get; set; }
        public List<DocumentUploadDto> Documents { get; set; } = new();
    }

    public class DocumentUploadDto
    {
        public int Id { get; set; }
        public int VehicleOwnerId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedDate { get; set; }
    }
}
