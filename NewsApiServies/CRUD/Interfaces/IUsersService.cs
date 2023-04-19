using DataAccess.Entities;
using NewsApiDomin.Models;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Services.CRUD.Interfaces
{
    public interface IUsersService : IBaseCRUDService<User>
    {
        Task<User> CheckNameAndEmail(string displayName, string email);
        Task<User> CheckDisplayName(string displayName);
        Task<User> UserAuth(Login userLogin);
    }
}
