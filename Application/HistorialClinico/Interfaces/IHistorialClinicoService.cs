using Application.Core.Interfaces;
using Application.HistorialClinico.Dtos;
using Domain.Entities;
using Shared.Common;

namespace Application.HistorialClinico.Interfaces
{
    public interface IHistorialClinicoService : ICrudCoreService<HistorialClinicoResponseDto, HistorialClinicoSaveDto, int>
    {
        Task<PaginadoResponse<HistorialClinicoResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}