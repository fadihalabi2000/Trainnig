using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Auth.Interfaces
{
    public interface IAuthorAuthService
    {
        Task<AuthModel> RegisterAsync(CreateAuthor createAuthor);
        Task<AuthModel> GetTokenAsync(Login authorLogin);


    }
}

