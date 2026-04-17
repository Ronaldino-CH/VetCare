using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.HistorialClinico.Interfaces
{
    public interface IHistorialClinicoRepository : ICrudCoreRepository<Domain.Entities.HistorialClinico, int>
    {
        Task<PaginadoResponse<Domain.Entities.HistorialClinico>> BusquedaPaginado(PaginationRequest dto);
    }
}