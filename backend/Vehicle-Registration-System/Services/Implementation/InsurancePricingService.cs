using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Implementation;
using VehicleRegistrationSystem.Models.DTO.InsurancePricing;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class InsurancePricingService : IInsurancePricingService
    {
        private readonly IInsurancePricingRepository insurancePricingRepository;
        private readonly IMapper mapper;
        private readonly IInsuranceRepository insuranceRepository;

        public InsurancePricingService(IInsurancePricingRepository insurancePricingRepository,
            IMapper mapper,
            IInsuranceRepository insuranceRepository)
        {
            this.insurancePricingRepository = insurancePricingRepository;
            this.mapper = mapper;
            this.insuranceRepository = insuranceRepository;
        }

        public async Task<Result<bool>> ValidateCreateAsync(CreateInsurancePriceRequestDto request)
        {
            if(!await insuranceRepository.ExistsAsync(x=>x.Id == request.InsuranceId))
            {
                return Result<bool>.Fail($"INVALID_Insurance_ID","Insurance" +
                    $" with the id {request.InsuranceId} not found");
            }

            // Get all existing prices for this insurance
            var existingPrices = await insurancePricingRepository
                .FindAsync(x=>x.InsuranceId == request.InsuranceId);

            // Check for overlapping ranges
            var hasOverlap = existingPrices.Any(x =>
            request.MinKw <= x.MaxKw && request.MaxKw >= x.MinKw);

            if (hasOverlap)
            {
                return Result<bool>.Fail("INSURANCE_PRICE_OVERLAP",
                    "This price range overlaps with an existing range");
            }

            /* Check for gaps — if there are existing prices, the new range
             must connect to an existing one */
            if (existingPrices.Any())
            {
                var connectsToExisting = existingPrices.Any(x=>
                request.MinKw == x.MaxKw + 1 || request.MaxKw == x.MinKw - 1);

                if(!connectsToExisting)
                {
                    return Result<bool>.Fail("INSURANCE_PRICE_GAP",
                    "Price range must connect to an existing range without gaps");
                }
            }

            // Validate that MinKw is less than MaxKw
            if (request.MinKw >= request.MaxKw)
            {
                return Result<bool>.Fail("INSURANCE_PRICE_INVALID_RANGE",
                    "MinKw must be less than MaxKw");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsurancePriceDto>> CreateAsync(CreateInsurancePriceRequestDto request)
        {
            var validationResult = await ValidateCreateAsync(request);

            if(!validationResult.Success)
            {
                return Result<InsurancePriceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insurancePriceDomain = mapper.Map<InsurancePrice>(request);

            insurancePriceDomain = await insurancePricingRepository.AddAsync(insurancePriceDomain);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return Result<InsurancePriceDto>.Ok(response, "New insurance price has been created!");
        }

        public async Task<Result<List<InsurancePriceDto>>> GetAllAsync()
        {
            var insurancePricesDomainList = await insurancePricingRepository.GetAllAsync();

            var response = mapper.Map<List<InsurancePriceDto>>(insurancePricesDomainList);

            return Result<List<InsurancePriceDto>>.Ok(response);
        }

        public async Task<Result<bool>> ValidateGetByIdAsync(Guid id)
        {
            if(!await insurancePricingRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail($"INSURANCE_PRICE_ID_INVALID","Insurance price with the" +
                    $" id {id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsurancePriceDto>> GetByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByIdAsync(id);

            if (!validationResult.Success)
            {
                return Result<InsurancePriceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insurancePriceDomain = await insurancePricingRepository.GetByIdAsync(id);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return Result<InsurancePriceDto>.Ok(response);
        }

        public async Task<Result<bool>> ValidateGetByInsuranceIdAsync(Guid id)
        {
            if (!await insurancePricingRepository.ExistsAsync(x => x.InsuranceId == id))
            {
                return Result<bool>.Fail
                    ("INSURANCE_PRICE_INSURANCEID_INVALID","Insurance price with the" +
                    $" insurance id {id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<InsurancePriceDto>> GetByInsuranceIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByInsuranceIdAsync(id);

            if (!validationResult.Success)
            {
                return Result<InsurancePriceDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var insurancePriceDomain = await insurancePricingRepository.GetByInsuranceIdAsync(id, 10);

            var response = mapper.Map<InsurancePriceDto>(insurancePriceDomain);

            return Result<InsurancePriceDto>.Ok(response);
        }
    }
}
