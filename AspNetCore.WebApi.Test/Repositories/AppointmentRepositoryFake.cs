using AspNetCore.WebApi.Entities;
using AspNetCore.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.WebApi.Test.Repositories
{
    public class AppointmentRepositoryFake : IAppointmentRepository
    {
        void IAppointmentRepository.Add(AppointmentItem item)
        {
            throw new NotImplementedException();
        }

        void IAppointmentRepository.Delete(int id)
        {
            throw new NotImplementedException();
        }

        IQueryable<AppointmentItem> IAppointmentRepository.GetByDate(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        AppointmentItem IAppointmentRepository.GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        bool IAppointmentRepository.Save()
        {
            throw new NotImplementedException();
        }

        AppointmentItem IAppointmentRepository.Update(int id, AppointmentItem item)
        {
            throw new NotImplementedException();
        }
    }
}
