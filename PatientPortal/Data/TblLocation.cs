using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalApp.Data
{
    public class TblLocation
    {
        [Key]
        public int LocationId { get; set; }

        public Guid LocationIdentifier { get; set; }
        public string Name { get; set; } = string.Empty;
        public float? Lat { get; set; }
        public float? Long { get; set; }
        public string? Address { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }


    }
}
