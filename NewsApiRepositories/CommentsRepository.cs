
using NewsApiData;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;

namespace Repositories
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task AddAsync(Comment entity)
        {
            DateTime dateTimeUtcNow = DateTime.UtcNow;
            entity.CommentDate = dateTimeUtcNow;

            await Task.Run(() => base.AddAsync(entity));
        }

        public new async Task UpdateAsync(Comment entity)
        {
            await Task.Run(() => base.UpdateAsync(entity));
        }
      

     
    }
}
