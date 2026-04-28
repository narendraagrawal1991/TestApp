using OfficeApp.DTOs;
using OfficeApp.Models;

namespace OfficeApp.Services.Interfaces
{
    public interface ILookupService
    {
        Task<List<AutoCompleteDto>> SearchPartiesAsync(string term);
        Task<List<AutoCompleteDto>> SearchVehiclesAsync(string term);
        Task<List<KeyValueClass>> GetVehicleOwnerLookupAsync(List<int> array);
        Task<List<KeyValueClass>> GetPartyLookupAsync(List<int> array);
    }
}
