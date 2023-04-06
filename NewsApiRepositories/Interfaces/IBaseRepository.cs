
using DataAccess.Entities.Abstractions.Interfaces;

using System.Linq.Expressions;

namespace Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : IBaseNormalEntity
    {
        Task<List<TEntity>> GetAllAsync();
        Task <TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
       
    }
}
