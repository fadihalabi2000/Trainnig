using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionReservationController : ControllerBase
    {
        private readonly IBaseService<Reservation> _reservationService;
        private readonly IBaseService<ReservationRoom> _reservationRoomService;
        private readonly IBaseService<ReservationService> _reservationServiceService;
        private readonly ILogger<AccuontController> _logger;
        private readonly service.IBaseService<Room> baseService;
        private readonly AppDBContext _context;
        public TransactionReservationController(
            IBaseService<Reservation> reservationService,
            IBaseService<ReservationRoom> reservationRoomService,
            IBaseService<ReservationService> reservationServiceService,
             IBaseService<Room> baseService,
                     AppDBContext context)
        {
            _reservationService = reservationService;
            _reservationRoomService = reservationRoomService;
            _reservationServiceService = reservationServiceService;
            this.baseService = baseService;
            _context = context;
        }
        [HttpGet("GetUnreservedRooms")]
        public async Task<ActionResult<IEnumerable<ReservationRoomView>>> GetUnreservedRooms(DateTime userStartDate, DateTime userEndDate)
        {   //DateTime startDate, DateTime endDate
            //DateTime userStartDate = DateTime.Parse("2024-02-19");
            //DateTime userEndDate = DateTime.Parse("2024-02-21");
       
            try
            {
                var allReservationRoom = await this._reservationRoomService.GetAllAsync();
               var ListOfResrvationID= allReservationRoom.Where(reservation=>(reservation.TrainingStartDate.Date >= userStartDate.Date &&
                    reservation.TrainingStartDate.Date <= userEndDate.Date)
                    || (reservation.TrainingEndDate >= userStartDate.Date &&
                    reservation.TrainingEndDate.Date <= userEndDate.Date)).Select(x=>x.RoomId);
                var allRoom=await this.baseService.GetAllAsync();

                if (!allReservationRoom.Any()&& !allRoom.Any())
                {
                    return NotFound("There are no ReservationRoom or Rooms");
                }
                else
                {
                   
                    var availableRooms = allRoom.Where(room => !ListOfResrvationID
                    .Contains(room.ID)).ToList();
                    


                    return Ok(availableRooms);
                }
            }
            catch
            {
                return BadRequest("Something went wrong in the request, please try again.");
            }
        } 
    
    
        [HttpPost("AddTransactionReservation")]
        public async Task<IActionResult> AddTransactionReservation([FromBody] TransctionReservationView transctionReservationView)
        {
            try
            {
                 using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            

                            // 1. إضافة الحجز
                            var reservation = new Reservation
                            {    
                                AccountId = transctionReservationView.AccountId,
                                ReservationDate = transctionReservationView.ReservationDate,
                                NumberOfAttendees = transctionReservationView.NumberOfAttendees,
                                IsFree = transctionReservationView.IsFree,
                                BookingRequestImage = transctionReservationView.BookingRequestImage,
                            };

                           
                            //await this._reservationService.AddAsync(reservation);
                            await this._context.reservations.AddAsync(reservation);
                            await _context.SaveChangesAsync();

                        var allReservation = await this._reservationService.GetAllAsync();
                        var lastReservationId = allReservation.OrderByDescending(r => r.ID)
                                                  .Select(r => r.ID)
                                                  .FirstOrDefault();

                        Response.Headers.Append($"Account-ID", lastReservationId.ToString());

                                // 2. الحصول على معرف الحجز الذي تم إضافته
                                int reservationId = lastReservationId;

                                // 3. إضافة الغرف المحجوزة
                                foreach (var room in transctionReservationView.reservationRoomViews)
                                {
                                    var reservationRoom = new ReservationRoom
                                    {
                                        ReservationId = reservationId,
                                        RoomId = room.RoomId,
                                        TrainingStartDate = room.TrainingStartDate,
                                        TrainingEndDate = room.TrainingEndDate,
                                        RoomCostPerDay = room.RoomCostPerDay,
                                        TrainingName = room.TrainingName
                                    };
                                // await _reservationRoomService.AddAsync(reservationRoom);
                                await _context.reservationRooms.AddAsync(reservationRoom);
                            }

                                // 4. إضافة الخدمات المقدمة
                                foreach (var service in transctionReservationView.reservationServiceViews)
                                {
                                    var reservationService = new ReservationService
                                    {
                                        ReservationId = reservationId,
                                        ServiceId = service.ServiceId,
                                        DurationDays = service.DurationDays,
                                        numberofBeneficiaries = service.numberofBeneficiaries,
                                        UnitPrice = service.UnitPrice,
                                        IsFree = service.IsFree
                                    };

                                //await _reservationServiceService.AddAsync(reservationService);
                                await _context.reservationServices.AddAsync(reservationService);
                            }

                            transctionReservationView.ReservationId = reservationId;

                            await _context.SaveChangesAsync();
                            // اتمام العمليات بنجاح
                            transaction.Commit();

                                return Ok(transctionReservationView);
                            
                            
                        }
                        catch (Exception)
                        {
                            // فشل أثناء العمليات، قم بالتراجع
                            transaction.Rollback();
                            throw;
                        }
                    }
              
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }


}

