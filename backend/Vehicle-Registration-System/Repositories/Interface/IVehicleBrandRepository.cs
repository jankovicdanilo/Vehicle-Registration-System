using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleBrandRepository
    {
        Task<List<VehicleBrand>> ListByTypeId(Guid id);

        Task<VehicleBrand> AddAsync(VehicleBrand vehicleBrand);

        Task<VehicleBrand?> GetByIdAsync(Guid id);

        Task<VehicleBrand?> DeleteAsync(Guid id);

        Task<VehicleBrand?> UpdateAsync(VehicleBrand vehicleBrand);
    }
}
