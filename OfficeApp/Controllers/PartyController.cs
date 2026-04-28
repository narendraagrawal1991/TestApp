using Microsoft.AspNetCore.Mvc;
using OfficeApp.DTOs;
using OfficeApp.Filters;
using OfficeApp.Helpers;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    [ServiceFilter(typeof(AuthenticationFilter))]
    public class PartyController : Controller
    {
        private readonly IPartyService _partyService;
        private readonly ILogger<PartyController> _logger;

        public PartyController(IPartyService partyService, ILogger<PartyController> logger)
        {
            _partyService = partyService;
            _logger = logger;
        }

        // GET: Party
        public async Task<IActionResult> Index()
        {
            var parties = await _partyService.GetAllPartiesAsync();
            return View(parties);
        }

        // GET: Party/Create
        public IActionResult Create()
        {
            ViewBag.StateList = IndianStatesHelper.GetIndianStates();
            return View(new PartyDto());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartyDto party)
        {
            ViewBag.StateList = IndianStatesHelper.GetIndianStates();
            int transportId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            if (!string.IsNullOrEmpty(party.PartyName))
            {
                var (success, errorField, errorMessage) =
                    await _partyService.CreateOrUpdatePartyAsync(party, transportId, userId);

                if (!success)
                {
                    ModelState.AddModelError(errorField!, errorMessage!);
                    return View(party);
                }

                return RedirectToAction("Index");
            }
            return View(party);
        }

        // GET: Party/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var party = await _partyService.GetPartyByIdAsync(id.Value);
            if (party == null) return NotFound();

            ViewBag.StateList = IndianStatesHelper.GetIndianStates();
            return View("Create", party); // reuse the Create view
        }

        // GET: Party/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var party = await _partyService.GetPartyByIdAsync(id.Value);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _partyService.DeletePartyAsync(id);
            return RedirectToAction("Index");
        }
    }
}
