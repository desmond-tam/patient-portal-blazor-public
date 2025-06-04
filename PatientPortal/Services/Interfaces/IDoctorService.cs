using PatientPortalApp.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PatientPortalApp.Models;
using BlazorBootstrap;

namespace PatientPortalApp.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorModel>> GetPage(DoctorSearchModel search, IEnumerable<SortingItem<DoctorModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(DoctorSearchModel search);
        Task<DoctorModel> GetItem(int id);
        Task<int> SaveDoctor(DoctorInputModel model);
        Task Switch(int id);
        Task<IEnumerable<DoctorModel>> GetAll();
    }
}
