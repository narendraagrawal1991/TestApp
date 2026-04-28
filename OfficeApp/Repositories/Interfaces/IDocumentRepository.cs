using OfficeApp.Models;

namespace OfficeApp.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        // Vehicle Owner Documents
        Task<DocumentUpload?> GetDocumentByIdAsync(int id);
        Task AddDocumentAsync(DocumentUpload doc);
        Task RemoveDocumentAsync(DocumentUpload doc);

        // LR Form Documents
        Task<LRDocumentUpload?> GetLRDocumentByIdAsync(int id);
        Task AddLRDocumentAsync(LRDocumentUpload doc);
        Task RemoveLRDocumentAsync(LRDocumentUpload doc);

        // Item Charges
        Task<ItemCharges?> GetItemChargeByIdAsync(int id);
        Task AddItemChargeAsync(ItemCharges charge);
        Task RemoveItemChargeAsync(ItemCharges charge);
    }
}
