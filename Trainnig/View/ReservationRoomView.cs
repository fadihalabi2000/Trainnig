using TrainnigApI.Model;

namespace TrainnigApI.View
{
    public class ReservationRoomView
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }
        public DateTime TrainingStartDate { get; set; }
        public DateTime TrainingEndDate { get; set; }
        public double RoomCostPerDay { get; set; }
        public string? TrainingName { get; set; }
    }
}
