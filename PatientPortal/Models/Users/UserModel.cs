using PatientPortalApp.Data;

namespace PatientPortalApp.Models
{
    public class UserModel : TblUser
    {
        public string FullName => $"{FirstName}, {LastName}";

    }
}