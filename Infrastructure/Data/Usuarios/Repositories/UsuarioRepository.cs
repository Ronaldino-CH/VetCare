using Application.Usuarios.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Usuarios.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly VetCareDbContext _context;

        public UsuarioRepository(VetCareDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Usuario>> FindAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.IdUsuario)
                .ToListAsync();
        }

        public async Task<Usuario?> FindByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<Usuario?> FindByUserNameAsync(string userName)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<Usuario> AddAsync(Usuario entity)
        {
            await _context.Usuarios.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Usuario> UpdateAsync(Usuario entity)
        {
            _context.Usuarios.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DisableAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null)
                return false;

            usuario.EstadoUsuario = false;
            usuario.FechaEdita = DateTime.Now;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool?> ChangeStatusAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null)
                return null;

            usuario.EstadoUsuario = !usuario.EstadoUsuario;
            usuario.FechaEdita = DateTime.Now;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return usuario.EstadoUsuario;
        }

        public async Task<PaginadoResponse<Usuario>> BusquedaPaginado(PaginationRequest dto)
        {
            var contex = _context.Set<Usuario>().Include(u => u.Rol).AsQueryable();

            if (!string.IsNullOrWhiteSpace(dto.Sort))
            {
                var ColumnsOrder = dto.Sort.Split(".");
                if (ColumnsOrder.Length == 2)
                {
                    var column = ColumnsOrder[0];
                    var order = ColumnsOrder[1];

                    contex = column switch
                    {
                        "userName" => order == "desc" ? contex.OrderByDescending(p => p.UserName) : contex.OrderBy(p => p.UserName),
                        "nombres" => order == "desc" ? contex.OrderByDescending(p => p.Nombres) : contex.OrderBy(p => p.Nombres),
                        "estadoUsuario" => order == "desc" ? contex.OrderByDescending(p => p.EstadoUsuario) : contex.OrderBy(p => p.EstadoUsuario),
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

                    if (id == "estadoUsuario")
                    {
                        if (value == "activo") contex = contex.Where(p => p.EstadoUsuario == true);
                        if (value == "inactivo") contex = contex.Where(p => p.EstadoUsuario == false);
                    }
                    else if (id == "userName") contex = contex.Where(p => p.UserName.Contains(value));
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

            PaginadoResponse<Usuario> response = new(data, meta);
            return response;
        }
    }
}
