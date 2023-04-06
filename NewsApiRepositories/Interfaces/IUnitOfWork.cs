using DataAccess.Entities.Abstractions.Interfaces;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface IUnitOfWork 
    {
        public IBaseRepository<User> UserRepository { get; }
        public IBaseRepository<Log> LogRepository { get; }
        public IBaseRepository<Author> AuthorRepository { get; }
        public IBaseRepository<Like> LikeRepository { get; }
        public IBaseRepository<FavoriteCategorie> FavoriteCategorieRepository { get; }
        public IBaseRepository<Category> CategoryRepository { get; }
        public IBaseRepository<Image> ImageRepository { get; }
        public IBaseRepository<ArticleImage> ArticleImageRepository { get; }

       
        public IArticleRepository ArticleRepository { get; }
        public ICommentsRepository CommentRepository { get; }
     
        Task<bool> CommitAsync();
    }
}
