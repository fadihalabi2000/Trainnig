using NewsApiDomin.Models;

namespace Services.CRUD.Interfaces
{
    public interface ICommentsService : IBaseCRUDService<Comment>
    {
        Task<List<Comment>> GetAllByIdArticleAsync(int articleId);
    }
}
