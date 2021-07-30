using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
    public interface IManhourReportService
    {
        ManhourReportViewModel Init(string userName);
        Task<int> Delete(string id);
        Task<List<UserName>> GetsUserName(string groupCode);
        Task<string> GetsGroupName(string groupCode);
        Task<List<ThemeName>> GetsThemeName();
        Task<List<GroupNames>> GetsGroupName();
        Task<List<WorkContent>> GetsWorkContent(string themeNo);
        Task<List<UserScreenItem>> GetsManHourReportSaved(string userScreenName);
        IDictionary<string, string> ValidateReport(ManHourReportSearch data);
        IDictionary<string, string> ValidateSaveName(ManHourReportSearch data);
        Task<List<ManhourReport>> SetManhourReport(ManHourReportSearch data);
        List<double> ManhourReportMonthly(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails);
        List<double> ManhourReportDaily(string group, string user, string theme, DateTime toDate, DateTime fromDate, string workContentCodes, string workContentDetails);
        Task<ExportModel> GetDataReportCSV(ManHourReportSearch data);
        string SaveScreen(ManHourReportSearch data,string userName);
        List<SelectListItem> GetsScreen(string user);
    }
}
