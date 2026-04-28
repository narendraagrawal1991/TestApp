using OfficeApp.Models;

namespace OfficeApp.Repositories.Interfaces
{
    public interface ILRFormRepository
    {
        Task<List<LRForm>> GetAllAsync();

        Task<List<LRForm>> GetAllAsync(int transportId);
        Task<LRForm?> GetByIdAsync(int id);
        Task<LRForm?> GetByIdWithDocumentsAsync(int transportId,int id);
        Task AddAsync(LRForm form);
        Task UpdateAsync(LRForm form);
        Task DeleteAsync(LRForm form);
        Task<bool> ExistsByLRNoAsync(string lrNo, int transportId, int? excludeId = null);
        Task<List<ItemEntry>> GetItemEntriesByLRIdAsync(int lrId);
        Task AddItemEntryAsync(ItemEntry item);
        Task RemoveItemEntriesAsync(int lrId);
        Task<List<ItemCharges>> GetItemChargesByLRIdAsync(int lrId);
    }
}
