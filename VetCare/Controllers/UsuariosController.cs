using Application.Usuarios.Dtos;
using Application.Usuarios.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> FindAll()
        {
            var result = await _usuarioService.FindAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> FindById(int id)
        {
            var result = await _usuarioService.FindByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Usuario no encontrado." });

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create([FromBody] UsuarioSaveDto dto)
        {
            var result = await _usuarioService.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id, [FromBody] UsuarioSaveDto dto)
        {
            var result = await _usuarioService.EditAsync(id, dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("activos")]
        [Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> GetActivos()
        {
            var result = await _usuarioService.GetActivosAsync();
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _usuarioService.ChangeStatusAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("BusquedaPaginado")]
        [Authorize(Roles = "1")]
        public async Task<Results<BadRequest, Ok<PaginadoResponse<UsuarioResponseDto>>>> BusquedaPaginado([FromQuery] PaginationRequest dto)
        {
            var response = await _usuarioService.BusquedaPaginado(dto);

            if (response != null)
            {
                return TypedResults.Ok(response);
            }

            return TypedResults.BadRequest();
        }
    }
}
