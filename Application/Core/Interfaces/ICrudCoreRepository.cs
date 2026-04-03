using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Interfaces
{
    /// <summary>
    /// Contrato base para repositorios CRUD.
    /// T: entidad
    /// ID: tipo de la clave primaria
    /// </summary>
    public interface ICrudCoreRepository<T, ID> where T : class
    {
        Task<IReadOnlyList<T>> FindAllAsync();
        Task<T?> FindByIdAsync(ID id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DisableAsync(ID id);
        Task SaveChangesAsync();
        Task<bool?> ChangeStatusAsync(ID id);
    }
}
