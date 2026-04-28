using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeApp.Models
{
    [Table("ItemEntry")]
    public class ItemEntry
    {
        public int Id { get; set; }
        public int LRID { get; set; }
        public string? Packages { get; set; }
        public string? Description { get; set; }
        public string? Actual { get; set; }
        public int Charged { get; set; }
        [NotMapped]
        public int Total { get; set; }
    }
}
