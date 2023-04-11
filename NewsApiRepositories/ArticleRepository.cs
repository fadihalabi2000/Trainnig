
using Microsoft.EntityFrameworkCore;
using NewsApiData;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;

namespace Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        private readonly NewsApiDbContext dbContext;

        public ArticleRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public new async Task<List<Article>> GetAllAsync()
        {
            var dbSet = this.dbContext.Set<Article>().Include(i=>i.Images).Include(c=>c.Comments).Include(l=>l.Likes);


            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public new async Task<Article> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<Article>().Include(i => i.Images).Include(c => c.Comments).Include(l => l.Likes).ToListAsync();

            Article? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }
    }
}
