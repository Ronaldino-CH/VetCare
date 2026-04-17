using AutoMapper;
using Domain.Entities;

namespace Application.EstadoCitas.Dtos.Profiles
{
    public class EstadoCitaProfileDto : Profile
    {
        public EstadoCitaProfileDto()
        {
            CreateMap<EstadoCita, EstadoCitaResponseDto>();
            CreateMap<EstadoCitaSaveDto, EstadoCita>();
        }
    }
}
