using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Insurance;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository insuranceRepository;
        private readonly IMapper mapper;

        public InsuranceService(IInsuranceRepository insuranceRepository, IMapper mapper)
        {
            this.insuranceRepository = insuranceRepository;
            this.mapper = mapper;
        }

        public async Task<Result<InsuranceDto>> CreateInsuranceAsync(CreateInsuranceRequestDto request)
        {
            var insuranceDomain = mapper.Map<Insurance>(request);

            insuranceDomain = await insuranceRepository.AddAsync(insuranceDomain);

            var response = mapper.Map<InsuranceDto>(insuranceDomain);

            return Result<InsuranceDto>.Ok(response, "New insurance has been created!");
        }

        public async Task<Result<List<InsuranceDto>>> GetAllAsync()
        {
            var insuranceDomainList = await insuranceRepository.GetAllAsync();

            var response = mapper.Map<List<InsuranceDto>>(insuranceDomainList);

            return Result<List<InsuranceDto>>.Ok(response);
        }

        public async Task<Result<bool>> ValidateGetInsuranceByIdAsync(Guid id)
        {
            if(!await insuranceRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail("INVALID_INSURANCE_ID","Insurance with the" +
                    $" id {id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsuranceDto>> GetInsuranceByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetInsuranceByIdAsync(id);

            if (!validationResult.Success)
            {
                return Result<InsuranceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insuranceDomain = await insuranceRepository.GetByIdAsync(id);

            var response = mapper.Map<InsuranceDto>(insuranceDomain);

            return Result<InsuranceDto>.Ok(response);
        }

        public async Task<Result<bool>> ValidateDeleteAsync(Guid id)
        {
            if (!await insuranceRepository.ExistsAsync(x => x.Id == id))
            {
                return Result<bool>.Fail($"INVALID_INSURANCE_ID","Insurance with the" +
                    $" id {id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsuranceDto>> DeleteAsync(Guid id)
        {
            var validationResult = await ValidateGetInsuranceByIdAsync(id);

            if (!validationResult.Success)
            {
                return Result<InsuranceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insuranceDomain = await insuranceRepository.DeleteAsync(id);

            var response = mapper.Map<InsuranceDto>(insuranceDomain);

            return Result<InsuranceDto>.Ok(response, "Insurance has been deleted");
        }

        public async Task<Result<bool>> ValidateUpdateAsync(UpdateInsuranceRequestDto request)
        {
            if (!await insuranceRepository.ExistsAsync(x => x.Id == request.Id))
            {
                return Result<bool>.Fail($"INVALID_INSURANCE_ID", "Insurance with the" +
                    $" id {request.Id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsuranceDto>> UpdateAsync(UpdateInsuranceRequestDto request)
        {
            var validationResult = await ValidateUpdateAsync(request);

            if (!validationResult.Success)
            {
                return Result<InsuranceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insuranceDomain = mapper.Map<Insurance>(request);

            var updatedInsurance = await insuranceRepository.UpdateAsync(insuranceDomain);

            var response = mapper.Map<InsuranceDto>(updatedInsurance);

            return Result<InsuranceDto>.Ok(response, "Insurance has been updated!");
        }
    }
}





