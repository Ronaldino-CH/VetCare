using Application.Citas.Dtos;
using Application.Citas.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;

        public CitasController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        /// <summary>
        /// Obtiene todos los citas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _citaService.FindAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un cita por su id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _citaService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Cita no encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Crea un cita nuevo.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CitaSaveDto dto)
        {
            var result = await _citaService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Edita un cita existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CitaSaveDto dto)
        {
            var result = await _citaService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        //[HttpGet("activos")]
        //public async Task<IActionResult> GetActivos()
        //{
        //    var result = await _citaService.GetActivosAsync();
        //    return Ok(result);
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> ChangeStatus(int id)
        //{
        //    var result = await _citaService.ChangeStatusAsync(id);

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpGet("BusquedaPaginado")]
        [AllowAnonymous]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<CitaResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _citaService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
