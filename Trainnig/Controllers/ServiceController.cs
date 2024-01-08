using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {

        private readonly AppDBContext _context;
        private readonly ILogger<ServiceController> _logger;
        public ServiceController(AppDBContext context, ILogger<ServiceController> logger)
        {
            _context = context;
            _logger = logger;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> GetAllService()
        {

            return await _context.services.ToListAsync();
            _logger.LogWarning("get allServiceby", _context.services.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServiceById(int id)
        {
            var Service = await _context.services
                       .FirstOrDefaultAsync(s => s.ID == id);
            try
            {
                if (Service == null)
                {
                    return NotFound($"This Service id: {id} does not exist");
                }
                return Ok(Service);
            }
            
             catch
            {
                return BadRequest("try agin something wong in request");
            }
           

        }
        [HttpPost]
        public async Task<ActionResult<ServiceView>> PostRoom(ServiceView serviceView)
        {
            try
            {
                var lastServiceId = _context.services
                               .OrderByDescending(s => s.ID)
                               .Select(s => s.ID)
                               .FirstOrDefault();
                Service service = new Service()
                {
                    Name = serviceView.Name,
                    Price = serviceView.Price,
                };
              
                    try
                    {
                        await _context.services.AddAsync(service);
                        await _context.SaveChangesAsync();
                        if (lastServiceId >= 0)
                        {
                        lastServiceId += 1;
                            Response.Headers.Append("Service-ID", lastServiceId.ToString());
                        }
                        return Ok("saccessfuly add Service");
                    }
                    catch (Exception)
                    { return NotFound("fialed to add Service soory"); }

            }


            catch (Exception)
            {
                return Conflict("sorry try agin");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceById(int id)
        {
            try
            {
                var service = await _context.services
                      .FirstOrDefaultAsync(s => s.ID == id);
                if (service == null)
                {
                    return NotFound($"The service id {service.ID} does not exist");
                }

                _context.services.Remove(service);
                await _context.SaveChangesAsync();

                return Ok($"Deleted successfully service id {service.ID}");
            }
            catch (Exception ex)
            { return Conflict(ex.ToString()); }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Putservice(int id, ServiceView serviceView)
        {

            try
            {
                var existingService = await _context.services
                                   .FirstOrDefaultAsync(s => s.ID == id);

                if (existingService == null)
                {
                    return NotFound("The Room does not exist");
                }
                existingService.Name = serviceView.Name;
                existingService.Price = serviceView.Price;  

                 await _context.SaveChangesAsync();
                return Ok("update saccess");
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("The Service does not exist");
            }


        }

    }
}
