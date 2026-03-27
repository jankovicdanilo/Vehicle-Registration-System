using Microsoft.AspNetCore.Mvc;
using VehicleRegistrationSystem.Models.DTO;
using VehicleRegistrationSystem.Models.DTO.Common;
using VehicleRegistrationSystem.Models.DTO.Vehicle;
using VehicleRegistrationSystem.Services.Interface;

namespace VehicleRegistrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? searchQuery, [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            var result = await vehicleService.GetAllAsync(searchQuery, pageSize, pageNumber);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequestDto request)
        {
            var result = await vehicleService.CreateVehicleAsync(request);

            if (!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return CreatedAtAction(nameof(GetVehicleById), new { id = result.Data.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] Guid id)
        {
            var result  = await vehicleService.GetVehicleByIdAsync(id);

            if(!result.Success)
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = result.ErrorCode,
                    Message = result.Message
                });
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await vehicleService.DeleteVehicleAsync(id);

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
        public async Task<IActionResult> Update([FromBody] UpdateVehicleDto updateRequest)
        {
            var result = await vehicleService.UpdateVehicleAsync(updateRequest);

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




