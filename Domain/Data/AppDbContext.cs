using ITentikaTest.Entities;
using Microsoft.EntityFrameworkCore;


namespace ITentikaTest.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Incident> Incidents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //todo read from cfg
            var sqlServerConnectionStruing = "Server=(localdb)\\MSSQLLocalDB; Database=EventsIncidentsDb; Trusted_Connection=True";
            optionsBuilder.UseSqlServer(sqlServerConnectionStruing);
        }
    }
}
