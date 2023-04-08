using NewsApiDomin.Models;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface IUnitOfWorkRepo
    {
        public IBaseRepository<User> UserRepository { get; }
        public IBaseRepository<Log> LogRepository { get; }
        public IBaseRepository<Author> AuthorRepository { get; }
        public IBaseRepository<Like> LikeRepository { get; }  
        public IBaseRepository<Image> ImageRepository { get; }

        public IBaseRepository<Category> CategoryRepository { get; }
        public IArticleRepository ArticleRepository { get; }
        public ICommentsRepository CommentRepository { get; }
      
        Task<bool> CommitAsync();
    }
}
