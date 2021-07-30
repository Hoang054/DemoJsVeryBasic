using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class SelectThemeModel
    {
        public List<Theme> Themes { get; set; }
        public SearchThemeParam  HistoryInput { get; set; }
    }
}
