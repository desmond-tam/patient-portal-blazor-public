using PatientPortalApp.Data;

namespace PatientPortalApp.Models
{
    public class BookingModel : TblBooking
    {
        public DoctorModel? Doctor { get; set; }
        public LocationModel? Location { get; set; }
        public PatientModel? Patient { get; set; }
        public string DoctorName => Doctor?.FirstName + " " + Doctor?.LastName;
        public string PatientName => Patient?.FirstName + " " + Patient?.LastName;
        public string MedicareNumber => Patient?.MedicareNumber ?? "";
        
    }
}
