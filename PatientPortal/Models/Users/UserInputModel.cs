using PatientPortalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{
    public class UserInputModel 
    {
        public int? UserId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string? EmailAddress { get; set; }
        public string? MiddleName { get; set; }
        [RegularExpression(@"^\d{10}$",ErrorMessage = "Invalid Mobile number, it must be 10 digit long.")]
        public string? MobileNumber { get; set; }
       
    }
}