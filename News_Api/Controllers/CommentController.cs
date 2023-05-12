using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/Article/{articleId}/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public CommentController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ListCommentView>>> GetAll(int articleId,int pageNumber = 1, int pageSize = 10)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                var comments = await unitOfWorkService.CommentsService.GetAllByIdArticleAsync(articleId);
                var users = await unitOfWorkService.UsersService.GetAllAsync();
                List<ListCommentView> listCommentViews = comments.Join(
                                         users, comment => comment.UserId, user => user.Id,
                                         (comment, user) =>
                                         new ListCommentView
                                         {
                                             Id=comment.Id,
                                             UserId = user.Id,
                                             ArticleId = comment.ArticleId,
                                             UserDisplayName = user.DisplayName,
                                             UserProfilePicture = user.ProfilePicture,
                                             CommentText = comment.CommentText
                                         }).ToList();
                if (listCommentViews.Count() > 0)
                {

                    (listCommentViews, var paginationData) = await unitOfWorkService.ListCommentViewPagination.GetPaginationAsync(pageNumber, pageSize, listCommentViews);
                    if (listCommentViews.Count() > 0)
                    {
                       
                        Response.Headers.Add("X-Pagination",
                  JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All Comment table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, listCommentViews });
                    }
                    else
                    {
                        return NotFound();
                    }
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
        public async Task<ActionResult<ListCommentView>> GetById(int articleId, int id)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                var commentById = await unitOfWorkService.CommentsService.GetByIdAsync(id);
                if (commentById == null)
                {
                    await logger.LogWarning("Failed to fetch Comment with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    var user = await unitOfWorkService.UsersService.GetByIdAsync(commentById.UserId);
                   ListCommentView listCommentView=new ListCommentView { ArticleId=commentById.ArticleId,CommentText=commentById.CommentText,UserDisplayName=user.DisplayName,UserProfilePicture=user.ProfilePicture,UserId=user.Id,Id=commentById.Id};  
                    await logger.LogInformation("Comment with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(listCommentView);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Comment with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<CommentView>> Post(int articleId, CreateComment createComment)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            try
            {

                Comment comments= mapper.Map<Comment>(createComment);
                comments.ArticleId = articleId;
                await unitOfWorkService.CommentsService.AddAsync(comments);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.CommentsService.GetAllAsync();
                    var commentId = lastID.Max(b => b.Id);
                    comments = await unitOfWorkService.CommentsService.GetByIdAsync(commentId);
                    CommentView comment = mapper.Map<CommentView>(comments);
                    await logger.LogInformation("Comment with ID " + commentId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return await Task.Run(() => CreatedAtRoute("GetComment", new { articleId = articleId, id = commentId, }, comment));

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
        public async Task<ActionResult> Put(int articleId, int id, UpdateComment updateComment)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }
            try
            {
              Comment comment=  await unitOfWorkService.CommentsService.GetByIdAsync(id);
                mapper.Map(updateComment, comment);
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
