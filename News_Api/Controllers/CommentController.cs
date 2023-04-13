using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;

        public CommentController(IUnitOfWorkService unitOfWorkService, IMyLogger logger)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentView>>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var comment = await unitOfWorkService.CommentsService.GetAllAsync();
                if (comment.Count() > 0)
                {
                    (comment, var paginationData) = await unitOfWorkService.CommentPagination.GetPaginationAsync(pageNumber, pageSize, comment);
                    var comments = comment.Select(c => new CommentView { Id = c.Id, ArticleId = c.ArticleId, UserId = c.UserId, CommentDate = c.CommentDate, CommentText = c.CommentText }).ToList();
                    Response.Headers.Add("X-Pagination",
              JsonSerializer.Serialize(paginationData));
                    await logger.LogInformation("All Comment table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(new { paginationData, comments });
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs Comment ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogWarning("An Warning occurred while fetching all logs Comment ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetComment")]
        public async Task<ActionResult<CommentView>> GetById(int id)
        {


            try
            {
                var comment = await unitOfWorkService.CommentsService.GetByIdAsync(id);
                var commentsView =  new CommentView { Id = comment.Id, ArticleId = comment.ArticleId, UserId = comment.UserId, CommentDate = comment.CommentDate, CommentText = comment.CommentText };
                if (commentsView == null)
                {
                    await logger.LogWarning("Failed to fetch Comment with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    await logger.LogInformation("Comment with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(commentsView);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Comment with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult> Post(CreateComment createComment)
        {

            try
            {
                var comment = new Comment() { CommentText= createComment.CommentText,
                                              ArticleId=createComment.ArticleId,
                                              UserId=createComment.UserId};

                await unitOfWorkService.CommentsService.AddAsync(comment);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.CommentsService.GetAllAsync();
                    var commentId = lastID.Max(b => b.Id);
                    comment = await unitOfWorkService.CommentsService.GetByIdAsync(commentId);
                    var commentsView = new CommentView { Id = comment.Id, ArticleId = comment.ArticleId, UserId = comment.UserId, CommentDate = comment.CommentDate, CommentText = comment.CommentText };
                    await logger.LogInformation("Comment with ID " + commentId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return CreatedAtRoute("GetComment", new
                    {
                        id = commentId,
                    }, commentsView);

                }
                else
                {
                    await logger.LogWarning("An warning occurred when adding the Comment", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when adding the Comment", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateComment updateComment)
        {
            try
            {
              var comment=  await unitOfWorkService.CommentsService.GetByIdAsync(id);
                 comment.CommentDate = DateTime.Now;
                comment.CommentText = updateComment.CommentText;
               
                await unitOfWorkService.CommentsService.UpdateAsync(comment);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Comment with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when updateing the Comment ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the Comment ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.CommentsService.DeleteAsync(id);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Comment with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the Comment ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the Comment ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
    }
}
