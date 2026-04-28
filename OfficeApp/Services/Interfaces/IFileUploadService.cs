using Microsoft.AspNetCore.Http;

namespace OfficeApp.Services.Interfaces
{
    public interface IFileUploadService
    {
        /// <summary>
        /// Saves a file to wwwroot/Uploads/{subFolder}/{entityId}/{docType}_{fileName}.
        /// Returns the relative URL path.
        /// </summary>
        Task<string> SaveFileAsync(IFormFile file, string subFolder, int entityId, string docType);

        /// <summary>
        /// Deletes a file from the wwwroot by its relative path.
        /// </summary>
        void DeleteFile(string relativePath);
    }
}
