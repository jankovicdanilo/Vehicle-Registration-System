using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.VehicleModel;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IMapper mapper;
        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IVehicleBrandRepository vehicleBrandRepository;
        private readonly IVehicleRepository vehicleRepository;

        public VehicleModelService(IMapper mapper,IVehicleModelRepository vehicleModelRepository, 
            IVehicleBrandRepository vehicleBrandRepository, IVehicleRepository vehicleRepository)
        {
            this.mapper = mapper;
            this.vehicleModelRepository = vehicleModelRepository;
            this.vehicleBrandRepository = vehicleBrandRepository;
            this.vehicleRepository = vehicleRepository;
        }

        public async Task<Result<bool>> 
            ValidateVehicleModelCreateRequestAsync(CreateVehicleModelRequestDto request)
        {
            var existingModel = await vehicleModelRepository.ExistsAsync
                (x=>x.Name.ToLower() == request.Name.ToLower() && 
                x.VehicleBrandId == request.VehicleBrandId);

            if (existingModel)
            {
                return Result<bool>.Fail("VEHICLE_MODEL_EXISTS",
                    "Model already exists for the given vehicle brand");
            }

            if(!await vehicleBrandRepository.ExistsAsync(x=>x.Id == request.VehicleBrandId))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_NOT_FOUND","Vehicle brand with the id " +
                    $"{request.VehicleBrandId} doesnt exist");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleModelDto>> 
            CreateVehicleModelAsync(CreateVehicleModelRequestDto request)
        {
            var validationResult = await ValidateVehicleModelCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<VehicleModelDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleModelDomain = mapper.Map<VehicleModel>(request);

            vehicleModelDomain = await vehicleModelRepository.AddAsync(vehicleModelDomain);

            var response = mapper.Map<VehicleModelDto>(vehicleModelDomain);

            return Result<VehicleModelDto>.Ok
                (response, "New vehicle model has successfully been created!");
        }

        public async Task<Result<bool>?> ValidateVehicleModelDeleteRequestAsync(Guid id)
        {
            if(!await vehicleModelRepository.ExistsAsync(x=>x.Id==id))
            {
                return Result<bool>.Fail("VEHICLE_MODEL_NOT_FOUND", "Vehicle MODEL with the id " +
                    $"{id} doesnt exist");
            }

            if(await vehicleRepository.ExistsAsync(x => x.VehicleModelId == id))
            {
                return Result<bool>.Fail("VEHICLE_MODEL_IN_USE",
                    "Cannot delete model because it's assigned to existing vehicles");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleModelDto>> DeleteVehicleModelAsync(Guid id)
        {
            var validationResult = await ValidateVehicleModelDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<VehicleModelDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleModelDeleted = await vehicleModelRepository.DeleteAsync(id);

            var response = mapper.Map<VehicleModelDto>(vehicleModelDeleted);

            return Result<VehicleModelDto>.Ok(response, "Vehicle model has successfully been deleted!");
        }

        public async Task<Result<bool>?> 
            ValidateVehicleModelUpdateRequestAsync(UpdateVehicleModelRequestDto request)
        {
            if(!await vehicleModelRepository.ExistsAsync(x=>x.Id == request.Id))
            {
                return Result<bool>.Fail("VEHICLE_MODEL_NOT_FOUND", "Vehicle model with the id " +
                    $"{request.Id} doesnt exist");
            }

            var existingModel = await vehicleModelRepository.ExistsAsync
                (x => x.Name.ToLower() == request.Name.ToLower() &&
                x.VehicleBrandId == request.VehicleBrandId && x.Id != request.Id);

            if (existingModel)
            {
                return Result<bool>.Fail("VEHICLE_MODEL_EXISTS",
                    "Model already exists for the given vehicle brand");
            }

            if (!await vehicleBrandRepository.ExistsAsync(x => x.Id == request.VehicleBrandId))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_NOT_FOUND", "Vehicle brand with the id " +
                    $"{request.VehicleBrandId} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleModelDto>> 
            UpdateVehicleModelAsync(UpdateVehicleModelRequestDto request)
        {
            var validationResult = await ValidateVehicleModelUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<VehicleModelDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleModelDomain = mapper.Map<VehicleModel>(request);

            var updatedVehicleModelDomain = await vehicleModelRepository.
                UpdateAsync(vehicleModelDomain);

            var response = mapper.Map<VehicleModelDto>(updatedVehicleModelDomain);

            return Result<VehicleModelDto>.Ok(response, "Vehicle brand has been successfully updated!");
        }

        public async Task<Result<bool>?> ValidateVehicleModelGetByIdAsync(Guid id)
        {
            if(!await vehicleModelRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail("VEHICLE_MODEL_NOT_FOUND", "Vehicle MODEL with the id " +
                    $"{id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleModelDto>> GetById(Guid id)
        {
            var validatedResult = await ValidateVehicleModelGetByIdAsync(id);

            if (!validatedResult.Success)
            {
                return Result<VehicleModelDto>.Fail(validatedResult.ErrorCode,validatedResult.Message);
            }

            var vehicleModelDomain = await vehicleModelRepository.GetByIdAsync(id);

            var response = mapper.Map<VehicleModelDto>(vehicleModelDomain);

            return Result<VehicleModelDto>.Ok(response);
        }

        public async Task<Result<bool>?> ValidateVehicleModelGetByBrandIdAsync(Guid id)
        {
            if(!await vehicleModelRepository.ExistsAsync(x=>x.VehicleBrandId == id))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_NOT_FOUND", "Vehicle brand with the id " +
                    $"{id} not found");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<List<VehicleModelDto>>> GetByBrandId(Guid id)
        {
            var validatedResult = await ValidateVehicleModelGetByBrandIdAsync(id);

            if (!validatedResult.Success)
            {
                return Result<List<VehicleModelDto>>.Fail(validatedResult.ErrorCode,validatedResult.Message);
            }

            var vehicleModelDomainList = await vehicleModelRepository.ListByBrandId(id);

            var response = mapper.Map<List<VehicleModelDto>>(vehicleModelDomainList);

            return Result<List<VehicleModelDto>>.Ok(response);

        }
    }
}






