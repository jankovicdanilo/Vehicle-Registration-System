using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Repositories.Interface
{
    public interface IInsuranceRepository
    {
        Task<Insurance> CreateInsuranceAsync(Insurance insurance);

        Task<List<Insurance>> GetAllAsync();

        Task<Insurance?> GetInsuranceByIdAsync(Guid id);

        Task<Insurance?> DeleteAsync(Guid id);

        Task<Insurance?> UpdateAsync(Insurance request);
    }
}
