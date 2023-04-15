using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiRepositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {

        private readonly NewsApiDbContext dbContext;

        public AuthorRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Author> CheckNameAndEmail(string displayName, string email)
        {
            var dbSet = await this.dbContext.Set<Author>().FirstOrDefaultAsync(a => a.DisplayName==displayName&&a.Email==email);
            return dbSet!;
        }


        public new async Task<List<Author>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<Author>().Include(a=>a.Article.OrderByDescending(a=>a.PublishDate));


            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).OrderByDescending(a=>a.DisplayName).ToListAsync());
        }
        public new async Task<Author> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<Author>().Include(a=>a.Article.OrderByDescending(a => a.PublishDate)).ToListAsync();

            Author? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }

        public async Task<Author> AutherAuth(Login authorLogin)
        {
            var dbSet = await this.dbContext.Set<Author>().FirstOrDefaultAsync(a => a.Email == authorLogin.Email && a.Password == authorLogin.Password);
            return dbSet!;
        }

        public async Task<Author> CheckDisplayName(string displayName)
        {
            var dbSet = await this.dbContext.Set<Author>().FirstOrDefaultAsync(a => a.DisplayName == displayName );
            return dbSet!;
        }
    }
}
