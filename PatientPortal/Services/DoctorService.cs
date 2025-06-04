using PatientPortalApp.Data;
using PatientPortalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using PatientPortalApp.Models;
using BlazorBootstrap;

namespace PatientPortalApp.Services
{
    public class DoctorService :IDoctorService
    {
        private readonly PatientPortalContext _context;

        public DoctorService(PatientPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorModel>> GetAll()
        {
            IList<DoctorModel> list = await (from e in _context.Doctors
                                             join f in _context.Users on e.UserId equals f.UserId
                                             where f.IsActive && e.IsActive
                                             select new DoctorModel
                                             {
                                                 DoctorId = e.DoctorId,
                                                 User = new UserModel()
                                                 {
                                                     UserId = f.UserId,
                                                     EmailAddress = f.EmailAddress,
                                                     FirstName = f.FirstName,
                                                     LastName = f.LastName,
                                                 },
                                                 ProviderNumber = e.ProviderNumber
                                             }).AsNoTracking()
                                             .ToListAsync();
              
            return list;
        }

        public async Task<DoctorModel> GetItem(int id)
        {
            var rec = await (from e in _context.Doctors
                             join f in _context.Users on e.UserId equals f.UserId
                             where e.DoctorId == id
                             select new
                             {
                                 e,
                                 f
                             }).AsNoTracking()
                      .FirstAsync();


            return new DoctorModel()
            {
                DoctorId = rec.e.DoctorId,
                UserId = rec.e.UserId,
                IsActive = rec.e.IsActive,
                ProviderNumber = rec.e.ProviderNumber,
                User = new UserModel() { 
                    UserId = rec.f.UserId , 
                    EmailAddress = rec.f.EmailAddress, 
                    FirstName = rec.f.FirstName,
                    LastName = rec.f.LastName,
                    MobileNumber = rec.f.MobileNumber
                }
            };
        }

        public async Task<IEnumerable<DoctorModel>> GetPage(DoctorSearchModel search, IEnumerable<SortingItem<DoctorModel>> sortings, int page, int pageSize)
        {
            var q = from e in _context.Doctors
                    join u in _context.Users on e.UserId equals u.UserId
                    where
                        (u.FirstName.ToLower().IndexOf(search.FirstName ?? "") >= 0 || string.IsNullOrEmpty(search.FirstName)) &&
                        (u.LastName.ToLower().IndexOf(search.LastName ?? "") >= 0 || string.IsNullOrEmpty(search.LastName)) &&
                        (e.IsActive == search.IsActive || search.IsActive == null)
                    select new
                    {
                        e,
                        u
                    };

            if (sortings != null && sortings.Count() > 0)
            {
                var sortitem = sortings.First();
                q = q.OrderByString(sortitem.SortString, sortitem.SortDirection == SortDirection.Descending);
            }

            return await q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .Select(x => new DoctorModel()
                    {
                        DoctorId = x.e.DoctorId,
                        UserId = x.e.UserId,
                        ProviderNumber = x.e.ProviderNumber,
                        IsActive = x.e.IsActive,
                        User = new UserModel()
                        {
                            UserId = x.u.UserId,
                            EmailAddress = x.u.EmailAddress,
                            FirstName = x.u.FirstName,
                            LastName = x.u.LastName,
                            MobileNumber = x.u.MobileNumber
                        }
                    })
                    .ToListAsync();
        }

        public async Task<int> GetTotalRecords(DoctorSearchModel search)
        {
            var q = from e in _context.Doctors
                    join u in _context.Users on e.UserId equals u.UserId
                    where
                        (u.FirstName.ToLower().IndexOf(search.FirstName ?? "") >= 0 || string.IsNullOrEmpty(search.FirstName)) &&
                        (u.LastName.ToLower().IndexOf(search.LastName ?? "") >= 0 || string.IsNullOrEmpty(search.LastName)) &&
                        (e.IsActive == search.IsActive || search.IsActive == null)
                    select new
                    {
                        e,
                        u
                    };

            return await q.CountAsync();
        }

        public async Task<int> SaveDoctor(DoctorInputModel model)
        {

            var rec = new TblDoctor();
            rec.UserId = model.UserId ?? -1;
            rec.ProviderNumber = model.ProviderNumber;
            rec.IsActive = true;
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            _context.Doctors.Add(rec);
            await _context.SaveChangesAsync();
            return rec.DoctorId;
        }

        public async Task Switch(int id)
        {
            var rec = await _context.Doctors.Where(x => x.DoctorId == id).FirstOrDefaultAsync();
            if (rec == null)
            {
                return;
            }
            rec.IsActive = !rec.IsActive;
            _context.Update(rec);
            await _context.SaveChangesAsync();
            _context.Entry(rec).State = EntityState.Detached;
        }


    }
}
