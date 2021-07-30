using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class InitDataModel
    {
        public List<Group> Groups { get; set; }
        public List<SalesObject> Objects { get; set; }
        public string DateSelect { get; set; }
        public string pageHistory { get; set; }
    }
}
