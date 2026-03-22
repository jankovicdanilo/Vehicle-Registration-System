using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Registration;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IRegistrationVehicleRepository : IRepositoryBase<Registration>
    {
        Task<(List<RegistrationVehicleListItemDto> Items, int TotalCount)> GetAllAsync
            (string? searchQuery = null,int pageNumber =1, int pageSize = 10);

        Task<Registration?> UpdateAsync(Registration request);
    }
}
