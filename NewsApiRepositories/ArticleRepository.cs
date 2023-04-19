
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
           // .Include(c => c.Comments.OrderByDescending(c => c.CommentDate)).Include(l => l.Likes.OrderByDescending(l => l.LikeDate))
            var dbSet = this.dbContext.Set<Article>().Include(i=>i.Images).Include(c => c.Comments.OrderByDescending(c => c.CommentDate)).Include(l => l.Likes.OrderByDescending(l => l.LikeDate));

            return await Task.Run(() => dbSet.Where(c => c.IsDeleted == false).OrderByDescending(d=>d.PublishDate).ToListAsync());
        }
        public new async Task<Article> GetByIdAsync(int id)
        {

            var dbSet = await this.dbContext.Set<Article>().Include(i => i.Images).Include(c => c.Comments.OrderByDescending(c => c.CommentDate)).Include(l => l.Likes.OrderByDescending(l => l.LikeDate)).ToListAsync();

            Article? entity = dbSet.FirstOrDefault(c => c.Id == id && c.IsDeleted == false);
            return entity!;


        }
    }
}
