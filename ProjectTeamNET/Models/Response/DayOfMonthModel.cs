using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class DayOfMonthModel
    {
        public string userNo { get; set; }
        public double totalWorkHour { get; set; }
        public bool  isHoliday { get; set; }
    }
}
