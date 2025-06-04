using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalApp.Data
{
    public class TblSlot
    {
        [Key]
        public int SlotId { get; set; }
        public int DoctorId { get; set; }
        public int LocationId { get; set; }
        public string Times { get; set; } = string.Empty;

        public string Weekdays { get; set; } = string.Empty;
        public Boolean IsDeleted { get; set; } = false;
        public DateTimeOffset ModifiedDate { get; set; }
        public int ModifiedBy { get; set; } = 1;
      
    }
}
