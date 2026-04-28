using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;

namespace OfficeApp.Repositories.Implementations
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetAllAsync()
            => await _context.Companies.ToListAsync();
        public async Task<List<Company>> GetAllAsync(List<int> companyIds)
           => await _context.Companies.Where(c => companyIds.Contains(c.CompanyId)).ToListAsync();


        public async Task<Company?> GetByIdAsync(int id)
            => await _context.Companies.FindAsync(id);

        public async Task AddAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Update(company);
            await _context.SaveChangesAsync();
            //var entry = _context.Entry(company);
            //entry.State = EntityState.Modified;
            //entry.Property(x => x.createdate).IsModified = false;
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}
