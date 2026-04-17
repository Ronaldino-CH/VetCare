using Application.Roles.Dtos;
using Application.Roles.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1")]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _rolService.FindAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _rolService.FindByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Rol no encontrado." });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RolSaveDto dto)
        {
            var result = await _rolService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RolSaveDto dto)
        {
            var result = await _rolService.EditAsync(id, dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("BusquedaPaginado")]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<RolResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _rolService.BusquedaPaginado(dto);
            if (response != null)
                return TypedResults.Ok(response);

            return TypedResults.BadRequest();
        }
    }
}
