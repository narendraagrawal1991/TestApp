using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Services.Implementations;
using OfficeApp.Services.Interfaces;
using System.Text;

namespace OfficeApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        private readonly ICompanyService _companyService;

        public AccountApiController(IConfiguration config, IAccountService accountService, ILogger<AccountController> logger, ICompanyService companyService)
        {
            _config = config;
            _accountService = accountService;
            _logger = logger;
            _companyService = companyService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto model)
        {
            var user = await _accountService.ValidateLoginAsync(model);
            if (user != null)
            {
                var token = GenerateToken(user.UserName, user.UserId, user.CompanyId); // userId, companyId

                return Ok(new LoginModelResponse()
                {
                    token = token,
                    userId = user.UserId,
                    Username = user.UserName,
                    companyIds = await GetCompanyDTO(user.CompanyId),
                });
            }
            return Unauthorized();
        }
        private async Task<List<CompanyDto>> GetCompanyDTO(string CompanyIds)
        {
            List<int> companyIds = CompanyIds.Split(",").Select(int.Parse).ToList();
            List<CompanyDto> companies = await _companyService.GetAllCompaniesAsync();
            companies = companies.Where(c => companyIds.Contains(c.CompanyId)).ToList();
            return companies;
        }
        private string GenerateToken(string username, int userId, string companyId)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new System.Security.Claims.Claim("username", username),
            new System.Security.Claims.Claim("UserId", userId.ToString()),
            new System.Security.Claims.Claim("CompanyId", companyId.ToString())
        };

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
