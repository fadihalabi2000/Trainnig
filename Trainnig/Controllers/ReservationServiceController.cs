using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationServiceController : ControllerBase
    {
        private readonly ILogger<ReservationServiceController> _logger;
        private readonly service.IBaseService<ReservationService> baseService;
        private readonly service.IBaseService<Center> baseService1;
        private readonly AppDBContext _context;
        public ReservationServiceController(ILogger<ReservationServiceController> logger,
                                      IBaseService<ReservationService> baseService,
        IBaseService<Center> baseService1,
        AppDBContext context
                                 
            )
        {

            _logger = logger;
            this.baseService = baseService;
            this.baseService1 = baseService1;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationServiceView>>> GetAllAccounts()
        {
            try
            {
                var allReservationService = await this.baseService.GetAllAsync();

                if (!allReservationService.Any())
                {
                    return NotFound("There are no ReservationService");
                }

                return Ok(allReservationService);
            }
            catch
            {
                return BadRequest("Something went wrong in the request, please try again.");
            }
        }
        [HttpGet("{id}", Name = "GetReservationServiceByID")]
        public async Task<ActionResult<ReservationService>> GetReservationRoomById(int id)
        {

            var ReservationServiceById = await this.baseService.GetByIdAsync(id);


            try
            {
                if (ReservationServiceById != null)
                {
                    return Ok(ReservationServiceById);

                }
                else
                {
                    return NotFound($"The ReservationServiceById, id {id} does not exist");
                }


            }
            catch
            {
                return BadRequest("try agin something wong in request");
            }
        }


        /// <summary>
        /// notes remmber add validation to the service and reservation id 
        /// </summary>
        /// <param name="reservationServiceView"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ReservationServiceView>> PostReservationService(
                                          ReservationServiceView reservationServiceView)
        {
            try
            {
               
                               
                var AllReservationService = await this.baseService.GetAllAsync();
                var lastReservationServiceId = AllReservationService
                                          .OrderByDescending(b => b.ID)
                                          .Select(b => b.ID)
                                          .FirstOrDefault();
                if (reservationServiceView.ServiceId >= 0 &&
                      reservationServiceView.ReservationId >= 0 &&
                      lastReservationServiceId >= 0
                            )
                {
                    ReservationService reservationService = new ReservationService()
                    {
                        ReservationId = reservationServiceView.ReservationId,
                        ServiceId = reservationServiceView.ServiceId,
                        numberofBeneficiaries = reservationServiceView.numberofBeneficiaries,
                        DurationDays = reservationServiceView.DurationDays,
                        UnitPrice = reservationServiceView.UnitPrice,
                        IsFree = reservationServiceView.IsFree
                    };
                    await this.baseService.AddAsync(reservationService);
                    // await _context.SaveChangesAsync();
                    if (lastReservationServiceId >= 0)
                    {
                        lastReservationServiceId += 1;
                        Response.Headers.Append($"ReservationService-ID",
                                          lastReservationServiceId.ToString());
                    }
                    return Ok(reservationService);
                }
                else { return BadRequest( "Service or resevation dose not exsist"); }
               
            }
            catch (Exception)
            {
                return Conflict("sorry  add try agin some think worng");
            }
        }
        [HttpDelete("{id}", Name = "DeleteReservationServiceByID")]
        public async Task<IActionResult> DeleteReservationServiceByID(int id)
        {
            try
            {
                var ReservationServiceById = await this.baseService.GetByIdAsync(id);

                if (ReservationServiceById == null)
                {
                    return NotFound("The ReservationService does not exist");
                }
                else
                {
                    await this.baseService.DeleteAsync(id);

                    return Ok($"Deleted successfully ReservationServiceById id {id}");
                }

            }
            catch (Exception ex)
            {
                return Conflict(ex.ToString());
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationServiceById(int id,
                                       ReservationServiceView reservationServiceView)
        {
            try
            {
                var ReservationServiceforUpdate = await this.baseService.GetByIdAsync(id);

                if (ReservationServiceforUpdate == null)
                {
                    return NotFound($"The ReservationService id {id} does not exist");
                }
                else
                {
                    ReservationServiceforUpdate.ServiceId = reservationServiceView.ServiceId;
                    ReservationServiceforUpdate.ReservationId = reservationServiceView.ReservationId;
                    ReservationServiceforUpdate.numberofBeneficiaries = reservationServiceView.numberofBeneficiaries;
                    ReservationServiceforUpdate.UnitPrice = reservationServiceView.UnitPrice;
                    ReservationServiceforUpdate.IsFree = reservationServiceView.IsFree;
                    ReservationServiceforUpdate.DurationDays = reservationServiceView.DurationDays;
                    await this.baseService.UpdateAsync(ReservationServiceforUpdate);
                    Response.Headers.Append($" updatedReservationService that id:"
                                            , id.ToString());
                    return Ok($"update successfully ReservationService id( {id} )");


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
