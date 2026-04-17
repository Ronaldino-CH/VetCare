using Application.Roles.Dtos;
using Application.Roles.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.Roles.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IRolRepository rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RolResponseDto>> FindAllAsync()
        {
            var roles = await _rolRepository.FindAllAsync();
            return _mapper.Map<IReadOnlyList<RolResponseDto>>(roles);
        }

        public async Task<RolResponseDto?> FindByIdAsync(int id)
        {
            var rol = await _rolRepository.FindByIdAsync(id);
            if (rol == null)
                return null;

            return _mapper.Map<RolResponseDto>(rol);
        }

        public async Task<OperationResult<RolResponseDto>> CreateAsync(RolSaveDto saveDto)
        {
            var existeRol = await _rolRepository.FindByNombreAsync(saveDto.Nombre);
            if (existeRol != null)
                return OperationResult<RolResponseDto>.Fail("Ya existe un rol con ese nombre.");

            var rol = _mapper.Map<Rol>(saveDto);
            await _rolRepository.AddAsync(rol);

            var rolDto = _mapper.Map<RolResponseDto>(rol);
            return OperationResult<RolResponseDto>.Ok(rolDto, "Rol creado correctamente.");
        }

        public async Task<OperationResult<RolResponseDto>> EditAsync(int id, RolSaveDto saveDto)
        {
            var rol = await _rolRepository.FindByIdAsync(id);
            if (rol == null)
                return OperationResult<RolResponseDto>.Fail("Rol no encontrado.");

            var existeRol = await _rolRepository.FindByNombreAsync(saveDto.Nombre);
            if (existeRol != null && existeRol.IdRol != id)
                return OperationResult<RolResponseDto>.Fail("Ya existe otro rol con ese nombre.");

            _mapper.Map(saveDto, rol);

            var rolActualizado = await _rolRepository.UpdateAsync(rol);
            var response = _mapper.Map<RolResponseDto>(rolActualizado);

            return OperationResult<RolResponseDto>.Ok(response, "Rol actualizado correctamente.");
        }

        public Task<OperationResult<bool>> DisabledAsync(int id)
        {
            throw new NotSupportedException("El módulo Roles no soporta cambio de estado.");
        }

        public Task<IReadOnlyList<RolResponseDto>> GetActivosAsync()
        {
            throw new NotSupportedException("El módulo Roles no soporta estado activo/inactivo.");
        }

        public Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            throw new NotSupportedException("El módulo Roles no soporta cambio de estado.");
        }

        public async Task<PaginadoResponse<RolResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _rolRepository.BusquedaPaginado(dto);
            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Rol>();
            var rsMapp = _mapper.Map<ICollection<RolResponseDto>>(data);
            return new PaginadoResponse<RolResponseDto>(rsMapp, rs.Meta);
        }
    }
}
