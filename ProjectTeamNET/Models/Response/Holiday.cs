using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class Holiday
    {
        public string User_no { get; set; }
        public string site_code { get; set; }
        public DateTime Date { get; set; }
        public bool Horiday { get; set; }
        public int YearConver { get; set; }
        public int MonthConver { get; set; }
    }
}
