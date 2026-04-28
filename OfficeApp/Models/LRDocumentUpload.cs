using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeApp.Models
{
    [Table("LRDocument")]
    public class LRDocumentUpload
    {
        [Key]
        public int Id { get; set; }
        public int LRFormsId { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }
        public virtual LRForm LRForms { get; set; }
    }
}
