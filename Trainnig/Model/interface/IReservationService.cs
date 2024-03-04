using TrainnigApI.service;
using TrainnigApI.View;

namespace TrainnigApI.Model
{
    public interface IReservationService 
    {
        Task AddTransactionReservation(TransctionReservationView transactionReservationView);
    }
}
