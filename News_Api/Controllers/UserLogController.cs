using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.LogViewModel;
using Services.Transactions.Interfaces;
using System.Text.Json;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class UserLogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public UserLogController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
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
                    var usersLogView = logs.Select(l => new UserLogView { Content = l.Content, UserId = l.UserId, logLevel = l.logLevel });
                    Response.Headers.Add("X-Pagination",
             JsonSerializer.Serialize(paginationData));
                    return Ok(new { paginationData, usersLogView });
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
