
using NewsApiData;
using NewsApiDomin.Models;
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

        public IBaseRepository<Log> LogRepository { get; private set; }

        public IBaseRepository<Author> AuthorRepository { get; private set; }

        public IBaseRepository<Like> LikeRepository { get; private set; }

        public IBaseRepository<FavoriteCategorie> FavoriteCategorieRepository { get; private set; }

        public IBaseRepository<Category> CategoryRepository { get; private set; }

        public IBaseRepository<Image> ImageRepository { get; private set; }

        public IBaseRepository<ArticleImage> ArticleImageRepository { get; private set; }

        public UnitOfWorkRepo(NewsApiDbContext dbContext)
        {
            this.dbContext = dbContext;
            ArticleRepository = new ArticleRepository(this.dbContext);
            CommentRepository = new CommentsRepository(this.dbContext);

            UserRepository = new BaseRepository<User>(this.dbContext);
            LogRepository = new BaseRepository<Log>(this.dbContext);
            AuthorRepository = new BaseRepository<Author>(this.dbContext);
            LikeRepository = new BaseRepository<Like>(this.dbContext);
            FavoriteCategorieRepository = new BaseRepository<FavoriteCategorie>(this.dbContext);
            CategoryRepository = new BaseRepository<Category>(this.dbContext);
            ImageRepository = new BaseRepository<Image>(this.dbContext);
            ArticleImageRepository = new BaseRepository<ArticleImage>(this.dbContext);


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
