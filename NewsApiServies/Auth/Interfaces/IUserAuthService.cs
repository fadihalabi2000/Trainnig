﻿using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Auth.Interfaces
{
    public interface IUserAuthService
    {
       
         Task<AuthModel> RegisterAsync(CreateUser createUser, List<Image> image);
         Task<AuthModel> GetTokenAsync(Login userLogin);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);



    }
}
