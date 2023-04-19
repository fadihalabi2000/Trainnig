using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiRepositories.Interfaces
{
    public interface IAuthorRepository:IBaseRepository<Author>
    {
        //Task<Author> CheckNameAndEmail(string displayName, string email);
        //Task<Author> CheckDisplayName(string displayName);
        //Task<Author> AutherAuth(Login authorLogin);
    }
}
