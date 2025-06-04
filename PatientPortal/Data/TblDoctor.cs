using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace PatientPortalApp.Data
{
    public class TblDoctor
    {
        [Key]
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string? ProviderNumber { get; set; } 
        public bool IsActive { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }

    }
}
