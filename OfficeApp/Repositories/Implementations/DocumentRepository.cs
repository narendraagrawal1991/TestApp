using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vehicle Owner Documents
        public async Task<DocumentUpload?> GetDocumentByIdAsync(int id)
            => await _context.DocumentUploads.FindAsync(id);

        public async Task AddDocumentAsync(DocumentUpload doc)
        {
            _context.DocumentUploads.Add(doc);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDocumentAsync(DocumentUpload doc)
        {
            _context.DocumentUploads.Remove(doc);
            await _context.SaveChangesAsync();
        }

        // LR Form Documents
        public async Task<LRDocumentUpload?> GetLRDocumentByIdAsync(int id)
            => await _context.LRDocumentUploads.FindAsync(id);

        public async Task AddLRDocumentAsync(LRDocumentUpload doc)
        {
            _context.LRDocumentUploads.Add(doc);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLRDocumentAsync(LRDocumentUpload doc)
        {
            _context.LRDocumentUploads.Remove(doc);
            await _context.SaveChangesAsync();
        }

        // Item Charges
        public async Task<ItemCharges?> GetItemChargeByIdAsync(int id)
            => await _context.ItemCharges.FindAsync(id);

        public async Task AddItemChargeAsync(ItemCharges charge)
        {
            _context.ItemCharges.Add(charge);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemChargeAsync(ItemCharges charge)
        {
            _context.ItemCharges.Remove(charge);
            await _context.SaveChangesAsync();
        }
    }
}
