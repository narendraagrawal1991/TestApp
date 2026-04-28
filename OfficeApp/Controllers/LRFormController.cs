using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeApp.DTOs;
using OfficeApp.Filters;
using OfficeApp.Helpers;
using OfficeApp.Models;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class LRFormController : Controller
    {
        private readonly ILRFormService _lrFormService;
        private readonly ILogger<LRFormController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int transportId = 0;
        private int userId = 0;
        public LRFormController(ILRFormService lrFormService, ILogger<LRFormController> logger, IHttpContextAccessor accessor)
        {
            _lrFormService = lrFormService;
            _logger = logger;
            _httpContextAccessor = accessor;
            transportId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("CompanyId"));
            userId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
        }

        public async Task<IActionResult> Index()
        {
            var forms = await _lrFormService.GetAllLRFormsAsync(transportId);

            // Map DTOs to entities for existing view compatibility
            var result = forms.Select(f => new LRForm
            {
                Id = f.Id,
                Consignor_Name = f.Consignor_Name,
                Consignee_Name = f.Consignee_Name,
                VehicleNo_Name = f.VehicleNo_Name,
                LRNo = f.LRNo,
                LRDate = f.LRDate
            }).ToList();

            return View(result);
        }

        public IActionResult Create()
        {
            var model = new LRFormViewModel
            {
                LRForms = new LRForm
                {
                    PaymentType = "To Pay",
                    Items = new List<ItemEntry>(),
                    ItemCharges = new List<ItemCharges>(),
                    LRDate = DateTime.Now,
                    createdate = DateTime.Now,
                    Modifydate = DateTime.Now,
                    Id = 0
                },
                DocumentTypes = GetDocumentTypes(),
                UploadedDocuments = new List<LRDocumentUpload>(),
                ChangesType = "",
                Amount = 0
            };

            ViewBag.States = IndianStatesHelper.GetIndianStates();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LRFormViewModel model)
        {
            ViewBag.States = IndianStatesHelper.GetIndianStates();
            if (!string.IsNullOrEmpty(model.LRForms.LRNo))
            {
                var dto = MapLRFormToDto(model.LRForms);

                var (success, errorField, errorMessage) =
                    await _lrFormService.CreateOrUpdateLRFormAsync(dto, transportId, userId);

                if (!success)
                {
                    ModelState.AddModelError(errorField!, errorMessage!);
                    return View(model);
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }

        // GET: LRForm/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _lrFormService.GetLRFormForEditAsync(id, transportId);
            if (dto == null) return NotFound();

            ViewBag.States = IndianStatesHelper.GetIndianStates();

            var lrForm = MapDtoToLRForm(dto);

            var model = new LRFormViewModel
            {
                LRForms = lrForm,
                DocumentTypes = GetDocumentTypes(),
                UploadedDocuments = dto.Documents.Select(d => new LRDocumentUpload
                {
                    Id = d.Id,
                    LRFormsId = d.LRFormsId,
                    DocumentType = d.DocumentType,
                    FilePath = d.FilePath,
                    UploadedDate = d.UploadedDate
                }).ToList()
            };

            return View("Create", model);
        }

        // GET: LRForm/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            await _lrFormService.DeleteLRFormAsync(id, transportId);
            return RedirectToAction("Index");
        }

        // POST: Upload More Documents
        [HttpPost]
        public async Task<IActionResult> UploadDocument(int ownerId, string selectedDocumentType, IFormFile uploadFile)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                await _lrFormService.UploadDocumentAsync(ownerId, selectedDocumentType, uploadFile);
            }
            return RedirectToAction("Edit", new { id = ownerId });
        }

        // DELETE document
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var lrId = await _lrFormService.DeleteDocumentAsync(id);
            return RedirectToAction("Edit", new { id = lrId });
        }

        // POST: Add Charges
        [HttpPost]
        public async Task<IActionResult> AddChagrges(int ownerId, string ChangesType, int Amount)
        {
            if (!string.IsNullOrEmpty(ChangesType) && Amount > 0)
            {
                await _lrFormService.AddChargeAsync(ownerId, ChangesType, Amount);
            }
            return RedirectToAction("Edit", new { id = ownerId });
        }

        // DELETE charge
        public async Task<IActionResult> DeleteChagrges(int id)
        {
            var lrId = await _lrFormService.DeleteChargeAsync(id);
            return RedirectToAction("Edit", new { id = lrId });
        }

        public async Task<IActionResult> Print(int id)
        {
            // Print functionality preserved - requires Microsoft.Reporting
            var dto = await _lrFormService.GetLRFormForEditAsync(id, transportId);
            if (dto == null) return NotFound();

            // TODO: Implement report generation via a dedicated reporting service
            return NotFound("Print functionality requires report service implementation.");
        }

        private static List<SelectListItem> GetDocumentTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Invoice", Value = "Invoice" },
                new SelectListItem { Text = "EwayBill", Value = "EwayBill" },
                new SelectListItem { Text = "LR Copy", Value = "LRCopy" },
                new SelectListItem { Text = "POD", Value = "POD" }
            };
        }

        private static LRFormDto MapLRFormToDto(LRForm f) => new()
        {
            Id = f.Id,
            Consignor = f.Consignor,
            Consignor_Name = f.Consignor_Name ?? "",
            Consignee = f.Consignee,
            Consignee_Name = f.Consignee_Name ?? "",
            VehicleNo = f.VehicleNo,
            VehicleNo_Name = f.VehicleNo_Name ?? "",
            GstPaidByConsignor = f.GstPaidByConsignor,
            GstPaidByConsignee = f.GstPaidByConsignee,
            To = f.To,
            From = f.From,
            Address1 = f.Address1,
            Address2 = f.Address2,
            City = f.City,
            District = f.District,
            State = f.State,
            Pincode = f.Pincode,
            ContactNo = f.ContactNo,
            LRNo = f.LRNo,
            LRDate = f.LRDate,
            InvoiceNo = f.InvoiceNo,
            Value = f.Value,
            EWayBillNo = f.EWayBillNo,
            Freight = f.Freight,
            Charges = f.Charges,
            StCh = f.StCh,
            GST = f.GST,
            Other = f.Other,
            Advance = f.Advance,
            Remarks = f.Remarks,
            PaymentType = f.PaymentType,
            Items = f.Items ?? new List<ItemEntry>()
        };

        private static LRForm MapDtoToLRForm(LRFormDto d) => new()
        {
            Id = d.Id,
            Consignor = d.Consignor,
            Consignor_Name = d.Consignor_Name,
            Consignee = d.Consignee,
            Consignee_Name = d.Consignee_Name,
            VehicleNo = d.VehicleNo,
            VehicleNo_Name = d.VehicleNo_Name,
            GstPaidByConsignor = d.GstPaidByConsignor,
            GstPaidByConsignee = d.GstPaidByConsignee,
            To = d.To,
            From = d.From,
            Address1 = d.Address1,
            Address2 = d.Address2,
            City = d.City,
            District = d.District,
            State = d.State,
            Pincode = d.Pincode,
            ContactNo = d.ContactNo,
            LRNo = d.LRNo,
            LRDate = d.LRDate,
            InvoiceNo = d.InvoiceNo,
            Value = d.Value,
            EWayBillNo = d.EWayBillNo,
            Freight = d.Freight,
            Charges = d.Charges,
            StCh = d.StCh,
            GST = d.GST,
            Other = d.Other,
            Advance = d.Advance,
            Remarks = d.Remarks,
            PaymentType = d.PaymentType,
            Items = d.Items,
            ItemCharges = d.ItemCharges
        };
    }
}
