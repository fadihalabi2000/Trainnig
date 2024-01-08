using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainnigApI.Data;
using TrainnigApI.Model;

namespace TrainnigApI.service
{
    public class BaseService<TEntity> : IBaseService<TEntity> 
    where TEntity : class, IBaseNormalEntity
    {
        private readonly AppDBContext dbContext;

        public BaseService(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await dbContext.Set<TEntity>().ToListAsync();
        }

        //public async Task<TEntity?> GetByIdAsync(int id)
        //{
        //    DbSet<TEntity> dbSet = this.dbContext.Set<TEntity>();

        //    TEntity? entity = await dbSet.FirstOrDefaultAsync(p => p.Id == id);
        //    return entity;
        //}

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            //await Task.Run(() => {
            //    this.dbContext.Set<TEntity>().AddAsync(entity);
            //});
            await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await dbContext.Set<TEntity>()
                          .FirstOrDefaultAsync(a => a.ID == id);

            if (entity != null)
            {
                dbContext.Set<TEntity>().Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
