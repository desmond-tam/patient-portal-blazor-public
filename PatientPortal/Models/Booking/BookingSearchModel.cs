using PatientPortalApp.Data;

namespace PatientPortalApp.Models
{
    public class BookingSearchModel
    {
        public string? DoctorFirstName { get; set; }
        public string? DoctorLastName { get; set; }
        public string? LocationName { get; set; }
        public DateTime BookingDate { get; set; }

    }
}