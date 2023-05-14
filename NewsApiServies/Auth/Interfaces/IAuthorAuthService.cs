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
        Task<AuthModel> RegisterAsync(CreateAuthor createAuthor, List<Image> image);
        Task<AuthModel> GetTokenAsync(Login authorLogin);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

    }
}

