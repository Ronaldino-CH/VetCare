using Application.EstadoCitas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.EstadoCitas.Interfaces
{
    /// <summary>
    /// Contrato específico para la lógica de negocio de clientes.
    /// </summary>
    public interface IEstadoCitaService : ICrudCoreService<EstadoCitaResponseDto, EstadoCitaSaveDto, int>
    {
        Task<PaginadoResponse<EstadoCitaResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}
