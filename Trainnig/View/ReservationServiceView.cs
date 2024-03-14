using TrainnigApI.Model;

namespace TrainnigApI.View
{
    public class ReservationServiceView
    {
    
        public int ReservationId { get; set; }
        public int ServiceId { get; set; }
        public int? DurationDays { get; set; } //عدد ايام
        public double? numberofBeneficiaries { get; set; } //عدد المستفيدين من الخدمة
        public double? UnitPrice { get; set; }
        public bool IsFree { get; set; }
    }
}
