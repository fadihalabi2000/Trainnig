﻿using JWTRefreshTokenInDotNet6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.Interfaces;
using NewsApiServies.CRUD.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService userAuthService;
        private readonly IWebHostEnvironment environment;
        private readonly IUnitOfWorkService unitOfWorkService;

        public UserAuthController(IUserAuthService userAuthService, IWebHostEnvironment env, IUnitOfWorkService unitOfWorkService)
        {
            this.userAuthService = userAuthService;
            this.environment = env;
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpPost("UserRegister")]
        public async Task<ActionResult<AuthModel>> RegisterAsync( [FromForm]CreateUser createUser)
        {
            var contentPath = this.environment.ContentRootPath;
            List<Image> image = await unitOfWorkService.ArticleService.UploadImage(createUser.ProfilePicture, contentPath);

            var result = await userAuthService.RegisterAsync(createUser,image);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("UserLogin")]
        public async Task<ActionResult<AuthModel>> GetTokenAsync( Login  userLogin )
        {
        
            var result = await userAuthService.GetTokenAsync(userLogin);

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

            var result = await userAuthService.RefreshTokenAsync(refreshToken!);

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

            var result = await userAuthService.RevokeTokenAsync(token);

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
