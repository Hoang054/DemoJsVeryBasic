using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class SaveData
    {
        public List<Manhour> Update { get; set; }
        public List<Manhour> Delete { get; set; }
        public List<Manhour> Insert { get; set; }
        public List<Manhour> NeedUpdate { get; set; }
        public List<Manhour> ForUpdate { get; set; }

    }
}
