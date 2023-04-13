
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
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Services.Auth
{


    public class AuthorAuthService : IAuthorAuthService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public AuthorAuthService(IUnitOfWorkRepo unitOfWorkRepo, IOptions<JWT> jwt)
        {

            _jwt = jwt.Value;
            this.unitOfWorkRepo = unitOfWorkRepo;
        }
        public async Task<AuthModel> GetTokenAsync(Login authorLogin)
        {
            var authModel = new AuthModel();

            var author = await unitOfWorkRepo.AuthorRepository.AutherAuth(authorLogin);

            if (author is null)
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var auth = new AuthModel { Email = author.Email, DisplayName = author.DisplayName, Roles = Roles.Author.ToString(), Id = author.Id };
            return await CreateToken.CreateJwtToken(auth, _jwt);
        }

        public async Task<AuthModel> RegisterAsync(CreateAuthor createAuthor)
        {
            var user = await unitOfWorkRepo.AuthorRepository.CheckNameAndEmail(createAuthor.DisplayName, createAuthor.Email);
            if (user is not null)
                return new AuthModel { Message = "Email Or DisplayName is already registered!" };
            var author = new Author
            {

                Email = createAuthor.Email,
                Password = createAuthor.Password,
                ProfilePicture = createAuthor.ProfilePicture,
                DisplayName = createAuthor.DisplayName,
                Bio = createAuthor.Bio,
            };

            await unitOfWorkRepo.AuthorRepository.AddAsync(author);



            if (await unitOfWorkRepo.CommitAsync() == false)
            {
                return new AuthModel { Message = "errors" };
            }
            var lastId = await unitOfWorkRepo.AuthorRepository.GetAllAsync();
            var id = lastId.Max(b => b.Id);
            var auth = new AuthModel { Email = createAuthor.Email, DisplayName = createAuthor.DisplayName, Roles = Roles.Author.ToString(), Id = id };
            return await CreateToken.CreateJwtToken(auth, _jwt);
        }
    }
}