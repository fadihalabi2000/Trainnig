
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories;
using NewsApiRepositories.Interfaces;
using Repositories;
using Repositories.Interfaces;

using Services.Transactions.Interfaces;
using System.Threading.Tasks;

namespace Services.Transactions
{
   
    public class UnitOfWorkRepo : IUnitOfWorkRepo
    {
        private readonly NewsApiDbContext dbContext;
      
        public IArticleRepository ArticleRepository { get; private set; }

        public ICommentsRepository CommentRepository { get; private set; }

        public IBaseRepository<User> UserRepository { get; private set; }

        public ILogRepository LogRepository { get; private set; }

        public IBaseRepository<Author> AuthorRepository { get; private set; }

        public IBaseRepository<Like> LikeRepository { get; private set; }


        public ICategoryRepository CategoryRepository { get; private set; }

        public IBaseRepository<Image> ImageRepository { get; private set; }


        public UnitOfWorkRepo(NewsApiDbContext dbContext)
        {
            this.dbContext = dbContext;
            ArticleRepository = new ArticleRepository(this.dbContext);
            CommentRepository = new CommentsRepository(this.dbContext);
            CategoryRepository = new CategoryReository(this.dbContext);
            LogRepository = new LogRepository(this.dbContext);

            UserRepository = new BaseRepository<User>(this.dbContext);
            AuthorRepository = new BaseRepository<Author>(this.dbContext);
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
