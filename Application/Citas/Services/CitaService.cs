using Application.Citas.Dtos;
using Application.Citas.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.Citas.Services
{
    /// <summary>
    /// Servicio que contiene la lógica de negocio del módulo Citas.
    /// </summary>
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IMapper _mapper;

        public CitaService(ICitaRepository citaRepository, IMapper mapper)
        {
            _citaRepository = citaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los citas.
        /// </summary>
        public async Task<IReadOnlyList<CitaResponseDto>> FindAllAsync()
        {
            var citas = await _citaRepository.FindAllAsync();

            return _mapper.Map<IReadOnlyList<CitaResponseDto>>(citas);
        }

        /// <summary>
        /// Busca un cita por su identificador.
        /// </summary>
        public async Task<CitaResponseDto?> FindByIdAsync(int id)
        {
            var cita = await _citaRepository.FindByIdAsync(id);

            if (cita == null)
                return null;

            return _mapper.Map<CitaResponseDto>(cita);
        }

        /// <summary>
        /// Crea un nuevo cita validando que el documento no exista.
        /// </summary>
        public async Task<OperationResult<CitaResponseDto>> CreateAsync(CitaSaveDto saveDto)
        {
            //var existeCita = await _citaRepository.FindByAsync(saveDto.Nombre);

            //if (existeCita != null)
            //    return OperationResult<CitaResponseDto>.Fail("Ya existe una cita con ese nombre.");

            Cita cita = _mapper.Map<Cita>(saveDto);

            //cita.EstadoCita = true;
            cita.FechaCrea = DateTime.Now;

            await _citaRepository.AddAsync(cita);

            var citaDto = _mapper.Map<CitaResponseDto>(cita);

            return OperationResult<CitaResponseDto>.Ok(citaDto, "Cita creada correctamente.");
        }

        /// <summary>
        /// Edita un cita existente.
        /// </summary>
        public async Task<OperationResult<CitaResponseDto>> EditAsync(int id, CitaSaveDto saveDto)
        {
            var cita = await _citaRepository.FindByIdAsync(id);

            if (cita == null)
                return OperationResult<CitaResponseDto>.Fail("Cita no encontrada.");

            //var existeCita = await _citaRepository.FindByAsync(saveDto.Nombre);

            //if (existeCita != null && existeCita.IdCita != id)
            //    return OperationResult<CitaResponseDto>.Fail("Ya existe otra cita con ese nombre.");

            // 🔥 AQUÍ está la magia con AutoMapper
            _mapper.Map(saveDto, cita);

            // Campos que no vienen del DTO
            cita.FechaEdita = DateTime.Now;

            var citaActualizado = await _citaRepository.UpdateAsync(cita);

            var response = _mapper.Map<CitaResponseDto>(citaActualizado);

            return OperationResult<CitaResponseDto>.Ok(response, "Cita actualizada correctamente.");
        }

        /// <summary>
        /// Desactiva un cita de forma lógica.
        /// </summary>
        public async Task<OperationResult<bool>> DisabledAsync(int id)
        {
            var resultado = await _citaRepository.DisableAsync(id);

            if (!resultado)
                return OperationResult<bool>.Fail("Cita no encontrado.");

            return OperationResult<bool>.Ok(true, "Cita desactivado correctamente.");
        }

        /// <summary>
        /// Lista solo los citas activos.
        /// </summary>
        public async Task<IReadOnlyList<CitaResponseDto>> GetActivosAsync()
        {
            //var citas = await _citaRepository.FindAllAsync();

            //var activos = citas.Where(c => c.EstadoCita);

            //return _mapper.Map<IReadOnlyList<CitaResponseDto>>(activos);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convierte una entidad Cita a CitaResponseDto.
        /// </summary>
        //private static CitaResponseDto MapToResponseDto(Cita cita)
        //{
        //    return new CitaResponseDto
        //    {
        //        IdCita = cita.IdCita,
        //        Nombre = cita.Nombre,
        //        Especie = cita.Especie,
        //        Raza = cita.Raza,
        //        Sexo = cita.Sexo,
        //        FechaNacimiento = cita.FechaNacimiento,
        //        Peso = cita.Peso,
        //        Color = cita.Color,
        //        IdCliente = cita.IdCliente,
        //        EstadoCita = cita.EstadoCita,
        //        FechaCrea = cita.FechaCrea,
        //        FechaEdita = cita.FechaEdita
        //    };
        //}

        public async Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            //var resultado = await _citaRepository.ChangeStatusAsync(id);

            //if (resultado == null)
            //    return OperationResult<bool>.Fail("Cita no encontrado.");

            //var mensaje = resultado.Value
            //    ? "Cita activado correctamente."
            //    : "Cita desactivado correctamente.";

            //return OperationResult<bool>.Ok(true, mensaje);
            throw new NotImplementedException();
        }

        public async Task<PaginadoResponse<CitaResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _citaRepository.BusquedaPaginado(dto);

            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Cita>();

            var rsMapp = _mapper.Map<ICollection<CitaResponseDto>>(data);

            return new PaginadoResponse<CitaResponseDto>(rsMapp, rs.Meta);
        }

        
    }
}