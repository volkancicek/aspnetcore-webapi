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
using AutoMapper;
using System;

namespace AspNetCore.WebApi.Test
{
    public class AppointmentControllerTest
    {
        private AppointmentController _controller;
        private Mock<IAppointmentRepository> _repository;
        private IMapper _mapper;
        public AppointmentControllerTest()
        {
            _mapper = GetMapper();
            _repository = new Mock<IAppointmentRepository>();
            _controller = new AppointmentController(_repository.Object, _mapper);
            _repository.Setup(x => x.Save()).Returns(true);
        }

        public class GetSingleAppointmentTests : AppointmentControllerTest
        {
            [Fact]
            public void Should_get_single_appointment_with_existing_id()
            {
                var appointment = new AppointmentItem()
                {
                    Id = 1
                };

                _repository.Setup(x => x.GetSingle(1)).Returns(appointment);
                var result = _controller.GetSingleAppointment(1);
                _repository.Verify(x => x.GetSingle(1), Times.Once);
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);
            }

            [Fact]
            public void Should_not_found_single_appointment_with_unknown_id()
            {   
                var result = _controller.GetSingleAppointment(2);
                _repository.Verify(x => x.GetSingle(2), Times.Once);
                Assert.NotNull(result);
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public void Should_get_correct_single_appointment_item()
            {
                var appointment = new AppointmentItem()
                {
                    Id = 1,
                    CustomerName = "test"
                };
                _repository.Setup(x => x.GetSingle(1)).Returns(appointment);
                var result = _controller.GetSingleAppointment(1) as OkObjectResult;
                _repository.Verify(x => x.GetSingle(1), Times.Once);

                Assert.IsType<AppointmentItemDto>(result.Value);
                Assert.Equal(appointment.Id, (result.Value as AppointmentItemDto).Id);
                Assert.Equal(appointment.CustomerName, (result.Value as AppointmentItemDto).CustomerName);
            }

        }

        public class DeleteAppointmentTests : AppointmentControllerTest
        {
            [Fact]
            public void Should_not_found_not_existing_item()
            {
                var deleteResult = _controller.DeleteAppointment(2);
                _repository.Verify(x => x.GetSingle(2), Times.Once);

                Assert.NotNull(deleteResult);
                Assert.IsType<NotFoundResult>(deleteResult);
            }

            [Fact]
            public void Should_delete_existing_item()
            {
                var appointment = new AppointmentItem()
                {
                    Id = 1,
                };
                _repository.Setup(x => x.GetSingle(1)).Returns(appointment);
                var deleteResult = _controller.DeleteAppointment(1);
                _repository.Verify(x => x.GetSingle(1), Times.Once);
                _repository.Verify(x => x.Delete(1), Times.Once);
                _repository.Verify(x => x.Save(), Times.Once);

                Assert.NotNull(deleteResult);
                Assert.IsType<NoContentResult>(deleteResult);
            }
        }

        public class CreateAppointmentTests : AppointmentControllerTest
        {
            [Fact]
            public void Should_create_appointment()
            {
                var appointment = new AppointmentCreateDto()
                {
                    Price = 100,
                    CustomerName = "test",
                    Status = AppointmentStatus.Active,
                    CarModel = "test car"
                };
                var appointmentItem = _mapper.Map<AppointmentItem>(appointment);
                _repository.Setup(x => x.GetSingle(0)).Returns(appointmentItem);
                var result = _controller.CreateAppointment(appointment);
                //_repository.Verify(x => x.Add(appointmentItem), Times.Once);
                _repository.Verify(x => x.GetSingle(0), Times.Once);
                _repository.Verify(x => x.Save(), Times.Once);

                Assert.NotNull(result);
                Assert.IsType<CreatedAtRouteResult>(result);
            }
        }

        private IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });
            return mappingConfig.CreateMapper();
        }
    }
}

