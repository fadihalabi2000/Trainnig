using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories;


namespace NewsApiRepositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        private readonly NewsApiDbContext dbContext;

        public LogRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Log>> GetAllAuthorsLogAsync()
        {
            var dbSet = this.dbContext.Set<Log>();


            return await Task.Run(() => dbSet.Where(l=> l.UserId==int.MaxValue).ToListAsync());
        }

        public async Task<List<Log>> GetAllUsersLogAsync()
        {
            var dbSet = this.dbContext.Set<Log>();


            return await Task.Run(() => dbSet.Where(l => l.AuthorId == int.MaxValue).ToListAsync());
        }

      

     

      
    }
        
}
