using AspNetCore.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.WebApi.Repositories
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
           : base(options)
        {

        }

        public DbSet<AppointmentItem> AppointmentItems { get; set; }

    }
}
