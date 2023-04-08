
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.UserViewModel;
using Services.Transactions.Interfaces;

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
        public async Task<ActionResult<List<Article>>> GetAll()
        {

            try
            {
                var Users = await unitOfWorkService.ArticleService.GetAllAsync();
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
        public async Task<ActionResult<Article>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.ArticleService.GetByIdAsync(id);
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
        public async Task<ActionResult> Post(CreateOrUpdateArticle article)
        {

            try
            {
               var articles=new Article();

                await unitOfWorkService.ArticleService.AddAsync(articles);

                if (await unitOfWorkService.CommitAsync())
                    return Ok(articles);
                else
                    return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CreateOrUpdateArticle article)
        {
            try
            {
                await unitOfWorkService.ArticleService.GetByIdAsync(id);
                var articles = new Article();
                await unitOfWorkService.ArticleService.UpdateAsync(articles);
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
