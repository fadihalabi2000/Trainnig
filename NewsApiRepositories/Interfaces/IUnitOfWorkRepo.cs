using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface IUnitOfWorkRepo
    {
        public IBaseRepository<User> UserRepository { get; }
        public ILogRepository LogRepository { get; }
        public IBaseRepository<Author> AuthorRepository { get; }
        public IBaseRepository<Like> LikeRepository { get; }  
        public IBaseRepository<Image> ImageRepository { get; }

        public ICategoryRepository CategoryRepository { get; }
        public IArticleRepository ArticleRepository { get; }
        public ICommentsRepository CommentRepository { get; }
      
        Task<bool> CommitAsync();
    }
}
