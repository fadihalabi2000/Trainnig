using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<AccuontController> _logger;
        private readonly service.IBaseService<Reservation> baseService;
        public ReservationController(ILogger<AccuontController> logger, IBaseService<Reservation> baseService)
        {

            _logger = logger;
            this.baseService = baseService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservation()
        {
            try
            {
                var reservation = await this.baseService.GetAllAsync();

                if (!reservation.Any())
                {
                    return NotFound("There are no reservation");
                }

                return Ok(reservation);
            }
            catch
            {
                return BadRequest("Something went wrong in the request, please try again.");
            }
        }
        [HttpGet("{id}", Name = "GetReservationByID")]
        public async Task<ActionResult<Reservation>> GetAccountById(int id)
        {

            var reservationById = await this.baseService.GetByIdAsync(id);


            try
            {
                if (reservationById == null)
                {
                    return NotFound($"The Reservation id {id} does not exist");
                }

                return Ok(reservationById);
            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
        }
        [HttpPost]
        public async Task<ActionResult<ReservationView>> PostReservation(ReservationView reservationView)
        {
            try
            {

            
                Reservation reservation = new Reservation()
                {
                     AccountId=reservationView.AccountId,
                     BookingRequestImage=reservationView.BookingRequestImage,
                     IsFree=reservationView.IsFree,
                     ReservationDate=reservationView.ReservationDate,
                     NumberOfAttendees=reservationView.NumberOfAttendees,
                };
               
                // await _context.SaveChangesAsync();
                 await this.baseService.AddAsync(reservation);
                    var allReservation = await this.baseService.GetAllAsync();
                    var lastReservationId = allReservation.OrderByDescending(r => r.ID)
                                              .Select(r => r.ID)
                                              .FirstOrDefault();
              
                    Response.Headers.Append($"Account-ID", lastReservationId.ToString());
                reservation.ID = lastReservationId;
                    return Ok(reservation);
                   // return Ok(reservation);
               
             
               
            }
            catch (Exception)
            {
               
                return BadRequest("try agin something wong in request");
            }
        }
        [HttpDelete("{id}", Name = "DeleteReservationByID")]
        public async Task<IActionResult> DeleteReservationByID(int id)
        {
            try
            {
                var ReservationById = await this.baseService.GetByIdAsync(id);

                if (ReservationById == null)
                {
                    return NotFound($"The Reservation id:{id} does not exist");
                }
                else
                {
                    await this.baseService.DeleteAsync(id);

                    return Ok($"Deleted successfully Reservation id {ReservationById.ID}");
                  // return Ok (ReservationById);
                }

            }
            catch (Exception )
            {
                return BadRequest("try agin something wong in request");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationByID(int id, ReservationView reservationView)
        {
            try
            {
                var ReservationByIdForUpdate = await this.baseService.GetByIdAsync(id);

                if (ReservationByIdForUpdate == null)
                {
                    return NotFound($"The Reservation id {id} does not exist");
                }
                else
                {
                    ReservationByIdForUpdate.AccountId= reservationView.AccountId;
                    ReservationByIdForUpdate.BookingRequestImage = reservationView.BookingRequestImage;
                    ReservationByIdForUpdate.NumberOfAttendees= reservationView.NumberOfAttendees;
                    ReservationByIdForUpdate.IsFree= reservationView.IsFree;
                    ReservationByIdForUpdate.ReservationDate= reservationView.ReservationDate;
                     await this.baseService.UpdateAsync(ReservationByIdForUpdate);
                    Response.Headers.Append($" updatedReservation with id to:", id.ToString());
                    return Ok($"update successfully account id( {ReservationByIdForUpdate.ID}  )");
                // return Ok (ReservationByIdForUpdate);
                                  
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
