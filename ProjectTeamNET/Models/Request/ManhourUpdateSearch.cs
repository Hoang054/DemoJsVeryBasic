using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class ManhourUpdateSearch
    {
        public string  Year { get; set; }
        public string Month { get; set; }
        public string Group { get; set; }
        public string User { get; set; }

    }
}
