using PatientPortalApp.Data;
using PatientPortalApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Microsoft.CodeAnalysis.Operations;
using BlazorBootstrap;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;
using NuGet.Versioning;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public class LocationService :ILocationService
    {
        private readonly PatientPortalContext _context;

        public LocationService(PatientPortalContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<LocationModel>> GetAll()
        {
            IList<LocationModel> list = await _context.Locations
                            .AsNoTracking()
                            .Select(x => new LocationModel
                            {
                                Name = x.Name,
                                LocationId = x.LocationId,
                                LocationIdentifier = x.LocationIdentifier,
                                IsEnabled = x.IsEnabled
                            })
                            .ToListAsync();
            return list;
        }

        public async Task<LocationModel> GetItem(int id)
        {
            var rec = await _context.Locations.Where(x => x.LocationId == id)
                    .AsNoTracking()
                    .FirstAsync();
            return new LocationModel()
            {
                LocationId = rec.LocationId,
                Name = rec.Name,
                LocationIdentifier = rec.LocationIdentifier,
                IsEnabled = rec.IsEnabled
            };
        }


        private IQueryable<TblLocation> _whereExpression(IQueryable<TblLocation> q,LocationSearchModel search)
        {
            q = q.Where(x => x.Name.ToLower().IndexOf(search.Name ?? "") >= 0 || string.IsNullOrEmpty(search.Name));
            q = q.Where(x => x.IsEnabled == search.IsEnabled || search.IsEnabled == null);
            //q = q.WhereString<TblLocation,LocationSearchModel>(search,"Name");
            return q;
        }

        public async Task<IEnumerable<LocationModel>> GetPage(LocationSearchModel search,IEnumerable<SortingItem<LocationModel>> sortings, int page, int pageSize)
        {

            IQueryable<TblLocation> q = _context.Locations.AsQueryable();
            q = _whereExpression(q, search);
            if (sortings != null && sortings.Count() > 0)
            {
                var sortitem = sortings.First();
                q = q.OrderByString(sortitem.SortString,sortitem.SortDirection == SortDirection.Descending);
            }

            return await q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .Select(x => new LocationModel()
                    {
                        LocationId = x.LocationId,
                        LocationIdentifier = x.LocationIdentifier,
                        IsEnabled = x.IsEnabled,
                        Name = x.Name
                    })
                    .ToListAsync();
        }

        public async Task<int> GetTotalRecords(LocationSearchModel search)
        {
            IQueryable<TblLocation> q = _context.Locations;
            q = _whereExpression(q, search);
            return await q.CountAsync();
        }

        public async Task<Guid> SaveLocation(LocationInputModel? model)
        {
            
            var rec = new TblLocation();
            rec.Name = model?.Name ?? string.Empty;
            rec.LocationIdentifier = Guid.NewGuid();
            rec.IsEnabled = true;
            rec.ModifiedDate = DateTimeOffset.Now;
            rec.ModifiedBy = 1;
            _context.Locations.Add(rec);
            await _context.SaveChangesAsync();
            return rec.LocationIdentifier;
        }

        public async Task SwitchLocation(int id)
        {
            var rec = await _context.Locations.Where(x => x.LocationId == id).FirstOrDefaultAsync();
            if (rec == null)
            {
                return;
            }
            rec.IsEnabled = !rec.IsEnabled;
            _context.Update(rec);
            await _context.SaveChangesAsync();
            _context.Entry(rec).State = EntityState.Detached;
        }
    }
}
