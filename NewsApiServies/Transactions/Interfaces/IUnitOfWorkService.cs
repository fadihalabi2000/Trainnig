using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD.Interfaces;
using System.Threading.Tasks;

namespace Services.Transactions.Interfaces
{
    public interface IUnitOfWorkService
    {
        public IBaseCRUDService<Category> CategoryService { get; }
        public IArticleImageService ArticleImageService { get; }
        public IArticleService ArticleService { get; }
        public IAuthorService AuthorService { get; }
        public ICommentsService CommentsService { get; }
        public IFavoriteCategoryService FavoriteCategoryService { get; }
        public IImageService ImageService { get; }
        public ILikeService LikeService { get; }
        public ILogService LogService { get; }
        public IUsersService UsersService{ get; }
        Task<bool> CommitAsync();
    }
}
