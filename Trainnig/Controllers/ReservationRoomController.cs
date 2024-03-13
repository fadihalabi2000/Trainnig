using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationRoomController : ControllerBase
    {
        private readonly ILogger<ReservationRoomController> _logger;
        private readonly service.IBaseService<ReservationRoom> baseService;
        public ReservationRoomController(ILogger<ReservationRoomController> logger,
                                      IBaseService<ReservationRoom> baseService)
        {

            _logger = logger;
            this.baseService = baseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationRoomView>>> GetAllAccounts()
        {
            try
            {
             var  allReservationRoom = await this.baseService.GetAllAsync();

                if (!allReservationRoom.Any())
                {
                    return NotFound("There are no ReservationRoom");
                }
                else
                {

                    return Ok(allReservationRoom);

                }
                

                
               
            }
            catch
            {
                return BadRequest("Something went wrong in the request, please try again.");
            }
        }
        [HttpGet("{id}", Name = "GetReservationRoomByID")]
        public async Task<ActionResult<ReservationRoomView>> GetReservationRoomById(int id)
        {

            var ReservationRoomById = await this.baseService.GetByIdAsync(id);


            try
            {
                if (ReservationRoomById != null)
                {
                    return Ok(ReservationRoomById);

                }
                else
                {
                    return NotFound($"The ReservationRoomById id {id} does not exist");
                }


            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationRoomView>> PostCenter(ReservationRoomView reservationRoomView)
        {
            try
            {

              
                ReservationRoom reservationRoom = new ReservationRoom()
                {
                    ReservationId = reservationRoomView.ReservationId,
                    RoomId = reservationRoomView.RoomId,
                    TrainingStartDate = reservationRoomView.TrainingStartDate,
                    TrainingEndDate = reservationRoomView.TrainingEndDate,
                    RoomCostPerDay = reservationRoomView.RoomCostPerDay,
                    TrainingName = reservationRoomView.TrainingName
                };
                await this.baseService.AddAsync(reservationRoom);
                // await _context.SaveChangesAsync();

                var AllReservationRoom = await this.baseService.GetAllAsync();
                var lastReservationRoomId = AllReservationRoom
                                          .OrderByDescending(b => b.ID)
                                          .Select(b => b.ID)
                                          .FirstOrDefault();

                Response.Headers.Append($"ReservationRoom-ID",
                                      lastReservationRoomId.ToString());
                reservationRoom.ID = lastReservationRoomId;


                return Ok(reservationRoom);
            }
            catch (Exception)
            {
                return Conflict("sorry  add try agin some think worng");
            }
        }
        [HttpDelete("{id}", Name = "DeleteReservationRoomByID")]
        public async Task<IActionResult> DeletereservationRoomByID(int id)
        {
            try
            {
                var ReservationRoomById = await this.baseService.GetByIdAsync(id);

                if (ReservationRoomById == null)
                {
                    return NotFound("The account does not exist");
                }
                else {
                    await this.baseService.DeleteAsync(id);

                    return Ok($"Deleted successfully ReservationRoom id {ReservationRoomById.ID}");
                }
                
            }
            catch (Exception ex)
            {
                return Conflict(ex.ToString());
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccountByID(int id,
                                        ReservationRoomView reservationRoomView)
        {
            try
            {
                var ReservationRoomforUpdate = await this.baseService.GetByIdAsync(id);

                if (ReservationRoomforUpdate == null)
                {
                    return NotFound($"The ReservationRoom id {id} does not exist");
                }
                else
                {
                    ReservationRoomforUpdate.ReservationId= reservationRoomView.ReservationId;
                    ReservationRoomforUpdate.RoomId= reservationRoomView.RoomId;
                    ReservationRoomforUpdate.RoomCostPerDay = reservationRoomView.RoomCostPerDay;
                    ReservationRoomforUpdate.TrainingStartDate = reservationRoomView.TrainingStartDate;
                    ReservationRoomforUpdate.TrainingEndDate = reservationRoomView.TrainingEndDate;
                    ReservationRoomforUpdate.TrainingName = reservationRoomView.TrainingName;
                       await this.baseService.UpdateAsync(ReservationRoomforUpdate);
                    Response.Headers.Append($" updatedReservationRoom that id:"
                                            , id.ToString());
                    return Ok($"update successfully account id( {id} )");


                }

            }
            catch (Exception)
            {
                return BadRequest("Something went wrong in " +
                         " the request, please try again.");
            }
        }

    }
}
