using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
             IBaseService<Room> baseService
                                                                       )
        {
            _reservationService = reservationService;
            _reservationRoomService = reservationRoomService;
            _reservationServiceService = reservationServiceService;
            this.baseService = baseService;

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
        public async Task<IActionResult>AddTransactionReservation([FromBody] TransctionReservationView transctionReservationView)
        {
            try 
            {
                var allReservation = await this._reservationService.GetAllAsync();
                var lastReservationId = allReservation.OrderByDescending(r => r.ID)
                                          .Select(r => r.ID)
                                          .FirstOrDefault();
                // 1. إضافة الحجز
                var reservation = new Reservation
                {
                    AccountId = transctionReservationView.AccountId,
                    ReservationDate = transctionReservationView.ReservationDate,
                    NumberOfAttendees = transctionReservationView.NumberOfAttendees,
                    IsFree = transctionReservationView.IsFree,
                    BookingRequestImage = transctionReservationView.BookingRequestImage,
                    //ReservationRooms = transctionReservationView.reservationRoomViews ,
                    //reservationServices = transctionReservationView.reservationServices
                };
                // await _context.SaveChangesAsync();
                if (lastReservationId >= 0)
                {
                    await this._reservationService.AddAsync(reservation);

                    lastReservationId += 1;
                    Response.Headers.Append($"Account-ID", lastReservationId.ToString());
                  
                    // 2. الحصول على معرف الحجز الذي تم إضافته
                    //int reservationId = reservation.ID;
                    int reservationId = lastReservationId;


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
                         await _reservationRoomService.AddAsync(reservationRoom);
                                      
                    }
                  //كود من اجل الاضافة في مكان لا يحوي تضارب
                    // 3. إضافة الغرف المحجوزة
                    //        foreach (var room in transctionReservationView.reservationRoomViews)
                    //        {
                    //            var roomReservations = await _reservationRoomService.GetAllAsync();
                    //            var roomReservationswithcon = roomReservations.Where(r =>
                    //r.RoomId == room.RoomId &&
                    //r.TrainingEndDate.Month == room.TrainingEndDate.Month&&
                    //r.TrainingEndDate.Year == room.TrainingEndDate.Year)
                    //.OrderBy(r => r.TrainingStartDate).ToList();
                    //            for (int i = 0; i < roomReservationswithcon.Count()-1; i++)

                    //            {
                    //                    var currentRoomReservation = roomReservationswithcon[i];
                    //                var nextcurrentRoomReservation = roomReservationswithcon[i+1];
                    //                if(room.TrainingStartDate>currentRoomReservation.TrainingEndDate&&
                    //                  nextcurrentRoomReservation==null)
                    //                {
                    //                    var reservationRoom = new ReservationRoom
                    //                    {
                    //                        ReservationId = reservationId, 
                    //                        RoomId = room.RoomId,
                    //                        TrainingStartDate = room.TrainingStartDate,
                    //                        TrainingEndDate = room.TrainingEndDate,
                    //                        RoomCostPerDay = room.RoomCostPerDay,
                    //                        TrainingName = room.TrainingName
                    //                    };
                    //                    await _reservationRoomService.AddAsync(reservationRoom);
                    //                    break;
                    //                }
                    //                else if(room.TrainingStartDate > currentRoomReservation.TrainingEndDate&&
                    //                    room.TrainingEndDate<nextcurrentRoomReservation.TrainingStartDate)
                    //                {
                    //                    var reservationRoom = new ReservationRoom
                    //                    {
                    //                        ReservationId = reservationId,
                    //                        RoomId = room.RoomId,
                    //                        TrainingStartDate = room.TrainingStartDate,
                    //                        TrainingEndDate = room.TrainingEndDate,
                    //                        RoomCostPerDay = room.RoomCostPerDay,
                    //                        TrainingName = room.TrainingName
                    //                    };
                    //                    await _reservationRoomService.AddAsync(reservationRoom);
                    //                    break;
                    //                }
                    //                else { return Conflict($"Room {room.RoomId} is already booked during the specified period"); }

                    //            }



                    //        }

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

                        await _reservationServiceService.AddAsync(reservationService);
                    }

                    return Ok(transctionReservationView);
                }
                else
                {
                    return Conflict("sorry failed  add try agin");
                }



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }
    }
}

