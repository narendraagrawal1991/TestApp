using OfficeApp.DTOs;

namespace OfficeApp.Services.Interfaces
{
    public interface IPartyService
    {
        Task<List<PartyDto>> GetAllPartiesAsync();
        Task<PartyDto?> GetPartyByIdAsync(int id);
        /// <summary>
        /// Creates or updates a party. Returns (success, errorField, errorMessage).
        /// </summary>
        Task<(bool Success, string? ErrorField, string? ErrorMessage)> CreateOrUpdatePartyAsync(
            PartyDto dto, int transportId, int userId);
        Task DeletePartyAsync(int id);
    }
}
