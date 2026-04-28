using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeApp.Models
{
    [Table("ItemCharges")]
    public class ItemCharges
    {
        [Key]
        public int Id { get; set; }
        public int LRFormsId { get; set; }
        public string DocumentType { get; set; }
        public int Freight { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}
