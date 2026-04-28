using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IWebHostEnvironment env, ILogger<FileUploadService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder, int entityId, string docType)
        {
            var fileName = docType + "_" + Path.GetFileName(file.FileName);
            var directoryPath = Path.Combine(_env.WebRootPath, "Uploads", subFolder, entityId.ToString());

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var fullPath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/Uploads/{subFolder}/{entityId}/{fileName}";
            _logger.LogInformation("File saved: {Path}", relativePath);
            return relativePath;
        }

        public void DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("File deleted: {Path}", relativePath);
            }
        }
    }
}
