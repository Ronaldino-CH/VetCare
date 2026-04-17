using Application.HistorialClinico.Dtos;
using Application.HistorialClinico.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1,2,3")]
    public class HistorialClinicoController : ControllerBase
    {
        private readonly IHistorialClinicoService _historialClinicoService;

        public HistorialClinicoController(IHistorialClinicoService historialClinicoService)
        {
            _historialClinicoService = historialClinicoService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _historialClinicoService.FindAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _historialClinicoService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Historial clínico no encontrado." });

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Create([FromBody] HistorialClinicoSaveDto dto)
        {
            var result = await _historialClinicoService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> Edit(int id, [FromBody] HistorialClinicoSaveDto dto)
        {
            var result = await _historialClinicoService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("BusquedaPaginado")]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<HistorialClinicoResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _historialClinicoService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
