using AutoMapper;
using Domain.Entities;

namespace Application.Roles.Dtos.Profiles
{
    public class RolProfileDto : Profile
    {
        public RolProfileDto()
        {
            CreateMap<Rol, RolResponseDto>();
            CreateMap<RolSaveDto, Rol>();
        }
    }
}
