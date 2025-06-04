using System.ComponentModel.DataAnnotations;
namespace PatientPortalApp.Models
{
    public class PatientInputModel
    {
        public int? PatientId { get; set; }
        [Required(ErrorMessage = "Please select user.")]
        public int? UserId { get; set; }

        public string? MedicareNumber { get; set; }
    }
}
