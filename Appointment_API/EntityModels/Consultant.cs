using System.ComponentModel.DataAnnotations;

namespace AppointmentInfo.Models
{
    public class Consultant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
