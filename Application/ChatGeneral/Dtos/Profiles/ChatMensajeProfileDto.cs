using Application.ChatGeneral.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.ChatGeneral.Dtos.Profiles
{
    public class ChatMensajeProfileDto : Profile
    {
        public ChatMensajeProfileDto()
        {
            CreateMap<ChatMensaje, ChatMensajeResponseDto>()
                .ForMember(
                    dest => dest.NombreUsuario,
                    opt => opt.MapFrom(src => $"{src.Usuario.Nombres} {src.Usuario.Apellidos}".Trim()))
                .ForMember(dest => dest.IdRol, opt => opt.MapFrom(src => src.Usuario.IdRol))
                .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(src => src.Usuario.Rol.Nombre));
        }
    }
}
