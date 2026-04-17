using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.Roles.Interfaces
{
    public interface IRolRepository : ICrudCoreRepository<Rol, int>
    {
        Task<Rol?> FindByNombreAsync(string nombre);
        Task<PaginadoResponse<Rol>> BusquedaPaginado(PaginationRequest dto);
    }
}
