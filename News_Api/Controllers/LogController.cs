using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.LogViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public LogController(IUnitOfWorkService unitOfWorkService)
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



        [HttpGet("GetAllAuthorLog")]
        public async Task<ActionResult<List<AuthorLogView>>> GetAllAuthorLog()
        {

            try
            {
                var logs = await unitOfWorkService.LogService.GetAllAuthorsLogAsync();
                var authorsLogView = logs.Select(l => new AuthorLogView { Content = l.Content, AuthorId = l.AuthorId, logLevel = l.logLevel });
                if (authorsLogView.Count() > 0)
                    return Ok(authorsLogView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> GetById(int id)
        {


            try
            {
                var log = await unitOfWorkService.LogService.GetByIdAsync(id);
               
                if (log == null)
                    return BadRequest();
                else
                    return Ok(log);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }



    }
}
