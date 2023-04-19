using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels;
using Services.Transactions.Interfaces;
using System.Text.Json;
using AutoMapper;
using NewsApiDomin.ViewModels.LogViewModel.AuthorLogViewModel;
using NewsApiDomin.ViewModels.LogViewModel.UserLogViewModel;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AuthorLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMapper mapper;

        public AuthorLogController(IUnitOfWorkService unitOfWorkService,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.mapper = mapper;
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
                    //var authorsLogView = logs.Select(l => new AuthorLogView { Content = l.Content, AuthorId = l.AuthorId, logLevel = l.logLevel });
                    if (logs.Count() > 0)
                    {
                        List<AuthorLogView> authorsLogView = mapper.Map<List<AuthorLogView>>(logs);
                        Response.Headers.Add("X-Pagination",
                       JsonSerializer.Serialize(paginationData));
                        return Ok(new { paginationData, authorsLogView });
                    }
                    else
                    {
                        return NotFound();
                    }
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
        public async Task<ActionResult<List<AuthorLogView>>> GetAllLogAuthorById(int id)
        {


            try
            {
                var logById = await unitOfWorkService.LogService.GetLogAuthorByIdAsync(id);
                //var authorLogView = log.Select(l => new AuthorLogView { Content = l.Content, AuthorId = l.AuthorId, logLevel = l.logLevel });
                if (logById == null)
                    return BadRequest();
                else
                {
                   List<AuthorLogView> authorLog = mapper.Map<List<AuthorLogView>>(logById);
                    return Ok(authorLog);
                }

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        public async Task<ActionResult<AuthorLogView>> Post(CreateAuthorLog  createAuthorLog)
        {

            try
            {
                Log log = mapper.Map<Log>(createAuthorLog);
                log.DateCreated = DateTime.Now;
                await unitOfWorkService.LogService.AddAsync(log);

                if (await unitOfWorkService.CommitAsync())
                {
                    AuthorLogView authorLog = mapper.Map<AuthorLogView>(log);
                    return Ok(authorLog);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
