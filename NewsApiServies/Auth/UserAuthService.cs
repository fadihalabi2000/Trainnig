
using AutoMapper;
using DataAccess.Entities.Abstractions.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth.ClassStatic;
using NewsApiServies.Auth.Interfaces;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Services.Auth
{
    public class UserAuthService : IUserAuthService
    {
 
        private readonly JWT _jwt;
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMapper mapper;

        public UserAuthService(IUnitOfWorkService unitOfWorkService, IOptions<JWT> jwt,IMapper mapper)
        {

            _jwt = jwt.Value;
            this.unitOfWorkService = unitOfWorkService;
            this.mapper = mapper;
        }

        public async Task<AuthModel> RegisterAsync(CreateUser createUser)
        {
            User user = await unitOfWorkService.UsersService.CheckNameAndEmail(createUser.DisplayName, createUser.Email);
            if (user is not null)
                return new AuthModel { Message = "Email Or DisplayName is already registered!" };

            mapper.Map(createUser, user);
            user = mapper.Map<User>(createUser);
            //user = new User
            //{
            //    FirstName = createUser.FirstName,
            //    LastName = createUser.LastName,
            //    Email = createUser.Email,
            //    Password = createUser.Password,
            //    ProfilePicture = createUser.ProfilePicture,
            //    DisplayName = createUser.DisplayName,
            //};

            await unitOfWorkService.UsersService.AddAsync(user!);

           

            if (await unitOfWorkService.CommitAsync()==false)
            {
                return new AuthModel { Message = "errors" };
            }
            var refreshToken = CreateToken.GenerateRefreshToken();
            user!.RefreshTokens?.Add(refreshToken);
            await unitOfWorkService.UsersService.UpdateAsync(user!);
            await unitOfWorkService.CommitAsync();


            var lastId = await unitOfWorkService.UsersService.GetAllAsync();
            var id = lastId.Max(b => b.Id);
            var auth=new AuthModel { Email = createUser.Email,DisplayName=createUser.DisplayName,Roles=Role.User,Id=id };
            auth = await CreateToken.CreateJwtToken(auth, _jwt);
            auth.RefreshToken = refreshToken.Token;
            auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
            return auth;


        }

        public async Task<AuthModel> GetTokenAsync(Login userLogin)
        {
            var authModel = new AuthModel();

            var user = await unitOfWorkService.UsersService.UserAuth(userLogin);

            if (user is null )
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var auth = new AuthModel { Email = user.Email, DisplayName = user.DisplayName, Roles = Role.User, Id = user.Id };
            auth = await CreateToken.CreateJwtToken(auth, _jwt);
            var IsActive = user.RefreshTokens!.Any(t => (DateTime.UtcNow >= t.ExpiresOn));
            IsActive = user.RefreshTokens!.Any(t => t.RevokedOn == null && !IsActive);

            if (IsActive)
            {
                var activeRefreshToken = user.RefreshTokens!.LastOrDefault(t => t.ExpiresOn > DateTime.UtcNow);
                auth.RefreshToken = activeRefreshToken!.Token;
                auth.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = CreateToken.GenerateRefreshToken();
                auth.RefreshToken = refreshToken.Token;
                auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens!.Add(refreshToken);
                await unitOfWorkService.UsersService.UpdateAsync(user!);
            }
            return auth;

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
            var auth = new AuthModel { Email = user.Email, DisplayName = user.DisplayName, Roles = Role.User.ToString(), Id = user.Id };
            auth = await CreateToken.CreateJwtToken(auth, _jwt);
            auth.IsAuthenticated = true;
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            return auth;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var users = await unitOfWorkService.UsersService.GetAllAsync();
            var user = users.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == token));

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