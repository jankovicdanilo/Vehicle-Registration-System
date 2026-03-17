using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IRegistrationVehicleRepository : IRepositoryBase<Registration>
    {
        Task<(List<Registration> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null,int pageNumber =1, int pageSize = 1000);

        Task<Registration?> UpdateAsync(Registration request);
    }
}
