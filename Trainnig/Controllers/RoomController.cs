using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly ILogger<RoomController> _logger;
        public RoomController(AppDBContext context, ILogger<RoomController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRoom()
        {
            try
            {
                var rooms = await _context.rooms.ToListAsync();

                if (!rooms.Any())
                {
                    return NotFound("There are no Room");
                }

                return Ok(rooms);
               
              
            }
            catch (Exception ) {

                return BadRequest("Something went wrong in " +
                           " the request, please try again.");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoomById(int id)
        {
            var Room = await _context.rooms
                       .FirstOrDefaultAsync(r => r.ID == id);
            try
            {
                if (Room == null)
            {
                return NotFound($"This room id: {id} does not exist");
            }

                return Ok(Room);
            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
           
        }
        [HttpPost]
        public async Task<ActionResult<RoomView>> PostRoom(RoomView roomView)
        {
            try
            {
                var lastRoomId = _context.rooms
                               .OrderByDescending(b => b.ID)
                               .Select(b => b.ID)
                               .FirstOrDefault();
                var center = await _context.Centers
                       .FirstOrDefaultAsync(r => r.ID == roomView.CenterId);

                if (center!= null)
                {
                    // return NotFound($"This room id: {id} does not exist");
                    Room room = new Room()
                    {
                        Name = roomView.Name,
                        Capacity = roomView.Capacity,
                        CenterId = roomView.CenterId,
                    };
                    try
                    {
                        await _context.rooms.AddAsync(room);
                        await _context.SaveChangesAsync();
                        if (lastRoomId >=0)
                        {
                            lastRoomId += 1;
                            Response.Headers.Append("Room-ID", lastRoomId.ToString());
                       
                        }
                        Room roomwithId =new Room()
                        { ID=lastRoomId,
                        Name = roomView.Name,
                        Capacity = roomView.Capacity,
                        CenterId = roomView.CenterId,
                        
                        };
                        return Ok(roomwithId);
                    }
                    catch (Exception ) 
                    { return NotFound("fialed to add room soory"); }
                   
                }
                else
                {
                    return Conflict($"This room id: {roomView.CenterId} does not exist");
                }
            }
              
            
            catch (Exception)
            {
                return Conflict("sorry try agin");
            }
        }
        // DELETE: api/Center
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomById(int id)
        {
            try
            {
                var Room = await _context.rooms
                      .FirstOrDefaultAsync(r => r.ID == id);
                if (Room == null)
                {
                    return NotFound("The Room does not exist");
                }

                _context.rooms.Remove(Room);
                await _context.SaveChangesAsync();

                // return Ok($"Deleted successfully center id {Room.ID}");
                return Ok(Room);
            }
            catch (Exception ex)
            { return Conflict(ex.ToString()); }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoom(int id, RoomView roomView)
        {
           
            try
            {
                var existingRoom = await _context.rooms
                                   .FirstOrDefaultAsync(r => r.ID == id);

                if (existingRoom == null)
                {
                    return NotFound("The Room does not exist");
                }
                existingRoom.Name =roomView.Name;
                existingRoom.Capacity=roomView.Capacity;

                await _context.SaveChangesAsync();
                return Ok("update saccess");
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound("The center does not exist");
            }


    }
}
}
