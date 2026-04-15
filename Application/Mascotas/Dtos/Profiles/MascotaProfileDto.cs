using AutoMapper;
using Domain.Entities;

namespace Application.Mascotas.Dtos.Profiles
{
    public class MascotaProfileDto : Profile
    {
        public MascotaProfileDto()
        {
            CreateMap<Mascota, MascotaResponseDto>();
            CreateMap<MascotaSaveDto, Mascota>();
        }
    }
}
