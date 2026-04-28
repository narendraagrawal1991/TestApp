using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleOwnerApiController : Controller
    {
        private readonly IVehicleOwnerService _ownerService;
        private readonly ILogger<VehicleOwnerController> _logger;

        public VehicleOwnerApiController(IVehicleOwnerService ownerService, ILogger<VehicleOwnerController> logger)
        {
            _ownerService = ownerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _ownerService.GetAllOwnersAsync();
            return Ok(owners);
        }

        [HttpGet("document-types")]
        public IActionResult GetDocumentTypes()
        {
            var docs = new List<string>
        {
            "RC",
            "PanCard",
            "Declaration",
            "Driving Lic",
            "Bank Details",
            "Aadharcard No"
        };
            return Ok(docs);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] VehicleOwnerDto dto)
        {
            int transportId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            var (success, ownerId, errorField, errorMessage) =
                await _ownerService.CreateOrUpdateOwnerAsync(dto, transportId, userId);

            if (!success)
            {
                return BadRequest(new
                {
                    field = errorField,
                    message = errorMessage
                });
            }

            return Ok(new { ownerId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _ownerService.GetOwnerByIdWithDocumentsAsync(id);

            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpPost("upload-document")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required");

            await _ownerService.UploadDocumentAsync(request.OwnerId, request.DocumentType, request.File);

            return Ok(new { message = "Uploaded successfully" });
        }

        [HttpDelete("document/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var ownerId = await _ownerService.DeleteDocumentAsync(id);

            return Ok(new { ownerId });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(int id)
        {
            await _ownerService.DeleteOwnerWithDocumentsAsync(id);

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
