using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeApp.DTOs;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.ValidateLoginAsync(model);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("CompanyIds", user.CompanyId);
                    var companyIds = user.CompanyId.Split(",").Select(int.Parse).ToList();
                    HttpContext.Session.SetInt32("CompanyId", companyIds.First());

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Invalid username or password";
                _logger.LogWarning("Failed login attempt for '{Username}'", model.Username);
            }

            return View(model);
        }


        public IActionResult Logout()
        {
            var user = HttpContext.Session.GetString("UserName");
            HttpContext.Session.Clear();
            _logger.LogInformation("User '{User}' logged out", user);
            return RedirectToAction("Login", "Account");
        }
    }
}