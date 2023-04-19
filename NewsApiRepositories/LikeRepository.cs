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
    public class LikeRepository :BaseRepository<Like>, ILikeReository
    {
        private readonly NewsApiDbContext dbContext;

        public LikeRepository(NewsApiDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
