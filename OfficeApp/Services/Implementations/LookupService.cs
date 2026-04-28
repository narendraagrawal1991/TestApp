using Microsoft.EntityFrameworkCore;
using OfficeApp.Data;
using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class LookupService : ILookupService
    {
        private readonly ApplicationDbContext _context;

        public LookupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AutoCompleteDto>> SearchPartiesAsync(string term)
        {
            return await _context.Parties
                .Where(t => t.PartyName.ToLower().StartsWith(term.ToLower()))
                .Select(t => new AutoCompleteDto { Id = t.PartyId, Label = t.PartyName })
                .ToListAsync();
        }

        public async Task<List<AutoCompleteDto>> SearchVehiclesAsync(string term)
        {
            return await _context.VehicleOwners
                .Where(t => t.VehicleNo.ToLower().StartsWith(term.ToLower()))
                .Select(t => new AutoCompleteDto { Id = t.Id, Label = t.VehicleNo })
                .ToListAsync();
        }

        public async Task<List<KeyValueClass>> GetVehicleOwnerLookupAsync(List<int> array)
        {
            if (array.Any())
            {
                return await _context.VehicleOwners.Where(t => array.Any(v => v == t.Id))
                .Select(t => new KeyValueClass { Id = t.Id, Name = t.VehicleNo })
                .ToListAsync();
            }
            return await _context.VehicleOwners
                .Select(t => new KeyValueClass { Id = t.Id, Name = t.VehicleNo })
                .ToListAsync();
        }

        public async Task<List<KeyValueClass>> GetPartyLookupAsync(List<int> array)
        {
            if (array.Any())
            {
                return await _context.Parties.Where(t => array.Any(v => v == t.PartyId))
            .Select(t => new KeyValueClass { Id = t.PartyId, Name = t.PartyName })
            .ToListAsync();
            }
            return await _context.Parties
            .Select(t => new KeyValueClass { Id = t.PartyId, Name = t.PartyName })
            .ToListAsync();
        }
    }
}
