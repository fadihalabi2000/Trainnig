using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CommentViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public CommentController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAll()
        {

            try
            {
                var comments = await unitOfWorkService.CommentsService.GetAllAsync();
                if (comments.Count() > 0)
                    return Ok(comments);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetById(int id)
        {


            try
            {
                var comment = await unitOfWorkService.CommentsService.GetByIdAsync(id);
                if (comment == null)
                    return BadRequest();
                else
                    return Ok(comment);

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
                    return Ok(comment);
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
