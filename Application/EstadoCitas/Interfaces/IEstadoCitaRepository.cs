using Application.EstadoCitas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.EstadoCitas.Interfaces
{
    /// <summary>
    /// Contrato específico para acceso a datos de clientes.
    /// Hereda operaciones CRUD base.
    /// </summary>
    public interface IEstadoCitaRepository : ICrudCoreRepository<EstadoCita, int>
    {
        Task<EstadoCita?> FindByAsync(string value);
        Task<PaginadoResponse<EstadoCita>> BusquedaPaginado(PaginationRequest dto);
    }
}
