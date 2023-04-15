using NewsApiDomin.Models;
using Services.CRUD.Interfaces;


namespace NewsApiServies.CRUD.Interfaces
{
    public interface IAuthorService :IBaseCRUDService<Author>
    {
        Task<Author> CheckDisplayName(string displayName);
    }
}
