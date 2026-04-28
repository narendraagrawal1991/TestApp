using Microsoft.SqlServer.Server;
using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class LRFormService : ILRFormService
    {
        private readonly ILRFormRepository _repo;
        private readonly IDocumentRepository _docRepo;
        private readonly IFileUploadService _fileService;
        private readonly ILookupService _lookupService;
        private readonly ILogger<LRFormService> _logger;

        public LRFormService(
            ILRFormRepository repo,
            IDocumentRepository docRepo,
            IFileUploadService fileService,
            ILookupService lookupService,
            ILogger<LRFormService> logger)
        {
            _repo = repo;
            _docRepo = docRepo;
            _fileService = fileService;
            _lookupService = lookupService;
            _logger = logger;
        }

        public async Task<List<LRFormDto>> GetAllLRFormsAsync()
        {
            var forms = await _repo.GetAllAsync();
            List<int> partyArray = forms.ToList().Select(t => t.Consignee).ToList();
            partyArray.AddRange(forms.ToList().Select(t => t.Consignor).ToList());
            var partyLookup = await _lookupService.GetPartyLookupAsync(partyArray);
            List<int> vehicleArray = forms.ToList().Select(t => t.VehicleNo).ToList();
            var vehicleLookup = await _lookupService.GetVehicleOwnerLookupAsync(vehicleArray);

            return forms.Select(f =>
            {
                var dto = MapToDto(f);
                dto.Consignor_Name = f.Consignor == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == f.Consignor)?.Name ?? "";
                dto.Consignee_Name = f.Consignee == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == f.Consignee)?.Name ?? "";
                dto.VehicleNo_Name = f.VehicleNo == 0 ? "" : vehicleLookup.FirstOrDefault(t => t.Id == f.VehicleNo)?.Name ?? "";
                return dto;
            }).ToList();
        }

        public async Task<List<LRFormDto>> GetAllLRFormsAsync(int transportId)
        {
            var forms = await _repo.GetAllAsync(transportId);
            List<int> partyArray = forms.ToList().Select(t => t.Consignee).ToList();
            partyArray.AddRange(forms.ToList().Select(t => t.Consignor).ToList());
            var partyLookup = await _lookupService.GetPartyLookupAsync(partyArray);
            List<int> vehicleArray = forms.ToList().Select(t => t.VehicleNo).ToList();
            var vehicleLookup = await _lookupService.GetVehicleOwnerLookupAsync(vehicleArray);

            return forms.Select(f =>
            {
                var dto = MapToDto(f);
                dto.Consignor_Name = f.Consignor == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == f.Consignor)?.Name ?? "";
                dto.Consignee_Name = f.Consignee == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == f.Consignee)?.Name ?? "";
                dto.VehicleNo_Name = f.VehicleNo == 0 ? "" : vehicleLookup.FirstOrDefault(t => t.Id == f.VehicleNo)?.Name ?? "";
                return dto;
            }).ToList();
        }

        public async Task<LRFormDto?> GetLRFormForEditAsync(int id,int transportId)
        {
            var form = await _repo.GetByIdWithDocumentsAsync(id,transportId);
            if (form == null) return null;
            var partyLookup = await _lookupService.GetPartyLookupAsync(new List<int> { form.Consignor, form.Consignee });
            var vehicleLookup = await _lookupService.GetVehicleOwnerLookupAsync(new List<int> { form.VehicleNo });

            var dto = MapToDto(form);
            dto.Consignor_Name = form.Consignor == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == form.Consignor)?.Name ?? "";
            dto.Consignee_Name = form.Consignee == 0 ? "" : partyLookup.FirstOrDefault(t => t.Id == form.Consignee)?.Name ?? "";
            dto.VehicleNo_Name = form.VehicleNo == 0 ? "" : vehicleLookup.FirstOrDefault(t => t.Id == form.VehicleNo)?.Name ?? "";

            var items = await _repo.GetItemEntriesByLRIdAsync(id);
            foreach (var item in items)
            {
                if (int.TryParse(item.Actual, out int number))
                    item.Total = number + item.Charged;
                else
                    item.Total = item.Charged;
            }
            dto.Items = items;

            dto.ItemCharges = (await _repo.GetItemChargesByLRIdAsync(id)).ToList();

            dto.Documents = form.Documents?.Select(d => new LRDocumentUploadDto
            {
                Id = d.Id,
                LRFormsId = d.LRFormsId,
                DocumentType = d.DocumentType,
                FilePath = d.FilePath,
                UploadedDate = d.UploadedDate
            }).ToList() ?? new List<LRDocumentUploadDto>();

            return dto;
        }

        public async Task<(bool Success, string? ErrorField, string? ErrorMessage)> CreateOrUpdateLRFormAsync(
            LRFormDto dto, int transportId, int userId)
        {
            if (dto.Id == 0)
            {
                if (await _repo.ExistsByLRNoAsync(dto.LRNo, transportId))
                    return (false, "LRForms.LRNo", "LRNo already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.createdate = DateTime.Now;
                entity.Modifydate = DateTime.Now;

                await _repo.AddAsync(entity);

                foreach (var item in dto.Items)
                {
                    item.LRID = entity.Id;
                    await _repo.AddItemEntryAsync(item);
                }

                _logger.LogInformation("LRForm '{LRNo}' created with ID {Id}", entity.LRNo, entity.Id);
            }
            else
            {
                if (await _repo.ExistsByLRNoAsync(dto.LRNo, transportId, dto.Id))
                    return (false, "LRForms.LRNo", "LRNo already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.Modifydate = DateTime.Now;

                await _repo.UpdateAsync(entity);
                await _repo.RemoveItemEntriesAsync(entity.Id);

                foreach (var item in dto.Items)
                {
                    item.LRID = entity.Id;
                    await _repo.AddItemEntryAsync(item);
                }

                _logger.LogInformation("LRForm ID {Id} updated", entity.Id);
            }

            return (true, null, null);
        }

        public async Task DeleteLRFormAsync(int id, int transportId)
        {
            var form = await _repo.GetByIdWithDocumentsAsync(id,transportId);
            if (form == null) return;

            foreach (var doc in form.Documents.ToList())
            {
                _fileService.DeleteFile(doc.FilePath);
                await _docRepo.RemoveLRDocumentAsync(doc);
            }

            await _repo.DeleteAsync(form);
            _logger.LogInformation("LRForm ID {Id} deleted", id);
        }

        public async Task UploadDocumentAsync(int lrId, string docType, IFormFile file)
        {
            var relativePath = await _fileService.SaveFileAsync(file, "LRDocument", lrId, docType);

            var doc = new LRDocumentUpload
            {
                LRFormsId = lrId,
                DocumentType = docType,
                FilePath = relativePath,
                UploadedDate = DateTime.Now
            };

            await _docRepo.AddLRDocumentAsync(doc);
            _logger.LogInformation("Document uploaded for LRForm {Id}: {DocType}", lrId, docType);
        }

        public async Task<int> DeleteDocumentAsync(int docId)
        {
            var doc = await _docRepo.GetLRDocumentByIdAsync(docId);
            if (doc == null) return 0;

            _fileService.DeleteFile(doc.FilePath);
            int lrId = doc.LRFormsId;
            await _docRepo.RemoveLRDocumentAsync(doc);
            _logger.LogInformation("Document ID {DocId} deleted for LRForm {LRId}", docId, lrId);
            return lrId;
        }

        public async Task AddChargeAsync(int lrId, string type, int amount)
        {
            var charge = new ItemCharges
            {
                LRFormsId = lrId,
                DocumentType = type,
                Freight = amount,
                UploadedDate = DateTime.Now
            };

            await _docRepo.AddItemChargeAsync(charge);
            _logger.LogInformation("Charge added for LRForm {Id}: {Type} = {Amount}", lrId, type, amount);
        }

        public async Task<int> DeleteChargeAsync(int chargeId)
        {
            var charge = await _docRepo.GetItemChargeByIdAsync(chargeId);
            if (charge == null) return 0;

            int lrId = charge.LRFormsId;
            await _docRepo.RemoveItemChargeAsync(charge);
            _logger.LogInformation("Charge ID {ChargeId} deleted for LRForm {LRId}", chargeId, lrId);
            return lrId;
        }

        private static LRFormDto MapToDto(LRForm f) => new()
        {
            Id = f.Id,
            Consignor = f.Consignor,
            Consignee = f.Consignee,
            VehicleNo = f.VehicleNo,
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
            PaymentType = f.PaymentType
        };

        private static LRForm MapToEntity(LRFormDto d) => new()
        {
            Id = d.Id,
            Consignor = d.Consignor,
            Consignee = d.Consignee,
            VehicleNo = d.VehicleNo,
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
            PaymentType = d.PaymentType
        };
    }
}
