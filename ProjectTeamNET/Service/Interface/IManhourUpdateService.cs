using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    public interface IManhourUpdateService 
    {
        Task<List<ManhourUpdateViewModel>> GetDataSearchByKeySearch(ManhourUpdateSearch keySearch);
        Task<string> GetFunctionClassByUserNo(string funtioncClass);
        Task<List<UserScreenItem>> GetHistorySearchByUserNo(string funtioncClass);
        Task<ManhourUpdate> GetGroupAndUser(string userId);
        Task<ExportModel> ExportCSV(string user, string group);
        Task<string> ImportCSV(IFormFile files);     
        Task<List<int>> GetHolidayByKeySearch(ManhourUpdateSearch keySearch);
        Task<ManHourUpdateSearchModel> Search(ManhourUpdateSearch keySearch, string userId, string siteCode);
        Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no);
        bool Save(ManhourUpdateSave saveData);
        ManhourUpdateUserSelectList GetUserInGroup(string groupId);

    }
}
