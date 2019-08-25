using System;
using AspNetCore.WebApi.Dtos;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AspNetCore.WebApi.Helpers
{
    public class AppointmentsHelper : IAppointmentsHelper
    {
        private Random rand = new Random();
        private AppSettings appSettings { get; set; }

        public AppointmentsHelper(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
        }

        public void ScheduleRandomDateAppointment(AppointmentRepository repository, AppointmentCreateDto appointmentCreateDto)
        {
            var appointmentItem = Mapper.Map<AppointmentItem>(appointmentCreateDto);
            appointmentItem.AppointmentDate = GetRandomAppointmentDate(appSettings.MaximumDaysForRandomAppointmentDate);
            repository.Add(appointmentItem);
            repository.Save();
        }

        public DateTime GetRandomAppointmentDate(int daysToAdd)
        {
            var date = DateTime.Today.AddDays(rand.Next(daysToAdd));
            date = GetFirstWorkingDay(date);
            var randomHour = 9 + rand.Next(8);
            return new DateTime(date.Year, date.Month, date.Day, randomHour, 0, 0);
        }

        private DateTime GetFirstWorkingDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Sunday)
                date.AddDays(1);
            else if (date.DayOfWeek == DayOfWeek.Saturday)
                date.AddDays(2);
            return date;
        }
    }
}
