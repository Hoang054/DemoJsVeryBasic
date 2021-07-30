using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManHourUpdateSearchModel
    {
        public List<ManhourUpdateViewModel> models { get; set; }
        public List<int> holiday { get; set; }
    }
}
