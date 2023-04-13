
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories;
using NewsApiRepositories.Interfaces;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Repositories;
using Repositories.Interfaces;
using System.Threading.Tasks;

namespace NewsApiRepositories.UnitOfWorkRepository
{

    public class UnitOfWorkRepo : IUnitOfWorkRepo
    {
        private readonly NewsApiDbContext dbContext;

        public IArticleRepository ArticleRepository { get; private set; }

        public IBaseRepository<Comment> CommentRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public ILogRepository LogRepository { get; private set; }

        public IAuthorRepository AuthorRepository { get; private set; }

        public IBaseRepository<Like> LikeRepository { get; private set; }


        public ICategoryRepository CategoryRepository { get; private set; }

        public IBaseRepository<Image> ImageRepository { get; private set; }


        public UnitOfWorkRepo(NewsApiDbContext dbContext)
        {
            this.dbContext = dbContext;
            ArticleRepository = new ArticleRepository(this.dbContext);

            CategoryRepository = new CategoryReository(this.dbContext);
            LogRepository = new LogRepository(this.dbContext);

            CommentRepository = new BaseRepository<Comment>(this.dbContext);
            UserRepository = new UserRepository(this.dbContext);
            AuthorRepository = new AuthorRepository(this.dbContext);
            LikeRepository = new BaseRepository<Like>(this.dbContext);

            ImageRepository = new BaseRepository<Image>(this.dbContext);



        }

        public async Task<bool> CommitAsync()
        {
            if (await dbContext.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }


    }
}
