using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Models.DTO.Common;
using VehicleRegistrationSystem.Models.DTO.VehicleType;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IMapper mapper;
        private readonly IVehicleTypeService vehicleTypeService;

        public VehicleTypeController(IVehicleTypeRepository vehicleTypeRepository, IMapper mapper,
            IVehicleTypeService vehicleTypeService)
        {
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.mapper = mapper;
            this.vehicleTypeService = vehicleTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var response = await vehicleTypeService.GetAllAsync();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleTypeRequestDto request)
        {
            var result = await vehicleTypeService.CreateVehicleTypeAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await vehicleTypeService.GetById(id);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await vehicleTypeService.DeleteVehicleTypeAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleTypeRequestDto request)
        {
            var result = await vehicleTypeService.UpdateVehicleTypeAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);
        }
    }
}
