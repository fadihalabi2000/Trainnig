using TrainnigApI.Model;

namespace TrainnigApI.View
{
    public class TransctionReservationView
    {
        public int AccountId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfAttendees { get; set; }
        public bool IsFree { get; set; }
        public string? BookingRequestImage { get; set; }
        public List<ReservationRoomView> reservationRoomViews { get; set; } = new List<ReservationRoomView> { };
        public List<ReservationServiceView> reservationServiceViews { get; set; } = new List<ReservationServiceView> { };
    } 
   
}
