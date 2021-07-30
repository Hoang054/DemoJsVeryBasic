using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourUpdateViewModel : Manhour
    {

        public string User_name { get; set; }

        public string Theme_name1 { get; set; }
        public string Work_contents_code_name { get; set; }
        public string Theme_name2 { get; set; }
        List<int> Holiday { get; set; }
    }
}
