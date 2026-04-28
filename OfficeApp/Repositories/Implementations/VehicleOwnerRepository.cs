using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class VehicleOwnerRepository : IVehicleOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleOwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleOwner>> GetAllAsync()
            => await _context.VehicleOwners.ToListAsync();

        public async Task<VehicleOwner?> GetByIdAsync(int id)
            => await _context.VehicleOwners.FindAsync(id);

        public async Task<VehicleOwner?> GetByIdWithDocumentsAsync(int id)
            => await _context.VehicleOwners
                .Include(v => v.Documents)
                .FirstOrDefaultAsync(v => v.Id == id);

        public async Task AddAsync(VehicleOwner owner)
        {
            _context.VehicleOwners.Add(owner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleOwner owner)
        {
            //_context.Entry(owner).State = EntityState.Modified;
            
            //await _context.SaveChangesAsync();

            var entry = _context.Entry(owner);
            entry.State = EntityState.Modified;
            entry.Property(x => x.createdate).IsModified = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VehicleOwner owner)
        {
            _context.VehicleOwners.Remove(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByVehicleNoAsync(string vehicleNo, int? excludeId = null)
        {
            return await _context.VehicleOwners.AnyAsync(v =>
                v.VehicleNo.ToLower() == vehicleNo.ToLower()
                && (!excludeId.HasValue || v.Id != excludeId.Value));
        }
    }
}
