using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IRegistrationVehicleRepository
    {
        Task<Registration> AddRegistrationAsync(Registration request);

        Task<(List<Registration> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null,int pageNumber =1, int pageSize = 1000);

        Task<Registration?> GetByIdAsync(Guid id);

        Task<Registration?> DeleteAsync(Guid id);

        Task<Registration?> UpdateAsync(Registration request);
    }
}
