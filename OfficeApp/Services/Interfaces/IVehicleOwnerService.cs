using Microsoft.AspNetCore.Http;
using OfficeApp.DTOs;

namespace OfficeApp.Services.Interfaces
{
    public interface IVehicleOwnerService
    {
        Task<List<VehicleOwnerDto>> GetAllOwnersAsync();
        Task<VehicleOwnerDto?> GetOwnerByIdWithDocumentsAsync(int id);
        /// <summary>
        /// Creates or updates an owner. Returns (success, ownerId, errorField, errorMessage).
        /// </summary>
        Task<(bool Success, int OwnerId, string? ErrorField, string? ErrorMessage)> CreateOrUpdateOwnerAsync(
            VehicleOwnerDto dto, int transportId, int userId);
        Task DeleteOwnerWithDocumentsAsync(int id);
        Task UploadDocumentAsync(int ownerId, string docType, IFormFile file);
        Task<int> DeleteDocumentAsync(int docId);
    }
}
