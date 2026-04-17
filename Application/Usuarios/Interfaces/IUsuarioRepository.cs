using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.Usuarios.Interfaces
{
    public interface IUsuarioRepository : ICrudCoreRepository<Usuario, int>
    {
        Task<Usuario?> FindByUserNameAsync(string userName);
        Task<PaginadoResponse<Usuario>> BusquedaPaginado(PaginationRequest dto);
    }
}