using Application.EstadoCitas.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EstadoCitas.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de clientes.
    /// Aquí se realiza el acceso real a la base de datos.
    /// </summary>
    public class EstadoCitaRepository : IEstadoCitaRepository
    {
        private readonly VetCareDbContext _context;

        public EstadoCitaRepository(VetCareDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los clientes registrados.
        /// </summary>
        public async Task<IReadOnlyList<EstadoCita>> FindAllAsync()
        {
            return await _context.EstadoCitas
                .OrderBy(c => c.IdEstadoCita)
                .ToListAsync();
        }

        /// <summary>
        /// Busca un cliente por su id.
        /// </summary>
        public async Task<EstadoCita?> FindByIdAsync(int id)
        {
            return await _context.EstadoCitas
                .FirstOrDefaultAsync(c => c.IdEstadoCita == id);
        }

        /// <summary>
        /// Busca un cliente por documento.
        /// </summary>
        public async Task<EstadoCita?> FindByAsync(string value)
        {
            return await _context.EstadoCitas
                .FirstOrDefaultAsync(c => c.NombreEstado == value);
        }

        /// <summary>
        /// Agrega un cliente nuevo a la base de datos.
        /// </summary>
        public async Task<EstadoCita> AddAsync(EstadoCita entity)
        {
            await _context.EstadoCitas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        public async Task<EstadoCita> UpdateAsync(EstadoCita entity)
        {
            _context.EstadoCitas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Desactiva lógicamente un cliente.
        /// </summary>
        public async Task<bool> DisableAsync(int id)
        {
            var cliente = await _context.EstadoCitas.FirstOrDefaultAsync(c => c.IdEstadoCita == id);

            if (cliente == null)
                return false;

            _context.EstadoCitas.Update(cliente);
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
            var cliente = await _context.EstadoCitas.FirstOrDefaultAsync(c => c.IdEstadoCita == id);

            if (cliente == null)
                return false;

            _context.EstadoCitas.Update(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginadoResponse<EstadoCita>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<EstadoCita>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "nombreEstado" => order == "desc" ? contex.OrderByDescending(p => p.NombreEstado) : contex.OrderBy(p => p.NombreEstado),
                        //"estadoEstadoCita" => order == "desc" ? contex.OrderByDescending(p => p.EstadoEstadoCita) : contex.OrderBy(p => p.EstadoEstadoCita),
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

                    if (id == "estadoEstadoCita")
                    {
                        //if (value == "activo") contex = contex.Where(p => p.EstadoEstadoCita == true);
                        //if (value == "inactivo") contex = contex.Where(p => p.EstadoEstadoCita == false);
                    }
                    else if (id == "nombreEstado") contex = contex.Where(p => p.NombreEstado.Contains(value));
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


            PaginadoResponse<EstadoCita> response = new(data, meta);

            return response;
        }
    }
}