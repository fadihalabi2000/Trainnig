
using DataAccess.Entities.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using NewsApiData;
using Repositories.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repositories
{
    public  class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class, IBaseNormalEntity
    {
        private readonly NewsApiDbContext dbContext;

        public BaseRepository(NewsApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

     
        public async Task<List<TEntity>> GetAllAsync()
        {
            DbSet<TEntity> dbSet = this.dbContext.Set<TEntity>();

           
            return await Task.Run(() => dbSet.Where(p => p.IsDeleted == false).ToListAsync());
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            DbSet<TEntity> dbSet = this.dbContext.Set<TEntity>();

            TEntity? entity = await dbSet.FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            return entity!;
            
               
        }

        public async Task AddAsync(TEntity entity)
        {
            await Task.Run(() => {
                this.dbContext.Set<TEntity>().Add(entity);
                this.dbContext.SaveChanges();
            });
           
           
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => {
                this.dbContext.Set<TEntity>().Update(entity);
                 this.dbContext.SaveChanges();
            });
           


        }

        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                TEntity entity = this.dbContext.Set<TEntity>().Find(id)!;
                entity!.IsDeleted = true;
                this.dbContext.SaveChanges();
            });
        }

       
    }
}
