
using NewsApiData;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;

namespace Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {


        public ArticleRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task AddAsync(Article entity)
        {
            DateTime dateTimeUtcNow = DateTime.UtcNow;
            entity.PublishDate = dateTimeUtcNow;
            entity.UpdateDate = dateTimeUtcNow;

            await Task.Run(() => base.AddAsync(entity));
        }

        public new async Task UpdateAsync(Article entity)
        {
            entity.UpdateDate = DateTime.UtcNow;
            await Task.Run(() => base.UpdateAsync(entity));
        }
    }
}
