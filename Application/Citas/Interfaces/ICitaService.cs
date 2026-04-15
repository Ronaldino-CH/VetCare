using Application.Citas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Citas.Interfaces
{
    /// <summary>
    /// Contrato específico para la lógica de negocio de clientes.
    /// </summary>
    public interface ICitaService : ICrudCoreService<CitaResponseDto, CitaSaveDto, int>
    {
        Task<PaginadoResponse<CitaResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}
