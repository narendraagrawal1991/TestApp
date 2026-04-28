using Microsoft.AspNetCore.Mvc;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    public class AutoCompleteController : Controller
    {
        private readonly ILookupService _lookupService;

        public AutoCompleteController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet]
        public async Task<JsonResult> Consignor(string term)
        {
            var result = await _lookupService.SearchPartiesAsync(term);
            return Json(result.Select(r => new { id = r.Id, label = r.Label }));
        }

        [HttpGet]
        public async Task<JsonResult> Consignee(string term)
        {
            var result = await _lookupService.SearchPartiesAsync(term);
            return Json(result.Select(r => new { id = r.Id, label = r.Label }));
        }

        [HttpGet]
        public async Task<JsonResult> Vehicle(string term)
        {
            var result = await _lookupService.SearchVehiclesAsync(term);
            return Json(result.Select(r => new { id = r.Id, label = r.Label }));
        }
    }
}
