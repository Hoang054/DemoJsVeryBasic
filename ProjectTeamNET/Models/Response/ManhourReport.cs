using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourReport
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string ThemeCode { get; set; }
        public string ThemeName { get; set; }
        public string WorkContentCode { get; set; }
        public string WorkContentCodeName { get; set; }
        public string WorkContentDetail { get; set; }
        public double Overalltotal { get; set; }
        public string Column { get; set; }
        public double Total { get; set; }
        public List<double> Monthly { get; set; }
        public List<double> Daily { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
