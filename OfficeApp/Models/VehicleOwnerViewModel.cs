using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http; // Add this at the top

namespace OfficeApp.Models
{
    public class VehicleOwnerViewModel
    {
        public VehicleOwner Owner { get; set; }

        public IFormFile UploadFile { get; set; }

        public string SelectedDocumentType { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; }

        public List<DocumentUpload> UploadedDocuments { get; set; }
    }
}
