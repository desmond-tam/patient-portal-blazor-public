
using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{

    public class DoctorInputModel
    {
        public int? DoctorId { get; set; }
        [Required(ErrorMessage ="Please select user.")]
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Provider number must be provided.")]
        public string? ProviderNumber { get; set; }
    }
}
