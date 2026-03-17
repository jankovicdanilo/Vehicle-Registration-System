using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleBrandRepository : IRepositoryBase<VehicleBrand>
    {
        Task<List<VehicleBrand>> ListByTypeId(Guid id);

        Task<VehicleBrand?> UpdateAsync(VehicleBrand vehicleBrand);
    }
}
