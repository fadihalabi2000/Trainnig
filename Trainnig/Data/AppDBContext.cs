using Microsoft.EntityFrameworkCore;
using TrainnigApI.Model;

namespace TrainnigApI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }
        public DbSet<Center> Centers { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<ReservationRoom> reservationRooms { get; set; }
        public DbSet<Service> services { get; set; }
        public DbSet<ReservationService> reservationServices { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<AccountMovement> accountMovements { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        
        
        //    optionsBuilder.UseSqlServer("Data Source =(localdb)\\MSSQLLocalDB ; " +
        //                                "Initial Catalog = TrainingCenter");
        //}
        //if (!optionsBuilder.IsConfigured)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //       .SetBasePath(Directory.GetCurrentDirectory())
        //       .AddJsonFile("appsettings.json")
        //       .Build();
        //    var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
        //    optionsBuilder.UseSqlServer(connectionString);
        //}
    }
}
