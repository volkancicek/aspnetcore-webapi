using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.WebApi.Dtos
{
    public class AppointmentCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Created { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
