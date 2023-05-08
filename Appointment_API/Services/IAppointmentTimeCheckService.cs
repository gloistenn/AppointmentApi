namespace AppointmentInfo.Services
{
    public interface IAppointmentTimeCheckService
    {
        bool IsWithinOpeningHours(DateTime startTime, DateTime endTime);
        bool IsAvailable(DateTime startTime, DateTime endTime, int durationInMinutes, int consultantId);
    }
}
