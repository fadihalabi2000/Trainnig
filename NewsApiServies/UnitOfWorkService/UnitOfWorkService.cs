

using NewsApiData;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.CRUD;
using NewsApiServies.CRUD.Interfaces;
using NewsApiServies.Pagination;
using NewsApiServies.Pagination.Interface;
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

        //CURD
        public IBaseCRUDService<Category> CategoryService { get; private set; }

        public IArticleService ArticleService { get; private set; }

        public IAuthorService AuthorService { get; private set; }

        public ICommentsService CommentsService { get; private set; }


        public IImageService ImageService { get; private set; }

        public ILikeService LikeService { get; private set; }

        public ILogService LogService { get; private set; }

        public IUsersService UsersService { get; private set; }

        //pagination
        public IPaginationService<Category> CategoryPagination { get; private set; }

        public IPaginationService<Article> ArticlePagination { get; private set; }

        public IPaginationService<Author> AuthorPagination { get; private set; }

        public IPaginationService<Log> LogPagination { get; private set; }

        public IPaginationService<User> UserPagination { get; private set; }

        public IPaginationService<Comment> CommentPagination { get; private set; }

        public IPaginationService<Image> ImagePagination { get; private set; }

        public UnitOfWorkService(NewsApiDbContext dbContext,IUnitOfWorkRepo unitOfWorkRepo)
        {
            this.dbContext = dbContext;
            CategoryService =new BaseCRUDService<Category>(unitOfWorkRepo.CategoryRepository);
            ArticleService = new ArticleService(unitOfWorkRepo);
            AuthorService = new AuthorService(unitOfWorkRepo);
            CommentsService = new CommentsService(unitOfWorkRepo);
  
            ImageService = new ImageService(unitOfWorkRepo);
            LikeService = new LikeService(unitOfWorkRepo);
            LogService = new LogService(unitOfWorkRepo);
            UsersService = new UsersService(unitOfWorkRepo);

            //pagination
            CategoryPagination = new PaginationService<Category>();
            AuthorPagination = new PaginationService<Author>();
            ArticlePagination = new PaginationService<Article>();
            UserPagination = new PaginationService<User>();
            LogPagination = new PaginationService<Log>();
            CommentPagination = new PaginationService<Comment>();
            ImagePagination = new PaginationService<Image>();

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
