﻿
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
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
    public class ArticleController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public ArticleController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<ArticlView>)>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var article = await unitOfWorkService.ArticleService.GetAllAsync();

                if (article.Count() > 0)
                {
                    (article, var paginationData) = await unitOfWorkService.ArticlePagination.GetPaginationAsync(pageNumber, pageSize, article);
                    //var articles = article.Select(a => new ArticlView
                    //{
                    //    Id = a.Id,
                    //    AuthorId = a.AuthorId,
                    //    CategoryId = a.CategoryId,
                    //    Content = a.Content,
                    //    Comments = a.Comments,
                    //    Images = a.Images,
                    //    ViewCount = a.ViewCount,
                    //    Title = a.Title,
                    //    Likes = a.Likes,
                    //    PublishDate = a.PublishDate,
                    //    UpdateDate = a.PublishDate,
                    //});
                    if (article.Count() > 0)
                    {
                        List<ArticlView> articles = mapper.Map<List<ArticlView>>(article);
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


        [HttpGet("{id}",Name = "GetArticle")]
        public async Task<ActionResult<ArticlView>> GetById(int id)
        {


            try
            {
                var article = await unitOfWorkService.ArticleService.GetByIdAsync(id);
                //var articleView = new ArticlView
                //{
                //    Id = article.Id,
                //    AuthorId = article.AuthorId,
                //    CategoryId = article.CategoryId,
                //    Content = article.Content   ,
                //    Comments = article.Comments,
                //    Images = article.Images,
                //    ViewCount = article.ViewCount,
                //    Title = article.Title,
                //    Likes = article.Likes,
                //    PublishDate = article.PublishDate,
                //    UpdateDate = article.PublishDate,
                    
                //};
                if (article == null)
                {
                    await logger.LogWarning("Failed to fetch Article with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    ArticlView articleView = mapper.Map<ArticlView>(article);
                    await logger.LogInformation("Article with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return Ok(articleView);
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
        public async Task<ActionResult<ArticlView>> Post(CreateArticle createArticle)
        {

            try
            {
                //var article=new Article
                //{

                //    AuthorId = createArticle.AuthorId,
                //    CategoryId = createArticle.CategoryId,
                //    Content = createArticle.Content,
                //    ViewCount = createArticle.ViewCount,
                //    Title = createArticle.Title,
                // //   Images= createArticle.Images,

                //};
                Article article = mapper.Map<Article>(createArticle);
                await unitOfWorkService.ArticleService.AddAsync(article);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.ArticleService.GetAllAsync();
                    var articleId = lastID.Max(b => b.Id);
                    article = await unitOfWorkService.ArticleService.GetByIdAsync(articleId);
                    //var articleView = new ArticlView { Id = article.Id,AuthorId= article.AuthorId,CategoryId= article.CategoryId, ViewCount= article.ViewCount,
                    //                                   PublishDate=article.PublishDate,UpdateDate= article.UpdateDate,Comments=article.Comments,Content = article.Content,
                    //                                   Images = article.Images,Likes = article.Likes,Title=article.Title
                    //};
                    ArticlView articleView = mapper.Map<ArticlView>(article);
                    await logger.LogInformation("Article with ID " + articleId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return CreatedAtRoute("GetArticle", new
                    {
                        id = articleId,
                    }, articleView);
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
                //article.CategoryId = updateArticle.CategoryId;
                //article.Content = updateArticle.Content;
                //article.ViewCount = updateArticle.ViewCount;
                //article.Title = updateArticle.Title;
                //article.Likes = updateArticle.Likes;
                //article.Comments = updateArticle.Comments;
                //article = mapper.Map<Article>(updateArticle);
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
                //article.CategoryId = patchArticle.CategoryId;
                //article.Content = patchArticle.Content;
                //article.ViewCount = patchArticle.ViewCount;
                //article.Title = patchArticle.Title;
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
