using AppointmentInfo.Data;
using AppointmentInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentInfo.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;
        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }
      
        public IEnumerable<Appointment> GetAppointmentsByDate(int consultantId, DateTime startTime)
        {
            List<Models.Appointment> existingAppointments = _context.Appointments
                    .Where(a => a.ConsultantId == consultantId && a.StartTime.Date == startTime.Date)
                    .ToList();
            return existingAppointments;
        }
    }
}
