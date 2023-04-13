using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.LikeViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public LikeController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LikeView>>> GetAll()
        {

            try
            {
                var likes = await unitOfWorkService.LikeService.GetAllAsync();
                var likesView =  likes.Select(l => new LikeView() { Id = l.Id, ArticleId = l.ArticleId, LikeDate = l.LikeDate, UserId = l.UserId }).ToList();
                if (likesView.Count() > 0)
                {
                   
                    return Ok(likesView);
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


        [HttpGet("{id}",Name = "GetLike")]
        public async Task<ActionResult<LikeView>> GetById(int id)
        {


            try
            {
                var like = await unitOfWorkService.LikeService.GetByIdAsync(id);
                if (like == null)
                {
                    return BadRequest();
                }
                else
                {
                    var likeView = new LikeView { Id = like.Id, ArticleId=like.ArticleId,LikeDate=like.LikeDate,UserId=like.UserId };
               
                    return Ok(likeView);
                }

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<LikeView>> Post(CreateLike createLike)
        {

            try
            {
            
                var like= new Like { ArticleId = createLike.ArticleId, UserId = createLike.UserId };
                await unitOfWorkService.LikeService.AddAsync(like);
              
                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.LikeService.GetAllAsync();
                    var likeId = lastID.Max(b => b.Id);
                    like=await unitOfWorkService.LikeService.GetByIdAsync(likeId);
                    var likeView = new LikeView {Id=like.Id, ArticleId = like.ArticleId, LikeDate = like.LikeDate, UserId = like.UserId };
                    return CreatedAtRoute("GetLike", new
                    {
                        id = likeId,
                    } , likeView) ;

                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }



        //[HttpPut("{id}")]
        //public async Task<ActionResult> Put(int id, Like article)
        //{
        //    try
        //    {
        //        await unitOfWorkService.LikeService.GetByIdAsync(id);
        //        var articlesImage = new Like();
        //        await unitOfWorkService.LikeService.UpdateAsync(articlesImage);
        //        if (await unitOfWorkService.CommitAsync())
        //            return NoContent();
        //        else
        //            return BadRequest();

        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }

        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.LikeService.DeleteAsync(id);
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
