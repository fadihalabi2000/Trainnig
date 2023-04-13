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

        public async Task<User> CheckNameAndEmail(string displayName, string email)
        {
            var dbSet = await this.dbContext.Set<User>().FirstOrDefaultAsync(u=>u.DisplayName==displayName&&u.Email==email);
            return dbSet!;
        }

        public new async Task<List<User>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<User>().Include(l=>l.likes).Include(c=>c.Comments);


            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public new async Task<User> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<User>().Include(l => l.likes).Include(c => c.Comments).ToListAsync();

            User? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }

        public async Task<User> UserAuth(Login userLogin)
        {
            var dbSet = await this.dbContext.Set<User>().FirstOrDefaultAsync(u => u.Password == userLogin.Password && u.Email == userLogin.Email);
            return dbSet!;
        }
    }
}
