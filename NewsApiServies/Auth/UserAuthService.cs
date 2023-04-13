
using DataAccess.Entities.Abstractions.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth.ClassStatic;
using NewsApiServies.Auth.Interfaces;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;
using Services.Transactions;
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
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public UserAuthService(IUnitOfWorkRepo unitOfWorkRepo, IOptions<JWT> jwt)
        {

            _jwt = jwt.Value;
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

        public async Task<AuthModel> RegisterAsync(CreateUser createUser)
        {
            var user = await unitOfWorkRepo.UserRepository.CheckNameAndEmail(createUser.DisplayName, createUser.Email);
            if (user is not null)
                return new AuthModel { Message = "Email Or DisplayName is already registered!" };
            user = new User
            {
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                Email = createUser.Email,
                Password = createUser.Password,
                ProfilePicture = createUser.ProfilePicture,
                DisplayName = createUser.DisplayName,
            };
           
               await  unitOfWorkRepo.UserRepository.AddAsync(user);

           

            if (await unitOfWorkRepo.CommitAsync()==false)
            {
                return new AuthModel { Message = "errors" };
            }
            var lastId = await unitOfWorkRepo.UserRepository.GetAllAsync();
            var id = lastId.Max(b => b.Id);
            var auth=new AuthModel { Email = createUser.Email,DisplayName=createUser.DisplayName,Roles=Roles.User.ToString(),Id=id };
             return await  CreateToken.CreateJwtToken(auth,_jwt);

   

         
        }

        public async Task<AuthModel> GetTokenAsync(Login userLogin)
        {
            var authModel = new AuthModel();

            var user = await unitOfWorkRepo.UserRepository.UserAuth(userLogin);

            if (user is null )
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var auth = new AuthModel { Email = user.Email, DisplayName = user.DisplayName, Roles = Roles.User.ToString(), Id = user.Id };
            return await CreateToken.CreateJwtToken(auth, _jwt);

        }


      
    }
}