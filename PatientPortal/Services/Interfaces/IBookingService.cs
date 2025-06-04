using BlazorBootstrap;
using PatientPortalApp.Data;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingModel>> GetPage(BookingSearchModel search, IEnumerable<SortingItem<BookingModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(BookingSearchModel search);
       // Task<SlotModel> GetItem(int id);
        Task SaveBooking(BookingInputModel model);

        Task Switch(int id);
        Task<IEnumerable<BookingModel>> GetAll();

        Task<IEnumerable<LocationModel>> GetLocationsForBooking();
        Task<IEnumerable<DoctorModel>> GetDoctorsForBooking(int? locationId);
    }
}
