using JWTRefreshTokenInDotNet6.Models;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiServies.Auth.Interfaces;
using Services.Auth;

namespace NewsApi.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminAuthService adminAuthService;

        public AdminController(IAdminAuthService  adminAuthService)
        {
            this.adminAuthService = adminAuthService;
        }



        [HttpPost("AdminLogin")]
        public async Task<ActionResult<AuthModel>> GetTokenAsync(Login authorLogin)
        {

            var result = await adminAuthService.GetTokenAsync(authorLogin);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            if (result.RefreshToken!=String.Empty&&result.RefreshToken!=null)
                SetRefreshTokenInCookie(result.RefreshToken!, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await adminAuthService.RefreshTokenAsync(refreshToken!);

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

            var result = await adminAuthService.RevokeTokenAsync(token);

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
