using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Data
{
    public class TblPatient
    {
        [Key]
        public int PatientId { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTimeOffset? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; } = 1;

        public DateTimeOffset? CreationDate { get; set; }
        public int CreatedBy { get; set; } = 1;
        public int UserId { get; set; }
        public string? MedicareNumber { get; set; }

    }
}
