using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class InsurancePricingService : IInsurancePricingService
    {
        private readonly IInsurancePricingRepository insurancePricingRepository;
        private readonly IMapper mapper;
        private readonly VehicleRegistrationDbContext appDbContext;

        public InsurancePricingService(IInsurancePricingRepository insurancePricingRepository, 
            IMapper mapper, VehicleRegistrationDbContext appDbContext)
        {
            this.insurancePricingRepository = insurancePricingRepository;
            this.mapper = mapper;
            this.appDbContext = appDbContext;
        }

        public async Task<RepositoryResult<bool>> ValidateCreateAsync(CreateInsurancePriceRequestDto request)
        {
            if(!await appDbContext.Insurances.AnyAsync(x=>x.Id == request.InsuranceId))
            {
                return RepositoryResult<bool>.Fail($"INVALID_Insurance_ID: Insurance" +
                    $" with the id {request.InsuranceId} not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<InsurancePriceDto>> CreateAsync(CreateInsurancePriceRequestDto request)
        {
            var validationResult = await ValidateCreateAsync(request);

            if(!validationResult.Success)
            {
                return RepositoryResult<InsurancePriceDto>.Fail(validationResult.Message);
            }

            var insurancePriceDomain = mapper.Map<InsurancePrice>(request);

            insurancePriceDomain = await insurancePricingRepository.CreateAsync(insurancePriceDomain);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return RepositoryResult<InsurancePriceDto>.Ok(response, "New insurance price has been created!");
        }

        public Task<RepositoryResult<bool>> ValidateGetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RepositoryResult<List<InsurancePriceDto>>> GetAllAsync()
        {
            var insurancePricesDomainList = await insurancePricingRepository.GetAllAsync();

            var response = mapper.Map<List<InsurancePriceDto>>(insurancePricesDomainList);

            return RepositoryResult<List<InsurancePriceDto>>.Ok(response);
        }

        public async Task<RepositoryResult<bool>> ValidateGetByIdAsync(Guid id)
        {
            if(!await appDbContext.InsurancePrices.AnyAsync(x=>x.Id == id))
            {
                return RepositoryResult<bool>.Fail($"INSURANCE_PRICE_ID_INVALID: Insurance price with the" +
                    $" id {id} not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<InsurancePriceDto>> GetByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByIdAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<InsurancePriceDto>.Fail(validationResult.Message);
            }

            var insurancePriceDomain = await insurancePricingRepository.GetByIdAsync(id);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return RepositoryResult<InsurancePriceDto>.Ok(response);
        }

        public async Task<RepositoryResult<bool>> ValidateGetByInsuranceIdAsync(Guid id)
        {
            if (!await appDbContext.InsurancePrices.AnyAsync(x => x.InsuranceId == id))
            {
                return RepositoryResult<bool>.Fail
                    ($"INSURANCE_PRICE_INSURANCEID_INVALID: Insurance price with the" +
                    $" insurance id {id} not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<InsurancePriceDto>> GetByInsuranceIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByInsuranceIdAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<InsurancePriceDto>.Fail(validationResult.Message);
            }

            var insurancePriceDomain = await insurancePricingRepository.GetByInsuranceIdAsync(id, 10);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return RepositoryResult<InsurancePriceDto>.Ok(response);
        }
    }
}
