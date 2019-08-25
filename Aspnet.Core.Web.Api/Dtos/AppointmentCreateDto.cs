using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.WebApi.Dtos
{
    public class AppointmentCreateDto
    {
        [Required]
        public string CustomerName { get; set; }
        public string CarModel { get; set; }
        public decimal Price { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime Created { get; set; }
        public AppointmentStatus Status { get; set; }



    }
}
