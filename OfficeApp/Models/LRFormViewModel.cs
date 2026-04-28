using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; // Add this at the top

namespace OfficeApp.Models
{
    public class LRFormViewModel
    {
        public LRForm LRForms { get; set; }

        public IFormFile UploadFile { get; set; }

        public string SelectedDocumentType { get; set; }

        public List<SelectListItem> DocumentTypes { get; set; }

        public List<LRDocumentUpload> UploadedDocuments { get; set; }

        [DisplayName("Changes Type")]
        public string ChangesType { get; set; }

        [DisplayName("Amount")]
        public int Amount { get; set; }
    }
}
