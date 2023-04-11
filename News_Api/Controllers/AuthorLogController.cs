using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.LogViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public AuthorLogController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
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


        [HttpGet("{id}", Name = "GetAllLogAuthorById")]
        public async Task<ActionResult<AuthorLogView>> GetAllLogAuthorById(int id)
        {


            try
            {
                var log = await unitOfWorkService.LogService.GetLogAuthorByIdAsync(id);
                var authorLogView = log.Select(l => new AuthorLogView { Content = l.Content, AuthorId = l.AuthorId, logLevel = l.logLevel });
                if (authorLogView == null)
                    return BadRequest();
                else
                    return Ok(authorLogView);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
    }
}
