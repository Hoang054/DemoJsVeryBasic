using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{

    public class SearchThemeParam
    {
        public string ThemeNo { get; set; }
        public string ThemeName { get; set; }
        public string SalesObjectCode { get; set; }
        public string SoldFlg { get; set; }
        public string AccountingGroupCode { get; set; }
    }
}
