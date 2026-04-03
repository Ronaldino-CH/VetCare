using Application.Clientes.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Clientes.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de clientes.
    /// Aquí se realiza el acceso real a la base de datos.
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        private readonly VetCareDbContext _context;

        public ClienteRepository(VetCareDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los clientes registrados.
        /// </summary>
        public async Task<IReadOnlyList<Cliente>> FindAllAsync()
        {
            return await _context.Clientes
                .OrderBy(c => c.IdCliente)
                .ToListAsync();
        }

        /// <summary>
        /// Busca un cliente por su id.
        /// </summary>
        public async Task<Cliente?> FindByIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        /// <summary>
        /// Busca un cliente por documento.
        /// </summary>
        public async Task<Cliente?> FindByDocumentoAsync(string documento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Documento == documento);
        }

        /// <summary>
        /// Agrega un cliente nuevo a la base de datos.
        /// </summary>
        public async Task<Cliente> AddAsync(Cliente entity)
        {
            await _context.Clientes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        public async Task<Cliente> UpdateAsync(Cliente entity)
        {
            _context.Clientes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Desactiva lógicamente un cliente.
        /// </summary>
        public async Task<bool> DisableAsync(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return false;

            cliente.EstadoCliente = false;
            cliente.FechaEdita = DateTime.Now;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Guarda cambios pendientes en el contexto.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> ChangeStatusAsync(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
                return false;

            cliente.EstadoCliente = !cliente.EstadoCliente;
            cliente.FechaEdita = DateTime.Now;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();

            return cliente.EstadoCliente;
        }

        public async Task<PaginadoResponse<Cliente>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Cliente>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "nombres" => order == "desc" ? contex.OrderByDescending(p => p.Nombres) : contex.OrderBy(p => p.Nombres),
                        "estadoCliente" => order == "desc" ? contex.OrderByDescending(p => p.EstadoCliente) : contex.OrderBy(p => p.EstadoCliente),
                        _ => contex
                    };
                }
            }

            if (dto.Filters != null && dto.Filters.Length > 0)
            {
                foreach (var filter in dto.Filters)
                {
                    var id_value = filter.Split(":");

                    var id = id_value[0];
                    var value = id_value[1];

                    if (id == "estadoCliente")
                    {
                        if (value == "activo") contex = contex.Where(p => p.EstadoCliente == true);
                        if (value == "inactivo") contex = contex.Where(p => p.EstadoCliente == false);
                    }
                    else if (id == "nombres") contex = contex.Where(p => p.Nombres.Contains(value));
                }
            }

            var take = dto.Take ?? 5;
            var page = dto.Page ?? 1;
            var skip = (page - 1) * take;

            var data = await contex.Skip(skip).Take(take).ToListAsync();
            var total = await contex.CountAsync();

            var meta = new Meta
            {
                CurrentPage = page,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / take)
            };


            PaginadoResponse<Cliente> response = new(data, meta);

            return response;
        }
    }
}