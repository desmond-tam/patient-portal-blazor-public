using PatientPortalApp.Data;

namespace PatientPortalApp.Models
{
    public class PatientModel : TblPatient
    {
        public UserModel? User { get; set; }
        public string FirstName => User?.FirstName ?? "";
        public string LastName => User?.LastName ?? "";

        public string FullName => $"{FirstName}, {LastName}";
        public string EmailAddress => User?.EmailAddress ?? "";

        public string MobileNumber => User?.MobileNumber ?? "";

    }
}
