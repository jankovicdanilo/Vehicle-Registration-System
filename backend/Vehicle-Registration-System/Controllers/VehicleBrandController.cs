using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Models.DTO.Common;
using VehicleRegistrationSystem.Models.DTO.VehicleBrand;
using VehicleRegistrationSystem.Repositories.Interface;
using VehicleRegistrationSystem.Results;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBrandController : ControllerBase
    {
        private readonly IVehicleBrandRepository vehicleBrandRepository;
        private readonly IMapper mapper;
        private readonly IVehicleBrandService vehicleBrandService;

        public VehicleBrandController(IVehicleBrandRepository vehicleBrandRepository, IMapper mapper, 
            IVehicleBrandService vehicleBrandService)
        {
            this.vehicleBrandRepository = vehicleBrandRepository;
            this.mapper = mapper;
            this.vehicleBrandService = vehicleBrandService;
        }

        [HttpGet("ListById/{id:guid}")]
        public async Task<IActionResult> List(Guid id)
        {
            var result = await vehicleBrandService.GetListByType(id);

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleBrandRequestDto request)
        {
            var result = await vehicleBrandService.CreateVehicleBrand(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetById), new {id = result.Data.Id},result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await vehicleBrandService.GetById(id);

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
            var result = await vehicleBrandService.DeleteVehicleBrand(id);

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
        public async Task<IActionResult> Update([FromBody] UpdateVehicleBrandRequestDto request)
        {
            var result = await vehicleBrandService.UpdateVehicleBrand(request);

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
