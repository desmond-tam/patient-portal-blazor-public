
using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{

    public class LocationInputModel
    {
        public int? LocationId { get; set; }
        [Required]
        public string? Name { get; set; }

    }
}
