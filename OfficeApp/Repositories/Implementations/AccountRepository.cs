using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO: Replace plain-text password comparison with hashed password verification
        public async Task<LoginEntity?> GetUserAsync(string username, string password)
        {
            return await _context.LoginEntitys
                .FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);
        }
    }
}
