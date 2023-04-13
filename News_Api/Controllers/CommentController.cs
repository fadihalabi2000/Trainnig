using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using Services.Transactions.Interfaces;
using System.Xml.Linq;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public CommentController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentView>>> GetAll()
        {

            try
            {
                var comments = await unitOfWorkService.CommentsService.GetAllAsync();
                var commentsView= comments.Select(c=> new CommentView { Id=c.Id,ArticleId=c.ArticleId,UserId=c.UserId,CommentDate=c.CommentDate,CommentText=c.CommentText}).ToList();
                if (commentsView.Count() > 0)
                    return Ok(commentsView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

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
                    return BadRequest();
                else
                    return Ok(commentsView);

            }
            catch (Exception)
            {

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
                    return CreatedAtRoute("GetComment", new
                    {
                        id = commentId,
                    }, commentsView);
                    
                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

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
                    return NoContent();
                else
                    return BadRequest();

            }
            catch (Exception)
            {

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
                    return NoContent();
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
