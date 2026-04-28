namespace OfficeApp.DTOs
{
    public class PartyDto
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public string? GSTNo { get; set; }
        public string? ContactNo1 { get; set; }
        public string? ContactNo2 { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
    }
}
