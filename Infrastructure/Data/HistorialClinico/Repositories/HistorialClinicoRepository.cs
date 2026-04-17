using Application.HistorialClinico.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.HistorialClinico.Repositories
{
    public class HistorialClinicoRepository : IHistorialClinicoRepository
    {
        private readonly VetCareDbContext _context;

        public HistorialClinicoRepository(VetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Domain.Entities.HistorialClinico>> FindAllAsync()
        {
            return await _context.HistorialClinico
                .Include(h => h.Mascota)
                .Include(h => h.Veterinario)
                .OrderBy(h => h.IdHistorial)
                .ToListAsync();
        }

        public async Task<Domain.Entities.HistorialClinico?> FindByIdAsync(int id)
        {
            return await _context.HistorialClinico
                .Include(h => h.Mascota)
                .Include(h => h.Veterinario)
                .FirstOrDefaultAsync(h => h.IdHistorial == id);
        }

        public async Task<Domain.Entities.HistorialClinico> AddAsync(Domain.Entities.HistorialClinico entity)
        {
            await _context.HistorialClinico.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Domain.Entities.HistorialClinico> UpdateAsync(Domain.Entities.HistorialClinico entity)
        {
            _context.HistorialClinico.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DisableAsync(int id)
        {
            throw new NotSupportedException("El módulo HistorialClinico no soporta DisableAsync.");
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<bool?> ChangeStatusAsync(int id)
        {
            throw new NotSupportedException("El módulo HistorialClinico no soporta ChangeStatusAsync.");
        }

        public async Task<PaginadoResponse<Domain.Entities.HistorialClinico>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Domain.Entities.HistorialClinico>()
                .Include(h => h.Mascota)
                .Include(h => h.Veterinario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "diagnostico" => order == "desc" ? contex.OrderByDescending(p => p.Diagnostico) : contex.OrderBy(p => p.Diagnostico),
                        "tratamiento" => order == "desc" ? contex.OrderByDescending(p => p.Tratamiento) : contex.OrderBy(p => p.Tratamiento),
                        _ => contex
                    };
                }
            }

            if (dto.Filters != null && dto.Filters.Length > 0)
            {
                foreach (var filter in dto.Filters)
                {
                    var idValue = filter.Split(":", 2);
                    if (idValue.Length != 2) continue;

                    var id = idValue[0];
                    var value = idValue[1];

                    if (id == "idMascota" && int.TryParse(value, out var idMascota))
                    {
                        contex = contex.Where(p => p.IdMascota == idMascota);
                    }
                    else if (id == "mascota")
                    {
                        contex = contex.Where(p => p.Mascota != null && p.Mascota.Nombre.Contains(value));
                    }
                    else if (id == "diagnostico")
                    {
                        contex = contex.Where(p => p.Diagnostico.Contains(value));
                    }
                    else if (id == "tratamiento")
                    {
                        contex = contex.Where(p => p.Tratamiento.Contains(value));
                    }
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

            PaginadoResponse<Domain.Entities.HistorialClinico> response = new(data, meta);
            return response;
        }
    }
}
