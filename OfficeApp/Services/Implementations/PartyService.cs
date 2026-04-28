using OfficeApp.DTOs;
using OfficeApp.Models;
using OfficeApp.Repositories.Interfaces;
using OfficeApp.Services.Interfaces;

namespace OfficeApp.Services.Implementations
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _repo;
        private readonly ILogger<PartyService> _logger;

        public PartyService(IPartyRepository repo, ILogger<PartyService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<PartyDto>> GetAllPartiesAsync()
        {
            var parties = await _repo.GetAllAsync();
            return parties.Select(MapToDto).ToList();
        }

        public async Task<PartyDto?> GetPartyByIdAsync(int id)
        {
            var party = await _repo.GetByIdAsync(id);
            return party == null ? null : MapToDto(party);
        }

        public async Task<(bool Success, string? ErrorField, string? ErrorMessage)> CreateOrUpdatePartyAsync(
            PartyDto dto, int transportId, int userId)
        {
            if (dto.PartyId == 0)
            {
                // Create
                if (await _repo.ExistsByNameAsync(dto.PartyName))
                    return (false, "PartyName", "Party Name already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.createdate = DateTime.Now;
                entity.Modifydate = DateTime.Now;

                await _repo.AddAsync(entity);
                _logger.LogInformation("Party '{Name}' created with ID {Id}", entity.PartyName, entity.PartyId);
            }
            else
            {
                // Update
                if (await _repo.ExistsByNameAsync(dto.PartyName, dto.PartyId))
                    return (false, "PartyName", "Party Name already exists.");

                var entity = MapToEntity(dto);
                entity.TransportID = transportId;
                entity.UserID = userId;
                entity.Modifydate = DateTime.Now;

                await _repo.UpdateAsync(entity);
                _logger.LogInformation("Party ID {Id} updated", entity.PartyId);
            }

            return (true, null, null);
        }

        public async Task DeletePartyAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity != null)
            {
                await _repo.DeleteAsync(entity);
                _logger.LogInformation("Party ID {Id} deleted", id);
            }
        }

        private static PartyDto MapToDto(Party p) => new()
        {
            PartyId = p.PartyId,
            PartyName = p.PartyName,
            GSTNo = p.GSTNo,
            ContactNo1 = p.ContactNo1,
            ContactNo2 = p.ContactNo2,
            Address1 = p.Address1,
            Address2 = p.Address2,
            City = p.City,
            District = p.District,
            State = p.State,
            Pincode = p.Pincode
        };

        private static Party MapToEntity(PartyDto d) => new()
        {
            PartyId = d.PartyId,
            PartyName = d.PartyName,
            GSTNo = d.GSTNo,
            ContactNo1 = d.ContactNo1,
            ContactNo2 = d.ContactNo2,
            Address1 = d.Address1,
            Address2 = d.Address2,
            City = d.City,
            District = d.District,
            State = d.State,
            Pincode = d.Pincode
        };
    }
}
