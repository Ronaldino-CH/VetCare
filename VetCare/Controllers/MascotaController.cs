using Application.Mascotas.Dtos;
using Application.Mascotas.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;

        public MascotasController(IMascotaService mascotaService)
        {
            _mascotaService = mascotaService;
        }

        /// <summary>
        /// Obtiene todos los mascotas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _mascotaService.FindAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un mascota por su id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _mascotaService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Mascota no encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Crea un mascota nuevo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MascotaSaveDto dto)
        {
            var result = await _mascotaService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Edita un mascota existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] MascotaSaveDto dto)
        {
            var result = await _mascotaService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("activos")]
        public async Task<IActionResult> GetActivos()
        {
            var result = await _mascotaService.GetActivosAsync();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _mascotaService.ChangeStatusAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("BusquedaPaginado")]
        [AllowAnonymous]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<MascotaResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _mascotaService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
