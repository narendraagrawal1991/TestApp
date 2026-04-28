using Microsoft.AspNetCore.Http;
using OfficeApp.DTOs;

namespace OfficeApp.Services.Interfaces
{
    public interface ILRFormService
    {
        Task<List<LRFormDto>> GetAllLRFormsAsync();
        Task<List<LRFormDto>> GetAllLRFormsAsync(int transportId);
        Task<LRFormDto?> GetLRFormForEditAsync(int id, int transportId);
        /// <summary>
        /// Creates or updates an LR Form. Returns (success, errorField, errorMessage).
        /// </summary>
        Task<(bool Success, string? ErrorField, string? ErrorMessage)> CreateOrUpdateLRFormAsync(
            LRFormDto dto, int transportId, int userId);
        Task DeleteLRFormAsync(int id, int transportId);
        Task UploadDocumentAsync(int lrId, string docType, IFormFile file);
        Task<int> DeleteDocumentAsync(int docId);
        Task AddChargeAsync(int lrId, string type, int amount);
        Task<int> DeleteChargeAsync(int chargeId);
    }
}
