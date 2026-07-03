using Application.ChatGeneral.Dtos;
using Application.ChatGeneral.Interfaces;
using AutoMapper;
using Domain.Entities;
using Shared.Common;

namespace Application.ChatGeneral.Services
{
    public class ChatGeneralService : IChatGeneralService
    {
        private const int MaxMensajeLength = 1000;
        private readonly IChatMensajeRepository _chatMensajeRepository;
        private readonly IMapper _mapper;

        public ChatGeneralService(IChatMensajeRepository chatMensajeRepository, IMapper mapper)
        {
            _chatMensajeRepository = chatMensajeRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ChatMensajeResponseDto>> GetHistorialRecienteAsync(int take)
        {
            var safeTake = take <= 0 ? 50 : Math.Min(take, 200);
            var mensajes = await _chatMensajeRepository.GetRecentActiveAsync(safeTake);

            return _mapper.Map<IReadOnlyList<ChatMensajeResponseDto>>(mensajes);
        }

        public async Task<OperationResult<ChatMensajeResponseDto>> EnviarMensajeAsync(int idUsuario, string mensaje)
        {
            if (idUsuario <= 0)
                return OperationResult<ChatMensajeResponseDto>.Fail("Usuario invalido.");

            var mensajeNormalizado = (mensaje ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(mensajeNormalizado))
                return OperationResult<ChatMensajeResponseDto>.Fail("El mensaje no puede estar vacio.");

            if (mensajeNormalizado.Length > MaxMensajeLength)
                return OperationResult<ChatMensajeResponseDto>.Fail($"El mensaje no puede superar {MaxMensajeLength} caracteres.");

            var usuario = await _chatMensajeRepository.FindUsuarioConRolAsync(idUsuario);
            if (usuario == null)
                return OperationResult<ChatMensajeResponseDto>.Fail("No se encontro el usuario del mensaje.");

            if (!usuario.EstadoUsuario)
                return OperationResult<ChatMensajeResponseDto>.Fail("El usuario se encuentra inactivo.");

            var entity = new ChatMensaje
            {
                IdUsuario = idUsuario,
                Mensaje = mensajeNormalizado,
                FechaEnvio = DateTime.Now,
                Activo = true
            };

            var created = await _chatMensajeRepository.AddAsync(entity);
            var createdWithUsuario = await _chatMensajeRepository.FindByIdWithUsuarioAsync(created.IdMensaje);

            if (createdWithUsuario == null)
                return OperationResult<ChatMensajeResponseDto>.Fail("No se pudo obtener el mensaje creado.");

            var response = _mapper.Map<ChatMensajeResponseDto>(createdWithUsuario);
            return OperationResult<ChatMensajeResponseDto>.Ok(response, "Mensaje enviado correctamente.");
        }
    }
}
