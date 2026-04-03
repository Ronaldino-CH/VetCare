using Application.Clientes.Dtos;
using Application.Core.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clientes.Interfaces
{
    /// <summary>
    /// Contrato específico para la lógica de negocio de clientes.
    /// </summary>
    public interface IClienteService : ICrudCoreService<ClienteResponseDto, ClienteSaveDto, int>
    {
        Task<PaginadoResponse<ClienteResponseDto>> BusquedaPaginado(PaginationRequest dto);
    }
}
