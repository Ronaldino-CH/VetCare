using Application.Clientes.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.Clientes.Interfaces
{
    /// <summary>
    /// Contrato específico para acceso a datos de clientes.
    /// Hereda operaciones CRUD base.
    /// </summary>
    public interface IClienteRepository : ICrudCoreRepository<Cliente, int>
    {
        Task<Cliente?> FindByDocumentoAsync(string documento);
        Task<PaginadoResponse<Cliente>> BusquedaPaginado(PaginationRequest dto);
    }
}
