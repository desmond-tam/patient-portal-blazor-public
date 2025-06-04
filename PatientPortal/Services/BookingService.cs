using PatientPortalApp.Data;
using PatientPortalApp.Models;
using PatientPortalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using BlazorBootstrap;
using System.Text.Json;
using System.Drawing.Printing;

namespace PatientPortalApp.Services
{
    public class BookingService :IBookingService
    {
        private readonly PatientPortalContext _context;

        public BookingService(PatientPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingModel>> GetAll()
        {
            var q = from e in _context.Bookings
                    join p in _context.Patients on e.PatientId equals p.PatientId
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join l in _context.Locations on e.LocationId equals l.LocationId
                    join du in _context.Users on d.UserId equals du.UserId
                    join pu in _context.Users on p.UserId equals pu.UserId
                    select new
                    {
                        e,
                        p,
                        d,
                        l,
                        du,
                        pu
                    };

            return await q.AsNoTracking()
                    .Select(x => new BookingModel()
                    {
                        DoctorId = x.e.DoctorId,
                        LocationId = x.e.LocationId,
                        PatientId = x.e.PatientId,
                        BookingDate = x.e.BookingDate,
                        StartTime = x.e.StartTime,
                        EndTime = x.e.EndTime,
                        Doctor = new DoctorModel()
                        {
                            DoctorId = x.d.DoctorId,
                            User = new UserModel()
                            {
                                EmailAddress = x.du.EmailAddress,
                                FirstName = x.du.FirstName,
                                LastName = x.du.LastName
                            },
                        },
                        Patient = new PatientModel()
                        {
                            PatientId = x.p.PatientId,
                            User = new UserModel()
                            {
                                EmailAddress = x.pu.EmailAddress,
                                FirstName = x.pu.FirstName,
                                LastName = x.pu.LastName,

                            },
                            MedicareNumber = x.p.MedicareNumber
                        },
                        Location = new LocationModel()
                        {
                            LocationId = x.l.LocationId,
                            Address = x.l.Address,
                            Name = x.l.Name,
                        }
                    }).ToListAsync();

        }


        public async Task<IEnumerable<BookingModel>> GetPage(BookingSearchModel search, IEnumerable<SortingItem<BookingModel>> sortings, int page, int pageSize)
        {
            var q = from e in _context.Bookings
                    join p in _context.Patients on e.PatientId equals p.PatientId
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join l in _context.Locations on e.LocationId equals l.LocationId
                    join du in _context.Users on d.UserId equals du.UserId
                    join pu in _context.Users on p.UserId equals pu.UserId
                    where
                        (du.FirstName.ToLower().IndexOf(search.DoctorFirstName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorFirstName)) &&
                        (du.LastName.ToLower().IndexOf(search.DoctorLastName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorLastName)) &&
                        (l.Name.ToLower().IndexOf(search.LocationName ?? "") >=0  || search.LocationName == null)
                    select new
                    {
                        e,p,d,l,du,pu
                    };

            if (sortings != null && sortings.Count() > 0)
            {
                var sortitem = sortings.First();
                q = q.OrderByString(sortitem.SortString, sortitem.SortDirection == SortDirection.Descending);
            }

            return await q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .Select(x => new BookingModel()
                    {
                        DoctorId = x.e.DoctorId,
                        LocationId = x.e.LocationId,
                        PatientId = x.e.PatientId,
                        BookingDate = x.e.BookingDate,
                        StartTime = x.e.StartTime,
                        EndTime = x.e.EndTime,
                        Doctor = new DoctorModel()
                        {
                            DoctorId = x.d.DoctorId,
                            User = new UserModel()
                            {
                                EmailAddress = x.du.EmailAddress,
                                FirstName = x.du.FirstName,
                                LastName = x.du.LastName
                            },
                        },
                        Patient = new PatientModel()
                        {
                            PatientId = x.p.PatientId,
                            User = new UserModel()
                            {
                                EmailAddress = x.pu.EmailAddress,
                                FirstName = x.pu.FirstName,
                                LastName = x.pu.LastName,

                            },
                            MedicareNumber = x.p.MedicareNumber
                        },
                        Location = new LocationModel()
                        {
                            LocationId = x.l.LocationId,
                            Address = x.l.Address,
                            Name = x.l.Name,
                        }
                    }).ToListAsync();
        }

        public async Task<int> GetTotalRecords(BookingSearchModel search)
        {
            var q = from e in _context.Bookings
                    join p in _context.Patients on e.PatientId equals p.PatientId
                    join d in _context.Doctors on e.DoctorId equals d.DoctorId
                    join l in _context.Locations on e.LocationId equals l.LocationId
                    join du in _context.Users on d.UserId equals du.UserId
                    join pu in _context.Users on p.UserId equals pu.UserId
                    where
                        (du.FirstName.ToLower().IndexOf(search.DoctorFirstName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorFirstName)) &&
                        (du.LastName.ToLower().IndexOf(search.DoctorLastName ?? "") >= 0 || string.IsNullOrEmpty(search.DoctorLastName)) &&
                        (l.Name.ToLower().IndexOf(search.LocationName ?? "") >= 0 || search.LocationName == null)
                    select new
                    {
                        e,
                        p,
                        d,
                        l,
                        du,
                        pu
                    };

            return await q.CountAsync();
        }

        public async Task SaveBooking(BookingInputModel model)
        {

            var rec = new TblBooking();
            rec.DoctorId = model.DoctorId ?? -1;
            rec.LocationId = model.LocationId ?? -1;
            rec.PatientId = model.PatientId ?? -1;
            rec.BookingDate = model.BookingDate ?? DateTime.Now;
            rec.StartTime = model.StartTime;
            rec.EndTime = model.EndTime;
            rec.CreationDate = DateTimeOffset.Now;
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            rec.CreatedBy = 1;
           
            _context.Bookings.Add(rec);

            await _context.SaveChangesAsync();
        }

        public async Task Switch(int id)
        {
            var rec = await _context.Bookings.Where(x => x.BookingId == id).FirstOrDefaultAsync();
            if (rec == null)
            {
                return;
            }
            rec.IsDeleted = !rec.IsDeleted;
            _context.Update(rec);
            await _context.SaveChangesAsync();
            _context.Entry(rec).State = EntityState.Detached;
        }

        public async Task<IEnumerable<LocationModel>> GetLocationsForBooking()
        {
            var q = from l in _context.Locations
                    where l.IsEnabled == true && _context.Slots.Any(x => x.LocationId == l.LocationId && x.IsDeleted == false)
                    select new
                    {
                        l
                    };

            return await q.AsNoTracking()
                    .Select(x => new LocationModel()
                    {
                        Address = x.l.Address,
                        Name = x.l.Name
                    }).ToListAsync();
                    
        }

        public async Task<IEnumerable<DoctorModel>> GetDoctorsForBooking(int? locationId)
        {
            var q = from d in _context.Doctors
                    join u in _context.Users on d.UserId equals u.UserId
                    where d.IsActive == true 
                        && _context.Slots.Any(x => x.LocationId == locationId && x.IsDeleted == false && x.DoctorId == d.DoctorId)
                    select new
                    {
                        d,u
                    };

            return await q.AsNoTracking()
                    .Select(x => new DoctorModel()
                    {
                        DoctorId = x.d.DoctorId,
                        User = new UserModel()
                        {
                            FirstName = x.u.FirstName,
                            LastName = x.u.LastName
                        }
                    }).ToListAsync();
        }

    }
}
