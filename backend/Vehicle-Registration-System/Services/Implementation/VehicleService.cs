using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.Vehicle;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class VehicleService : IVehicleService
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IVehicleBrandRepository vehicleBrandRepository;
        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IRegistrationVehicleRepository registrationVehicleRepository;

        public VehicleService(IMapper mapper, IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository, IVehicleBrandRepository vehicleBrandRepository,
            IVehicleModelRepository vehicleModelRepository, 
            IRegistrationVehicleRepository registrationVehicleRepository)
        {
            this.mapper = mapper;
            this.vehicleRepository = vehicleRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.vehicleBrandRepository = vehicleBrandRepository;
            this.vehicleModelRepository = vehicleModelRepository;
            this.registrationVehicleRepository = registrationVehicleRepository;
        }

        public async Task<Result<bool>> 
            ValidateVehicleCreateRequestAsync(CreateVehicleRequestDto request)
        {
            if (!await vehicleTypeRepository.ExistsAsync(t => t.Id == request.VehicleTypeId))
                return Result<bool>.Fail("TYPE_NOT_FOUND", "Vehicle type doesn't exist");

            if (!await vehicleBrandRepository.ExistsAsync(m => m.Id == request.VehicleBrandId))
                return Result<bool>.Fail("BRAND_NOT_FOUND", "Vehicle brand doesn't exist");

            if (!await vehicleModelRepository.ExistsAsync(m => m.Id == request.VehicleModelId))
                return Result<bool>.Fail("MODEL_NOT_FOUND", "Vehicle model doesn't exist");

            bool isValid = await vehicleRepository.IsVehicleModelValidAsync
                (request.VehicleModelId, request.VehicleBrandId, request.VehicleTypeId);

            if (!isValid)
                return Result<bool>.Fail("INVALID_COMBINATION",
                    "Model doesn't match brand and vehicle type");

            if (await vehicleRepository.ExistsAsync(x => x.ChassisNumber == request.ChassisNumber))
                return Result<bool>.Fail("CHASSIS_NUMBER_EXISTS", "Chassis number already used");

            if (request.ProductionYear < 1900 || request.ProductionYear > DateTime.Now.Year)
                return Result<bool>.Fail("INVALID_YEAR", "Invalid production year");

            if (request.EnginePowerKw <= 0)
                return Result<bool>.Fail("INVALID_ENGINE_POWER",
                    "Engine power must be greater than zero");

            if (request.ProductionYear > request.FirstRegistrationDate.Year)
                return Result<bool>.Fail("PRODUCTION_DATE_AFTER_FIRST_REGISTRATION",
                    "The car's production year must be before or equal to its first registration date.");

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleDto>> CreateVehicleAsync(CreateVehicleRequestDto request)
        {
            var validationResult = await ValidateVehicleCreateRequestAsync(request);
            if (!validationResult.Success)
                return Result<VehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);

            var vehicleDomain = mapper.Map<Vehicle>(request);

            vehicleDomain = await vehicleRepository.AddAsync(vehicleDomain);

            var response = mapper.Map<VehicleDto>(vehicleDomain);

            return Result<VehicleDto>.Ok(response, "New vehicle has successfully been created!");
        }


        public async Task<Result<bool>?> ValidateVehicleDeleteRequestAsync(Guid id)
        {
            var existingVehicle = await vehicleRepository.ExistsAsync(x => x.Id == id);
            
            if(!existingVehicle)
            {
                return Result<bool>.Fail("VEHICLE_NOT_FOUND", $"Vehicle with Id {id} was not found");
            }

            var isRegistered = await registrationVehicleRepository.ExistsAsync(x => x.VehicleId == id);

            if (isRegistered)
            {
                return Result<bool>.Fail(
                    "VEHICLE_REGISTRATION_EXISTS",
                    "Vehicle can't be deleted because its registration still exists");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleDto>> DeleteVehicleAsync(Guid id)
        {
            var validationResult = await ValidateVehicleDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<VehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var deletedVehicle = await vehicleRepository.DeleteAsync(id);

            var response = mapper.Map<VehicleDto>(deletedVehicle);

            return Result<VehicleDto>.Ok(response, "Vehicle has successfully been deleted!");
        }

        public async Task<Result<bool>?> ValidateVehicleUpdateRequestAsync(UpdateVehicleDto request)
        {
            if(request.Id == Guid.Empty)
            {
                return Result<bool>.Fail("INVALID_VEHICLE_ID",
                    "Vehicle ID is required and cannot be empty");
            }

            if(!await vehicleRepository.ExistsAsync(x => x.Id == request.Id))
            {
                return Result<bool>.Fail("VEHICLE_NOT_FOUND",
                    $"Vehicle with the Id {request?.Id} was not found");
            }

            if (!await vehicleTypeRepository.ExistsAsync(t => t.Id == request.VehicleTypeId))
                return Result<bool>.Fail("TYPE_NOT_FOUND", "Vehicle type doesn't exist");

            if (!await vehicleBrandRepository.ExistsAsync(m => m.Id == request.VehicleBrandId))
                return Result<bool>.Fail("BRAND_NOT_FOUND","Vehicle brand doesn't exist");

            if (!await vehicleModelRepository.ExistsAsync(m => m.Id == request.VehicleModelId))
                return Result<bool>.Fail("MODEL_NOT_FOUND", "Vehicle model doesn't exist");

            var isValid = await vehicleRepository.IsVehicleModelValidAsync
                (request.VehicleModelId, request.VehicleBrandId, request.VehicleTypeId);

            if (!isValid)
                return Result<bool>.Fail("INVALID_COMBINATION",
                    "Model doesn't match brand and vehicle type");



            if (await vehicleRepository.ExistsAsync(x => x.ChassisNumber == request.ChassisNumber &&
            x.Id != request.Id))
                return Result<bool>.Fail("CHASSIS_NUMBER_EXISTS","Chassis number already used");

            if (request.ProductionYear < 1900 || request.ProductionYear > DateTime.Now.Year)
                return Result<bool>.Fail("INVALID_YEAR", "Invalid production year");

            if (request.EnginePowerKw <= 0)
                return Result<bool>.Fail("INVALID_ENGINE_POWER",
                    "Engine power must be greater than zero");

            if (request.ProductionYear > request.FirstRegistrationDate.Year)
                return Result<bool>.Fail("PRODUCTION_DATE_AFTER_FIRST_REGISTRATION",
                    "The car's production year must be before or equal to its first registration date.");

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleDto>> UpdateVehicleAsync(UpdateVehicleDto request)
        {
            var validationResut = await ValidateVehicleUpdateRequestAsync(request);

            if (!validationResut.Success)
            {
                return Result<VehicleDto>.Fail(validationResut.ErrorCode,validationResut.Message);
            }

            var vehicleDomain = mapper.Map<Vehicle>(request);

            var updatedVehicle = await vehicleRepository.UpdateVehicleAsync(vehicleDomain);

            var response = mapper.Map<VehicleDto>(updatedVehicle);

            return Result<VehicleDto>.Ok(response, "Vehicle has successfully been updated!");
        }

        public async Task<Result<PagedResult<VehicleListItemDto>>> GetAllAsync(string? searchQuery = null, 
            int pageSize = 10, int pageNumber = 1)
        {
            var (vehicles, totalCount) = await vehicleRepository.GetAllAsync(searchQuery, pageNumber, pageSize);

            var response = new PagedResult<VehicleListItemDto>
            {
                Items = vehicles,
                TotalCount = totalCount
            };

            return Result<PagedResult<VehicleListItemDto>>.Ok(response);
        }

        public async Task<Result<bool>?> ValidateGetVehicleByIdRequestAsync(Guid id)
        {
            if(!await vehicleRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail("VEHICLE_NOT_FOUND",$"Vehicle with the Id {id} was not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleDto>> GetVehicleByIdAsync(Guid id)
        {
            var validationResult = await ValidateGetVehicleByIdRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<VehicleDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var response = await vehicleRepository.GetVehicleByIdAsync(id);

            return Result<VehicleDto>.Ok(response);
        }
    }
}


