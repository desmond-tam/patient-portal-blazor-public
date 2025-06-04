using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{
    public class BookingInputModel
    {
        [Required(ErrorMessage = "Location is required")]
        public int? LocationId { get; set; }
        [Required(ErrorMessage = "Doctor is required")]
        public int? DoctorId { get; set; }
        [Required(ErrorMessage = "Patient is required")]
        public int? PatientId { get; set; }
        [Required(ErrorMessage = "Booking Date is required")]        
        
        public DateTime? BookingDate { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public string StartTime { get; set; }  = string.Empty;
        [Required(ErrorMessage = "End time is required")]
        public string EndTime { get; set; } = string.Empty;



    }
}
