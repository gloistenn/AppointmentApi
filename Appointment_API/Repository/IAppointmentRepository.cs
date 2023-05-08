using AppointmentInfo.Models;

namespace AppointmentInfo.Repository
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAppointmentsByDate(int consultantId, DateTime startTime);
    }
}
