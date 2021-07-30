using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class MenuViewModel
    {
        public string userNo { get; set; }
        public double[] totalWorkHour { get; set; }
        public Dictionary<int,bool> holidays { get; set; }
        public bool check { get; set; }
        public ProcessingMonthViewModel processingMonth { get; set; }
    }

    
}
