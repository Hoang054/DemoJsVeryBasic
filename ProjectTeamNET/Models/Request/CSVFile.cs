using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Request
{
    public class CSVFile
    {
        public IFormFileCollection Csv { get; set; }
    }
}
