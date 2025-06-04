using Azure.Security.KeyVault.Certificates;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientPortalApp.Data
{
    public class TblUser
    {
        [Key]
        public int UserId { get; set; }
        public Guid UserIdentifier { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public bool? EmailAddressIsValid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? MobileNumber { get; set; } = string.Empty;
        public bool? MobileNumberIsValid { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }

    }
}
