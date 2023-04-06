using Microsoft.AspNetCore.Mvc;
using NewsApiDomin.Models;
using NewsApiServies.CRUD;
using NewsApiServies.CRUD.Interfaces;
using Services.Transactions.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWorkService categoryService;

        // GET: api/<ValuesController>
        public ValuesController(IUnitOfWorkService categoryService)
        {
            this.categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<IList<Category>>> Get()
        {
            var x=await this.categoryService.ategoryService.GetAllAsync();
            return Ok(x);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
           
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
