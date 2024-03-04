using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CenterController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly ILogger<CenterController> _logger;
        private readonly service.IBaseService<Center> baseService;

        public CenterController(AppDBContext context, ILogger<CenterController> logger,IBaseService<Center> baseService)
        {
            _context = context;
            _logger = logger;
            this.baseService = baseService;
        }
      

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Center>>> GetAllCenters()
       {
            try
            {
                var center = await this.baseService.GetAllAsync();

                if (!center.Any())
                {
                    return NotFound(" There are no accounts in system");
                }

                return Ok(center);

                //return await _context.Centers.ToListAsync();
                //_logger.LogWarning("get allcenterby", _context.Centers.ToListAsync());
            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Center>> GetCenterById(int id)
        {
         
            var Center = await _context.Centers
                      .FirstOrDefaultAsync(r => r.ID == id);

            try
            {
                if (Center == null)
                {
                    return NotFound($"The Center id {id} does not exist");
                }

                return Ok(Center);
            }
            catch { 
                return BadRequest("try agin something wong in request");
            }
        }
        [HttpPost]
        public async Task<ActionResult<CenterView>> PostCenter(CenterView carview)
        {
            try
            {
                var lastCenterId = _context.Centers
              .OrderByDescending(b => b.ID)
              .Select(b => b.ID)
              .FirstOrDefault();
                if (lastCenterId != null)
                {
                    Center center = new Center()
                    {
                        Name = carview.Name,
                        Location = carview.Location
                    };
                    await _context.Centers.AddAsync(center);
                    await _context.SaveChangesAsync();
                    if (lastCenterId >= 0)
                    {
                        lastCenterId += 1;
                        Response.Headers.Append($"Center-ID", lastCenterId.ToString());
                    }
                    return Ok(center);
                }
                else
                {
                    return BadRequest();
                }
               
            }
            catch (Exception ) {
                return Conflict("sorry  add try agin");
            }
        }
        // DELETE: api/Center
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCenterById(int id)
        {
            
            try
            {
                var center = await _context.Centers
                            .FirstOrDefaultAsync(c => c.ID == id);
                if (center == null)
                {
                    return NotFound("The center does not exist");
                }

                _context.Centers.Remove(center);
                await _context.SaveChangesAsync();

                return Ok(center);
            
            }
            catch (Exception ex) 
            { return Conflict(ex.ToString()); }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCenter(int id, CenterView centerView)
        {
           
            try
            {
                var existingCenter = await _context.Centers
                                   .FirstOrDefaultAsync(c => c.ID == id);

                if (existingCenter == null)
                {
                    return NotFound("The center does not exist");
                }
                existingCenter.Name = centerView.Name;
                existingCenter.Location = centerView.Location;
                await _context.SaveChangesAsync();
                return Ok(existingCenter);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("The center does not exist");
            }


        }

    }
}
