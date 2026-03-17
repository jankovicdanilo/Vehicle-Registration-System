using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleRepository : IRepositoryBase<Vehicle>
    {
        Task<(List<Vehicle> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageSize = 1000, int pageNumber = 1);

        Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle);

        Task<bool> IsVehicleModelValidAsync(Guid modelId, Guid brandId, Guid typeId);
    }
}
