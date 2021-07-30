using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Interface
{
   public interface ILoginService
    {
     
        public string GetDomainUrl();
        Task<UserInfo> GetInfoUser(string userNo);
        public string GetCurrentUserName();
    }
}
