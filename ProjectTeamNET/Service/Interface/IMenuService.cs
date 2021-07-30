using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
   public interface IMenuService
    {
        //Task<List<DayOfMonthModel>> GetDayOfMonths();
        Task<ProcessingMonthViewModel> GetProcessMonth();
        Task<double[]> GetWorkHourOfMonth(string userNo, int year, int month);
        Task <Dictionary<int, bool>> GetHoliday(string userNo,int year, int month);

        Task<MenuViewModel> SendDataToController(string userNo);
    }
}
