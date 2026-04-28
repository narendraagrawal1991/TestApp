using Microsoft.AspNetCore.Mvc;
using OfficeApp.DTOs;
using OfficeApp.Filters;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return View(companies);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View(new CompanyDto());
        }

        // POST: Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyDto company)
        {
            if (!string.IsNullOrEmpty(company.CompanyName))
            {
                await _companyService.CreateCompanyAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var company = await _companyService.GetCompanyByIdAsync(id.Value);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyDto company)
        {
            if (id != company.CompanyId) return NotFound();
            if (!string.IsNullOrEmpty(company.CompanyName))
            {
                await _companyService.UpdateCompanyAsync(id, company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            await _companyService.DeleteCompanyAsync(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
