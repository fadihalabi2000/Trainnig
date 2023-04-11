using NewsApiDomin.Models;
using Services.CRUD.Interfaces;


namespace NewsApiServies.CRUD.Interfaces
{
    public interface ILogService:IBaseCRUDService<Log>
    {
        Task<List<Log>> GetAllUsersLogAsync();
        Task<List<Log>> GetAllAuthorsLogAsync();
        Task<List<Log>> GetLogAuthorByIdAsync(int id);
        Task<List<Log>> GetLogUserByIdAsync(int id);
   
    }
}
