using Application.Usuarios.Dtos;
using Application.Usuarios.Interfaces;
using Application.Roles.Interfaces;
using Domain.Entities;
using Shared.Common;
using Shared.Security;
using AutoMapper;

namespace Application.Usuarios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IRolRepository rolRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> FindAllAsync()
        {
            var usuarios = await _usuarioRepository.FindAllAsync();
            return _mapper.Map<IReadOnlyList<UsuarioResponseDto>>(usuarios);
        }

        public async Task<UsuarioResponseDto?> FindByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.FindByIdAsync(id);
            if (usuario == null)
                return null;

            return _mapper.Map<UsuarioResponseDto>(usuario);
        }

        public async Task<OperationResult<UsuarioResponseDto>> CreateAsync(UsuarioSaveDto saveDto)
        {
            // Validar que el rol exista
            var rol = await _rolRepository.FindByIdAsync(saveDto.IdRol);
            if (rol == null)
                return OperationResult<UsuarioResponseDto>.Fail("El rol especificado no existe.");

            // Validar que el UserName no exista
            var existeUsuario = await _usuarioRepository.FindByUserNameAsync(saveDto.UserName);
            if (existeUsuario != null)
                return OperationResult<UsuarioResponseDto>.Fail("Ya existe un usuario con ese nombre de usuario.");

            var usuario = _mapper.Map<Usuario>(saveDto);
            usuario.PasswordHash = PasswordSecurity.HashPassword(saveDto.PasswordHash);
            usuario.EstadoUsuario = true;
            usuario.FechaCrea = DateTime.Now;

            await _usuarioRepository.AddAsync(usuario);

            var usuarioDto = _mapper.Map<UsuarioResponseDto>(usuario);
            return OperationResult<UsuarioResponseDto>.Ok(usuarioDto, "Usuario creado correctamente.");
        }

        public async Task<OperationResult<UsuarioResponseDto>> EditAsync(int id, UsuarioSaveDto saveDto)
        {
            var usuario = await _usuarioRepository.FindByIdAsync(id);
            if (usuario == null)
                return OperationResult<UsuarioResponseDto>.Fail("Usuario no encontrado.");

            // Validar que el rol exista
            var rol = await _rolRepository.FindByIdAsync(saveDto.IdRol);
            if (rol == null)
                return OperationResult<UsuarioResponseDto>.Fail("El rol especificado no existe.");

            // Validar que el UserName no exista en otro usuario
            var existeUsuario = await _usuarioRepository.FindByUserNameAsync(saveDto.UserName);
            if (existeUsuario != null && existeUsuario.IdUsuario != id)
                return OperationResult<UsuarioResponseDto>.Fail("Ya existe otro usuario con ese nombre de usuario.");

            _mapper.Map(saveDto, usuario);
            usuario.PasswordHash = PasswordSecurity.HashPassword(saveDto.PasswordHash);
            usuario.FechaEdita = DateTime.Now;

            var usuarioActualizado = await _usuarioRepository.UpdateAsync(usuario);
            var response = _mapper.Map<UsuarioResponseDto>(usuarioActualizado);

            return OperationResult<UsuarioResponseDto>.Ok(response, "Usuario actualizado correctamente.");
        }

        public async Task<OperationResult<bool>> DisabledAsync(int id)
        {
            var resultado = await _usuarioRepository.DisableAsync(id);
            if (!resultado)
                return OperationResult<bool>.Fail("Usuario no encontrado.");

            return OperationResult<bool>.Ok(true, "Usuario desactivado correctamente.");
        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> GetActivosAsync()
        {
            var usuarios = await _usuarioRepository.FindAllAsync();
            return usuarios
                .Where(u => u.EstadoUsuario)
                .Select(u => _mapper.Map<UsuarioResponseDto>(u))
                .ToList();
        }

        public async Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            var resultado = await _usuarioRepository.ChangeStatusAsync(id);
            if (resultado == null)
                return OperationResult<bool>.Fail("Usuario no encontrado.");

            var mensaje = resultado.Value
                ? "Usuario activado correctamente."
                : "Usuario desactivado correctamente.";

            return OperationResult<bool>.Ok(true, mensaje);
        }

        public async Task<PaginadoResponse<UsuarioResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _usuarioRepository.BusquedaPaginado(dto);
            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Usuario>();
            var rsMapp = _mapper.Map<ICollection<UsuarioResponseDto>>(data);
            return new PaginadoResponse<UsuarioResponseDto>(rsMapp, rs.Meta);
        }
    }
}
