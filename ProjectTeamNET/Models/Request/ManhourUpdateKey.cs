using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class ManhourUpdateKey
    {
        public Int16 Year { get; set; }
        public Int16 Month { get; set; }
        public string User_no { get; set; }
        public string Theme_no { get; set; }
        public string Work_contents_class { get; set; }
        public string Work_contents_detail { get; set; }
        public string Work_contents_code { get; set; }
    }
}
