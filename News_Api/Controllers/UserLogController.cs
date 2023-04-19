using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.LogViewModel.UserLogViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.Transactions.Interfaces;
using System.Collections.Generic;
using System.Text.Json;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class UserLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMapper mapper;

        public UserLogController(IUnitOfWorkService unitOfWorkService,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.mapper = mapper;
        }
        [HttpGet("GetAllUsersLog")]
        public async Task<ActionResult<(PaginationMetaData, List<UserLogView>)>> GetAllUsersLog(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var logs = await unitOfWorkService.LogService.GetAllUsersLogAsync();

                if (logs.Count() > 0)
                {
                    (logs, var paginationData) = await unitOfWorkService.LogPagination.GetPaginationAsync(pageNumber, pageSize, logs);
                    //var usersLogView = logs.Select(l => new UserLogView { Content = l.Content, UserId = l.UserId, logLevel = l.logLevel });
                    if (logs.Count() > 0)
                    {
                        List<UserLogView> usersLogView = mapper.Map<List<UserLogView>>(logs);
                        Response.Headers.Add("X-Pagination",
                 JsonSerializer.Serialize(paginationData));
                        return Ok(new { paginationData, usersLogView });
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
        [HttpGet("{id}")]
        public async Task<ActionResult<List<UserLogView>>> GetAllLogUserById(int id)
        {


            try
            {
                var log = await unitOfWorkService.LogService.GetLogUserByIdAsync(id);
                //var userLogView = log.Select(l => new UserLogView { Content = l.Content, UserId = l.UserId, logLevel = l.logLevel });
                if (log == null)
                    return BadRequest();
                else
                {
                    List<UserLogView> userLogView = mapper.Map<List<UserLogView>>(log);
                    return Ok(userLogView);
                }

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<ActionResult<UserLogView>> Post(CreateUserLog  createUserLog)
        {

            try
            {
                Log log = mapper.Map<Log>(createUserLog);
                log.DateCreated = DateTime.Now;
                await unitOfWorkService.LogService.AddAsync(log);

                if (await unitOfWorkService.CommitAsync())
                {
                    UserLogView logView = mapper.Map<UserLogView>(log);
                    return Ok(logView);
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
