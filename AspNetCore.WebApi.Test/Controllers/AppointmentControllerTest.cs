using System.Linq;
using System.Net;
using AspNetCore.WebApi.Controllers;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using GenFu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using AspNetCore.WebApi.Dtos;

namespace AspNetCore.WebApi.Test
{
    public class AppointmentControllerTest
    {
        private AppointmentController _controller;
        private Mock<IAppointmentRepository> _repository;
        public AppointmentControllerTest()
        {
            _repository = new Mock<IAppointmentRepository>();

            _controller = new AppointmentController(_repository.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };


        }
        [Fact]
        public void GetSingleAppointmentTest()
        {
            var appointment = new AppointmentItem()
            {
                Id = 1,
                Price = 100,
                CustomerName = "test",
                Status = AppointmentStatus.Active
            };
            _repository.Setup(x => x.GetSingle(It.IsAny<int>())).Returns(appointment);

            var result = _controller.GetSingleAppointment(1);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void DeleteAppointmentTest()
        {
            _repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            var deleteResult = _controller.DeleteAppointment(1);
            _repository.VerifyAll();
        }

    }
}

