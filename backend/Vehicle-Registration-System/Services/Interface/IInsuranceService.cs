using VehicleRegistrationSystem.Models.DTO.Insurance;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IInsuranceService
    {

        Task<Result<bool>> ValidateGetInsuranceByIdAsync(Guid id);

        Task<Result<bool>> ValidateDeleteAsync(Guid id);

        Task<Result<bool>> ValidateUpdateAsync(UpdateInsuranceRequestDto request);

        Task<Result<InsuranceDto>> CreateInsuranceAsync(CreateInsuranceRequestDto request);

        Task<Result<List<InsuranceDto>>> GetAllAsync();

        Task<Result<InsuranceDto>> GetInsuranceByIdAsync(Guid id);

        Task<Result<InsuranceDto>> DeleteAsync(Guid id);

        Task<Result<InsuranceDto>> UpdateAsync(UpdateInsuranceRequestDto request);
    }
}


