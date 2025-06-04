using BlazorBootstrap;
using PatientPortalApp.Data;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public interface ISlotService
    {
        Task<IEnumerable<SlotModel>> GetPage(SlotSearchModel search, IEnumerable<SortingItem<SlotModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(SlotSearchModel search);
       // Task<SlotModel> GetItem(int id);
        Task SaveSlot(SlotInputModel model);
        Task<IEnumerable<SlotModel>> GetAll();
    }
}
