using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace NewsApiRepositories.UnitOfWorkRepository.Interface
{
    public interface IUnitOfWorkRepo
    {
        public IBaseRepository<Image> ImageRepository { get; }
        public ILogRepository LogRepository { get; }
        public ILikeReository LikeRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public IUserRepository UserRepository { get; }
        public IAuthorRepository AuthorRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IArticleRepository ArticleRepository { get; }

        Task<bool> CommitAsync();
    }
}
