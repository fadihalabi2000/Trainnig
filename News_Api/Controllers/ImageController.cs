using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public ImageController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Image>>> GetAll()
        {

            try
            {
                var Users = await unitOfWorkService.ImageService.GetAllAsync();
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
        public async Task<ActionResult<Image>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.ImageService.GetByIdAsync(id);
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
        public async Task<ActionResult> Post(Image article)
        {

            try
            {
                var articlesImage = new Image();

                await unitOfWorkService.ImageService.AddAsync(articlesImage);

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
        public async Task<ActionResult> Put(int id, Image article)
        {
            try
            {
                await unitOfWorkService.ImageService.GetByIdAsync(id);
                var articlesImage = new Image();
                await unitOfWorkService.ImageService.UpdateAsync(articlesImage);
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
                await unitOfWorkService.ImageService.DeleteAsync(id);
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
