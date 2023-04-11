using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories;
using System.Collections.Generic;


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

      

        public async Task<List<Log>> GetLogAuthorByIdAsync(int id)
        {
            var dbSet = this.dbContext.Set<Log>();
            return await Task.Run(() => dbSet.Where(c => c.AuthorId ==id).ToListAsync());


        }

       

        public async Task<List<Log>> GetLogUserByIdAsync(int id)
        {
            var dbSet = this.dbContext.Set<Log>();
            return await Task.Run(() => dbSet.Where(c => c.UserId == id).ToListAsync());
        }

        public async Task<List<Log>> GetAllUsersLogAsync()
        {
            var dbSet = this.dbContext.Set<Log>();


            return await Task.Run(() => dbSet.Where(l => l.AuthorId == int.MaxValue).ToListAsync());
        }



  




    }
        
}
