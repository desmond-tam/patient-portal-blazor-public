using PatientPortalApp.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using BlazorBootstrap;
using PatientPortalApp.Models;

namespace PatientPortalApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetPage(UserSearchModel search, IEnumerable<SortingItem<UserModel>> sortings, int page, int pageSize);
        Task<int> GetTotalRecords(UserSearchModel search);

        Task<IEnumerable<UserModel>> GetAllForDoctor();
        Task<IEnumerable<UserModel>> GetAllForPatient();
        Task<UserModel> GetItem(int id);
        Task<Guid> SaveUser(UserInputModel? model);
        Task Switch(int id);
    }
}
