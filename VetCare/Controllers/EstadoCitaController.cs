using Application.EstadoCitas.Dtos;
using Application.EstadoCitas.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1,2,3")]
    public class EstadoCitasController : ControllerBase
    {
        private readonly IEstadoCitaService _estadoCitaService;

        public EstadoCitasController(IEstadoCitaService estadoCitaService)
        {
            _estadoCitaService = estadoCitaService;
        }

        /// <summary>
        /// Obtiene todos los estadoCitas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _estadoCitaService.FindAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un estadoCita por su id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _estadoCitaService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "EstadoCita no encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Crea un estadoCita nuevo.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] EstadoCitaSaveDto dto)
        {
            var result = await _estadoCitaService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Edita un estadoCita existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id, [FromBody] EstadoCitaSaveDto dto)
        {
            var result = await _estadoCitaService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        //[HttpGet("activos")]
        //public async Task<IActionResult> GetActivos()
        //{
        //    var result = await _estadoCitaService.GetActivosAsync();
        //    return Ok(result);
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> ChangeStatus(int id)
        //{
        //    var result = await _estadoCitaService.ChangeStatusAsync(id);

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpGet("BusquedaPaginado")]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<EstadoCitaResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _estadoCitaService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
