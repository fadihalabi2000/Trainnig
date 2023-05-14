
using AutoMapper;
using DataAccess.Entities.Abstractions.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth.ClassStatic;
using NewsApiServies.Auth.Interfaces;
using Services.Transactions.Interfaces;

namespace Services.Auth
{


    public class AuthorAuthService : IAuthorAuthService
    {
        private readonly JWT _jwt;
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMapper mapper;

        public AuthorAuthService( IUnitOfWorkService unitOfWorkService, IOptions<JWT> jwt,IMapper mapper)
        {

            _jwt = jwt.Value;
            this.unitOfWorkService = unitOfWorkService;
            this.mapper = mapper;
        }

        public async Task<AuthModel> RegisterAsync(CreateAuthor createAuthor,List<Image> image)
        {
            Author author = await unitOfWorkService.AuthorService.CheckNameAndEmail(createAuthor.DisplayName, createAuthor.Email);
            if (author is not null)
                return new AuthModel { Message = "Email Or DisplayName is already registered!" };
           
            author = mapper.Map<Author>(createAuthor);
            author.ProfilePicture = image[0].ImageUrl;
            await unitOfWorkService.AuthorService.AddAsync(author);

            if (await unitOfWorkService.CommitAsync() == false)
            {
                return new AuthModel { Message = "errors" };
            }
            //refresh
            var refreshToken =CreateToken.GenerateRefreshToken();
            author!.RefreshTokens?.Add(refreshToken);
            await unitOfWorkService.AuthorService.UpdateAsync(author!);
            await unitOfWorkService.CommitAsync(); 


            var lastId = await unitOfWorkService.AuthorService.GetAllAsync();
            var id = lastId.Max(b => b.Id);
            var auth = new AuthModel { Email = createAuthor.Email, DisplayName = createAuthor.DisplayName, Roles = Role.Author.ToString(), Id = id ,RefreshToken=refreshToken.Token};
            auth=   await CreateToken.CreateJwtToken(auth, _jwt);
            auth.RefreshToken =refreshToken.Token;
            auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
            return auth;
        }
        public async Task<AuthModel> GetTokenAsync(Login authorLogin)
        {
            var authModel = new AuthModel();

            var author = await unitOfWorkService.AuthorService.AutherAuth(authorLogin);

            if (author is null)
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var auth = new AuthModel { Email = author.Email, DisplayName = author.DisplayName, Roles = Role.Author.ToString(), Id = author.Id };
            auth  = await CreateToken.CreateJwtToken(auth, _jwt);
            var IsActive = author.RefreshTokens!.Any(t => (DateTime.UtcNow >= t.ExpiresOn));
            IsActive = author.RefreshTokens!.Any(t => t.RevokedOn == null && !IsActive);

            if (IsActive)
            {
                var activeRefreshToken = author.RefreshTokens!.LastOrDefault(t => t.ExpiresOn > DateTime.UtcNow);
                auth.RefreshToken = activeRefreshToken!.Token;
                auth.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken =CreateToken.GenerateRefreshToken();
                auth.RefreshToken = refreshToken.Token;
                auth.RefreshTokenExpiration = refreshToken.ExpiresOn;
                author.RefreshTokens!.Add(refreshToken);
                await unitOfWorkService.AuthorService.UpdateAsync(author!);
                await unitOfWorkService.CommitAsync();
            }
            return auth;
        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();
            var authors = await unitOfWorkService.AuthorService.GetAllAsync();
            var author =  authors.SingleOrDefault(u => u.RefreshTokens!.Any(t => t.Token == token));

            if (author == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = author.RefreshTokens!.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken =CreateToken.GenerateRefreshToken();
            author.RefreshTokens!.Add(newRefreshToken);
            await unitOfWorkService.AuthorService.UpdateAsync(author);
            await unitOfWorkService.CommitAsync();
            var auth = new AuthModel { Email = author.Email, DisplayName = author.DisplayName, Roles = Role.Author.ToString(), Id = author.Id };
            auth = await CreateToken.CreateJwtToken(auth, _jwt);
            auth.IsAuthenticated = true;
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return auth;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var authors = await unitOfWorkService.AuthorService.GetAllAsync();
            var author =  authors.LastOrDefault(u => u.RefreshTokens!.Any(t => t.Token == token));

            if (author == null)
                return false;

            var refreshToken = author.RefreshTokens!.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            refreshToken.RevokedOn = DateTime.UtcNow;
            await unitOfWorkService.AuthorService.UpdateAsync(author);
            await unitOfWorkService.CommitAsync();

            return true;
        }
    }
}