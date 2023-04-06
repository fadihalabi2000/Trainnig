using DataAccess.Entities;
using NewsApiDomin.Models;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Services.CRUD.Interfaces
{
    public interface IUsersService : IBaseCRUDService<User>
    {
       
    }
}
