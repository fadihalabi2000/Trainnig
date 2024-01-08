using TrainnigApI.Model;

namespace TrainnigApI.service
{
    
    public interface IBaseService<TEntity> where TEntity :IBaseNormalEntity
    {
    
       
            Task<List<TEntity>> GetAllAsync();
            Task<TEntity> GetByIdAsync(int id);
            Task AddAsync(TEntity entity);
            Task UpdateAsync(TEntity entity);
            Task DeleteAsync(int id);

        
    }
}
