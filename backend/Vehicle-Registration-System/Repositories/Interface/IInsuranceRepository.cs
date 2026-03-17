using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Common;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IInsuranceRepository : IRepositoryBase<Insurance>
    {
        Task<List<Insurance>> GetAllAsync();

        Task<Insurance?> UpdateAsync(Insurance request);
    }
}
