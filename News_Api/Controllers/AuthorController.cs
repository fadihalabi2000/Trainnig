using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions.Interfaces;
using System.Text.Json;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public AuthorController(IUnitOfWorkService unitOfWorkService, IMyLogger logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<AuthorView>)>> GetAll()
        {

            try
            {
                var author = await unitOfWorkService.AuthorService.GetAllAsync();
                if (author.Count() > 0)
                  {
                    await logger.LogInformation("All Author table records fetched  ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    List<AuthorView> authors = mapper.Map<List<AuthorView>>(author);
                   return Ok(authors );
                }
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs Author ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred while fetching all logs Author ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorView>> GetById(int id,int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var authorById = await unitOfWorkService.AuthorService.GetByIdAsync(id);
                if (authorById == null)
                {
                    await logger.LogWarning("Failed to fetch Author with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    List<ArticleWithAuthorView> articles = new List<ArticleWithAuthorView>();

                    (articles, bool isCompleted) = await unitOfWorkService.ArticleService.GetArticlesAsync(authorById.Article);
                    if (isCompleted)
                    {
                        (articles, var paginationData) = await unitOfWorkService.ArticleWithAuthorViewPagination.GetPaginationAsync(pageNumber, pageSize, articles);
                        AuthorView author = mapper.Map<AuthorView>(authorById);
                        Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("Author with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData, author, articles });
                    }
                    else
                    {
                        return NotFound();
                    }

                }

            }
            catch (Exception)
            {
                await logger.LogErorr("Erorr to fetch Author with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }



        [Authorize(Roles = "Author,Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateAuthor updateAuthor)
        {
            try
            {
                Author author =  await unitOfWorkService.AuthorService.GetByIdAsync(id);
                var checkDisplayName = await unitOfWorkService.AuthorService.CheckDisplayName(updateAuthor.DisplayName);
                if (checkDisplayName is null)
                {
                    author = mapper.Map(updateAuthor,author);
                    await unitOfWorkService.AuthorService.UpdateAsync(author);
                    if (await unitOfWorkService.CommitAsync())
                    {
                        await logger.LogInformation("Author with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return NoContent();
                    }
                    else
                    {
                        await logger.LogWarning("An warning occurred when updateing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return BadRequest();
                    }
                }
                else 
                {
                    return BadRequest(new { Message = "the displayName already exists " });
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

        [Authorize(Roles = "Author,Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> patch(int id, JsonPatchDocument<UpdateAuthor> patchDocument)
        {
            try
            {
                Author author = await unitOfWorkService.AuthorService.GetByIdAsync(id);
                UpdateAuthor patchAuthor = mapper.Map<UpdateAuthor>(author);
                patchDocument.ApplyTo(patchAuthor);

                var checkDisplayName = await unitOfWorkService.UsersService.CheckDisplayName(patchAuthor.DisplayName);
                if (checkDisplayName is null)
                {
                    mapper.Map(patchAuthor,author);
                    await unitOfWorkService.AuthorService.UpdateAsync(author);
                    if (await unitOfWorkService.CommitAsync())
                    {
                        await logger.LogInformation("Author with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return NoContent();
                    }
                    else
                    {
                        await logger.LogWarning("An warning occurred when updateing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(new { Message = "the displayName already exists " });
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.AuthorService.DeleteAsync(id);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("Author with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the Author ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }
    }
}

//[HttpPost]
//public async Task<ActionResult<AuthorView>> Post(CreateAuthor createAuthor)
//{

//    try
//    {
//        var author = new Author
//        {

//            Email = createAuthor.Email,
//            Password = createAuthor.Password,
//            ProfilePicture = createAuthor.ProfilePicture,
//            DisplayName = createAuthor.DisplayName,
//            Bio=createAuthor.Bio,
//        };

//        await unitOfWorkService.AuthorService.AddAsync(author);

//        if (await unitOfWorkService.CommitAsync())
//        {
//            var lastID = await unitOfWorkService.AuthorService.GetAllAsync();
//            var authorId = lastID.Max(b => b.Id);
//            author = await unitOfWorkService.AuthorService.GetByIdAsync(authorId);
//            var authorView = new AuthorView
//            {
//                Id = author.Id,
//                DisplayName = author.DisplayName,
//                ProfilePicture = author.ProfilePicture,
//                Email = author.Email,
//                Password = author.Password,
//                Bio=author.Bio,
//            };
//            return CreatedAtRoute("GetAuthor", new
//            {
//                id = authorId,
//            }, authorView);
//        }
//        else
//            return BadRequest();

//    }
//    catch (Exception)
//    {

//        return BadRequest();
//    }
//}

