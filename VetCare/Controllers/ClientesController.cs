using Application.Clientes.Dtos;
using Application.Clientes.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1,2,3")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> FindAll()
        {
            var result = await _clienteService.FindAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Obtiene un cliente por su id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _clienteService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Cliente no encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Crea un cliente nuevo.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> Create([FromBody] ClienteSaveDto dto)
        {
            var result = await _clienteService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Edita un cliente existente.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromBody] ClienteSaveDto dto)
        {
            var result = await _clienteService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        
        [HttpGet("activos")]
        public async Task<IActionResult> GetActivos()
        {
            var result = await _clienteService.GetActivosAsync();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _clienteService.ChangeStatusAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("BusquedaPaginado")]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<ClienteResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _clienteService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
