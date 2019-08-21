using System;

namespace AspNetCore.WebApi.Dtos
{
    public class AppointmentGetByDateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}