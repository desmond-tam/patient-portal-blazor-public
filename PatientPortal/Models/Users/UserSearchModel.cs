using Microsoft.AspNetCore.Mvc.Infrastructure;
using PatientPortalApp.Data;

namespace PatientPortalApp.Models
{
    public class UserSearchModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailAddress { get; set; }
        public bool? IsActive { get; set; }

    }
}