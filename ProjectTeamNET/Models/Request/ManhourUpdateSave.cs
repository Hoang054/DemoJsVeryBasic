using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class ManhourUpdateSave
    {
        public List<Manhour> save { get; set; }
        public List<ManhourUpdateKey> delete { get; set; }

    }
}
