using Application.Mascotas.Dtos;
using Application.Mascotas.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.Mascotas.Services
{
    /// <summary>
    /// Servicio que contiene la lógica de negocio del módulo Mascotas.
    /// </summary>
    public class MascotaService : IMascotaService
    {
        private readonly IMascotaRepository _mascotaRepository;
        private readonly IMapper _mapper;

        public MascotaService(IMascotaRepository mascotaRepository, IMapper mapper)
        {
            _mascotaRepository = mascotaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los mascotas.
        /// </summary>
        public async Task<IReadOnlyList<MascotaResponseDto>> FindAllAsync()
        {
            var mascotas = await _mascotaRepository.FindAllAsync();

            return _mapper.Map<IReadOnlyList<MascotaResponseDto>>(mascotas);
        }

        /// <summary>
        /// Busca un mascota por su identificador.
        /// </summary>
        public async Task<MascotaResponseDto?> FindByIdAsync(int id)
        {
            var mascota = await _mascotaRepository.FindByIdAsync(id);

            if (mascota == null)
                return null;

            return _mapper.Map<MascotaResponseDto>(mascota);
        }

        /// <summary>
        /// Crea un nuevo mascota validando que el documento no exista.
        /// </summary>
        public async Task<OperationResult<MascotaResponseDto>> CreateAsync(MascotaSaveDto saveDto)
        {
            var existeMascota = await _mascotaRepository.FindByAsync(saveDto.Nombre);

            if (existeMascota != null)
                return OperationResult<MascotaResponseDto>.Fail("Ya existe una mascota con ese nombre.");

            Mascota mascota = _mapper.Map<Mascota>(saveDto);

            mascota.EstadoMascota = true;
            mascota.FechaCrea = DateTime.Now;

            await _mascotaRepository.AddAsync(mascota);

            var mascotaDto = _mapper.Map<MascotaResponseDto>(mascota);

            return OperationResult<MascotaResponseDto>.Ok(mascotaDto, "Mascota creada correctamente.");
        }

        /// <summary>
        /// Edita un mascota existente.
        /// </summary>
        public async Task<OperationResult<MascotaResponseDto>> EditAsync(int id, MascotaSaveDto saveDto)
        {
            var mascota = await _mascotaRepository.FindByIdAsync(id);

            if (mascota == null)
                return OperationResult<MascotaResponseDto>.Fail("Mascota no encontrada.");

            var existeMascota = await _mascotaRepository.FindByAsync(saveDto.Nombre);

            if (existeMascota != null && existeMascota.IdMascota != id)
                return OperationResult<MascotaResponseDto>.Fail("Ya existe otra mascota con ese nombre.");

            // 🔥 AQUÍ está la magia con AutoMapper
            _mapper.Map(saveDto, mascota);

            // Campos que no vienen del DTO
            mascota.FechaEdita = DateTime.Now;

            var mascotaActualizado = await _mascotaRepository.UpdateAsync(mascota);

            var response = _mapper.Map<MascotaResponseDto>(mascotaActualizado);

            return OperationResult<MascotaResponseDto>.Ok(response, "Mascota actualizada correctamente.");
        }

        /// <summary>
        /// Desactiva un mascota de forma lógica.
        /// </summary>
        public async Task<OperationResult<bool>> DisabledAsync(int id)
        {
            var resultado = await _mascotaRepository.DisableAsync(id);

            if (!resultado)
                return OperationResult<bool>.Fail("Mascota no encontrado.");

            return OperationResult<bool>.Ok(true, "Mascota desactivado correctamente.");
        }

        /// <summary>
        /// Lista solo los mascotas activos.
        /// </summary>
        public async Task<IReadOnlyList<MascotaResponseDto>> GetActivosAsync()
        {
            var mascotas = await _mascotaRepository.FindAllAsync();

            var activos = mascotas.Where(c => c.EstadoMascota);

            return _mapper.Map<IReadOnlyList<MascotaResponseDto>>(activos);
        }

        /// <summary>
        /// Convierte una entidad Mascota a MascotaResponseDto.
        /// </summary>
        //private static MascotaResponseDto MapToResponseDto(Mascota mascota)
        //{
        //    return new MascotaResponseDto
        //    {
        //        IdMascota = mascota.IdMascota,
        //        Nombre = mascota.Nombre,
        //        Especie = mascota.Especie,
        //        Raza = mascota.Raza,
        //        Sexo = mascota.Sexo,
        //        FechaNacimiento = mascota.FechaNacimiento,
        //        Peso = mascota.Peso,
        //        Color = mascota.Color,
        //        IdCliente = mascota.IdCliente,
        //        EstadoMascota = mascota.EstadoMascota,
        //        FechaCrea = mascota.FechaCrea,
        //        FechaEdita = mascota.FechaEdita
        //    };
        //}

        public async Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            var resultado = await _mascotaRepository.ChangeStatusAsync(id);

            if (resultado == null)
                return OperationResult<bool>.Fail("Mascota no encontrado.");

            var mensaje = resultado.Value
                ? "Mascota activado correctamente."
                : "Mascota desactivado correctamente.";

            return OperationResult<bool>.Ok(true, mensaje);
        }

        public async Task<PaginadoResponse<MascotaResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _mascotaRepository.BusquedaPaginado(dto);

            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Mascota>();

            var rsMapp = _mapper.Map<ICollection<MascotaResponseDto>>(data);

            return new PaginadoResponse<MascotaResponseDto>(rsMapp, rs.Meta);
        }
    }
}