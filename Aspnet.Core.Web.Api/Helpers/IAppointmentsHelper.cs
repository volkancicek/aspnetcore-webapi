using AspNetCore.WebApi.Dtos;
using AspNetCore.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Helpers
{
    public interface IAppointmentsHelper
    {
         void ScheduleRandomDateAppointment(AppointmentRepository repository, AppointmentCreateDto appointmentCreateDto);
    }
}
