using DataAccess;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiServies.CRUD;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.CRUD.Interfaces;
using Services.Transactions.Interfaces;

namespace Services.Transactions
{
    /// <summary>
    /// Since we use dependency injection, the same dbContext will be injected in the UnitOfWork and we do not need to manually create the repositories
    /// </summary>
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly NewsApiDbContext dbContext;
        public IBaseCRUDService<Category> CategoryService { get; private set; }

        public IArticleImageService ArticleImageService { get; private set; }

        public IArticleService ArticleService { get; private set; }

        public IAuthorService AuthorService { get; private set; }

        public ICommentsService CommentsService { get; private set; }

        public IFavoriteCategoryService FavoriteCategoryService { get; private set; }

        public IImageService ImageService { get; private set; }

        public ILikeService LikeService { get; private set; }

        public ILogService LogService { get; private set; }

        public IUsersService UsersService { get; private set; }

        public UnitOfWorkService(NewsApiDbContext dbContext,IUnitOfWorkRepo unitOfWorkRepo)
        {
            this.dbContext = dbContext;
            CategoryService =new BaseCRUDService<Category>(unitOfWorkRepo.CategoryRepository);
            ArticleImageService = new ArticleImageService(unitOfWorkRepo);
            ArticleService = new ArticleService(unitOfWorkRepo);
            AuthorService = new AuthorService(unitOfWorkRepo);
            CommentsService = new CommentsService(unitOfWorkRepo);
            FavoriteCategoryService = new FavoriteCategoryService(unitOfWorkRepo);
            ImageService = new ImageService(unitOfWorkRepo);
            LikeService = new LikeService(unitOfWorkRepo);
            LogService = new LogService(unitOfWorkRepo);
            UsersService = new UsersService(unitOfWorkRepo);
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
