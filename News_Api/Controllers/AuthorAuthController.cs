using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorAuthController : ControllerBase
    {
        private readonly IAuthorAuthService authorAuthService;

        public AuthorAuthController(IAuthorAuthService authorAuthService)
        {
            this.authorAuthService = authorAuthService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AuthorRegister")]
            public async Task<ActionResult<AuthModel>> RegisterAsync(CreateAuthor createAuthor)
            {


                var result = await authorAuthService.RegisterAsync(createAuthor);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                return Ok(result);
            }
        [HttpPost("AutherLogin")]
            public async Task<ActionResult<AuthModel>> GetTokenAsync(Login authorLogin)
            {

                var result = await authorAuthService.GetTokenAsync(authorLogin);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);

                return Ok(result);
            }
        }
}
