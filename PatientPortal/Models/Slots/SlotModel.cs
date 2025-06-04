using NuGet.Versioning;
using PatientPortalApp.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;

namespace PatientPortalApp.Models
{
    public class SlotModel : TblSlot
    {
        public DoctorModel? Doctor { get; set; }
        public LocationModel? Location { get; set; }

        public string DoctorName => $"{Doctor?.FirstName}, {Doctor?.LastName}";
        public string LocationName => $"{Location?.Name}";

        public TimeItemModel[] TimeRange => JsonSerializer.Deserialize<TimeItemModel[]>(Times) ?? new TimeItemModel[0];
        public string TimeRangeLine => TimeRange.Select(x => $"{x.StartTime}-{x.EndTime}").Aggregate((a, b) => a + "," + b).ToString();
        public WeekItemModel[] Repeat => JsonSerializer.Deserialize<WeekItemModel[]>(Weekdays) ?? new WeekItemModel[0];
        public string RepeatLine => Repeat.Select(x => x.WeekName).Aggregate((a, b) => a + "," + b).ToString();

        public AvailableTimeSlot[] GetAvailableSlot(DateTime start, DateTime end)
        {
            DateTime current = start;
            List<AvailableTimeSlot> slots = new List<AvailableTimeSlot>();
            while (current <= end)
            {
                WeekItemModel? weekday = Repeat.ToList().Where(x => x.WeekName == current.DayOfWeek.ToString()).FirstOrDefault();
                if (weekday != null)
                {
                    foreach (var time in TimeRange)
                    {
                        AvailableTimeSlot slot = new AvailableTimeSlot()
                        {
                            StartTime = time.StartTime,
                            EndTime = time.EndTime,
                            SlotDate = current
                        };
                        slots.Add(slot);
                    }
                }
                current = current.AddDays(1);
            }
            return slots.ToArray();
        }
    }

    public class WeekItemModel
    {
        public string WeekName { get; set; } = string.Empty;
    }

    public class TimeItemModel
    {
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;

    }

    public interface ISelectable
    {
        bool Selected { get; set; }
    }

    public class TimeItemInputModel : TimeItemModel, ISelectable
    {
        public bool Selected { get; set; } = false;
        public string Line => $"{StartTime} - {EndTime}";
    }

    public class WeekItemInputModel : WeekItemModel, ISelectable
    {
        public bool Selected { get; set; } = false;
    }

    public class AvailableTimeSlot
    {
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public DateTime SlotDate { get; set; }

    }



}
