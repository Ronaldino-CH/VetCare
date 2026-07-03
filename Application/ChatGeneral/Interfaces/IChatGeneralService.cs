using Application.ChatGeneral.Dtos;
using Shared.Common;

namespace Application.ChatGeneral.Interfaces
{
    public interface IChatGeneralService
    {
        Task<IReadOnlyList<ChatMensajeResponseDto>> GetHistorialRecienteAsync(int take);
        Task<OperationResult<ChatMensajeResponseDto>> EnviarMensajeAsync(int idUsuario, string mensaje);
    }
}
