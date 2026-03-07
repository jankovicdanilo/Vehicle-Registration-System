using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IVehicleRepository
    {
        Task<Vehicle> AddAsync(Vehicle vehicle);

        Task<(List<Vehicle> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null, int pageSize = 1000, int pageNumber = 1);

        Task<Vehicle?> GetVehicleByIdAsync(Guid id);

        Task<Vehicle?> DeleteVehicleAsync(Guid id);

        Task<Vehicle?> UpdateVehicleAsync(Vehicle vehicle);
    }
}
