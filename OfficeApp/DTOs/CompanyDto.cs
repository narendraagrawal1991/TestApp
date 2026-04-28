namespace OfficeApp.DTOs
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? ContactNo { get; set; }
        public string? EmailId { get; set; }
        public string? GSTNo { get; set; }
    }
}
