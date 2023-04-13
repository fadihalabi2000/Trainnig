using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.Transactions.Interfaces;
using System.Security.Claims;
using System.Text.Json;


namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;
 
        public CategoryController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }
        
        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<CategoryView>)>> GetAll(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                
                var category = await unitOfWorkService.CategoryService.GetAllAsync();
              
                if (category.Count() > 0)
                {
                    (category,var paginationData) = await unitOfWorkService.CategoryPagination.GetPaginationAsync(pageNumber, pageSize, category);
                    List<CategoryView> categories = category.Select(c => new CategoryView { Id = c.Id, CategoryName = c.CategoryName }).ToList();
                 
 
                    Response.Headers.Add("X-Pagination",
                   JsonSerializer.Serialize(paginationData));
                    return Ok(new {paginationData, categories });

                }
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetById(int id)
        {


            try
            {
                var category = await unitOfWorkService.CategoryService.GetByIdAsync(id);
                if (category == null)
                    return BadRequest();
                else
                    return Ok(category);

            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Author")]
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
                    return CreatedAtRoute("GetCategory", new
                    {
                        id = categoryId,
                    }, category);
                  
                }
                else
                    return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Author")]
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

        [Authorize(Roles = "Admin")]
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
