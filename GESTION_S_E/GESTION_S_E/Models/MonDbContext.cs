using Microsoft.EntityFrameworkCore;

namespace GESTION_S_E.Models
{
    public class MonDbContext : DbContext
    {
        public MonDbContext(DbContextOptions<MonDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salle> Salles { get; set; }
    }
}