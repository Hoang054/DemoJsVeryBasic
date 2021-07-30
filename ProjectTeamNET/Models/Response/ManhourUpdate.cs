using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectTeamNET.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Models.Response
{
    public class ManhourUpdate
    {
        public string today { get; set; }
        public string groupId { get; set; }
        public string userId { get; set; }
        public List<SelectListItem> groups { get; set; }
        public List<SelectListItem> users { get; set; }
        public List<SelectListItem> salesObject { get; set; }
        public List<SelectListItem> groupThemes { get; set; }      
    }
}