using System;
using System.Linq;
using AspNetCore.WebApi.Entities;

namespace AspNetCore.WebApi.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _appointmentDbContext;

        public AppointmentRepository(AppointmentDbContext appointmentDbContext)
        {
            _appointmentDbContext = appointmentDbContext;
        }

        public AppointmentItem GetSingle(int id)
        {
            return _appointmentDbContext.AppointmentItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(AppointmentItem item)
        {
            _appointmentDbContext.AppointmentItems.Add(item);
        }

        public void Delete(int id)
        {
            AppointmentItem appointmentItem = GetSingle(id);
            _appointmentDbContext.AppointmentItems.Remove(appointmentItem);
        }

        public AppointmentItem Update(int id, AppointmentItem item)
        {
            item.Updated = DateTime.Now;
            _appointmentDbContext.AppointmentItems.Update(item);
            return item;
        }

        public IQueryable<AppointmentItem> GetByDate(DateTime startDate, DateTime endDate)
        {
            IQueryable<AppointmentItem> appointmentItems = _appointmentDbContext.AppointmentItems.
                Where(x => x.AppointmentDate > startDate && x.AppointmentDate < endDate ).OrderBy(x => x.Price);
            
            return appointmentItems;
        }

        public bool Save()
        {
            return (_appointmentDbContext.SaveChanges() >= 0);
        }
    }
}
