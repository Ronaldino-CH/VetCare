using AutoMapper;
using Domain.Entities;

namespace Application.Usuarios.Dtos.Profiles
{
    public class UsuarioProfileDto : Profile
    {
        public UsuarioProfileDto()
        {
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<UsuarioSaveDto, Usuario>();
        }
    }
}