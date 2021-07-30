using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourReportCSV : ManhourReport
    {
        public string HeaderItems { get; set; }
        public int Total { get; set; }
        public int TypeDelimiter { get; set; }
        public int SingleQuote { get; set; }
    }
}
