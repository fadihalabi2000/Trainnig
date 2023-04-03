using Microsoft.EntityFrameworkCore;
using NewsApiDomin.Models;

namespace NewsApiData
{
    public class NewsApiDbContext : DbContext
    {
        public NewsApiDbContext(DbContextOptions<NewsApiDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ArticleImage> ArticleImages { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FavoriteCategorie> FavoriteCategorias { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}