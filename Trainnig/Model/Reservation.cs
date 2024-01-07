﻿

namespace TrainnigApI.Model
{
    public class Reservation: BaseNormalEntity
    {

        public int AccountId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int NumberOfAttendees { get; set; }
        public bool IsFree { get; set; }
        public string ?BookingRequestImage { get; set; }
     
    }

}