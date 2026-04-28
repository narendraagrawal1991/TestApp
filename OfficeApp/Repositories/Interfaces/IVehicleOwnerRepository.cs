using OfficeApp.Models;

namespace OfficeApp.Repositories.Interfaces
{
    public interface IVehicleOwnerRepository
    {
        Task<List<VehicleOwner>> GetAllAsync();
        Task<VehicleOwner?> GetByIdAsync(int id);
        Task<VehicleOwner?> GetByIdWithDocumentsAsync(int id);
        Task AddAsync(VehicleOwner owner);
        Task UpdateAsync(VehicleOwner owner);
        Task DeleteAsync(VehicleOwner owner);
        Task<bool> ExistsByVehicleNoAsync(string vehicleNo, int? excludeId = null);
    }
}
