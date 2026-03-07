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
    public class RegistrationVehicleService : IRegistrationVehicleService
    {
        private readonly VehicleRegistrationDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly IRegistrationVehicleRepository registrationVehicleRepository;
        private readonly IRegistrationCalculatorService registrationCalculatorService;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IInsurancePricingRepository insurancePricingRepository;

        public RegistrationVehicleService(VehicleRegistrationDbContext appDbContext,
            IMapper mapper, IRegistrationVehicleRepository registrationVehicleRepository, 
            IRegistrationCalculatorService registrationCalculatorService,
            IVehicleRepository vehicleRepository,
            IInsurancePricingRepository insurancePricingRepository)
        {
            this.appDbContext = appDbContext;
            this.mapper = mapper;
            this.registrationVehicleRepository = registrationVehicleRepository;
            this.registrationCalculatorService = registrationCalculatorService;
            this.vehicleRepository = vehicleRepository;
            this.insurancePricingRepository = insurancePricingRepository;
        }

        public async Task<RepositoryResult<bool>>
            ValidateRegistrationCreateRequestAsync(CreateRegistrationVehicleRequestDto request)
        {


            if (await appDbContext.Registrations.AnyAsync(x => x.VehicleId == request.VehicleId))
            {
                return RepositoryResult<bool>.Fail("VEHICLE_ALREADY_REGISTERED: " +
                    "Registration cannot be done because vehicle is already registered");
            }

            if(await appDbContext.Registrations.AnyAsync(x=>x.LicensePlate == request.LicensePlate))
            {
                return RepositoryResult<bool>.Fail("PLATE_NUMBER_EXISTS: " +
                    "Registration cannot be done because vehicle plate already exists");
            }

            if (request.RegistrationDate > DateTime.Now)
            {
                return RepositoryResult<bool>.Fail("REGISTRATION_INVALID_DATE: " +
                    "Date of registration cannot be in the future");
            }

            if (!await appDbContext.Vehicles.AnyAsync(x => x.Id == request.VehicleId))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_VEHICLE_INVALID_ID: " +
                    $"Vehicle with the id {request.VehicleId} doesnt exist");
            }

            if(!await appDbContext.Clients.AnyAsync(x=> x.Id == request.ClientId))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_CLIENT_INVALID_ID: " +
                    $"Client with the id {request.ClientId} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<RegistrationVehicleDto>>
            CreateRegistrationAsync(CreateRegistrationVehicleRequestDto request)
        {
            var validationResult = await ValidateRegistrationCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<RegistrationVehicleDto>.Fail(validationResult.Message);
            }

            var domainRegistration = mapper.Map<Registration>(request);

            var vehicle = await vehicleRepository.GetVehicleByIdAsync(request.VehicleId);
            int vehicleAge = DateTime.Now.Year - vehicle.ProductionYear;
            var insurancePrice = 
                await insurancePricingRepository.GetByInsuranceIdAsync(request.InsuranceId, vehicle.EnginePowerKw);

            domainRegistration.ExpirationDate = domainRegistration.RegistrationDate.AddMonths(12);

            domainRegistration.RegistrationPrice = registrationCalculatorService.CalculateRegistrationPrice(
                 vehicle.EnginePowerKw, insurancePrice.PricePerKw,
                 Convert.ToDecimal(vehicle.EngineCapacity),
                 vehicleAge,
                 vehicle.FuelType);

            domainRegistration = await registrationVehicleRepository.AddRegistrationAsync(domainRegistration);

            var registration = await registrationVehicleRepository.GetByIdAsync(domainRegistration.Id);

            var response = mapper.Map<RegistrationVehicleDto>(registration);

            return RepositoryResult<RegistrationVehicleDto>.Ok
                (response, "New vehicle registration has successfully been created!");
        }

        public async Task<RepositoryResult<bool>?> ValidateRegistrationDeleteRequestAsync(Guid id)
        {
            if (!await appDbContext.Registrations.AnyAsync(x => x.Id == id))
            {
                return RepositoryResult<bool>.Fail
                    ($"REGISTRATION_INVALID_ID: Registration with the id {id} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<RegistrationVehicleDto>> DeleteRegistrationAsync(Guid id)
        {
            var validationResult = await ValidateRegistrationDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return RepositoryResult<RegistrationVehicleDto>.Fail(validationResult.Message);
            }

            var deletedRegistrationDomain = await registrationVehicleRepository.DeleteAsync(id);

            var response = mapper.Map<RegistrationVehicleDto>(deletedRegistrationDomain);

            return RepositoryResult<RegistrationVehicleDto>.Ok
                (response, "Registration of the vehicle has been successfully deleted!");
        }

        public async Task<RepositoryResult<bool>?>
            ValidateRegistrationUpdateRequestAsync(UpdateRegistrationVehicleRequestDto request)
        {

            if (!await appDbContext.Registrations.AnyAsync(x => x.Id == request.Id))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_INVALID_ID: " +
                    $"Registration with the id {request.Id} not found");
            }

            if (await appDbContext.Registrations.AnyAsync
                (x => x.VehicleId == request.VehicleId && x.Id!=request.Id))
            {
                return RepositoryResult<bool>.Fail("VEHICLE_ALREADY_REGISTERED: " +
                    "Registration cannot be done because vehicle is already registered");
            }

            if (request.RegistrationDate > DateTime.Now)
            {
                return RepositoryResult<bool>.Fail("REGISTRATION_INVALID_DATE: Date of registration cannot be in the future");
            }

            if (!await appDbContext.Vehicles.AnyAsync(x => x.Id == request.VehicleId))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_VEHICLE_INVALID_ID: " +
                    $"Vehicle with the id {request.VehicleId} doesnt exist");
            }

            if (!await appDbContext.Clients.AnyAsync(x => x.Id == request.ClientId))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_CLIENT_INVALID_ID: " +
                    $"Client with the id {request.ClientId} doesnt exist");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<RegistrationVehicleDto>>
            UpdateRegistrationAsync(UpdateRegistrationVehicleRequestDto request)
        {
            var validationResult = await ValidateRegistrationUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return RepositoryResult<RegistrationVehicleDto>.Fail(validationResult.Message);
            }

            var registrationDomain = mapper.Map<Registration>(request);

            var result = await registrationVehicleRepository.UpdateAsync(registrationDomain);

            var response = mapper.Map<RegistrationVehicleDto>(result);

            return RepositoryResult<RegistrationVehicleDto>.Ok
                (response, "Registration of the vehicle has been successfully updated!");
        }

        public async Task<RepositoryResult<PagedResult<RegistrationVehicleDto>>> GetAllAsync
            (string? searchQuery = null, int pageNumber = 1, int pageSize = 1000)
        {
            var (registrations, totalCount) = await registrationVehicleRepository.GetAllAsync
                (searchQuery, pageNumber, pageSize);

            var response = new PagedResult<RegistrationVehicleDto>
            {
                Items = mapper.Map<List<RegistrationVehicleDto>>(registrations),
                TotalCount = totalCount
            };

            return RepositoryResult<PagedResult<RegistrationVehicleDto>>.Ok(response);
        }

        public async Task<RepositoryResult<bool>?> ValidateGetByIdAsync(Guid id)
        {
            if(!await appDbContext.Registrations.AnyAsync(x=>x.Id == id))
            {
                return RepositoryResult<bool>.Fail($"REGISTRATION_INVALID_ID: " +
                    $"Registration with the id {id} not found");
            }

            return RepositoryResult<bool>.Ok(true);
        }

        public async Task<RepositoryResult<RegistrationVehicleDto>> GetByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetByIdAsync(id);

            if(!validationResult.Success)
            {
                return RepositoryResult<RegistrationVehicleDto>.Fail(validationResult.Message);
            }

            var registrationDomain = await registrationVehicleRepository.GetByIdAsync(id);

            var response = mapper.Map<RegistrationVehicleDto>(registrationDomain);

            return RepositoryResult<RegistrationVehicleDto>.Ok(response);
        }

        public async Task<RepositoryResult<RegistrationVehicleDto>> GenerateConfirmation(Guid id)
        {
            var registration = await registrationVehicleRepository.GetByIdAsync(id);

            var response = mapper.Map<RegistrationVehicleDto> (registration);

            return RepositoryResult<RegistrationVehicleDto>.Ok(response);
        }
    }
}






