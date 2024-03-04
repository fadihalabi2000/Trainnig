﻿

namespace TrainnigApI.Model
{
    public class ReservationRoom: BaseNormalEntity
    {
     
        public int ReservationId { get; set; }
        public virtual Reservation? Reservation { get; set; } 
        public int RoomId { get; set; }
        public virtual Room? Room { get; set; }
        public DateTime TrainingStartDate { get; set; }
        public DateTime TrainingEndDate { get; set; }
        public double RoomCostPerDay { get; set; }
        public string ?TrainingName { get; set; }
      
    }

}
