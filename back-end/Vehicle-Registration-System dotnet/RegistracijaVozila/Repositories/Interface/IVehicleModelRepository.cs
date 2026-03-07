using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleModelRepository
    {
        Task<List<VehicleModel>> ListByBrandId(Guid id);

        Task<VehicleModel> AddAsync(VehicleModel vehicleModel);

        Task<VehicleModel?> DeleteAsync(Guid id);

        Task<VehicleModel?> GetByIdAsync(Guid id);

        Task<VehicleModel?> UpdateAsync(VehicleModel vehicleModel);
    }
}
