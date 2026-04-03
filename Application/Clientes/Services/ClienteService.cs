using Application.Clientes.Dtos;
using Application.Clientes.Interfaces;
using Domain.Entities;
using Shared.Common;
using AutoMapper;

namespace Application.Clientes.Services
{
    /// <summary>
    /// Servicio que contiene la lógica de negocio del módulo Clientes.
    /// </summary>
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos los clientes.
        /// </summary>
        public async Task<IReadOnlyList<ClienteResponseDto>> FindAllAsync()
        {
            var clientes = await _clienteRepository.FindAllAsync();

            return clientes.Select(MapToResponseDto).ToList();
        }

        /// <summary>
        /// Busca un cliente por su identificador.
        /// </summary>
        public async Task<ClienteResponseDto?> FindByIdAsync(int id)
        {
            var cliente = await _clienteRepository.FindByIdAsync(id);

            if (cliente == null)
                return null;

            return MapToResponseDto(cliente);
        }

        /// <summary>
        /// Crea un nuevo cliente validando que el documento no exista.
        /// </summary>
        public async Task<OperationResult<ClienteResponseDto>> CreateAsync(ClienteSaveDto saveDto)
        {
            var existeDocumento = await _clienteRepository.FindByDocumentoAsync(saveDto.Documento);

            if (existeDocumento != null)
                return OperationResult<ClienteResponseDto>.Fail("Ya existe un cliente con ese documento.");

            var cliente = new Cliente
            {
                Nombres = saveDto.Nombres,
                Apellidos = saveDto.Apellidos,
                Documento = saveDto.Documento,
                Telefono = saveDto.Telefono,
                Correo = saveDto.Correo,
                Direccion = saveDto.Direccion,
                EstadoCliente = true,
                FechaCrea = DateTime.Now
            };

            var clienteCreado = await _clienteRepository.AddAsync(cliente);

            return OperationResult<ClienteResponseDto>.Ok(MapToResponseDto(clienteCreado), "Cliente creado correctamente.");
        }

        /// <summary>
        /// Edita un cliente existente.
        /// </summary>
        public async Task<OperationResult<ClienteResponseDto>> EditAsync(int id, ClienteSaveDto saveDto)
        {
            var cliente = await _clienteRepository.FindByIdAsync(id);

            if (cliente == null)
                return OperationResult<ClienteResponseDto>.Fail("Cliente no encontrado.");

            var existeDocumento = await _clienteRepository.FindByDocumentoAsync(saveDto.Documento);

            if (existeDocumento != null && existeDocumento.IdCliente != id)
                return OperationResult<ClienteResponseDto>.Fail("Ya existe otro cliente con ese documento.");

            cliente.Nombres = saveDto.Nombres;
            cliente.Apellidos = saveDto.Apellidos;
            cliente.Documento = saveDto.Documento;
            cliente.Telefono = saveDto.Telefono;
            cliente.Correo = saveDto.Correo;
            cliente.Direccion = saveDto.Direccion;
            cliente.FechaEdita = DateTime.Now;

            var clienteActualizado = await _clienteRepository.UpdateAsync(cliente);

            return OperationResult<ClienteResponseDto>.Ok(MapToResponseDto(clienteActualizado), "Cliente actualizado correctamente.");
        }

        /// <summary>
        /// Desactiva un cliente de forma lógica.
        /// </summary>
        public async Task<OperationResult<bool>> DisabledAsync(int id)
        {
            var resultado = await _clienteRepository.DisableAsync(id);

            if (!resultado)
                return OperationResult<bool>.Fail("Cliente no encontrado.");

            return OperationResult<bool>.Ok(true, "Cliente desactivado correctamente.");
        }

        /// <summary>
        /// Lista solo los clientes activos.
        /// </summary>
        public async Task<IReadOnlyList<ClienteResponseDto>> GetActivosAsync()
        {
            var clientes = await _clienteRepository.FindAllAsync();

            return clientes
                .Where(c => c.EstadoCliente)
                .Select(MapToResponseDto)
                .ToList();
        }

        /// <summary>
        /// Convierte una entidad Cliente a ClienteResponseDto.
        /// </summary>
        private static ClienteResponseDto MapToResponseDto(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                IdCliente = cliente.IdCliente,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Documento = cliente.Documento,
                Telefono = cliente.Telefono,
                Correo = cliente.Correo,
                Direccion = cliente.Direccion,
                EstadoCliente = cliente.EstadoCliente,
                FechaCrea = cliente.FechaCrea
            };
        }

        public async Task<OperationResult<bool>> ChangeStatusAsync(int id)
        {
            var resultado = await _clienteRepository.ChangeStatusAsync(id);

            if (resultado == null)
                return OperationResult<bool>.Fail("Cliente no encontrado.");

            var mensaje = resultado.Value
                ? "Cliente activado correctamente."
                : "Cliente desactivado correctamente.";

            return OperationResult<bool>.Ok(true, mensaje);
        }

        public async Task<PaginadoResponse<ClienteResponseDto>> BusquedaPaginado(PaginationRequest dto)
        {
            var rs = await _clienteRepository.BusquedaPaginado(dto);

            if (rs == null)
                throw new Exception("La respuesta paginada del repositorio es null.");

            var data = rs.Data ?? new List<Cliente>();

            var rsMapp = _mapper.Map<ICollection<ClienteResponseDto>>(data);

            return new PaginadoResponse<ClienteResponseDto>(rsMapp, rs.Meta);
        }
    }
}