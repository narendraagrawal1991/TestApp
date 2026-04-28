using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OfficeApp.Models
{
    [Table("Party")]
    public class Party
    {
        [Key]
        public int PartyId { get; set; }

        [Required]
        public string PartyName { get; set; } = null!;

        public string? GSTNo { get; set; }

        public string? ContactNo1 { get; set; }

        public string? ContactNo2 { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? State { get; set; }

        public string? Pincode { get; set; }

        public int TransportID { get; set; }

        public int UserID { get; set; }

        public DateTime? createdate { get; set; }

        public DateTime? Modifydate { get; set; }
    }
}
