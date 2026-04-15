using Application.Mascotas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;

namespace Application.Mascotas.Interfaces
{
    /// <summary>
    /// Contrato específico para acceso a datos de clientes.
    /// Hereda operaciones CRUD base.
    /// </summary>
    public interface IMascotaRepository : ICrudCoreRepository<Mascota, int>
    {
        Task<Mascota?> FindByAsync(string value);
        Task<PaginadoResponse<Mascota>> BusquedaPaginado(PaginationRequest dto);
    }
}
