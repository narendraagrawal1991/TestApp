using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeApp.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? Pincode { get; set; }

        [StringLength(15)]
        public string? ContactNo { get; set; }

        [StringLength(15)]
        public string? EmailId { get; set; }

        [StringLength(15)]
        public string? GSTNo { get; set; }

       
    }
}
