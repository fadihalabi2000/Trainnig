using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.AuthorViewModel;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public AuthorController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorView>>> GetAll()
        {

            try
            {
                var authors = await unitOfWorkService.AuthorService.GetAllAsync();
                var usersView = authors.Select(u => new AuthorView
                {
                    Id = u.Id,
                    Bio=u.Bio,
                    DisplayName = u.DisplayName,
                    ProfilePicture = u.ProfilePicture,
                    Email = u.Email,
                    Password = u.Password
                });
                if (usersView.Count() > 0)
                    return Ok(usersView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name ="GetAuthor")]
        public async Task<ActionResult<Author>> GetById(int id)
        {


            try
            {
                var author = await unitOfWorkService.AuthorService.GetByIdAsync(id);
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


        [HttpPost]
        public async Task<ActionResult<AuthorView>> Post(CreateAuthor createAuthor)
        {

            try
            {
                var author = new Author
                {
                 
                    Email = createAuthor.Email,
                    Password = createAuthor.Password,
                    ProfilePicture = createAuthor.ProfilePicture,
                    DisplayName = createAuthor.DisplayName,
                    Bio=createAuthor.Bio,
                };

                await unitOfWorkService.AuthorService.AddAsync(author);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.AuthorService.GetAllAsync();
                    var authorId = lastID.Max(b => b.Id);
                    author = await unitOfWorkService.AuthorService.GetByIdAsync(authorId);
                    var authorView = new AuthorView
                    {
                        Id = author.Id,
                        DisplayName = author.DisplayName,
                        ProfilePicture = author.ProfilePicture,
                        Email = author.Email,
                        Password = author.Password,
                        Bio=author.Bio,
                    };
                    return CreatedAtRoute("GetAuthor", new
                    {
                        id = authorId,
                    }, authorView);
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
