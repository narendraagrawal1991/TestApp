using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeApp.DTOs;
using OfficeApp.Filters;
using OfficeApp.Models;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class VehicleOwnerController : Controller
    {
        private readonly IVehicleOwnerService _ownerService;
        private readonly ILogger<VehicleOwnerController> _logger;

        public VehicleOwnerController(IVehicleOwnerService ownerService, ILogger<VehicleOwnerController> logger)
        {
            _ownerService = ownerService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return View(owners);
        }

        private static List<SelectListItem> GetDocumentTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "RC", Value = "RC" },
                new SelectListItem { Text = "PanCard", Value = "PanCard" },
                new SelectListItem { Text = "Declaration", Value = "Declaration" },
                new SelectListItem { Text = "Driving Lic", Value = "Driving Lic" },
                new SelectListItem { Text = "Bank Details", Value = "Bank Details" },
                new SelectListItem { Text = "Aadharcard No", Value = "Aadharcard No" }
            };
        }

        // GET: Create
        public IActionResult Create()
        {
            var model = new VehicleOwnerViewModel
            {
                Owner = new VehicleOwner(),
                DocumentTypes = GetDocumentTypes(),
                UploadedDocuments = new List<DocumentUpload>()
            };
            model.Owner.Id = 0;
            return View(model);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleOwnerViewModel model)
        {
            model.DocumentTypes = GetDocumentTypes();
            int transportId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            if (!string.IsNullOrEmpty(model.Owner.VehicleNo))
            {
                var dto = new VehicleOwnerDto
                {
                    Id = model.Owner.Id,
                    VehicleNo = model.Owner.VehicleNo,
                    OwnerName = model.Owner.OwnerName,
                    OwnerMobileNo1 = model.Owner.OwnerMobileNo1,
                    OwnerMobileNo2 = model.Owner.OwnerMobileNo2,
                    DriverName = model.Owner.DriverName,
                    DriverMobileNo1 = model.Owner.DriverMobileNo1,
                    DriverMobileNo2 = model.Owner.DriverMobileNo2,
                    RCNo = model.Owner.RCNo,
                    PanCardNo = model.Owner.PanCardNo,
                    AadharCardNo = model.Owner.AadharCardNo
                };

                var (success, ownerId, errorField, errorMessage) =
                    await _ownerService.CreateOrUpdateOwnerAsync(dto, transportId, userId);

                if (!success)
                {
                    ModelState.AddModelError(errorField!, errorMessage!);
                    return View(model);
                }

                return RedirectToAction("Edit", new { id = ownerId });
            }

            return View(model);
        }

        // POST: Upload More Documents
        [HttpPost]
        public async Task<IActionResult> UploadDocument(int ownerId, string selectedDocumentType, IFormFile uploadFile)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                await _ownerService.UploadDocumentAsync(ownerId, selectedDocumentType, uploadFile);
            }
            return RedirectToAction("Edit", new { id = ownerId });
        }

        // DELETE document
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var ownerId = await _ownerService.DeleteDocumentAsync(id);
            return RedirectToAction("Edit", new { id = ownerId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _ownerService.GetOwnerByIdWithDocumentsAsync(id);
            if (dto == null) return NotFound();

            // Map DTO back to entity for existing Details view
            var owner = new VehicleOwner
            {
                Id = dto.Id,
                VehicleNo = dto.VehicleNo,
                OwnerName = dto.OwnerName,
                OwnerMobileNo1 = dto.OwnerMobileNo1,
                OwnerMobileNo2 = dto.OwnerMobileNo2,
                DriverName = dto.DriverName,
                DriverMobileNo1 = dto.DriverMobileNo1,
                DriverMobileNo2 = dto.DriverMobileNo2,
                RCNo = dto.RCNo,
                PanCardNo = dto.PanCardNo,
                AadharCardNo = dto.AadharCardNo,
                Documents = dto.Documents.Select(d => new DocumentUpload
                {
                    Id = d.Id,
                    VehicleOwnerId = d.VehicleOwnerId,
                    DocumentType = d.DocumentType,
                    FilePath = d.FilePath,
                    UploadedDate = d.UploadedDate
                }).ToList()
            };

            return View(owner);
        }

        // GET
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _ownerService.GetOwnerByIdWithDocumentsAsync(id);
            if (dto == null) return NotFound();

            var model = new VehicleOwnerViewModel
            {
                Owner = new VehicleOwner
                {
                    Id = dto.Id,
                    VehicleNo = dto.VehicleNo,
                    OwnerName = dto.OwnerName,
                    OwnerMobileNo1 = dto.OwnerMobileNo1,
                    OwnerMobileNo2 = dto.OwnerMobileNo2,
                    DriverName = dto.DriverName,
                    DriverMobileNo1 = dto.DriverMobileNo1,
                    DriverMobileNo2 = dto.DriverMobileNo2,
                    RCNo = dto.RCNo,
                    PanCardNo = dto.PanCardNo,
                    AadharCardNo = dto.AadharCardNo
                },
                DocumentTypes = GetDocumentTypes(),
                UploadedDocuments = dto.Documents.Select(d => new DocumentUpload
                {
                    Id = d.Id,
                    VehicleOwnerId = d.VehicleOwnerId,
                    DocumentType = d.DocumentType,
                    FilePath = d.FilePath,
                    UploadedDate = d.UploadedDate
                }).ToList()
            };

            return View("Create", model);
        }

        // GET: Confirm Delete
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _ownerService.GetOwnerByIdWithDocumentsAsync(id);
            if (dto == null) return NotFound();

            var owner = new VehicleOwner
            {
                Id = dto.Id,
                VehicleNo = dto.VehicleNo,
                OwnerName = dto.OwnerName,
                DriverName = dto.DriverName
            };
            return View(owner);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ownerService.DeleteOwnerWithDocumentsAsync(id);
            return RedirectToAction("Index");
        }
    }
}
