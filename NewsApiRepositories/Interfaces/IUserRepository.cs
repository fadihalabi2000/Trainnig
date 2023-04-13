using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewsApiRepositories.Interfaces
{
    public interface IUserRepository:IBaseRepository<User>
    {
        Task<User> CheckNameAndEmail(string displayName, string email);
        Task<User> UserAuth (Login userLogin);

    }
}
