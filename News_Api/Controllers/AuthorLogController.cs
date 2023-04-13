using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.LogViewModel;
using Services.Transactions.Interfaces;
using System.Text.Json;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AuthorLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public AuthorLogController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }
        [HttpGet("GetAllAuthorLog")]
        public async Task<ActionResult<(PaginationMetaData, List<AuthorLogView>)>> GetAllAuthorLog(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var logs = await unitOfWorkService.LogService.GetAllAuthorsLogAsync();

                if (logs.Count() > 0)
                {
                    (logs, var paginationData) = await unitOfWorkService.LogPagination.GetPaginationAsync(pageNumber, pageSize, logs);
                    var authorsLogView = logs.Select(l => new AuthorLogView { Content = l.Content, AuthorId = l.AuthorId, logLevel = l.logLevel });
                    Response.Headers.Add("X-Pagination",
                   JsonSerializer.Serialize(paginationData));
                    return Ok(new { paginationData, authorsLogView });
                }
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
