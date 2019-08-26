using System;
using System.Threading.Tasks;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using AspNetCore.WebApi.Helpers;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace AspNetCore.WebApi.Services
{
    public class InitDataService : IInitDataService
    {
        private AppSettings appSettings { get; set; }
        private AppointmentsHelper appointmentsHelper;

        public InitDataService(IOptions<AppSettings> settings, IMapper mapper)
        {
            appSettings = settings.Value;
            appointmentsHelper = new AppointmentsHelper(settings, mapper);
        }
        public async Task Initialize(AppointmentDbContext context)
        {
            string[] customers = { "John Oliver", "Trevor Noah", "Stephen Colbert" };
            string[] carModels = { "Audi A3", "Audi Q4", "Audi Q5" };

            for (int i = 0; i < customers.Length; i++)
            {
                var appointment = new AppointmentItem()
                {
                    Price = 100,
                    CustomerName = customers[i],
                    CarModel = carModels[i],
                    Created = DateTime.Now,
                    Status = AppointmentStatus.Active,
                    AppointmentDate = appointmentsHelper.GetRandomAppointmentDate(appSettings.MaximumDaysForRandomAppointmentDate)
                };
                context.AppointmentItems.Add(appointment);
            }
            await context.SaveChangesAsync();
        }
    }
}
