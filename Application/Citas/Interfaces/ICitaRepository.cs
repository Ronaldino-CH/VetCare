using Application.Citas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.Citas.Interfaces
{
    /// <summary>
    /// Contrato específico para acceso a datos de clientes.
    /// Hereda operaciones CRUD base.
    /// </summary>
    public interface ICitaRepository : ICrudCoreRepository<Cita, int>
    {
        Task<Cita?> FindByAsync(string value);
        Task<PaginadoResponse<Cita>> BusquedaPaginado(PaginationRequest dto);
    }
}
