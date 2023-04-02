using Microsoft.EntityFrameworkCore;

namespace NewsApiData
{
    public class NewsApiDbContext : DbContext
    {
        public NewsApiDbContext(DbContextOptions<NewsApiDbContext> options) : base(options)
        {

        }
    }
}