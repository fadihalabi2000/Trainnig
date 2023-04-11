using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.LogViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public UserLogController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }
        [HttpGet("GetAllUsersLog")]
        public async Task<ActionResult<List<UserLogView>>> GetAllUsersLog()
        {

            try
            {
                var logs = await unitOfWorkService.LogService.GetAllUsersLogAsync();
                var usersLogView = logs.Select(l => new UserLogView { Content = l.Content, UserId = l.UserId, logLevel = l.logLevel });
                if (usersLogView.Count() > 0)
                    return Ok(usersLogView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }


        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLogView>> GetAllLogUserById(int id)
        {


            try
            {
                var log = await unitOfWorkService.LogService.GetLogUserByIdAsync(id);
                var userLogView = log.Select(l => new UserLogView { Content = l.Content, UserId = l.UserId, logLevel = l.logLevel });
                if (userLogView == null)
                    return BadRequest();
                else
                    return Ok(userLogView);

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
