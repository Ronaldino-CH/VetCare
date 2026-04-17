using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Usuarios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Security;
using VetCare.Dtos.Auth;
using VetCare.Settings;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IUsuarioRepository usuarioRepository, IOptions<JwtSettings> jwtSettings)
        {
            _usuarioRepository = usuarioRepository;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Usuario y contrasena son obligatorios." });

            var userName = dto.UserName.Trim();
            var usuario = await _usuarioRepository.FindByUserNameAsync(userName);

            if (usuario == null || !PasswordSecurity.VerifyPassword(dto.Password, usuario.PasswordHash))
                return Unauthorized(new { message = "Credenciales invalidas." });

            if (!usuario.EstadoUsuario)
                return Unauthorized(new { message = "Tu usuario esta inactivo. Contacta al administrador." });

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresMinutes);
            var roleId = usuario.IdRol.ToString();
            var roleName = usuario.Rol?.Nombre ?? string.Empty;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new(ClaimTypes.Name, usuario.UserName),
                new(ClaimTypes.Role, roleId),
                new("roleName", roleName),
                new("fullName", $"{usuario.Nombres} {usuario.Apellidos}".Trim())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            var response = new LoginResponseDto
            {
                AccessToken = token,
                ExpiresAtUtc = expiresAt,
                IdUsuario = usuario.IdUsuario,
                UserName = usuario.UserName,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                IdRol = usuario.IdRol,
                RolNombre = roleName
            };

            return Ok(response);
        }
    }
}
