using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{
    public class SlotInputModel
    {
        public int? SlotId { get; set; }
        [Required(ErrorMessage = "Select doctor for consultance.")]
        public int? DoctorId { get; set; }
        [Required(ErrorMessage = "Select Location for consultance.")]
        public int? LocationId { get; set; }

        [MustHaveOneElement<TimeItemInputModel>(ErrorMessage = "Please select time from the list")]
        public List<TimeItemInputModel> TimeRange { get; set; } = new List<TimeItemInputModel>();
        [MustHaveOneElement<WeekItemInputModel>(ErrorMessage = "Please select weekdays from the list")]
        public List<WeekItemInputModel> Repeat { get; set; } = new List<WeekItemInputModel>();

    }



}
