using Application.Mascotas.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mascotas.Interfaces
{
    /// <summary>
    /// Contrato específico para la lógica de negocio de clientes.
    /// </summary>
    public interface IMascotaService : ICrudCoreService<MascotaResponseDto, MascotaSaveDto, int>
    {
        Task<PaginadoResponse<MascotaResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}
