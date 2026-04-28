using OfficeApp.Models;

namespace OfficeApp.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        // TODO: Replace plain-text password comparison with hashed password verification
        Task<LoginEntity?> GetUserAsync(string username, string password);
    }
}
