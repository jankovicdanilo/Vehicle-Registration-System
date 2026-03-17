using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleModelRepository : IRepositoryBase<VehicleModel>
    {
        Task<List<VehicleModel>> ListByBrandId(Guid id);

        Task<VehicleModel?> UpdateAsync(VehicleModel vehicleModel);
    }
}
