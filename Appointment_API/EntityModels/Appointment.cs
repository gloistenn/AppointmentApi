using AppointmentInfo_API.Helpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppointmentInfo.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [TypeConverter(typeof(DateTimeToConvert))]
        public DateTime StartTime { get; set; } = DateTime.Today;

        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        [TypeConverter(typeof(DateTimeToConvert))]
        public DateTime EndTime { get; set; } = DateTime.Today;
        public int AppointmentTypeId { get; set; }
        public int ConsultantId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContractNumber { get; set; }
        public decimal CustomerTotalContractAmount { get; set; }
    }

}
