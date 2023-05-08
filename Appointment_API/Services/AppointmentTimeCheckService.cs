using AppointmentInfo.Repository;
using Microsoft.EntityFrameworkCore;

namespace AppointmentInfo.Services
{
    public class AppointmentTimeCheckService: IAppointmentTimeCheckService
    {
        public readonly IAppointmentRepository _appointmentRepository;
        public AppointmentTimeCheckService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public bool IsWithinOpeningHours(DateTime startTime, DateTime endTime)
        {
            bool isValid = true;

           
            if (startTime.DayOfWeek == DayOfWeek.Friday && (startTime.TimeOfDay < new TimeSpan(8, 0, 0) || startTime.TimeOfDay > new TimeSpan(12, 0, 0)))
            {
                isValid = false;
            }
            else if (startTime.TimeOfDay < new TimeSpan(8, 0, 0) || startTime.TimeOfDay > new TimeSpan(16, 0, 0))
            {
                isValid = false;
            }

            
            if (endTime.DayOfWeek == DayOfWeek.Friday && (endTime.TimeOfDay < new TimeSpan(8, 0, 0) || endTime.TimeOfDay > new TimeSpan(12, 0, 0)))
            {
                isValid = false;
            }
            else if (endTime.TimeOfDay < new TimeSpan(9, 0, 0) || endTime.TimeOfDay > new TimeSpan(17, 0, 0))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsAvailable(DateTime startTime, DateTime endTime, int durationInMinutes, int consultantId)
        {
            bool isAvailable = true;

          
            if (startTime.Date != endTime.Date)
            {
                isAvailable = false;
            }

            
            if (isAvailable)
            {
               var existingAppointments = _appointmentRepository.GetAppointmentsByDate(consultantId, startTime);

                foreach (Models.Appointment existingAppointment in existingAppointments)
                {
                 
                    if (startTime < existingAppointment.EndTime && existingAppointment.StartTime < endTime)
                    {
                        isAvailable = false;
                        break;
                    }
                }
            }
            return isAvailable;
        }
    }
}
