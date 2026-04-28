using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using OfficeApp.DTOs;
using OfficeApp.Filters;
using OfficeApp.Models;
using OfficeApp.Services.Interfaces;
using System.Diagnostics;

namespace OfficeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICompanyService _companyService;
        public HomeController(ILogger<HomeController> logger, ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;   
        }

        [ServiceFilter(typeof(AuthenticationFilter))]
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.GetString("UserName");
            ViewBag.User = user;
            ViewBag.selectedCompanyId = HttpContext.Session.GetInt32("CompanyId") ?? 0;
            List<CompanyDto> companies = await GetCompanyDTO(HttpContext.Session.GetString("CompanyIds"));
            var model = new VehicleOwnerViewModel
            {
                Owner = new VehicleOwner(),
                DocumentTypes = GetDocumentTypes(companies),
                UploadedDocuments = new List<DocumentUpload>()
            };
            model.Owner.Id = 0;
            _logger.LogInformation("Home page accessed by user '{User}'", user);
            return View(model);
        }

        private async Task<List<CompanyDto>> GetCompanyDTO(string CompanyIds)
        {
            List<int> companyIds = CompanyIds.Split(",").Select(int.Parse).ToList();
            List<CompanyDto> companies = await _companyService.GetAllCompaniesAsync();
            companies = companies.Where(c => companyIds.Contains(c.CompanyId)).ToList();
            return companies;
        }

        private static List<SelectListItem> GetDocumentTypes(List<CompanyDto> companies)
        {
            List<SelectListItem> selectCopmany=new List<SelectListItem>();
            foreach (var item in companies)
            {
                selectCopmany.Add(new SelectListItem { Text = item.CompanyName, Value = item.CompanyId.ToString() });
            }
            return selectCopmany;
        }

        [HttpPost]
        public IActionResult SelectCompany(string selectedCompanyId)
        {
            HttpContext.Session.SetInt32("CompanyId", Convert.ToInt32(selectedCompanyId));
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
