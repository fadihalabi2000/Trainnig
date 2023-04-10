using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using Services.Transactions.Interfaces;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<ActionResult<List<CategoryView>>> GetAll()
        {

            try
            {
                var categories = await unitOfWorkService.CategoryService.GetAllAsync();
                var categoriesView = categories.Select(c=>new CategoryView { Id=c.Id,CategoryName=c.CategoryName}).ToList();
                if (categoriesView.Count() > 0)
                    return Ok(categoriesView);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetCategory")]
        public async Task<ActionResult<CategoryView>> GetById(int id)
        {


            try
            {
                var category = await unitOfWorkService.CategoryService.GetByIdAsync(id);
                var categoryView = new CategoryView { Id = category.Id, CategoryName = category.CategoryName, Articles = category.Articles };
                if (categoryView == null)
                    return BadRequest();
                else
                    return Ok(categoryView);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpPost]
        public async Task<ActionResult<CategoryView>> Post(CreateCategory createCategory)
        {

            try
            {
                var category = new Category {CategoryName=createCategory.CategoryName};

                await unitOfWorkService.CategoryService.AddAsync(category);

                if (await unitOfWorkService.CommitAsync())
                {
                    var lastID = await unitOfWorkService.CategoryService.GetAllAsync();
                    var categoryId = lastID.Max(b => b.Id);
                    category = await unitOfWorkService.CategoryService.GetByIdAsync(categoryId);
                    var categoryView = new CategoryView { Id = category.Id,CategoryName=category.CategoryName,Articles=category.Articles };
                    return CreatedAtRoute("GetCategory", new
                    {
                        id = categoryId,
                    }, categoryView);
                  
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
        public async Task<ActionResult> Put(int id, UpdateCategory updateCategory)
        {
            try
            {
               var category= await unitOfWorkService.CategoryService.GetByIdAsync(id);
                category.CategoryName = updateCategory.CategoryName;
                await unitOfWorkService.CategoryService.UpdateAsync(category);
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
