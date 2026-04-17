using AutoMapper;
using Domain.Entities;

namespace Application.HistorialClinico.Dtos.Profiles
{
    public class HistorialClinicoProfileDto : Profile
    {
        public HistorialClinicoProfileDto()
        {
            CreateMap<Domain.Entities.HistorialClinico, HistorialClinicoResponseDto>();
            CreateMap<HistorialClinicoSaveDto, Domain.Entities.HistorialClinico>();
        }
    }
}