using System;
using System.Linq;
using AspNetCore.WebApi.Entities;

namespace AspNetCore.WebApi.Repositories
{
    public interface IAppointmentRepository
    {
        AppointmentItem GetSingle(int id);
        void Add(AppointmentItem item);
        void Delete(int id);
        AppointmentItem Update(int id, AppointmentItem item);
        IQueryable<AppointmentItem> GetByDate(DateTime startDate, DateTime endDate);

        bool Save();
    }
}
