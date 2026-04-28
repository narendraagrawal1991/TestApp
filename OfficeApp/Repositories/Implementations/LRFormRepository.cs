using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class LRFormRepository : ILRFormRepository
    {
        private readonly ApplicationDbContext _context;

        public LRFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LRForm>> GetAllAsync()
            => await _context.LRForms.ToListAsync();

        public async Task<List<LRForm>> GetAllAsync(int transportId)
            => await _context.LRForms.Where(t => t.TransportID == transportId).ToListAsync();

        public async Task<LRForm?> GetByIdAsync(int id)
            => await _context.LRForms.FindAsync(id);

        public async Task<LRForm?> GetByIdWithDocumentsAsync(int id, int transportId)
            => await _context.LRForms
                .Include(l => l.Documents)
                .FirstOrDefaultAsync(l => l.Id == id && l.TransportID == transportId);

        public async Task AddAsync(LRForm form)
        {
            _context.LRForms.Add(form);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LRForm form)
        {
            //_context.Entry(form).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            var entry = _context.Entry(form);
            entry.State = EntityState.Modified;
            entry.Property(x => x.createdate).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LRForm form)
        {
            _context.LRForms.Remove(form);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByLRNoAsync(string lrNo, int transportId, int? excludeId = null)
        {
            return await _context.LRForms.AnyAsync(l =>
                l.TransportID == transportId && l.LRNo.ToLower() == lrNo.ToLower()
                && (!excludeId.HasValue || l.Id != excludeId.Value));
        }

        public async Task<List<ItemEntry>> GetItemEntriesByLRIdAsync(int lrId)
            => await _context.ItemEntrys.Where(i => i.LRID == lrId).ToListAsync();

        public async Task AddItemEntryAsync(ItemEntry item)
        {
            _context.ItemEntrys.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemEntriesAsync(int lrId)
        {
            var items = await _context.ItemEntrys.Where(i => i.LRID == lrId).ToListAsync();
            _context.ItemEntrys.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ItemCharges>> GetItemChargesByLRIdAsync(int lrId)
            => await _context.ItemCharges.Where(c => c.LRFormsId == lrId).ToListAsync();
    }
}
