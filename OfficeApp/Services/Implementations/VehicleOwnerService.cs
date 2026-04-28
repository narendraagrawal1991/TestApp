using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class VehicleOwnerService : IVehicleOwnerService
    {
        private readonly IVehicleOwnerRepository _repo;
        private readonly IDocumentRepository _docRepo;
        private readonly IFileUploadService _fileService;
        private readonly ILogger<VehicleOwnerService> _logger;

        public VehicleOwnerService(
            IVehicleOwnerRepository repo,
            IDocumentRepository docRepo,
            IFileUploadService fileService,
            ILogger<VehicleOwnerService> logger)
        {
            _repo = repo;
            _docRepo = docRepo;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<List<VehicleOwnerDto>> GetAllOwnersAsync()
        {
            var owners = await _repo.GetAllAsync();
            return owners.Select(MapToDto).ToList();
        }

        public async Task<VehicleOwnerDto?> GetOwnerByIdWithDocumentsAsync(int id)
        {
            var owner = await _repo.GetByIdWithDocumentsAsync(id);
            if (owner == null) return null;

            var dto = MapToDto(owner);
            dto.Documents = owner.Documents?.Select(d => new DocumentUploadDto
            {
                Id = d.Id,
                VehicleOwnerId = d.VehicleOwnerId,
                DocumentType = d.DocumentType,
                FilePath = d.FilePath,
                UploadedDate = d.UploadedDate
            }).ToList() ?? new List<DocumentUploadDto>();

            return dto;
        }

        public async Task<(bool Success, int OwnerId, string? ErrorField, string? ErrorMessage)> CreateOrUpdateOwnerAsync(
            VehicleOwnerDto dto, int transportId, int userId)
        {
            if (dto.Id == 0)
            {
                if (await _repo.ExistsByVehicleNoAsync(dto.VehicleNo))
                    return (false, 0, "Owner.VehicleNo", "Vehicle number already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.createdate = DateTime.Now;
                entity.Modifydate = DateTime.Now;

                await _repo.AddAsync(entity);
                _logger.LogInformation("VehicleOwner '{VehicleNo}' created with ID {Id}", entity.VehicleNo, entity.Id);
                return (true, entity.Id, null, null);
            }
            else
            {
                if (await _repo.ExistsByVehicleNoAsync(dto.VehicleNo, dto.Id))
                    return (false, dto.Id, "Owner.VehicleNo", "Vehicle number already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.Modifydate = DateTime.Now;

                await _repo.UpdateAsync(entity);
                _logger.LogInformation("VehicleOwner ID {Id} updated", entity.Id);
                return (true, entity.Id, null, null);
            }
        }

        public async Task DeleteOwnerWithDocumentsAsync(int id)
        {
            var owner = await _repo.GetByIdWithDocumentsAsync(id);
            if (owner == null) return;

            foreach (var doc in owner.Documents.ToList())
            {
                _fileService.DeleteFile(doc.FilePath);
                await _docRepo.RemoveDocumentAsync(doc);
            }

            await _repo.DeleteAsync(owner);
            _logger.LogInformation("VehicleOwner ID {Id} deleted with all documents", id);
        }

        public async Task UploadDocumentAsync(int ownerId, string docType, IFormFile file)
        {
            var relativePath = await _fileService.SaveFileAsync(file, "VehicleDocument", ownerId, docType);

            var doc = new DocumentUpload
            {
                VehicleOwnerId = ownerId,
                DocumentType = docType,
                FilePath = relativePath,
                UploadedDate = DateTime.Now
            };

            await _docRepo.AddDocumentAsync(doc);
            _logger.LogInformation("Document uploaded for VehicleOwner {Id}: {DocType}", ownerId, docType);
        }

        public async Task<int> DeleteDocumentAsync(int docId)
        {
            var doc = await _docRepo.GetDocumentByIdAsync(docId);
            if (doc == null) return 0;

            _fileService.DeleteFile(doc.FilePath);
            int ownerId = doc.VehicleOwnerId;
            await _docRepo.RemoveDocumentAsync(doc);
            _logger.LogInformation("Document ID {DocId} deleted for VehicleOwner {OwnerId}", docId, ownerId);
            return ownerId;
        }

        private static VehicleOwnerDto MapToDto(VehicleOwner v) => new()
        {
            Id = v.Id,
            VehicleNo = v.VehicleNo,
            OwnerName = v.OwnerName,
            OwnerMobileNo1 = v.OwnerMobileNo1,
            OwnerMobileNo2 = v.OwnerMobileNo2,
            DriverName = v.DriverName,
            DriverMobileNo1 = v.DriverMobileNo1,
            DriverMobileNo2 = v.DriverMobileNo2,
            RCNo = v.RCNo,
            PanCardNo = v.PanCardNo,
            AadharCardNo = v.AadharCardNo
        };

        private static VehicleOwner MapToEntity(VehicleOwnerDto d) => new()
        {
            Id = d.Id,
            VehicleNo = d.VehicleNo,
            OwnerName = d.OwnerName,
            OwnerMobileNo1 = d.OwnerMobileNo1,
            OwnerMobileNo2 = d.OwnerMobileNo2,
            DriverName = d.DriverName,
            DriverMobileNo1 = d.DriverMobileNo1,
            DriverMobileNo2 = d.DriverMobileNo2,
            RCNo = d.RCNo,
            PanCardNo = d.PanCardNo,
            AadharCardNo = d.AadharCardNo
        };
    }
}
