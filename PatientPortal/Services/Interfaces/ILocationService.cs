using PatientPortalApp.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using BlazorBootstrap;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationModel>> GetAll();

        Task<IEnumerable<LocationModel>> GetPage(LocationSearchModel search, IEnumerable<SortingItem<LocationModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(LocationSearchModel search);
        Task<LocationModel> GetItem(int id);
        Task<Guid> SaveLocation(LocationInputModel? model);
        Task SwitchLocation(int id);
    }
}
