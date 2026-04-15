using AutoMapper;
using Domain.Entities;

namespace Application.Citas.Dtos.Profiles
{
    public class CitaProfileDto : Profile
    {
        public CitaProfileDto()
        {
            CreateMap<Cita, CitaResponseDto>();
            CreateMap<CitaSaveDto, Cita>();
        }
    }
}
