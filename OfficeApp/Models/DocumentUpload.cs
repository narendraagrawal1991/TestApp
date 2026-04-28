using System.ComponentModel.DataAnnotations;

namespace OfficeApp.Models
{
    public class DocumentUpload
    {
        [Key]
        public int Id { get; set; }
        public int VehicleOwnerId { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }
        public virtual VehicleOwner VehicleOwner { get; set; }
    }
}
