using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.Interfaces;
using NewsApiServies.CRUD.Interfaces;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService userAuthService;

        public UserAuthController(IUserAuthService userAuthService)
        {
            this.userAuthService = userAuthService;
        }

        [HttpPost("UserRegister")]
        public async Task<ActionResult<AuthModel>> RegisterAsync( CreateUser createUser)
        {
         

            var result = await userAuthService.RegisterAsync(createUser);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("UserLogin")]
        public async Task<ActionResult<AuthModel>> GetTokenAsync( Login  userLogin )
        {
        
            var result = await userAuthService.GetTokenAsync(userLogin);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
