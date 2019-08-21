using System;

namespace AspNetCore.WebApi.Entities
{
    public class AppointmentItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal Price { get; set; }
    }
}
