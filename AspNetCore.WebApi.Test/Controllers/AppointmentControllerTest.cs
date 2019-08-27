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
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
            Setup();
        }

        public void Setup()
        {
            _repository.Setup(x => x.Save()).Returns(true);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            _controller.ObjectValidator = objectValidator.Object;
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
                var result = _controller.CreateAppointment(appointment).Result as CreatedAtRouteResult;
                _repository.Verify(x => x.GetSingle(0), Times.Once);
                _repository.Verify(x => x.Save(), Times.Once);

                Assert.NotNull(result);
                Assert.IsType<AppointmentItemDto>(result.Value);
            }

            [Fact]
            public void Should_return_bad_request_to_empty_appointment()
            {
                var result = _controller.CreateAppointment(null);

                Assert.IsType<BadRequestResult>(result.Result);
            }
        }

        public class UpdateAppointmentStatusTests : AppointmentControllerTest
        {
            [Fact]
            public void Should_update_existing_item_status()
            {
                var appointmentItem = new AppointmentItem() { Id = 1, Status = AppointmentStatus.Completed };
                var patchObject = new JsonPatchDocument<AppointmentUpdateStatusDto>();
                patchObject.Replace<AppointmentStatus>(s => s.Status, AppointmentStatus.Completed);
                _repository.Setup(x => x.GetSingle(1)).Returns(appointmentItem);
                _repository.Setup(x => x.Update(1, It.IsAny<AppointmentItem>())).Returns(appointmentItem);
                var result = _controller.UpdateAppointmentStatus(1, patchObject).Result as OkObjectResult;
                _repository.Verify(x => x.GetSingle(1), Times.Once);
                _repository.Verify(x => x.Update(1,appointmentItem), Times.Once);

                Assert.NotNull(result);
                Assert.IsType<AppointmentItemDto>(result.Value);
                Assert.Equal(AppointmentStatus.Completed, (result.Value as AppointmentItemDto).Status);
            }

            [Fact]
            public void Should_not_found_unknown_item()
            {
                var patchObject = new JsonPatchDocument<AppointmentUpdateStatusDto>();
                patchObject.Replace<AppointmentStatus>(s => s.Status, AppointmentStatus.Completed);
                var result = _controller.UpdateAppointmentStatus(1, patchObject).Result;
                _repository.Verify(x => x.GetSingle(1), Times.Once);

                Assert.NotNull(result);
                Assert.IsType<NotFoundResult>(result);

            }
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

        public class GetAppointmentsByDateTests:AppointmentControllerTest
        {
            [Fact]
            public void Should_get_appointments_by_date()
            {
                var dto = new AppointmentGetByDateDto()
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1)
                };
                var item1 = new AppointmentItem { Id = 1, AppointmentDate=DateTime.Now.AddDays(5), CustomerName = "test1" };
                var item2 = new AppointmentItem { Id = 2, AppointmentDate = DateTime.Now.AddDays(15), CustomerName = "test2" };
                var itemList = Enumerable.Empty<AppointmentItem>().AsQueryable();
                itemList.Append(item1);
                itemList.Append(item2);

                _repository.Setup(x => x.GetByDate(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(itemList);
                var result = _controller.GetAppointmentsByDate(dto) as OkObjectResult;
                _repository.Verify(x => x.GetByDate(dto.StartDate, dto.EndDate), Times.Once);

                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);
            }

            [Fact]
            public void Should_not_get_appointments_out_of_given_range()
            {
                var dto = new AppointmentGetByDateDto()
                {
                   StartDate = DateTime.Now,
                   EndDate = DateTime.Now
                };
                var result = _controller.GetAppointmentsByDate(dto);
                _repository.Verify(x => x.GetByDate(dto.StartDate,dto.EndDate), Times.Once);
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result);
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

