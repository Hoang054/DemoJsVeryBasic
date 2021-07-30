using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class ManHourReportSearch
    {
        public string Save { get; set; }
        public string toDate { get; set; }
        public string fromDate { get; set; }
        public string isTotal { get; set; }
        public string typeDelimiter { get; set; }
        public string isSingleQuote { get; set; }
        public string numberSelectedHeader { get; set; }
        public string selectedHeaderItems { get; set; }
        public string numberTheme { get; set; }
        public string themeNos { get; set; }
        public string workContentCodes { get; set; }
        public string workContentDetails { get; set; }
        public string Groups{ get; set; }
        public string numberGroup{ get; set; }
        public string Users{ get; set; }
        public string numberUser{ get; set; }
    }
}
