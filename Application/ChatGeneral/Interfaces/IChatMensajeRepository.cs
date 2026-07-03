using Domain.Entities;

namespace Application.ChatGeneral.Interfaces
{
    public interface IChatMensajeRepository
    {
        Task<ChatMensaje> AddAsync(ChatMensaje entity);
        Task<ChatMensaje?> FindByIdWithUsuarioAsync(int idMensaje);
        Task<IReadOnlyList<ChatMensaje>> GetRecentActiveAsync(int take);
        Task<Usuario?> FindUsuarioConRolAsync(int idUsuario);
    }
}
