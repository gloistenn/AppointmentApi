using System.ComponentModel.DataAnnotations;

namespace AppointmentInfo.Models
{
    public class AppointmentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
    }
}
