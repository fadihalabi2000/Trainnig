using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiRepositories
{
    public class UserRepository:BaseRepository<User>, IUserRepository
    {
        private readonly NewsApiDbContext dbContext;

        public UserRepository(NewsApiDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }


        public new async Task<List<User>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<User>().Include(r=>r.RefreshTokens);
            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public new async Task<User> GetByIdAsync(int id)
        {
            var dbSet = await this.dbContext.Set<User>().Include(l => l.likes).Include(c => c.Comments).ToListAsync();
            User? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;
        }

    }
}
