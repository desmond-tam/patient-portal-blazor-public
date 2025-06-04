using PatientPortalApp.Data;
using Microsoft.EntityFrameworkCore;
using BlazorBootstrap;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public class UserService : IUserService
    {
        private readonly PatientPortalContext _context;

        public UserService(PatientPortalContext context)
        {
            _context = context;
        }


        public async Task<UserModel> GetItem(int id)
        {
            var rec = await _context.Users.Where(x => x.UserId == id)
                    .AsNoTracking()
                    .FirstAsync();
            return new UserModel()
            {
                UserId = rec.UserId,
                FirstName = rec.FirstName,
                UserIdentifier = rec.UserIdentifier,
                IsActive = rec.IsActive,
                LastName = rec.LastName,
                MobileNumber = rec.MobileNumber,
                EmailAddress = rec.EmailAddress,
            };
        }



        private IQueryable<TblUser> _whereExpression(IQueryable<TblUser> q, UserSearchModel search)
        {
            q = q.Where(x => x.FirstName.ToLower().IndexOf(search.FirstName ?? "") >= 0 || string.IsNullOrEmpty(search.FirstName));
            q = q.Where(x => x.IsActive == search.IsActive || search.IsActive == null);
            q = q.Where(x => x.LastName.ToLower().IndexOf(search.LastName ?? "") >= 0 || string.IsNullOrEmpty(search.LastName));
            q = q.Where(x => x.EmailAddress.ToLower().IndexOf(search.EmailAddress ?? "") >= 0 || string.IsNullOrEmpty(search.EmailAddress));
            return q;
        }

        public async Task<IEnumerable<UserModel>> GetPage(UserSearchModel search, IEnumerable<SortingItem<UserModel>> sortings, int page, int pageSize)
        {

            IQueryable<TblUser> q = _context.Users.AsQueryable();
            q = _whereExpression(q, search);
            if (sortings != null && sortings.Count() > 0)
            {
                var sortitem = sortings.First();
                q = q.OrderByString(sortitem.SortString, sortitem.SortDirection == SortDirection.Descending);
            }

            return await q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .Select(x => new UserModel()
                    {
                        UserId = x.UserId,
                        FirstName = x.FirstName,
                        UserIdentifier = x.UserIdentifier,
                        IsActive = x.IsActive,
                        LastName = x.LastName,
                        MobileNumber = x.MobileNumber,
                        EmailAddress = x.EmailAddress,
                    })
                    .ToListAsync();
        }

        public async Task<int> GetTotalRecords(UserSearchModel search)
        {
            IQueryable<TblUser> q = _context.Users;
            q = _whereExpression(q, search);
            return await q.CountAsync();
        }

        public async Task<Guid> SaveUser(UserInputModel? model)
        {
            var rec = new TblUser();
            rec.FirstName = model?.FirstName ?? string.Empty;
            rec.LastName = model?.LastName ?? string.Empty;
            rec.MiddleName = model?.MiddleName ?? string.Empty;
            rec.MobileNumber = model?.MobileNumber ?? string.Empty;
            rec.EmailAddress = model?.EmailAddress ?? string.Empty;
            rec.UserIdentifier = Guid.NewGuid();
            rec.IsActive = true;
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            _context.Users.Add(rec);
            await _context.SaveChangesAsync();
            return rec.UserIdentifier;
        }

        public async Task Switch(int id)
        {
            var rec = await _context.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (rec == null)
            {
                return;
            }
            rec.IsActive = !rec.IsActive;
            _context.Update(rec);
            await _context.SaveChangesAsync();
            _context.Entry(rec).State = EntityState.Detached;
        }

        public async Task<IEnumerable<UserModel>> GetAllForDoctor()
        {
                    
            return await _context.Users
                    .Where(x => x.IsActive && !_context.Doctors.Any(y => y.UserId == x.UserId))
                    .AsNoTracking()
                    .Select(x => new UserModel
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserId = x.UserId

                    }).ToListAsync();
        }

        public async Task<IEnumerable<UserModel>> GetAllForPatient()
        {
            return await _context.Users
                    .Where(x => x.IsActive && !_context.Patients.Any(y => y.UserId == x.UserId))
                    .AsNoTracking()
                    .Select(x => new UserModel
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserId = x.UserId

                    }).ToListAsync();
        }
    }
}
