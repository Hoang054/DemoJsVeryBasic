using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourReportViewModel
    {
        public IList<SelectListItem> Users { get; set; }
        public IList<SelectListItem> Themes { get; set; }
        public List<GroupNames> GroupName { get; set; }
    }
}
