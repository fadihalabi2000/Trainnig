using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public new async Task<List<Author>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<Author>().Include(a=>a.Article);


            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public new async Task<Author> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<Author>().Include(a=>a.Article).ToListAsync();

            Author? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }
    }
}
