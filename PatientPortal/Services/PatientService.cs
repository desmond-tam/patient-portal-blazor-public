using PatientPortalApp.Data;
using PatientPortalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using PatientPortalApp.Models;
using BlazorBootstrap;
using System.Security.Cryptography.Xml;

namespace PatientPortalApp.Services
{
    public class PatientService :IPatientService
    {
        private readonly PatientPortalContext _context;

        public PatientService(PatientPortalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatientModel>> GetAll()
        {
            IList<PatientModel> list = await (from e in _context.Patients
                                             join f in _context.Users on e.UserId equals f.UserId
                                             where f.IsActive && e.IsActive
                                             select new PatientModel
                                             {
                                                 UserId = e.UserId,
                                                 User = new UserModel()
                                                 {
                                                     UserId = f.UserId,
                                                     EmailAddress = f.EmailAddress,
                                                     FirstName = f.FirstName,
                                                     LastName = f.LastName,
                                                 },
                                                 MedicareNumber = e.MedicareNumber,
                                             }).AsNoTracking()
                                             .ToListAsync();
              
            return list;
        }

        public async Task<PatientModel> GetItem(int id)
        {
            var rec = await (from e in _context.Patients
                             join f in _context.Users on e.UserId equals f.UserId
                             where e.PatientId == id
                             select new
                             {
                                 e,
                                 f
                             }).AsNoTracking()
                      .FirstAsync();


            return new PatientModel()
            {
                PatientId = rec.e.PatientId,
                UserId = rec.e.UserId,
                IsActive = rec.e.IsActive,
                MedicareNumber = rec.e.MedicareNumber,
                User = new UserModel() { 
                    UserId = rec.f.UserId , 
                    EmailAddress = rec.f.EmailAddress, 
                    FirstName = rec.f.FirstName,
                    LastName = rec.f.LastName,
                    MobileNumber = rec.f.MobileNumber
                }
            };
        }

        public async Task<IEnumerable<PatientModel>> GetPage(PatientSearchModel search, IEnumerable<SortingItem<PatientModel>> sortings, int page, int pageSize)
        {
            var q = from e in _context.Patients
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
                    .Select(x => new PatientModel()
                    {
                        PatientId = x.e.PatientId,
                        UserId = x.e.UserId,
                        MedicareNumber = x.e.MedicareNumber,
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

        public async Task<int> GetTotalRecords(PatientSearchModel search)
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

        public async Task<int> Save(PatientInputModel model)
        {

            var rec = new TblPatient();
            rec.UserId = model.UserId ?? -1;
            rec.MedicareNumber = model.MedicareNumber;
            rec.IsActive = true;
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            rec.CreationDate = DateTimeOffset.Now;
            rec.CreatedBy = 1;
            _context.Patients.Add(rec);
            await _context.SaveChangesAsync();
            return rec.PatientId;
        }

        public async Task Switch(int id)
        {
            var rec = await _context.Patients.Where(x => x.PatientId == id).FirstOrDefaultAsync();
            if (rec == null)
            {
                return;
            }
            rec.IsActive = !rec.IsActive;
            _context.Update(rec);
            await _context.SaveChangesAsync();
            _context.Entry(rec).State = EntityState.Detached;

        }

        public async Task<IEnumerable<PatientModel>> GetListForBooking()
        {
            var q = from e in _context.Patients
                    join u in _context.Users on e.UserId equals u.UserId
                    where e.IsActive == true
                    && u.IsActive == true
                    select new
                    {
                        e,u
                    };

            return await q.AsNoTracking()
                        .Select(x => new PatientModel
                        {
                            UserId = x.e.UserId,
                            User = new UserModel()
                            {
                                FirstName = x.u.FirstName,
                                LastName = x.u.LastName,
                            },
                            MedicareNumber = x.e.MedicareNumber,
                        }).ToListAsync();
        }


    }
}
