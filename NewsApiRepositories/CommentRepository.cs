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
    public class CommentRepository:BaseRepository<Comment>,ICommentRepository
    {
        private readonly NewsApiDbContext dbContext;

        public CommentRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
