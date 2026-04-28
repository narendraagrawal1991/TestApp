using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class PartyRepository : IPartyRepository
    {
        private readonly ApplicationDbContext _context;

        public PartyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Party>> GetAllAsync()
            => await _context.Parties.ToListAsync();

        public async Task<Party?> GetByIdAsync(int id)
            => await _context.Parties.FindAsync(id);

        public async Task AddAsync(Party party)
        {
            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Party party)
        {
            //_context.Entry(party).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            var entry = _context.Entry(party);
            entry.State = EntityState.Modified;
            entry.Property(x => x.createdate).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Party party)
        {
            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _context.Parties.AnyAsync(p =>
                p.PartyName.ToLower() == name.ToLower()
                && (!excludeId.HasValue || p.PartyId != excludeId.Value));
        }
    }
}
