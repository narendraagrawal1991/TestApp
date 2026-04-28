namespace OfficeApp.Models
{
    public class UploadDocumentRequest
    {
        public int OwnerId { get; set; }
        public string DocumentType { get; set; }
        public IFormFile File { get; set; }
    }
}
