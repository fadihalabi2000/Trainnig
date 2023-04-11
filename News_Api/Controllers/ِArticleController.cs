
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using Services.Transactions.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public ArticleController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArticlView>>> GetAll()
        {

            try
            {
                var articles = await unitOfWorkService.ArticleService.GetAllAsync();
                var articlesView = articles.Select(a => new ArticlView
                {
                    Id = a.Id,
                    AuthorId = a.AuthorId,
                    CategoryId = a.CategoryId,
                    Content = a.Content,
                    Comments = a.Comments,
                    Images = a.Images,
                    ViewCount = a.ViewCount,
                    Title = a.Title,
                    Likes = a.Likes,
                    PublishDate = a.PublishDate,
                    UpdateDate = a.PublishDate,
                    
                    
                });
                if (articlesView.Count() > 0)
                    return Ok(articlesView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetArticle")]
        public async Task<ActionResult<ArticlView>> GetById(int id)
        {


            try
            {
                var article = await unitOfWorkService.ArticleService.GetByIdAsync(id);
                var articleView = new ArticlView
                {
                    Id = article.Id,
                    AuthorId = article.AuthorId,
                    CategoryId = article.CategoryId,
                    Content = article.Content   ,
                    Comments = article.Comments,
                    Images = article.Images,
                    ViewCount = article.ViewCount,
                    Title = article.Title,
                    Likes = article.Likes,
                    PublishDate = article.PublishDate,
                    UpdateDate = article.PublishDate,
                    
                };
                if (articleView == null)
                    return BadRequest();
                else
                    return Ok(articleView);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<ArticlView>> Post(CreateArticle createArticle)
        {

            try
            {
               var article=new Article
               {
                  
                   AuthorId = createArticle.AuthorId,
                   CategoryId = createArticle.CategoryId,
                   Content = createArticle.Content,
                   ViewCount = createArticle.ViewCount,
                   Title = createArticle.Title,
                   Images= createArticle.Images,
                   
               };

                await unitOfWorkService.ArticleService.AddAsync(article);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.ArticleService.GetAllAsync();
                    var articleId = lastID.Max(b => b.Id);
                    article = await unitOfWorkService.ArticleService.GetByIdAsync(articleId);
                    var articleView = new ArticlView { Id = article.Id,AuthorId= article.AuthorId,CategoryId= article.CategoryId, ViewCount= article.ViewCount,
                                                       PublishDate=article.PublishDate,UpdateDate= article.UpdateDate,Comments=article.Comments,Content = article.Content,
                                                       Images = article.Images,Likes = article.Likes,Title=article.Title
                    };
                    return CreatedAtRoute("GetArticle", new
                    {
                        id = articleId,
                    }, articleView);
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
        public async Task<ActionResult> Put(int id, UpdateArticle updateArticle)
        {
            try
            {
               var article= await unitOfWorkService.ArticleService.GetByIdAsync(id);




                article.CategoryId = updateArticle.CategoryId;
                article.Content = updateArticle.Content;
                article.ViewCount = updateArticle.ViewCount;
                article.Title = updateArticle.Title;
                article.Likes = updateArticle.Likes;
                article.Comments = updateArticle.Comments;




                await unitOfWorkService.ArticleService.UpdateAsync(article);
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
                await unitOfWorkService.ArticleService.DeleteAsync(id);
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
