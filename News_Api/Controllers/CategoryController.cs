using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public CategoryController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {

            try
            {
                var Users = await unitOfWorkService.CategoryService.GetAllAsync();
                if (Users.Count() > 0)
                    return Ok(Users);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.CategoryService.GetByIdAsync(id);
                if (user == null)
                    return BadRequest();
                else
                    return Ok(user);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult> Post(Category article)
        {

            try
            {
                var articlesImage = new Category();

                await unitOfWorkService.CategoryService.AddAsync(articlesImage);

                if (await unitOfWorkService.CommitAsync())
                    return Ok(articlesImage);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Category article)
        {
            try
            {
                await unitOfWorkService.CategoryService.GetByIdAsync(id);
                var articlesImage = new Category();
                await unitOfWorkService.CategoryService.UpdateAsync(articlesImage);
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
                await unitOfWorkService.CategoryService.DeleteAsync(id);
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
