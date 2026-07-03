using System.Security.Claims;
using Application.ChatGeneral.Dtos;
using Application.ChatGeneral.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace VetCare.Hubs
{
    [Authorize(Roles = "1,2,3")]
    public class ChatGeneralHub : Hub
    {
        private readonly IChatGeneralService _chatGeneralService;

        public ChatGeneralHub(IChatGeneralService chatGeneralService)
        {
            _chatGeneralService = chatGeneralService;
        }

        public async Task SendMessage(EnviarChatMensajeDto dto)
        {
            var claimValue = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claimValue, out var idUsuario) || idUsuario <= 0)
                throw new HubException("No se pudo identificar el usuario autenticado.");

            var result = await _chatGeneralService.EnviarMensajeAsync(idUsuario, dto?.Mensaje ?? string.Empty);

            if (!result.Success || result.Data == null)
                throw new HubException(result.Message ?? "No se pudo enviar el mensaje.");

            await Clients.All.SendAsync("ReceiveMessage", result.Data);
        }
    }
}
