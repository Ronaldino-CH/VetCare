using Application.Citas.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Citas.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de mascotas.
    /// Aquí se realiza el acceso real a la base de datos.
    /// </summary>
    public class CitaRepository : ICitaRepository
    {
        private readonly VetCareDbContext _context;

        public CitaRepository(VetCareDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los mascotas registrados.
        /// </summary>
        public async Task<IReadOnlyList<Cita>> FindAllAsync()
        {
            return await _context.Citas
                .OrderBy(c => c.IdCita)
                .ToListAsync();
        }

        /// <summary>
        /// Busca un mascota por su id.
        /// </summary>
        public async Task<Cita?> FindByIdAsync(int id)
        {
            return await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        /// <summary>
        /// Busca un mascota por documento.
        /// </summary>
        public async Task<Cita?> FindByAsync(string documento)
        {
            //return await _context.Citas
            //    .FirstOrDefaultAsync(c => c.Nombre == documento);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Agrega un mascota nuevo a la base de datos.
        /// </summary>
        public async Task<Cita> AddAsync(Cita entity)
        {
            await _context.Citas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza los datos de un mascota existente.
        /// </summary>
        public async Task<Cita> UpdateAsync(Cita entity)
        {
            _context.Citas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Desactiva lógicamente un mascota.
        /// </summary>
        public async Task<bool> DisableAsync(int id)
        {
            var mascota = await _context.Citas.FirstOrDefaultAsync(c => c.IdCita == id);

            if (mascota == null)
                return false;

            //mascota.EstadoCita = false;
            mascota.FechaEdita = DateTime.Now;

            _context.Citas.Update(mascota);
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
            //var mascota = await _context.Citas.FirstOrDefaultAsync(c => c.IdCita == id);

            //if (mascota == null)
            //    return false;

            //mascota.EstadoCita = !mascota.EstadoCita;
            //mascota.FechaEdita = DateTime.Now;

            //_context.Citas.Update(mascota);
            //await _context.SaveChangesAsync();

            //return mascota.EstadoCita;
            throw new NotImplementedException();
        }

        public async Task<PaginadoResponse<Cita>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Cita>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "motivo" => order == "desc" ? contex.OrderByDescending(p => p.Motivo) : contex.OrderBy(p => p.Motivo),
                        //"estadoCita" => order == "desc" ? contex.OrderByDescending(p => p.EstadoCita) : contex.OrderBy(p => p.EstadoCita),
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

                    if (id == "estadoCita")
                    {
                        //if (value == "activo") contex = contex.Where(p => p.EstadoCita == true);
                        //if (value == "inactivo") contex = contex.Where(p => p.EstadoCita == false);
                    }
                    else if (id == "motivo") contex = contex.Where(p => p.Motivo.Contains(value));
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


            PaginadoResponse<Cita> response = new(data, meta);

            return response;
        }

        
    }
}