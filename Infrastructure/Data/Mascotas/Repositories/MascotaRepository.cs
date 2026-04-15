using Application.Mascotas.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Mascotas.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de mascotas.
    /// Aquí se realiza el acceso real a la base de datos.
    /// </summary>
    public class MascotaRepository : IMascotaRepository
    {
        private readonly VetCareDbContext _context;

        public MascotaRepository(VetCareDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los mascotas registrados.
        /// </summary>
        public async Task<IReadOnlyList<Mascota>> FindAllAsync()
        {
            return await _context.Mascotas
                .OrderBy(c => c.IdMascota)
                .ToListAsync();
        }

        /// <summary>
        /// Busca un mascota por su id.
        /// </summary>
        public async Task<Mascota?> FindByIdAsync(int id)
        {
            return await _context.Mascotas
                .FirstOrDefaultAsync(c => c.IdMascota == id);
        }

        /// <summary>
        /// Busca un mascota por documento.
        /// </summary>
        public async Task<Mascota?> FindByAsync(string documento)
        {
            return await _context.Mascotas
                .FirstOrDefaultAsync(c => c.Nombre == documento);
        }

        /// <summary>
        /// Agrega un mascota nuevo a la base de datos.
        /// </summary>
        public async Task<Mascota> AddAsync(Mascota entity)
        {
            await _context.Mascotas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza los datos de un mascota existente.
        /// </summary>
        public async Task<Mascota> UpdateAsync(Mascota entity)
        {
            _context.Mascotas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Desactiva lógicamente un mascota.
        /// </summary>
        public async Task<bool> DisableAsync(int id)
        {
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(c => c.IdMascota == id);

            if (mascota == null)
                return false;

            mascota.EstadoMascota = false;
            mascota.FechaEdita = DateTime.Now;

            _context.Mascotas.Update(mascota);
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
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(c => c.IdMascota == id);

            if (mascota == null)
                return false;

            mascota.EstadoMascota = !mascota.EstadoMascota;
            mascota.FechaEdita = DateTime.Now;

            _context.Mascotas.Update(mascota);
            await _context.SaveChangesAsync();

            return mascota.EstadoMascota;
        }

        public async Task<PaginadoResponse<Mascota>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Mascota>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "nombres" => order == "desc" ? contex.OrderByDescending(p => p.Nombre) : contex.OrderBy(p => p.Nombre),
                        "estadoMascota" => order == "desc" ? contex.OrderByDescending(p => p.EstadoMascota) : contex.OrderBy(p => p.EstadoMascota),
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

                    if (id == "estadoMascota")
                    {
                        if (value == "activo") contex = contex.Where(p => p.EstadoMascota == true);
                        if (value == "inactivo") contex = contex.Where(p => p.EstadoMascota == false);
                    }
                    else if (id == "nombres") contex = contex.Where(p => p.Nombre.Contains(value));
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


            PaginadoResponse<Mascota> response = new(data, meta);

            return response;
        }
    }
}