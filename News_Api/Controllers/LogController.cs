using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using Services.Transactions.Interfaces;

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IUnitOfWorkService unitOfWorkService;

        public LogController(IUnitOfWorkService unitOfWorkService)
        {
            this.unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetAll()
        {

            try
            {
                var Users = await unitOfWorkService.LogService.GetAllAsync();
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
        public async Task<ActionResult<Log>> GetById(int id)
        {


            try
            {
                var user = await unitOfWorkService.LogService.GetByIdAsync(id);
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
        public async Task<ActionResult> Post(Log article)
        {

            try
            {
                var articlesImage = new Log();

                await unitOfWorkService.LogService.AddAsync(articlesImage);

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
        public async Task<ActionResult> Put(int id, Log article)
        {
            try
            {
                await unitOfWorkService.LogService.GetByIdAsync(id);
                var articlesImage = new Log();
                await unitOfWorkService.LogService.UpdateAsync(articlesImage);
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
                await unitOfWorkService.LogService.DeleteAsync(id);
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
