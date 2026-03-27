using VehicleRegistrationSystem.Models.DTO.InsurancePricing;
using VehicleRegistrationSystem.Results;

namespace VehicleRegistrationSystem.Services.Interface
{
    public interface IInsurancePricingService
    {
        Task<Result<bool>> ValidateCreateAsync(CreateInsurancePriceRequestDto request);

        Task<Result<bool>> ValidateGetAllAsync();

        Task<Result<bool>> ValidateGetByIdAsync(Guid id);

        Task<Result<bool>> ValidateGetByInsuranceIdAsync(Guid id);

        Task<Result<InsurancePriceDto>> CreateAsync(CreateInsurancePriceRequestDto request);

        Task<Result<List<InsurancePriceDto>>> GetAllAsync();

        Task<Result<InsurancePriceDto>> GetByIdAsync(Guid id);

        Task<Result<InsurancePriceDto>> GetByInsuranceIdAsync(Guid id);
    }
}
