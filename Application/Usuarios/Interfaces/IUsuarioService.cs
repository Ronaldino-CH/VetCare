using Application.Core.Interfaces;
using Application.Usuarios.Dtos;
using Domain.Entities;

namespace Application.Usuarios.Interfaces
{
    public interface IUsuarioService : ICrudCoreService<UsuarioResponseDto, UsuarioSaveDto, int>
    {
        Task<PaginadoResponse<UsuarioResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}