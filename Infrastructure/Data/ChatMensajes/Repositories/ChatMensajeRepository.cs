using Application.ChatGeneral.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.ChatMensajes.Repositories
{
    public class ChatMensajeRepository : IChatMensajeRepository
    {
        private readonly VetCareDbContext _context;

        public ChatMensajeRepository(VetCareDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMensaje> AddAsync(ChatMensaje entity)
        {
            await _context.ChatMensajes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<ChatMensaje?> FindByIdWithUsuarioAsync(int idMensaje)
        {
            return await _context.ChatMensajes
                .Include(cm => cm.Usuario)
                .ThenInclude(u => u.Rol)
                .FirstOrDefaultAsync(cm => cm.IdMensaje == idMensaje);
        }

        public async Task<IReadOnlyList<ChatMensaje>> GetRecentActiveAsync(int take)
        {
            var mensajes = await _context.ChatMensajes
                .AsNoTracking()
                .Include(cm => cm.Usuario)
                .ThenInclude(u => u.Rol)
                .Where(cm => cm.Activo)
                .OrderByDescending(cm => cm.FechaEnvio)
                .Take(take)
                .ToListAsync();

            // El cliente espera el historial en orden cronologico ascendente.
            return mensajes.OrderBy(cm => cm.FechaEnvio).ToList();
        }

        public async Task<Usuario?> FindUsuarioConRolAsync(int idUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }
    }
}
