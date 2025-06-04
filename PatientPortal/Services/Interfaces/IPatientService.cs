using BlazorBootstrap;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientModel>> GetPage(PatientSearchModel search, IEnumerable<SortingItem<PatientModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(PatientSearchModel search);
        Task<PatientModel> GetItem(int id);
        Task<int> Save(PatientInputModel model);
        Task Switch(int id);
        Task<IEnumerable<PatientModel>> GetAll();

        Task<IEnumerable<PatientModel>> GetListForBooking();
    }
}
