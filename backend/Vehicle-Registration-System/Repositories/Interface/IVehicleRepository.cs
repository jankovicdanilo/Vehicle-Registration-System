using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleRepository : IRepositoryBase<Vehicle>
    {
        Task<(List<VehicleListItemDto> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageSize = 10, int pageNumber = 1);

        Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle);

        Task<bool> IsVehicleModelValidAsync(Guid modelId, Guid brandId, Guid typeId);
    }
}
