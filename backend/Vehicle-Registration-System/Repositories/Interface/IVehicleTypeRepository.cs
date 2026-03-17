using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleTypeRepository : IRepositoryBase<VehicleType>
    {
        Task<List<VehicleType>> GetAllAsync();

        Task<VehicleType?> UpdateAsync(VehicleType vehicleType);
    }
}
