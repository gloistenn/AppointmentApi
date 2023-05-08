using AppointmentInfo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentInfo.Data;
using AppointmentInfo.Services;
using AppointmentInfo.Repository;

namespace AppointmentInfo.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentDbContext _context;
        private readonly IAppointmentTimeCheckService _appointmentTimeCheck;
        private readonly IAppointmentRepository _appointmentRepository;
       

        public AppointmentsController(AppointmentDbContext context, IAppointmentTimeCheckService appointmentTimeCheckService, IAppointmentRepository appointmentRepository)
        {
            _context = context;
            _appointmentTimeCheck = appointmentTimeCheckService;
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Appointment>>> GetAppointmentList(DateTime startOfWeek, int appointmentTypeId)
        {                      
            DateTime weekEnd = startOfWeek.AddDays(7);

            var appointmentType = await _context.AppointmentTypes.FindAsync(appointmentTypeId);
            if (appointmentType == null)
            {
                return BadRequest($"Appointment type with id {appointmentTypeId} does not exist.");
            }

           
            var consultants = await _context.Consultants.ToListAsync();
            var availableAppointments = new List<Models.Appointment>();
            foreach (var consultant in consultants)
            {
                
                var existingAppointments = await _context.Appointments.Where(a => a.ConsultantId == consultant.Id &&
                                                                                   a.StartTime >= startOfWeek && a.EndTime <= weekEnd).ToListAsync();

                
                var availableTimeSlots = new List<DateTime>();
                DateTime currentDate = startOfWeek;
                while (currentDate < weekEnd)
                {
                   
                    if (currentDate.Hour >= 8 && currentDate.Hour < 12 || currentDate.Hour >= 13 && currentDate.Hour < 16 && currentDate.DayOfWeek != DayOfWeek.Friday || currentDate.Hour >= 8 && currentDate.Hour < 12 && currentDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        
                        var conflictingAppointments = existingAppointments.Where(a => a.StartTime <= currentDate && a.EndTime > currentDate).ToList();
                        if (conflictingAppointments.Count == 0)
                        {
                            availableTimeSlots.Add(currentDate);
                        }
                    }

                    currentDate = currentDate.AddMinutes(appointmentType.DurationInMinutes);
                }

                foreach (var timeSlot in availableTimeSlots)
                {
                    var appointment = new Models.Appointment
                    {
                        ConsultantId = consultant.Id,
                        AppointmentTypeId = appointmentTypeId,
                        StartTime = timeSlot,
                        EndTime = timeSlot.AddMinutes(appointmentType.DurationInMinutes)
                    };
                    availableAppointments.Add(appointment);
                }
            }

            return availableAppointments;
        }
        [HttpPost]
        public IActionResult PostAppointment([FromBody] Models.Appointment appointment)
        {
            try
            {
                if (!_appointmentTimeCheck.IsWithinOpeningHours(appointment.StartTime, appointment.EndTime))
                {
                    return BadRequest("The appointment is not within the opening hours.");
                }

                AppointmentType appointmentType = _context.AppointmentTypes.FirstOrDefault(a => a.Id == appointment.AppointmentTypeId);
                if (appointmentType == null)
                {
                    return BadRequest("Invalid appointment type.");
                }

                Consultant consultant = _context.Consultants.FirstOrDefault(c => c.Id == appointment.ConsultantId);
                if (consultant == null)
                {
                    return BadRequest("Invalid consultant.");
                }

                if (!_appointmentTimeCheck.IsAvailable(appointment.StartTime, appointment.EndTime, appointmentType.DurationInMinutes, consultant.Id))
                {
                    return BadRequest("Appointment time is not available.");
                }

                appointment.CustomerName = appointment.CustomerName.Trim();
                appointment.CustomerEmail = appointment.CustomerEmail.Trim();
                appointment.CustomerContractNumber = appointment.CustomerContractNumber.Trim();
                appointment.CustomerTotalContractAmount = Math.Round(appointment.CustomerTotalContractAmount, 2);

                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                return Ok("The appointment was created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the appointment.");
            }
        }

        
    }
}
