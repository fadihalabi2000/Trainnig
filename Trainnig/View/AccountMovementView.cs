using TrainnigApI.Abstration.Enum;
using TrainnigApI.Model;

namespace TrainnigApI.View
{
    public class AccountMovementView
    {
        public int? ReservationId { get; set; }
        public double MovementValue { get; set; }

        public MovementStatus Status { get; set; }
        public DateTime MovementDate { get; set; }
        public int AccountId { get; set; }
        public string? AccountStatement { get; set; }  //بيان الحساب
    }
}
