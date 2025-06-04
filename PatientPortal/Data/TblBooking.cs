using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalApp.Data
{
    public class TblBooking
    {
        [Key]
        public int BookingId { get; set; }
        public Guid BookingIdentifier { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int LocationId { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = String.Empty;
        public DateTime BookingDate { get; set; }
        public Boolean IsDeleted { get; set; } = false;
        public DateTimeOffset ModifiedDate { get; set; } = DateTimeOffset.Now;
        public int ModifiedBy { get; set; } = 1;
        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.Now;
        public int CreatedBy { get; set; } = 1;

    }
}
