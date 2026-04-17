using Application.Core.Interfaces;
using Application.Roles.Dtos;
using Domain.Entities;

namespace Application.Roles.Interfaces
{
    public interface IRolService : ICrudCoreService<RolResponseDto, RolSaveDto, int>
    {
        Task<PaginadoResponse<RolResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}
