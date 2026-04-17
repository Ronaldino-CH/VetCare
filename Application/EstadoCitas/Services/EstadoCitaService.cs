using Application.EstadoCitas.Dtos;
using Application.EstadoCitas.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.EstadoCitas.Services
{
    /// <summary>
    /// Servicio que contiene la lógica de negocio del módulo EstadoCitas.
    /// </summary>
    public class EstadoCitaService : IEstadoCitaService
    {
        private readonly IEstadoCitaRepository _estadoCitaRepository;
        private readonly IMapper _mapper;

        public EstadoCitaService(IEstadoCitaRepository estadoCitaRepository, IMapper mapper)
        {
            _estadoCitaRepository = estadoCitaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los estadoCitas.
        /// </summary>
        public async Task<IReadOnlyList<EstadoCitaResponseDto>> FindAllAsync()
        {
            var estadoCitas = await _estadoCitaRepository.FindAllAsync();

            return _mapper.Map<IReadOnlyList<EstadoCitaResponseDto>>(estadoCitas);
        }

        /// <summary>
        /// Busca un estadoCita por su identificador.
        /// </summary>
        public async Task<EstadoCitaResponseDto?> FindByIdAsync(int id)
        {
            var estadoCita = await _estadoCitaRepository.FindByIdAsync(id);

            if (estadoCita == null)
                return null;

            return _mapper.Map<EstadoCitaResponseDto>(estadoCita);
        }

        /// <summary>
        /// Crea un nuevo estadoCita validando que el documento no exista.
        /// </summary>
        public async Task<OperationResult<EstadoCitaResponseDto>> CreateAsync(EstadoCitaSaveDto saveDto)
        {
            var existeEstadoCita = await _estadoCitaRepository.FindByAsync(saveDto.NombreEstado);

            if (existeEstadoCita != null)
                return OperationResult<EstadoCitaResponseDto>.Fail("Ya existe una estadoCita con ese nombre.");

            EstadoCita estadoCita = _mapper.Map<EstadoCita>(saveDto);

            await _estadoCitaRepository.AddAsync(estadoCita);

            var estadoCitaDto = _mapper.Map<EstadoCitaResponseDto>(estadoCita);

            return OperationResult<EstadoCitaResponseDto>.Ok(estadoCitaDto, "EstadoCita creada correctamente.");
        }

        /// <summary>
        /// Edita un estadoCita existente.
        /// </summary>
        public async Task<OperationResult<EstadoCitaResponseDto>> EditAsync(int id, EstadoCitaSaveDto saveDto)
        {
            var estadoCita = await _estadoCitaRepository.FindByIdAsync(id);

            if (estadoCita == null)
                return OperationResult<EstadoCitaResponseDto>.Fail("EstadoCita no encontrada.");

            var existeEstadoCita = await _estadoCitaRepository.FindByAsync(saveDto.NombreEstado);

            if (existeEstadoCita != null && existeEstadoCita.IdEstadoCita != id)
                return OperationResult<EstadoCitaResponseDto>.Fail("Ya existe otra estadoCita con ese nombre.");

            // 🔥 AQUÍ está la magia con AutoMapper
            _mapper.Map(saveDto, estadoCita);

            // Campos que no vienen del DTO
            //estadoCita.FechaEdita = DateTime.Now;

            var estadoCitaActualizado = await _estadoCitaRepository.UpdateAsync(estadoCita);

            var response = _mapper.Map<EstadoCitaResponseDto>(estadoCitaActualizado);

            return OperationResult<EstadoCitaResponseDto>.Ok(response, "EstadoCita actualizada correctamente.");
        }

        /// <summary>
        /// Desactiva un estadoCita de forma lógica.
        /// </summary>
        public async Task<OperationResult<bool>> DisabledAsync(int id)
        {
            var resultado = await _estadoCitaRepository.DisableAsync(id);

            if (!resultado)
                return OperationResult<bool>.Fail("EstadoCita no encontrado.");

            return OperationResult<bool>.Ok(true, "EstadoCita desactivado correctamente.");
        }

        /// <summary>
        /// Lista solo los estadoCitas activos.
        /// </summary>
        public async Task<IReadOnlyList<EstadoCitaResponseDto>> GetActivosAsync()
        {
            //var estadoCitas = await _estadoCitaRepository.FindAllAsync();

            //var activos = estadoCitas.Where(c => c.EstadoEstadoCita);

            //return _mapper.Map<IReadOnlyList<EstadoCitaResponseDto>>(activos);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convierte una entidad EstadoCita a EstadoCitaResponseDto.
        /// </summary>
        //private static EstadoCitaResponseDto MapToResponseDto(EstadoCita estadoCita)
        //{
        //    return new EstadoCitaResponseDto
        //    {
        //        IdEstadoCita = estadoCita.IdEstadoCita,
        //        Nombre = estadoCita.Nombre,
        //        Especie = estadoCita.Especie,
        //        Raza = estadoCita.Raza,
        //        Sexo = estadoCita.Sexo,
        //        FechaNacimiento = estadoCita.FechaNacimiento,
        //        Peso = estadoCita.Peso,
        //        Color = estadoCita.Color,
        //        IdCliente = estadoCita.IdCliente,
        //        EstadoEstadoCita = estadoCita.EstadoEstadoCita,
        //        FechaCrea = estadoCita.FechaCrea,
        //        FechaEdita = estadoCita.FechaEdita
        //    };
        //}

        public async Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            var resultado = await _estadoCitaRepository.ChangeStatusAsync(id);

            if (resultado == null)
                return OperationResult<bool>.Fail("EstadoCita no encontrado.");

            var mensaje = resultado.Value
                ? "EstadoCita activado correctamente."
                : "EstadoCita desactivado correctamente.";

            return OperationResult<bool>.Ok(true, mensaje);
        }

        public async Task<PaginadoResponse<EstadoCitaResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _estadoCitaRepository.BusquedaPaginado(dto);

            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<EstadoCita>();

            var rsMapp = _mapper.Map<ICollection<EstadoCitaResponseDto>>(data);

            return new PaginadoResponse<EstadoCitaResponseDto>(rsMapp, rs.Meta);
        }
    }
}