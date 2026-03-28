using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Repositories.Implementation;
using VehicleRegistrationSystem.Models.DTO.Registration;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class RegistrationVehicleService : IRegistrationVehicleService
    {
        private readonly IMapper mapper;
        private readonly IRegistrationVehicleRepository registrationVehicleRepository;
        private readonly IRegistrationCalculatorService registrationCalculatorService;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IInsurancePricingRepository insurancePricingRepository;
        private readonly IClientRepository clientRepository;
        private readonly IInsuranceRepository insuranceRepository;

        public RegistrationVehicleService(IMapper mapper,
            IRegistrationVehicleRepository registrationVehicleRepository,
            IRegistrationCalculatorService registrationCalculatorService,
            IVehicleRepository vehicleRepository,
            IInsurancePricingRepository insurancePricingRepository,
            IClientRepository clientRepository,
            IInsuranceRepository insuranceRepository)
        {
            this.mapper = mapper;
            this.registrationVehicleRepository = registrationVehicleRepository;
            this.registrationCalculatorService = registrationCalculatorService;
            this.vehicleRepository = vehicleRepository;
            this.insurancePricingRepository = insurancePricingRepository;
            this.clientRepository = clientRepository;
            this.insuranceRepository = insuranceRepository;
        }

        public async Task<Result<bool>>
            ValidateRegistrationCreateRequestAsync(CreateRegistrationVehicleRequestDto request,
            Vehicle vehicle, InsurancePrice? insurancePrice)
        {
            if(await registrationVehicleRepository.ExistsAsync(x => x.VehicleId == request.VehicleId))
            {
                return Result<bool>.Fail("VEHICLE_ALREADY_REGISTERED",
                    "Registration cannot be done because vehicle is already registered");
            }

            if(await registrationVehicleRepository.ExistsAsync(x=>x.LicensePlate == request.LicensePlate))
            {
                return Result<bool>.Fail("PLATE_NUMBER_EXISTS",
                    "Registration cannot be done because vehicle plate already exists");
            }

            if (request.RegistrationDate > DateTime.UtcNow)
            {
                return Result<bool>.Fail("REGISTRATION_INVALID_DATE",
                    "Date of registration cannot be in the future");
            }

            if (!await vehicleRepository.ExistsAsync(x => x.Id == request.VehicleId))
            {
                return Result<bool>.Fail("REGISTRATION_VEHICLE_INVALID_ID",
                    $"Vehicle with the id {request.VehicleId} doesnt exist");
            }

            if(!await clientRepository.ExistsAsync(x=> x.Id == request.ClientId))
            {
                return Result<bool>.Fail("REGISTRATION_CLIENT_INVALID_ID",
                    $"Client with the id {request.ClientId} doesn't exist");
            }

            if(!await insuranceRepository.ExistsAsync(x => x.Id == request.InsuranceId))
            {
                return Result<bool>.Fail("REGISTRATION_INSURANCE_INVALID_ID",
                    $"Insurance with the id {request.InsuranceId} doesn't exist");
            }

            if (vehicle == null)
            {
                return Result<bool>.Fail("REGISTRATION_VEHICLE_INVALID_ID",
                    $"Vehicle with the id {request.VehicleId} doesnt exist");
            }

            if (insurancePrice == null)
            {
                return Result<bool>.Fail("REGISTRATION_INSURANCE_PRICE_INVALID_ID",
                    $"Insurance price for the given insurance isn't established yet");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<RegistrationVehicleDto>>
            CreateRegistrationAsync(CreateRegistrationVehicleRequestDto request)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId);

            if(vehicle == null)
            {
                return Result<RegistrationVehicleDto>.Fail(
                    "REGISTRATION_VEHICLE_INVALID_ID","Vehicle doesn't exist");
            }

            var insurancePrice = await insurancePricingRepository.GetByInsuranceIdAsync
                (request.InsuranceId, vehicle.EnginePowerKw);

            var validationResult = await ValidateRegistrationCreateRequestAsync
                (request, vehicle, insurancePrice);

            if (!validationResult.Success)
            {
                return Result<RegistrationVehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var domainRegistration = mapper.Map<Registration>(request);
            int vehicleAge = DateTime.UtcNow.Year - vehicle.ProductionYear;

            domainRegistration.ExpirationDate = domainRegistration.RegistrationDate.AddMonths(12);

            domainRegistration.RegistrationPrice = registrationCalculatorService.CalculateRegistrationPrice(
                 vehicle.EnginePowerKw, insurancePrice.PricePerKw,
                 Convert.ToDecimal(vehicle.EngineCapacity),
                 vehicleAge,
                 vehicle.FuelType);

            try
            {
                domainRegistration = await registrationVehicleRepository.AddAsync(domainRegistration);
            }
            catch(DbUpdateException ex)
            {
                if(ex.InnerException?.Message.Contains("UQ_Registration_VehicleId") == true)
                {
                    return Result<RegistrationVehicleDto>.Fail("VEHICLE_REGISTERED","Vehicle already registered");
                }

                if(ex.InnerException?.Message.Contains("UQ_Registration_LicensePlate") == true)
                {
                    return Result<RegistrationVehicleDto>.Fail("LICENSE_PLATE_EXISTS", 
                        "License plate already exists");
                }

                throw;
            }
            
            var registration = await registrationVehicleRepository.GetByIdAsync(domainRegistration.Id);

            var response = mapper.Map<RegistrationVehicleDto>(registration);

            return Result<RegistrationVehicleDto>.Ok
                (response, "New vehicle registration has successfully been created!");
        }

        public async Task<Result<bool>?> ValidateRegistrationDeleteRequestAsync(Guid id)
        {
            if (!await registrationVehicleRepository.ExistsAsync(x => x.Id == id))
            {
                return Result<bool>.Fail
                    ($"REGISTRATION_INVALID_ID","Registration with the id {id} doesnt exist");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<RegistrationVehicleDto>> DeleteRegistrationAsync(Guid id)
        {
            var validationResult = await ValidateRegistrationDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<RegistrationVehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var deletedRegistrationDomain = await registrationVehicleRepository.DeleteAsync(id);

            var response = mapper.Map<RegistrationVehicleDto>(deletedRegistrationDomain);

            return Result<RegistrationVehicleDto>.Ok
                (response, "Registration of the vehicle has been successfully deleted!");
        }

        public async Task<Result<bool>?>
            ValidateRegistrationUpdateRequestAsync(UpdateRegistrationVehicleRequestDto request)
        {

            if (!await registrationVehicleRepository.ExistsAsync(x => x.Id == request.Id))
            {
                return Result<bool>.Fail("REGISTRATION_INVALID_ID" ,
                    $"Registration with the id {request.Id} not found");
            }

            if (await registrationVehicleRepository.ExistsAsync
                (x => x.VehicleId == request.VehicleId && x.Id!=request.Id))
            {
                return Result<bool>.Fail("VEHICLE_ALREADY_REGISTERED",
                    "Registration cannot be done because vehicle is already registered");
            }

            if (request.RegistrationDate > DateTime.Now)
            {
                return Result<bool>.Fail("REGISTRATION_INVALID_DATE","Date of registration cannot be in the future");
            }

            if (!await vehicleRepository.ExistsAsync(x => x.Id == request.VehicleId))
            {
                return Result<bool>.Fail("REGISTRATION_VEHICLE_INVALID_ID",
                    $"Vehicle with the id {request.VehicleId} doesnt exist");
            }

            if (!await clientRepository.ExistsAsync(x => x.Id == request.ClientId))
            {
                return Result<bool>.Fail("REGISTRATION_CLIENT_INVALID_ID",
                    $"Client with the id {request.ClientId} doesnt exist");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<RegistrationVehicleDto>>
            UpdateRegistrationAsync(UpdateRegistrationVehicleRequestDto request)
        {
            var validationResult = await ValidateRegistrationUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<RegistrationVehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var registrationDomain = mapper.Map<Registration>(request);

            var result = await registrationVehicleRepository.UpdateAsync(registrationDomain);

            var response = mapper.Map<RegistrationVehicleDto>(result);

            return Result<RegistrationVehicleDto>.Ok
                (response, "Registration of the vehicle has been successfully updated!");
        }

        public async Task<Result<PagedResult<RegistrationVehicleListItemDto>>> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 10)
        {
            var (registrations, totalCount) = await registrationVehicleRepository.GetAllAsync
                (searchQuery, pageNumber, pageSize);

            var response = new PagedResult<RegistrationVehicleListItemDto>
            {
                Items = registrations,
                TotalCount = totalCount
            };

            return Result<PagedResult<RegistrationVehicleListItemDto>>.Ok(response);
        }

        public async Task<Result<bool>?> ValidateGetByIdAsync(Guid id)
        {
            if(!await registrationVehicleRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail("REGISTRATION_INVALID_ID",
                    $"Registration with the id {id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<RegistrationDetailsDto>> GetByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByIdAsync(id);

            if(!validationResult.Success)
            {
                return Result<RegistrationDetailsDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var registrationDomain = await registrationVehicleRepository.GetDetailedRegistrationAsync(id);

            var response = mapper.Map<RegistrationDetailsDto>(registrationDomain);

            return Result<RegistrationDetailsDto>.Ok(response);
        }

        public async Task<Result<RegistrationVehicleDto>> GenerateConfirmation(Guid id)
        {
            var registration = await registrationVehicleRepository.GetByIdAsync(id);

            var response = mapper.Map<RegistrationVehicleDto> (registration);

            return Result<RegistrationVehicleDto>.Ok(response);
        }
    }
}






