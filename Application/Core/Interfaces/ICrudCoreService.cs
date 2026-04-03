using Shared.Common;

namespace Application.Core.Interfaces
{
    /// <summary>
    /// Contrato base para servicios CRUD.
    /// TDto: dto de salida
    /// TSaveDto: dto para crear o editar
    /// ID: tipo de clave primaria
    /// </summary>
    public interface ICrudCoreService<TDto, TSaveDto, ID>
    {
        Task<IReadOnlyList<TDto>> FindAllAsync();
        Task<TDto?> FindByIdAsync(ID id);
        Task<OperationResult<TDto>> CreateAsync(TSaveDto saveDto);
        Task<OperationResult<TDto>> EditAsync(ID id, TSaveDto saveDto);
        Task<OperationResult<bool>> DisabledAsync(ID id);
        Task<IReadOnlyList<TDto>> GetActivosAsync();
        Task<OperationResult<bool>> ChangeStatusAsync(ID id);
    }
}
