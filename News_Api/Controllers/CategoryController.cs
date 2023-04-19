using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiServies.Auth.ClassStatic;
using Services.MyLogger;
using Services.Transactions;
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
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public CategoryController(IUnitOfWorkService unitOfWorkService,IMyLogger  logger,IMapper mapper)
        {
            this.unitOfWorkService = unitOfWorkService;
            this.logger = logger;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<(PaginationMetaData, List<CategoryView>)>> GetAll()
        {
            try
            { 
                var category = await unitOfWorkService.CategoryService.GetAllAsync();

                if (category.Count() > 0)
                {
                  
                    
                        List<CategoryView> categories = mapper.Map<List<CategoryView>>(category);
                    
                        await logger.LogInformation("All Category table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(categories);
                }      
                else
                {
                    await logger.LogWarning("An Warning occurred while fetching all logs category ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogWarning("An Warning occurred while fetching all logs category ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }


        [HttpGet("{id}",Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetById(int id, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var categoryById = await unitOfWorkService.CategoryService.GetByIdAsync(id);
                if (categoryById == null)
                {
                    await logger.LogWarning("Failed to fetch category with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
                else
                {
                    List<ArticleWithAuthorView> articles = new List<ArticleWithAuthorView>();

                    (articles, bool isCompleted) = await unitOfWorkService.ArticleService.GetArticlesAsync(categoryById.Articles);
                    if (isCompleted)
                    {
                        (articles, var paginationData) = await unitOfWorkService.ArticleWithAuthorViewPagination.GetPaginationAsync(pageNumber, pageSize, articles);
                        var category=mapper.Map<CategoryView>(categoryById);
                        Response.Headers.Add("X-Pagination",
                    JsonSerializer.Serialize(paginationData));
                        await logger.LogInformation("All Article By Category ID "+id+" table records fetched", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return Ok(new { paginationData,category,articles });
                    }
                    else
                    {
                        return NotFound();
                    }
                 
                }

            }
            catch (Exception)
            {
             await logger.LogErorr("Erorr to fetch category with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        public async Task<ActionResult<CategoryView>> Post(CreateCategory createCategory)
        {

            try
            {
               // var category = new Category {CategoryName=createCategory.CategoryName};
                Category categorys = mapper.Map<Category>(createCategory);

                var checkCategoryName = unitOfWorkService.CategoryService.CheckCategoryName(createCategory.CategoryName);
                if (checkCategoryName is null)
                {
                    await unitOfWorkService.CategoryService.AddAsync(categorys);

                    if (await unitOfWorkService.CommitAsync())
                    {
                        var lastID = await unitOfWorkService.CategoryService.GetAllAsync();
                        var categoryId = lastID.Max(b => b.Id);
                        categorys = await unitOfWorkService.CategoryService.GetByIdAsync(categoryId);
                        var ctegory = mapper.Map<CategoryView>(categorys);
                        await logger.LogInformation("category with ID " + categoryId + " added", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return CreatedAtRoute("GetCategory", new
                        {
                            id = categoryId,
                        }, ctegory);

                    }
                    else
                    {
                        await logger.LogWarning("An warning occurred when adding the category", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest(new { Message = "the CategoryName already exists " });
                }
            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when adding the category", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }


        [Authorize(Roles = "Admin,Author")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, UpdateCategory updateCategory)
        {
            try
            {
                var category = await unitOfWorkService.CategoryService.GetByIdAsync(id);
                //  category.CategoryName = updateCategory.CategoryName;
               
                var checkCategoryName =await unitOfWorkService.CategoryService.CheckCategoryName(updateCategory.CategoryName);
                if (checkCategoryName is not null)
                {
                    return BadRequest(new { Message = "the displayName already exists " });
                }
                else
                {
                    mapper.Map(updateCategory,category);
                    await unitOfWorkService.CategoryService.UpdateAsync(category);
                if (await unitOfWorkService.CommitAsync())
                {
                    await logger.LogInformation("category with ID " + id + " updated", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when updateing the category ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }
            }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when updateing the category ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
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
                {
                    await logger.LogInformation("category with ID " + id + " deleted", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return NoContent();
                }
                else
                {
                    await logger.LogWarning("An warning occurred when deleteing the category ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                    return BadRequest();
                }

            }
            catch (Exception)
            {
                await logger.LogErorr("An Erorr occurred when deleteing the category ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
                return BadRequest();
            }
        }
    }
}
//[HttpGet("{id}", Name = "GetCategory")]
//public async Task<ActionResult<Category>> GetById(int id)
//{


//    try
//    {
//        var category = await unitOfWorkService.CategoryService.GetByIdAsync(id);
//        if (category == null)
//        {
//            await logger.LogWarning("Failed to fetch category with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//            return BadRequest();
//        }
//        else
//        {
//            await logger.LogInformation("category with ID " + id + " fetched ", CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//            return Ok(category);
//        }

//    }
//    catch (Exception)
//    {
//        await logger.LogErorr("Erorr to fetch category with ID " + id, CurrentUser.Id(HttpContext), CurrentUser.Role(HttpContext));
//        return BadRequest();
//    }

//}