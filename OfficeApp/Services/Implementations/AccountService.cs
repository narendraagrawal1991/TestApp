using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository repo, ILogger<AccountService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<LoginEntity?> ValidateLoginAsync(LoginDto dto)
        {
            var user = await _repo.GetUserAsync(dto.Username, dto.Password);

            if (user != null)
                _logger.LogInformation("User '{Username}' logged in successfully", dto.Username);
            else
                _logger.LogWarning("Failed login attempt for username '{Username}'", dto.Username);

            return user;
        }
    }
}
