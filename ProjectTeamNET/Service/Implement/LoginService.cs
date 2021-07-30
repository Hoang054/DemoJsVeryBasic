using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices.AccountManagement;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Utils;
using System.Net.NetworkInformation;

namespace ProjectTeamNET.Service.Implement
{
    
    public class LoginService : ILoginService 
    {
        private readonly IConfiguration config;
        private readonly IBaseRepository<UserInfo> loginRepository;

        public LoginService(IConfiguration config, IBaseRepository<UserInfo> loginRepository)
        {
            this.config = config;
            this.loginRepository = loginRepository;
        }
     

        public string GetCurrentUserName()
        {
            string Name = System.Security.Principal.WindowsIdentity.GetCurrent().Name; ;
            return Name;
        }

        public string GetDomainUrl()
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties(); 
            var result = properties.DomainName;
            return result;
        }

        public async Task<UserInfo> GetInfoUser(string userNo)
        {
            var query = QueryLoader.GetQuery("ManCheckHour", "GetUserInfo");
            var param = new
            {
                user_no = userNo,
               
            };
            UserInfo result = await loginRepository.MenuSearch<UserInfo>(query, param);
            return result;
        }

       
    }
}
