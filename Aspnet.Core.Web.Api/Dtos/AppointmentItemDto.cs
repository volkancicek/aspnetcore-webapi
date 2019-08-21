using System;

namespace AspNetCore.WebApi.Dtos
{
    public class AppointmentItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime Created { get; set; }
        public decimal Price { get; set; }
    }
}
