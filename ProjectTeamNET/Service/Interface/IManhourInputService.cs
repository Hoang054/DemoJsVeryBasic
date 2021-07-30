using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManhourInputService
    {   
        /// <summary>
        /// Get manhour data for table
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<ManhourInputModel> GetManhourData(InputParamModel pModel);
        /// <summary>
        /// Init data form db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<InitDataModel> Init(InputParamModel pModel);
        Task<List<Group>> GetGroups();
        Task<List<WorkContents>> GetWorkContents();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<ManhourInput>> GetManhours(InputParamModel pModel);
        /// <summary>
        /// Get Screen item input history
        /// </summary>
        /// <param name="user_no"></param>
        /// <param name="screen_url">String of screen url need get history</param>
        /// <returns></returns>
        Task<List<UserScreenItem>> GetsScreenItemHistory(string user_no,string screen_url);
        /// <summary>
        /// Get pined theme in last month to check to insert for now month
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<List<ManhourInput>> GetPinTheme(InputParamModel pModel);
        /// <summary>
        /// get search theme history
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        Task<SearchThemeParam> GetHistoryThemes(string user_no);
        /// <summary>
        /// Search Theme by param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no);
        /// <summary>
        /// Handle save data from client 
        /// </summary>
        /// <param name="saveDatas"></param>
        /// <returns></returns>
        bool Save(SaveData saveDatas, UserInfo user);
        /// <summary>
        /// Get list date of week 
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        List<string> GetListDayOfWeek(DateTime date);
        /// <summary>
        /// update manhour data form client 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        int UpdateManhours(List<Manhour> manhours, string user_no);
        /// <summary>
        /// change theme
        /// </summary>
        /// <param name="oldData">list data need change</param>
        /// <param name="newData">list data to change</param>
        /// <returns></returns>
        int ChangeManhour(List<Manhour> oldData, List<Manhour> newData,string user_no);
        /// <summary>
        /// delete data if it deleted in data form client
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        int DeleteManhours(List<Manhour> manhours, string user_no);
        /// <summary>
        /// create manhour form list manhour
        /// </summary>
        /// <param name="manhours"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        int Create(List<Manhour> manhours, UserInfo user);
        /// <summary>
        /// Save page cliked history
        /// </summary>
        /// <param name="pModel"></param>
        bool SavePageHistory(InputParamModel pModel);
        /// <summary>
        /// Get horliday in month of year by group code
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<int>> GetHoliday(HolidayParam param);
        /// <summary>
        /// get list work contents by class code
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        Task<List<WorkContents>> GetWorkContentsByClass(string className);
        /// <summary>
        /// Export manhour data to csv
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        Task<ExportModel> ExportCSV(string userNo,string dateStr);
        /// <summary>
        /// Get data exprot
        /// </summary>
        /// <param name="pmodel"></param>
        /// <returns></returns>
        Task<List<ManhourUpdateViewModel>> GetDataExport(string userNo, DateTime date);
        /// <summary>
        /// check theme existed in database
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<bool> CheckThemeExist(ManhourKeys keys);

    }
}
