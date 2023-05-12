using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment environment;

        public ArticleController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper, IWebHostEnvironment env)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
            this.environment = env;
        }
        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<ArticleWithAuthorView>)>> GetAllArticle(int pageNumber = 1, int pageSize = 10)
        {
           try
           {
                List<Article> article = await unitOfWorkService.ArticleService.GetAllAsync();
                if (article.Count() > 0)
                {
                    List<ArticleWithAuthorView> articles = new List<ArticleWithAuthorView>();

                    (articles, bool isCompleted) = await unitOfWorkService.ArticleService.GetArticlesAsync(article);

                    if (isCompleted)
                    {
                        (articles, var paginationData) = await unitOfWorkService.ArticleWithAuthorViewPagination.GetPaginationAsync(pageNumber, pageSize, articles);
                        Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All Article table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, articles });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs Article ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));

                    return BadRequest();
                }
           }
           catch (Exception)
           {
                await logger.LogErorr("An Erorr occurred while fetching all logs Article ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
           }
        }
        [HttpGet("{id}", Name = "GetArticle")]
        public async Task<ActionResult<ArticleWithAuthorView>> GetArticleById(int id)
        {
            try
            {
                var articlById = await unitOfWorkService.ArticleService.GetByIdAsync(id);
                ;
                if (articlById == null)
                {
                    await logger.LogWarning("Failed to fetch Article with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    Author author = await unitOfWorkService.AuthorService.GetByIdAsync(articlById.AuthorId);
                    List<User> users = await unitOfWorkService.UsersService.GetAllAsync();
                    List<Like> likes = articlById.Likes;
                    List<Comment> comments = articlById.Comments;

                    List<ListLikeView> listLikeViews = likes.Join(
                                                       users, like => like.UserId,
                                                       user => user.Id,
                                                       (like, user) =>
                                                       new ListLikeView
                                                       {
                                                           id=like.Id,
                                                           UserId = user.Id,
                                                           ArticleId = like.ArticleId,
                                                           UserDisplayName = user.DisplayName,
                                                           UserProfilePicture = user.ProfilePicture
                                                       }).ToList();
                    List<ListCommentView> listCommentViews = comments.Join(
                                                             users, comment =>
                                                             comment.UserId,
                                                             user => user.Id,
                                                             (comment, user) =>
                                                             new ListCommentView
                                                             {Id = comment.Id,
                                                                 UserId = user.Id,
                                                                 ArticleId = comment.ArticleId,
                                                                 UserDisplayName = user.DisplayName,
                                                                 UserProfilePicture = user.ProfilePicture,
                                                                 CommentText = comment.CommentText
                                                             }).ToList();
                    ArticleWithAuthorView article = new ArticleWithAuthorView
                    {
                        Id = articlById.Id,
                        CategoryId = articlById.CategoryId,
                        Title = articlById.Title,
                        Content = articlById.Content,
                        ViewCount = articlById.ViewCount,
                        Images = articlById.Images,
                        PublishDate = articlById.PublishDate,
                        UpdateDate = articlById.UpdateDate,
                        AuthorDisplayName = author.DisplayName,
                        ProfilePicture = author.ProfilePicture
                    };
                    article.Comments = listCommentViews;
                    article.Likes = listLikeViews;
                    await logger.LogInformation("Article with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(article);
                }
            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Article with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        public async Task<ActionResult<ArticlView>> Post([FromForm] CreateArticle createArticle)
        {
           try
            {
                Article articles = new Article { AuthorId=createArticle.AuthorId , CategoryId=createArticle.CategoryId,
                                                Content=createArticle.Content,Title=createArticle.Title,
                ViewCount = createArticle.ViewCount
                };

                var contentPath = this.environment.ContentRootPath;

                articles.Images = await unitOfWorkService!.ArticleService.UploadImage(createArticle.Images, contentPath);
             await unitOfWorkService.ArticleService.AddAsync(articles);
             if (await unitOfWorkService.CommitAsync())
              {
                    var lastID = await unitOfWorkService.ArticleService.GetAllAsync();
                    var articleId = lastID.Max(b => b.Id);
                    articles = await unitOfWorkService.ArticleService.GetByIdAsync(articleId);
                    ArticlView article = mapper.Map<ArticlView>(articles);
                    await logger.LogInformation("Article with ID " + articleId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return await Task.Run(() => CreatedAtRoute("GetArticle", new { id = articleId, }, article));
              
              }
               else
               {
                    await logger.LogWarning("An warning occurred when adding the Article", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
               }
            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when adding the Article", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,Author")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateArticle updateArticle)
        {
            try
            {
                Article article = await unitOfWorkService.ArticleService.GetByIdAsync(id);
                mapper.Map(updateArticle, article);
                await unitOfWorkService.ArticleService.UpdateAsync(article);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Article with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when updateing the ArticleID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the ArticleID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
        [Authorize(Roles = "Author,Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> patch(int id, JsonPatchDocument<UpdateArticle> patchDocument)
        {
            try
            {
                Article article = await unitOfWorkService.ArticleService.GetByIdAsync(id);
                UpdateArticle patchArticle = mapper.Map<UpdateArticle>(article);
                patchDocument.ApplyTo(patchArticle);
                mapper.Map(patchArticle,article);
                var x = article;
                var z = patchArticle;
                    await unitOfWorkService.ArticleService.UpdateAsync(article);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("User with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(new { x, z });
                }
                else
                {
                    await logger.LogWarning("An warning occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the User ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin,Author")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.ArticleService.DeleteAsync(id);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Article with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the Article ID "+id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the Article ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
    }
}


//[HttpGet]
//public async Task<ActionResult<(PaginationMetaData, List<ArticlView>)>> GetAll(int pageNumber = 1, int pageSize = 10)
//{

//    try
//    {
//        var article = await unitOfWorkService.ArticleService.GetAllAsync();

//        if (article.Count() > 0)
//        {
//            (article, var paginationData) = await unitOfWorkService.ArticlePagination.GetPaginationAsync(pageNumber, pageSize, article);
//            //var articles = article.Select(a => new ArticlView
//            //{
//            //    Id = a.Id,
//            //    AuthorId = a.AuthorId,
//            //    CategoryId = a.CategoryId,
//            //    Content = a.Content,
//            //    Comments = a.Comments,
//            //    Images = a.Images,
//            //    ViewCount = a.ViewCount,
//            //    Title = a.Title,
//            //    Likes = a.Likes,
//            //    PublishDate = a.PublishDate,
//            //    UpdateDate = a.PublishDate,
//            //});
//            if (article.Count() > 0)
//            {
//                List<ArticlView> articles = mapper.Map<List<ArticlView>>(article);
//                Response.Headers.Add("X-Pagination",
//            JsonSerializer.Serialize(paginationData));
//                await logger.LogInformation("All Article table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//                return Ok(new { paginationData, articles });
//            }
//            else
//            {
//                return NotFound();
//            }
//        }
//        else
//        {
//            await logger.LogWarning("An Warning occurred while fetching all logs Article ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));

//            return BadRequest();
//        }

//    }
//    catch (Exception)
//    {
//        await logger.LogErorr("An Erorr occurred while fetching all logs Article ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//        return BadRequest();
//    }

//}


//[HttpGet("{id}",Name = "GetArticle")]
//public async Task<ActionResult<ArticlView>> GetById(int id)
//{


//    try
//    {
//        var articleById = await unitOfWorkService.ArticleService.GetByIdAsync(id);
//        //var articleView = new ArticlView
//        //{
//        //    Id = article.Id,
//        //    AuthorId = article.AuthorId,
//        //    CategoryId = article.CategoryId,
//        //    Content = article.Content   ,
//        //    Comments = article.Comments,
//        //    Images = article.Images,
//        //    ViewCount = article.ViewCount,
//        //    Title = article.Title,
//        //    Likes = article.Likes,
//        //    PublishDate = article.PublishDate,
//        //    UpdateDate = article.PublishDate,

//        //};
//        if (articleById == null)
//        {
//            await logger.LogWarning("Failed to fetch Article with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//            return BadRequest();
//        }
//        else
//        {
//            ArticlView article = mapper.Map<ArticlView>(articleById);
//            await logger.LogInformation("Article with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//            return Ok(article);
//        }

//    }
//    catch (Exception)
//    {
//        await logger.LogErorr("Erorr to fetch Article with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//        return BadRequest();
//    }

//}
