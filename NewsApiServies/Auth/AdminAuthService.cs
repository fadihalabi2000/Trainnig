using AutoMapper;
using Microsoft.Extensions.Options;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.ClassStatic;
using NewsApiServies.Auth.Interfaces;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Auth
{
    public class AdminAuthService:IAdminAuthService
    {

            private readonly JWT _jwt;
            private readonly IUnitOfWorkService unitOfWorkService;
            private readonly IMapper mapper;

            public AdminAuthService(IUnitOfWorkService unitOfWorkService, IOptions<JWT> jwt, IMapper mapper)
            {

                _jwt = jwt.Value;
                this.unitOfWorkService = unitOfWorkService;
                this.mapper = mapper;
            }

            public async Task<AuthModel> GetTokenAsync(Login userLogin)
            {
                var authModel = new AuthModel();
            if (userLogin.Email == "obada@gmail.com")
            {


                var user = await unitOfWorkService.UsersService.UserAuth(userLogin);

                if (user is null)
                {
                    authModel.Message = "Email or Password is incorrect!";
                    return authModel;
                }
                var auth = new AuthModel { Email = user.Email, DisplayName = user.DisplayName, Roles = Role.Admin, Id = user.Id };
                auth = await CreateToken.CreateJwtToken(auth, _jwt);
                var IsActive = user.RefreshTokens!.Any(t =>  (DateTime.UtcNow >= t.ExpiresOn));
                IsActive = user.RefreshTokens!.Any(t => t.RevokedOn == null && !IsActive);


                if (IsActive)
                {
                    var activeRefreshToken = user.RefreshTokens.LastOrDefault(t => t.ExpiresOn>DateTime.UtcNow);
                    auth.RefreshToken = activeRefreshToken!.Token;
                    auth.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
                }
                else
                {
                    var refreshToken = CreateToken.GenerateRefreshToken();
                    auth.RefreshToken = refreshToken.Token;
                    auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
                   refreshToken.UserId=user.Id;
                    user.RefreshTokens.Add(refreshToken);
                    await unitOfWorkService.UsersService.UpdateAsync(user!);
                    var x = user.RefreshTokens;
                    await unitOfWorkService.CommitAsync();
                }
                return auth;
            }
            else
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            }

            public async Task<AuthModel> RefreshTokenAsync(string token)
            {
                var authModel = new AuthModel();
                var users = await unitOfWorkService.UsersService.GetAllAsync();
                var user = users.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == token));

                if (user == null)
                {
                    authModel.Message = "Invalid token";
                    return authModel;
                }

                var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

                if (!refreshToken.IsActive)
                {
                    authModel.Message = "Inactive token";
                    return authModel;
                }

                refreshToken.RevokedOn = DateTime.UtcNow;

                var newRefreshToken = CreateToken.GenerateRefreshToken();
                user.RefreshTokens!.Add(newRefreshToken);
                await unitOfWorkService.UsersService.UpdateAsync(user);
                await unitOfWorkService.CommitAsync();
                var auth = new AuthModel { Email = user.Email, DisplayName = user.DisplayName, Roles = Role.Admin.ToString(), Id = user.Id };
                auth = await CreateToken.CreateJwtToken(auth, _jwt);
            auth.IsAuthenticated = true;
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                return auth;
            }

            public async Task<bool> RevokeTokenAsync(string token)
            {
                var users = await unitOfWorkService.UsersService.GetAllAsync();
                var user = users.LastOrDefault(u=>u.RefreshTokens.Any(r=>r.Token==token));

                if (user == null)
                    return false;

                var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

                if (!refreshToken.IsActive)
                    return false;

                refreshToken.RevokedOn = DateTime.UtcNow;
                await unitOfWorkService.UsersService.UpdateAsync(user);
                await unitOfWorkService.CommitAsync();

                return true;
            }



        
    }
}
