using AutoMapper;
using Domain.Entities;

namespace Application.Clientes.Dtos.Profiles
{
    public class ClienteProfileDto : Profile
    {
        public ClienteProfileDto() 
        {
            CreateMap<Cliente, ClienteResponseDto>();
            CreateMap<ClienteSaveDto, Cliente>();
        }
    }
}
