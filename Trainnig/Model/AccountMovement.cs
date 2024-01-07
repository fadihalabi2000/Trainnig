﻿

using TrainnigApI.Abstration.Enum;

namespace TrainnigApI.Model
{
    public class AccountMovement: BaseNormalEntity
    {
       
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public double MovementValue { get; set; }
      
        public MovementStatus Status { get; set; }
        public DateTime MovementDate { get; set; }
        public int AccountId { get; set; }
        public Account? Account { get; set; }
        public string ?AccountStatement { get; set; }  //بيان الحساب
    }

}
