

namespace TrainnigApI.Model
{
    public class ReservationService: BaseNormalEntity
    {
       
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public int? DurationDays { get; set; } //عدد ايام
        public double? numberofBeneficiaries{ get; set; } //عدد المستفيدين من الخدمة
        public double? UnitPrice { get; set; } 
        public bool IsFree { get; set; }
    }

}
