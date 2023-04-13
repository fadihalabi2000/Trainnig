using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.CategoryViewModel;
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

        public AuthorController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<AuthorView>)>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var author = await unitOfWorkService.AuthorService.GetAllAsync();
             
                if (author.Count() > 0)
                {
                    (author, var paginationData) = await unitOfWorkService.AuthorPagination.GetPaginationAsync(pageNumber, pageSize, author);
                    var authors = author.Select(u => new AuthorView
                    {
                        Id = u.Id,
                        Bio = u.Bio,
                        DisplayName = u.DisplayName,
                        ProfilePicture = u.ProfilePicture,
                        Email = u.Email,
                        Password = u.Password,
                        Article = u.Article,

                    });
                    Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationData));
                    return Ok(new { paginationData, authors });
                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorView>> GetById(int id)
        {


            try
            {
                var author = await unitOfWorkService.AuthorService.GetByIdAsync(id);
                var authorView= new AuthorView { Id=author.Id,Article=author.Article,DisplayName=author.DisplayName,Email=author.Email,Bio=author.Bio,Password=author.Password,ProfilePicture=author.ProfilePicture };
                if (author == null)
                    return BadRequest();
                else
                    return Ok(author);

            }
            catch (Exception)
            {

                return BadRequest();
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


        [Authorize(Roles = "Author")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateAuthor updateAuthor)
        {
            try
            {
                var author=  await unitOfWorkService.AuthorService.GetByIdAsync(id);
                author.Bio = updateAuthor.Bio;
                author.Password = updateAuthor.Password;
                author.ProfilePicture = updateAuthor.ProfilePicture;
                author.DisplayName = updateAuthor.DisplayName;

                await unitOfWorkService.AuthorService.UpdateAsync(author);
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await unitOfWorkService.AuthorService.DeleteAsync(id);
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
