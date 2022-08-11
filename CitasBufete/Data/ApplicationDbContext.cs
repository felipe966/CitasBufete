using Microsoft.EntityFrameworkCore;
using CitasBufete.Models;

namespace CitasBufete.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)   
        {

        }

        public DbSet<Cita>  Cita { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
    }
}
