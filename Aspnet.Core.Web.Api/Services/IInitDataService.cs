using System.Threading.Tasks;
using AspNetCore.WebApi.Repositories;

namespace AspNetCore.WebApi.Services
{
    public interface IInitDataService
    {
        Task Initialize(AppointmentDbContext context);
    }
}
