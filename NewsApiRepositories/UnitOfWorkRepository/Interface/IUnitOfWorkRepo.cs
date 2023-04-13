using NewsApiDomin.Models;
using NewsApiRepositories.Interfaces;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace NewsApiRepositories.UnitOfWorkRepository.Interface
{
    public interface IUnitOfWorkRepo
    {

        public ILogRepository LogRepository { get; }

        public IBaseRepository<Like> LikeRepository { get; }
        public IBaseRepository<Image> ImageRepository { get; }
        public IBaseRepository<Comment> CommentRepository { get; }

        public IUserRepository UserRepository { get; }
        public IAuthorRepository AuthorRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IArticleRepository ArticleRepository { get; }


        Task<bool> CommitAsync();
    }
}
