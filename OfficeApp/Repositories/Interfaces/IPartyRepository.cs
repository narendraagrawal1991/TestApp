using OfficeApp.Models;

namespace OfficeApp.Repositories.Interfaces
{
    public interface IPartyRepository
    {
        Task<List<Party>> GetAllAsync();
        Task<Party?> GetByIdAsync(int id);
        Task AddAsync(Party party);
        Task UpdateAsync(Party party);
        Task DeleteAsync(Party party);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    }
}
