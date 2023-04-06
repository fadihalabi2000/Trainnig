using DataAccess.Entities.Abstractions.Interfaces;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;


namespace Services.CRUD
{
    public abstract class BaseCRUDService<TEntity> : IBaseCRUDService<TEntity>
        where TEntity : IBaseNormalEntity
    {
        protected readonly IBaseRepository<TEntity> repository;

        public BaseCRUDService(IBaseRepository<TEntity> repository)
        {
            this.repository = repository;
        }


        public async Task<List<TEntity>> GetAllAsync()
        {
         return await this.repository.GetAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
                return await this.repository.GetByIdAsync(id);
             
        }

        public async Task AddAsync(TEntity entity)
        {
             await  this.repository.AddAsync(entity);
            
           
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await this.repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await this.repository.DeleteAsync(id);
        }

      
    }
}
