using PatientPortalApp.Data;
using PatientPortalApp.Models;
using PatientPortalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using BlazorBootstrap;
using System.Text.Json;

namespace PatientPortalApp.Services
{
    public class SlotService :ISlotService
    {
        private readonly PatientPortalContext _context;

        public SlotService(PatientPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SlotModel>> GetAll()
        {
            IList<SlotModel> list = await (from e in _context.Slots
                                           join d in _context.Doctors on e.DoctorId equals d.DoctorId
                                           join u in _context.Users on d.UserId equals u.UserId
                                           join l in _context.Locations on e.LocationId equals l.LocationId
                                           select new SlotModel
                                           {
                                               LocationId = e.LocationId,
                                               DoctorId = e.DoctorId,
                                               Doctor = new DoctorModel()
                                               {
                                                   UserId = u.UserId,
                                                   User = new UserModel
                                                   {
                                                       FirstName = u.FirstName,
                                                       LastName = u.LastName
                                                   }
                                               },
                                               Location = new LocationModel()
                                               {
                                                   Address = l.Address,
                                                   Name = l.Name
                                               },
                                               Times = e.Times,
                                               Weekdays = e.Weekdays
                                           }).AsNoTracking()
                                           .ToListAsync();
                                               

            return list;
        }


        public async Task<IEnumerable<SlotModel>> GetPage(SlotSearchModel search, IEnumerable<SortingItem<SlotModel>> sortings, int page, int pageSize)
        {
            var q = from e in _context.Slots
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join u in _context.Users on d.UserId equals u.UserId
                    join f in _context.Locations on e.LocationId equals f.LocationId
                    where
                        (u.FirstName.ToLower().IndexOf(search.DoctorFirstName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorFirstName)) &&
                        (u.LastName.ToLower().IndexOf(search.DoctorLastName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorLastName)) &&
                        (f.Name.ToLower().IndexOf(search.LocationName ?? "") >=0  || search.LocationName == null)
                    select new
                    {
                        e,u,f
                    };

            if (sortings != null && sortings.Count() > 0)
            {
                var sortitem = sortings.First();
                q = q.OrderByString(sortitem.SortString, sortitem.SortDirection == SortDirection.Descending);
            }

            return await q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .Select(x => new SlotModel()
                    {
                        DoctorId = x.e.DoctorId,
                        LocationId = x.e.LocationId,
                        SlotId = x.e.SlotId,
                        Times = x.e.Times,
                        Weekdays = x.e.Weekdays,
                        Doctor = new DoctorModel()
                        {
                            User = new UserModel()
                            {
                                FirstName = x.u.FirstName,
                                LastName = x.u.LastName,
                            }
                        },
                        Location = new LocationModel()
                        {
                            Name = x.f.Name
                        }
                    })
                    .ToListAsync();
        }

        public async Task<int> GetTotalRecords(SlotSearchModel search)
        {
            var q = from e in _context.Slots
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join u in _context.Users on d.UserId equals u.UserId
                    join l in _context.Locations on e.LocationId equals l.LocationId
                    where
                       (u.FirstName.ToLower().IndexOf(search.DoctorFirstName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorFirstName)) &&
                       (u.LastName.ToLower().IndexOf(search.DoctorLastName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorLastName)) &&
                       (l.Name.ToLower().IndexOf(search.LocationName ?? "") >= 0 || search.LocationName == null)
                    select new
                    {
                        e,
                        u,
                        l
                    };

            return await q.CountAsync();
        }

        public async Task SaveSlot(SlotInputModel model)
        {

            var rec = new TblSlot();
            rec.DoctorId = model.DoctorId ?? -1;
            rec.LocationId = model.LocationId ?? -1;
            var times = model.TimeRange.Where(x => x.Selected).Select(x => new TimeItemModel
            {
                EndTime = x.EndTime,
                StartTime = x.StartTime
            }).ToArray();
            rec.Times = JsonSerializer.Serialize(times);
            var repeats = model.Repeat.Where(x => x.Selected).Select(x => new WeekItemModel
            {
                WeekName = x.WeekName
            }).ToArray();
            rec.Weekdays = JsonSerializer.Serialize(repeats);
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            _context.Slots.Add(rec);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SlotModel>> GetDoctorSlots(int locationId,int doctorId)
        {
            var q = from e in _context.Slots
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join u in _context.Users on d.UserId equals u.UserId
                    join l in _context.Locations on e.LocationId equals l.LocationId
                    select new SlotModel()
                    {
                        DoctorId = e.DoctorId,
                        LocationId = e.LocationId,
                        Doctor = new DoctorModel()
                        {
                            UserId = u.UserId,
                            User = new UserModel()
                            {
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                MiddleName = u.MiddleName
                            }
                        },
                        Weekdays = e.Weekdays,
                        Times = e.Times,
                    };

            return await q.ToListAsync();
        }

    }
}
