using System;
using AspNetCore.WebApi.Dtos;
using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.Controllers
{
    public class AppointmentController:ControllerBase
    {
        
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController( IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        
        [HttpDelete]
        [Route("{id:int}", Name = nameof(DeleteAppointment))]
        public ActionResult DeleteAppointment(int id)
        {
            AppointmentItem appointmentItem = _appointmentRepository.GetSingle(id);

            if (appointmentItem == null)
            {
                return NotFound();
            }

            _appointmentRepository.Delete(id);

            if (!_appointmentRepository.Save())
            {
                throw new Exception("Deleting an appointment failed on save!");
            }

            return NoContent();
        }
        
        [HttpPost(Name = nameof(CreateAppointment))]
        public ActionResult<AppointmentItemDto> CreateAppointment([FromBody] AppointmentCreateDto appointmentCreateDto)
        {
            if (appointmentCreateDto == null)
            {
                return BadRequest();
            }

            AppointmentItem toAdd = Mapper.Map<AppointmentItem>(appointmentCreateDto);

            _appointmentRepository.Add(toAdd);

            if (!_appointmentRepository.Save())
            {
                throw new Exception("Creating an appointment failed on save!");
            }

            AppointmentItem newAppointmentItem = _appointmentRepository.GetSingle(toAdd.Id);

            return CreatedAtRoute(nameof(GetSingleAppointment), new { id = newAppointmentItem.Id },
                Mapper.Map<AppointmentItemDto>(newAppointmentItem));
        }
        
        [HttpPatch("{id:int}", Name = nameof(UpdateAppointmentStatus))]
        public ActionResult<AppointmentItemDto> UpdateAppointmentStatus(int id, [FromBody] JsonPatchDocument<AppointmentUpdateStatusDto> appointmentStatus)
        {
            if (appointmentStatus == null)
            {
                return BadRequest();
            }

            AppointmentItem existingEntity = _appointmentRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            AppointmentUpdateStatusDto appointmentUpdateStatusDto = Mapper.Map<AppointmentUpdateStatusDto>(existingEntity);
            appointmentStatus.ApplyTo(appointmentUpdateStatusDto, ModelState);

            TryValidateModel(appointmentUpdateStatusDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(appointmentUpdateStatusDto, existingEntity);
            AppointmentItem updated = _appointmentRepository.Update(id, existingEntity);

            if (!_appointmentRepository.Save())
            {
                throw new Exception("Updating an appointment status failed on save!");
            }

            return Ok(Mapper.Map<AppointmentItemDto>(updated));
        }  
       
        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleAppointment))]
        public ActionResult GetSingleAppointment(int id)
        {
            AppointmentItem appointmentItem = _appointmentRepository.GetSingle(id);

            if (appointmentItem == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<AppointmentItemDto>(appointmentItem));
        }
        
        [HttpGet]
        [Route("/bydate", Name = nameof(GetAppointmentsByDate))]
        public ActionResult GetAppointmentsByDate([FromBody]AppointmentGetByDateDto appointmentByDateDto)
        {
            var appointmentItems =
                _appointmentRepository.GetByDate(appointmentByDateDto.StartDate, appointmentByDateDto.EndDate);

            if (appointmentItems == null)
            {
                return NotFound();
            }

            foreach (var item in appointmentItems)
            {
                Mapper.Map<AppointmentItemDto>(item);
            }

            return Ok(appointmentItems);
        } 
    }
}