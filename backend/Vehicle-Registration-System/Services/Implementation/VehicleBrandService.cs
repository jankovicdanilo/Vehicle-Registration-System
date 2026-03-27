using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;
using VehicleRegistrationSystem.Data;
using VehicleRegistrationSystem.Models.Domain;
using VehicleRegistrationSystem.Models.DTO.VehicleBrand;

namespace VehicleRegistrationSystem.Services.Implementation
{
    public class VehicleBrandService : IVehicleBrandService
    {
        private readonly IVehicleBrandRepository vehicleBrandRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IVehicleModelRepository vehicleModelRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IMapper mapper;

        public VehicleBrandService(IVehicleBrandRepository vehicleBrandRepository,
            IVehicleTypeRepository vehicleTypeRepository,
            IVehicleModelRepository vehicleModelRepository,
            IVehicleRepository vehicleRepository,
            IMapper mapper)
        {
            this.vehicleBrandRepository = vehicleBrandRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.vehicleModelRepository = vehicleModelRepository;
            this.vehicleRepository = vehicleRepository;
            this.mapper = mapper;
        }

        public async Task<Result<bool>> 
            ValidateVehicleBrandCreateRequestAsync(CreateVehicleBrandRequestDto request)
        {
            var existingBrand = await vehicleBrandRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.ToLower() &&
                x.VehicleTypeId == request.VehicleTypeId);

            if (existingBrand)
            {
                return Result<bool>.Fail("VEHICLE_BRAND_EXISTS",
                    "Brand already exists for the given vehicle type");
            }

            if(!await vehicleTypeRepository.ExistsAsync(x=>x.Id == request.VehicleTypeId))
            {
                return Result<bool>.Fail("VEHICLE_TYPE_NOT_FOUND",$"Vehicle type with the id " +
                    $"{request.VehicleTypeId} doesnt exist");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleBrandDto>> 
            CreateVehicleBrand(CreateVehicleBrandRequestDto request)
        {
            var validationResult = await ValidateVehicleBrandCreateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<VehicleBrandDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleBrandDomain = mapper.Map<VehicleBrand>(request);
            var result = await vehicleBrandRepository.AddAsync(vehicleBrandDomain);

            var response = mapper.Map<VehicleBrandDto>(result);

            return Result<VehicleBrandDto>.Ok
                (response, "New vehicle brand has successfully been created!");
        }

        public async Task<Result<bool>> ValidateVehicleBrandDeleteRequestAsync(Guid id)
        {
            if(await vehicleModelRepository.ExistsAsync(x=>x.VehicleBrandId == id))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_HAS_MODELS",
                    " Vehicle brand has models and can't be deleted");
            }

            if(!await vehicleBrandRepository.ExistsAsync(x=>x.Id == id))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_NOT_FOUND",
                    $"Vehicle brand with Id {id} was not found");
            }

            if(await vehicleRepository.ExistsAsync(x=>x.VehicleBrandId == id))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_IN_USE",
                    "Cannot delete brand because it's assigned to existing vehicles");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleBrandDto>> DeleteVehicleBrand(Guid id)
        {
            var validationResult = await ValidateVehicleBrandDeleteRequestAsync(id);

            if (!validationResult.Success)
            {
                return Result<VehicleBrandDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleDomain = await vehicleBrandRepository.DeleteAsync(id);

            var response = mapper.Map<VehicleBrandDto>(vehicleDomain);

            return Result<VehicleBrandDto>.Ok(response, "Vehicle brand has successfully been deleted!");
        }

        public async Task<Result<bool>> 
            ValidateVehicleBrandUpdateRequestAsync(UpdateVehicleBrandRequestDto request)
        {
            if(!await vehicleBrandRepository.ExistsAsync(x => x.Id == request.Id))
            {
                return Result<bool>.Fail("VEHICLE_BRAND_NOT_FOUND",
                    $"Vehicle brand with the id {request.Id} doesnt exist");
            }

            var existingBrand = await vehicleBrandRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.ToLower() &&
                x.VehicleTypeId == request.VehicleTypeId && x.Id!=request.Id);

            if(existingBrand)
            {
                return Result<bool>.Fail("VEHICLE_BRAND_EXISTS" ,
                    "Brand already exists for the given vehicle type");
            }

            if (!await vehicleTypeRepository.ExistsAsync(x => x.Id == request.VehicleTypeId))
            {
                return Result<bool>.Fail("VEHICLE_TYPE_NOT_FOUND",$"Vehicle type with the id " +
                    $"{request.VehicleTypeId} doesnt exist");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleBrandDto>> 
            UpdateVehicleBrand(UpdateVehicleBrandRequestDto request)
        {
            var validationResult = await ValidateVehicleBrandUpdateRequestAsync(request);

            if (!validationResult.Success)
            {
                return Result<VehicleBrandDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleBrandDomain = mapper.Map<VehicleBrand>(request);

            var updatedBrandVehicle = await vehicleBrandRepository.UpdateAsync(vehicleBrandDomain);

            var response = mapper.Map<VehicleBrandDto>(updatedBrandVehicle);

            return Result<VehicleBrandDto>.Ok(response, "Vehicle brand has been successfully updated!");
        }

        public async Task<Result<bool>> ValidateVehicleBrandGetListByTypeAsync(Guid id)
        {
            if(!await vehicleBrandRepository.ExistsAsync(x=>x.VehicleTypeId ==  id))
            {
                return Result<bool>.Fail("INCORRECT TYPE ID",$"Vehicle type with the id {id}" +
                    $" doesn't exist!");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<List<VehicleBrandDto>>> GetListByType(Guid id)
        {
            var validationResult = await ValidateVehicleBrandGetListByTypeAsync(id);

            if (!validationResult.Success)
            {
                return Result<List<VehicleBrandDto>>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleBrandsDomain = await vehicleBrandRepository.ListByTypeId(id);

            var response = mapper.Map<List<VehicleBrandDto>>(vehicleBrandsDomain);

            return Result<List<VehicleBrandDto>>.Ok(response);
        }

        public async Task<Result<bool>> ValidateVehicleBrandGetByIdAsync(Guid id)
        {
            if (!await vehicleBrandRepository.ExistsAsync(x => x.Id == id))
            {
                return Result<bool>.Fail("INCORRECT BRAND ID", $"Vehicle brand with the  id {id}" +
                    $" doesn't exist!");
            }

            return Result<bool>.Ok(true);
        }

        public async Task<Result<VehicleBrandDto>> GetById(Guid id)
        {
            var validationResult = await ValidateVehicleBrandGetByIdAsync(id);

            if (!validationResult.Success)
            {
                return Result<VehicleBrandDto>.Fail(validationResult.ErrorCode,validationResult.Message);
            }

            var vehicleBrandDomain = await vehicleBrandRepository.GetByIdAsync(id);

            var response = mapper.Map<VehicleBrandDto>(vehicleBrandDomain);

            return Result<VehicleBrandDto>.Ok(response);
        }
    }
}






