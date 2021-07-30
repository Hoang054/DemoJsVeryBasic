using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ExportModel
    {
        public StringBuilder builder { get; set; }
        public string nameFile { get; set; }
        public string message { get; set; }
    }
}
