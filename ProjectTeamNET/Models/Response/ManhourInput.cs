using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{ 
    public class ManhourInput: Manhour
    {
        public string Theme_name1 { get; set; }
        public string Theme_name2 { get; set; }
        public string work_contents_class_name { get; set; }
        public string Work_contents_code_name { get; set; }

    }
}
