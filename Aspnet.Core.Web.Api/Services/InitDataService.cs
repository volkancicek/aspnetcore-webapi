using System;
using System.Threading.Tasks;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;

namespace AspNetCore.WebApi.Services
{
    public class InitDataService : IInitDataService
    {
        public async Task Initialize(AppointmentDbContext context)
        {
            context.AppointmentItems.Add(new AppointmentItem() {Price = 120, Name = "John", Surname = "Oliver", Created = DateTime.Now.AddDays(2), Status = AppointmentStatus.Active});
            context.AppointmentItems.Add(new AppointmentItem() {Price = 110, Name = "Trevor", Surname = "Noah", Created = DateTime.Now.AddDays(3), Status = AppointmentStatus.Active});

            await context.SaveChangesAsync();
        }
    }
}
