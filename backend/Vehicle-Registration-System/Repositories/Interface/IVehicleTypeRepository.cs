using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleTypeRepository
    {
        Task<List<VehicleType>> GetAllAsync();

        Task<VehicleType> AddAsync(VehicleType vehicleType);

        Task<VehicleType?> GetByIdAsync(Guid id);

        Task<VehicleType?> DeleteAsync(Guid id);

        Task<VehicleType?> UpdateAsync(VehicleType vehicleType);
    }
}
