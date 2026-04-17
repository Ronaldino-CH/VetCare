using Application.HistorialClinico.Dtos;
using Application.HistorialClinico.Interfaces;
using Application.Mascotas.Interfaces;
using Application.Usuarios.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.HistorialClinico.Services
{
    public class HistorialClinicoService : IHistorialClinicoService
    {
        private readonly IHistorialClinicoRepository _historialClinicoRepository;
        private readonly IMascotaRepository _mascotaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public HistorialClinicoService(IHistorialClinicoRepository historialClinicoRepository, IMascotaRepository mascotaRepository, IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _historialClinicoRepository = historialClinicoRepository;
            _mascotaRepository = mascotaRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<HistorialClinicoResponseDto>> FindAllAsync()
        {
            var historiales = await _historialClinicoRepository.FindAllAsync();
            return _mapper.Map<IReadOnlyList<HistorialClinicoResponseDto>>(historiales);
        }

        public async Task<HistorialClinicoResponseDto?> FindByIdAsync(int id)
        {
            var historial = await _historialClinicoRepository.FindByIdAsync(id);
            if (historial == null)
                return null;

            return _mapper.Map<HistorialClinicoResponseDto>(historial);
        }

        public async Task<OperationResult<HistorialClinicoResponseDto>> CreateAsync(HistorialClinicoSaveDto saveDto)
        {
            // Validar que la mascota exista
            var mascota = await _mascotaRepository.FindByIdAsync(saveDto.IdMascota);
            if (mascota == null)
                return OperationResult<HistorialClinicoResponseDto>.Fail("La mascota especificada no existe.");

            // Validar que el veterinario exista
            var veterinario = await _usuarioRepository.FindByIdAsync(saveDto.IdVeterinario);
            if (veterinario == null)
                return OperationResult<HistorialClinicoResponseDto>.Fail("El veterinario especificado no existe.");

            var historial = _mapper.Map<Domain.Entities.HistorialClinico>(saveDto);
            historial.FechaCrea = DateTime.Now;

            await _historialClinicoRepository.AddAsync(historial);

            var historialDto = _mapper.Map<HistorialClinicoResponseDto>(historial);
            return OperationResult<HistorialClinicoResponseDto>.Ok(historialDto, "Historial clínico creado correctamente.");
        }

        public async Task<OperationResult<HistorialClinicoResponseDto>> EditAsync(int id, HistorialClinicoSaveDto saveDto)
        {
            var historial = await _historialClinicoRepository.FindByIdAsync(id);
            if (historial == null)
                return OperationResult<HistorialClinicoResponseDto>.Fail("Historial clínico no encontrado.");

            // Validar que la mascota exista
            var mascota = await _mascotaRepository.FindByIdAsync(saveDto.IdMascota);
            if (mascota == null)
                return OperationResult<HistorialClinicoResponseDto>.Fail("La mascota especificada no existe.");

            // Validar que el veterinario exista
            var veterinario = await _usuarioRepository.FindByIdAsync(saveDto.IdVeterinario);
            if (veterinario == null)
                return OperationResult<HistorialClinicoResponseDto>.Fail("El veterinario especificado no existe.");

            _mapper.Map(saveDto, historial);
            historial.FechaEdita = DateTime.Now;

            var historialActualizado = await _historialClinicoRepository.UpdateAsync(historial);
            var response = _mapper.Map<HistorialClinicoResponseDto>(historialActualizado);

            return OperationResult<HistorialClinicoResponseDto>.Ok(response, "Historial clínico actualizado correctamente.");
        }

        public Task<OperationResult<bool>> DisabledAsync(int id)
        {
            throw new NotSupportedException("El módulo HistorialClinico no soporta DisabledAsync.");
        }

        public Task<IReadOnlyList<HistorialClinicoResponseDto>> GetActivosAsync()
        {
            throw new NotSupportedException("El módulo HistorialClinico no soporta GetActivosAsync.");
        }

        public Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            throw new NotSupportedException("El módulo HistorialClinico no soporta ChangeStatusAsync.");
        }

        public async Task<PaginadoResponse<HistorialClinicoResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _historialClinicoRepository.BusquedaPaginado(dto);
            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Domain.Entities.HistorialClinico>();
            var rsMapp = _mapper.Map<ICollection<HistorialClinicoResponseDto>>(data);
            return new PaginadoResponse<HistorialClinicoResponseDto>(rsMapp, rs.Meta);
        }
    }
}