using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public LikeController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<LikeView>>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var like = await unitOfWorkService.LikeService.GetAllAsync();
               
                if (like.Count() > 0)
                {
                    (like, var paginationData) = await unitOfWorkService.LikePagination.GetPaginationAsync(pageNumber, pageSize, like);
                    //var likes = like.Select(l => new LikeView() { Id = l.Id, ArticleId = l.ArticleId, LikeDate = l.LikeDate, UserId = l.UserId }).ToList();
                    if (like.Count() > 0)
                    {
                        List<LikeView> likes = mapper.Map<List<LikeView>>(like);
                        Response.Headers.Add("X-Pagination",
                  JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All Like table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, likes });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs Like ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogWarning("An Warning occurred while fetching all logs Like ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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
                    await logger.LogWarning("Failed to fetch Like with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    //var likeView = new LikeView { Id = like.Id, ArticleId=like.ArticleId,LikeDate=like.LikeDate,UserId=like.UserId };
                    LikeView likeView= mapper.Map<LikeView>(like);
                    await logger.LogInformation("Like with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(likeView);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Like with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<LikeView>> Post(CreateLike createLike)
        {

            try
            {
            
                //var like= new Like { ArticleId = createLike.ArticleId, UserId = createLike.UserId };
                Like like=mapper.Map<Like>(createLike);
                await unitOfWorkService.LikeService.AddAsync(like);
                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.LikeService.GetAllAsync();
                    var likeId = lastID.Max(b => b.Id);
                    like = await unitOfWorkService.LikeService.GetByIdAsync(likeId);
                    //var likeView = new LikeView { Id = like.Id, ArticleId = like.ArticleId, LikeDate = like.LikeDate, UserId = like.UserId };
                    LikeView likeView=mapper.Map<LikeView>(like);
                    await logger.LogInformation("Like with ID " + likeId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return CreatedAtRoute("GetLike", new
                    {
                        id = likeId,
                    }, likeView);

                }
                else
                {
                    await logger.LogWarning("An warning occurred when adding the Like ID", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when adding the Like", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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
                {
                    await logger.LogInformation("Like with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the Like ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the Like ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
    }
}
