using Application.Roles.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Roles.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly VetCareDbContext _context;

        public RolRepository(VetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Rol>> FindAllAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.IdRol)
                .ToListAsync();
        }

        public async Task<Rol?> FindByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == id);
        }

        public async Task<Rol?> FindByNombreAsync(string nombre)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre == nombre);
        }

        public async Task<Rol> AddAsync(Rol entity)
        {
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Rol> UpdateAsync(Rol entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<bool> DisableAsync(int id)
        {
            throw new NotSupportedException("El módulo Roles no soporta DisableAsync porque no existe columna de estado.");
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<bool?> ChangeStatusAsync(int id)
        {
            throw new NotSupportedException("El módulo Roles no soporta ChangeStatusAsync porque no existe columna de estado.");
        }

        public async Task<PaginadoResponse<Rol>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Rol>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "nombre" => order == "desc" ? contex.OrderByDescending(p => p.Nombre) : contex.OrderBy(p => p.Nombre),
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

                    if (id == "nombre")
                    {
                        contex = contex.Where(p => p.Nombre.Contains(value));
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

            PaginadoResponse<Rol> response = new(data, meta);
            return response;
        }
    }
}
