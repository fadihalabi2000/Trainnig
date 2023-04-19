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
    [Route("api/Article/{ArticleId}/[controller]")]
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
        public async Task<ActionResult<List<LikeView>>> GetAll(int articleId, int pageNumber = 1, int pageSize = 10)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }
            try
            {
                var like = await unitOfWorkService.LikeService.GetAllByIdArticleAsync(articleId);
               
                if (like.Count() > 0)
                {
                    (like, var paginationData) = await unitOfWorkService.LikePagination.GetPaginationAsync(pageNumber, pageSize, like);
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
        public async Task<ActionResult<LikeView>> GetById( int articleId, int id)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }

            try
            {
                var likeById = await unitOfWorkService.LikeService.GetByIdAsync(id);
                if (likeById == null)
                {
                    await logger.LogWarning("Failed to fetch Like with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    LikeView like= mapper.Map<LikeView>(likeById);
                    await logger.LogInformation("Like with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(like);
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Like with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<LikeView>> Post(int articleId, CreateLike createLike)
        {
            var article = await unitOfWorkService.LikeService.GetByIdAsync(articleId);
            if (article == null)
            {
                return NotFound();
            }
            try
            { 
                var allLikes = await unitOfWorkService.LikeService.GetAllByIdArticleAsync(articleId);
                var likeByIdUser = allLikes.FirstOrDefault(l=> l.UserId==articleId);
                if(likeByIdUser == null)
                {
                    return BadRequest();
                }

          
                Like likes=mapper.Map<Like>(createLike);
                likes.ArticleId = articleId;
                await unitOfWorkService.LikeService.AddAsync(likes);
                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.LikeService.GetAllAsync();
                    var likeId = lastID.Max(b => b.Id);
                    likes = await unitOfWorkService.LikeService.GetByIdAsync(likeId);
                    LikeView like=mapper.Map<LikeView>(likes);
                    await logger.LogInformation("Like with ID " + likeId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(like);

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
        //      var like= await unitOfWorkService.LikeService.GetByIdAsync(id);
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
