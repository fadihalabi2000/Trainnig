using JWTRefreshTokenInDotNet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorAuthController : ControllerBase
    {
        private readonly IAuthorAuthService authorAuthService;
        private readonly IWebHostEnvironment environment;
        private readonly IUnitOfWorkService unitOfWorkService;

        public AuthorAuthController(IAuthorAuthService authorAuthService, IWebHostEnvironment env,IUnitOfWorkService unitOfWorkService)
        {
            this.authorAuthService = authorAuthService;
            this.environment = env;
            this.unitOfWorkService = unitOfWorkService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AuthorRegister")]
            public async Task<ActionResult<AuthModel>> RegisterAsync([FromForm]CreateAuthor createAuthor)
        {
            var contentPath = this.environment.ContentRootPath;

           List<Image> image = await unitOfWorkService.ArticleService.UploadImage(createAuthor.ProfilePicture, contentPath);
            var result = await authorAuthService.RegisterAsync(createAuthor,image);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
            SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);
            return Ok(result);
            }
        [HttpPost("AutherLogin")]
            public async Task<ActionResult<AuthModel>> GetTokenAsync(Login authorLogin)
            {

                var result = await authorAuthService.GetTokenAsync(authorLogin);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
            }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await authorAuthService.RefreshTokenAsync(refreshToken!);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await authorAuthService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
