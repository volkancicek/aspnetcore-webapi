using System;

namespace AspNetCore.WebApi.Entities
{
    public class AppointmentItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CarModel { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal Price { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
