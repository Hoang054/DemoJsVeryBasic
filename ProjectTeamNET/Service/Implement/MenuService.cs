using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjectTeamNET.Service.Implement
{
    public class MenuService : IMenuService
    {
        private readonly IBaseRepository<Holiday> menuRepository;
        private readonly ProjectDbContext context;

        public MenuService(IBaseRepository<Holiday> menuRepository ,ProjectDbContext context)
        {
            this.menuRepository = menuRepository;
            this.context = context;
        }

        //Get Holiday of userNo
        public async Task<Dictionary<int, bool>> GetHoliday(string userNo, int year, int month)
        {
          
            Dictionary<int, bool> a = new Dictionary<int, bool>();
            var query = QueryLoader.GetQuery("ManCheckHour", "GetHoliday");
            var param = new
            {
                user_no = userNo,
                currentYear = year,
                currentMonth = month

            };
            List<Holiday> result = await menuRepository.Search<Holiday>(query, param);
            foreach(var i in result)
            {
                a.Add(i.Date.Day, i.Horiday);
            }
           
            return a;
        }

        //Get ProccessingMonth 
        public async  Task<ProcessingMonthViewModel> GetProcessMonth()
        {
            var temp = await context.ProcessingMonths.FirstOrDefaultAsync();
            var conver = temp.Processing_month.ToString();
            var Str1 = conver.Substring(0, 4);
            var Str2 = conver.Substring(4);
            var year = Int32.Parse(Str1);
            var month = Int32.Parse(Str2);
            var result = new ProcessingMonthViewModel
            {
                Month = month,
                Year = year
            };
            return result;
        }
        // Get WorkHour of UserNo
        public async Task<double[]> GetWorkHourOfMonth(string userNo, int year, int month)
        {                 
            var query = QueryLoader.GetQuery("ManCheckHour", "GetWorkHourOfMonth");
            double[] total = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var param = new
            {
                user_no = userNo,
                year = year,
                month = month

            };
            List<Manhour> result = await menuRepository.Search<Manhour>(query, param);
            foreach (var i in result)
            {
                total[0] += i.Day1;
                total[1] += i.Day2;
                total[2] += i.Day3;
                total[3] += i.Day4;
                total[4] += i.Day5;
                total[5] += i.Day6;
                total[6] += i.Day7;
                total[7] += i.Day8;
                total[8] += i.Day9;
                total[9] += i.Day10;
                total[10] += i.Day11;
                total[11] += i.Day12;
                total[12] += i.Day13;
                total[13] += i.Day14;
                total[14] += i.Day15;
                total[15] += i.Day16;
                total[16] += i.Day17;
                total[17] += i.Day18;
                total[18] += i.Day19;
                total[19] += i.Day20;
                total[20] += i.Day21;
                total[21] += i.Day22;
                total[22] += i.Day23;
                total[23] += i.Day24;
                total[24] += i.Day25;
                total[25] += i.Day26;
                total[26] += i.Day27;
                total[27] += i.Day28;
                total[28] += i.Day29;
                total[29] += i.Day30;
                total[30] += i.Day31;
               
            }
            return total.ToArray();
        }

        //Send all data to Controller
        public async Task<MenuViewModel> SendDataToController(string userNo)
        {                
            var checkWorkHours = true;
            var today = DateTime.Now;
            var prsmonth =await GetProcessMonth();
            var year =  prsmonth.Year;
            var month = prsmonth.Month;
            var holiday = await GetHoliday(userNo, year, month);
            var workHour = await GetWorkHourOfMonth(userNo, year, month);
            for (var i = 0; i <= today.Day; i++)
            {
                if (workHour[i] < 8)
                {
                    checkWorkHours = false;
                }
            }
            var result = new MenuViewModel
            {
                userNo = userNo,
                totalWorkHour = workHour,
                holidays = holiday,
                processingMonth = prsmonth,
                check = checkWorkHours

            };
            return result;
        }
        
    }
}
