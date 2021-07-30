using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
namespace ProjectTeamNET.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILoginService service;

        public LoginController(ILoginService service)
        {
            this.service = service;
        }

        public IActionResult AutoLogin()
        {
            var check = CheckAutoLogin();
            if (check)
            {
                UserPrincipal user = UserPrincipal.Current;
                string loginName = user.SamAccountName;
                var userInfo = service.GetInfoUser(loginName.ToUpper());

                HttpContext.Session.SetString("userNo", userInfo.Result.User_no);
                HttpContext.Session.SetString("roleCode", userInfo.Result.Role_code);
                HttpContext.Session.SetString("groupCode", userInfo.Result.Group_code);
                HttpContext.Session.SetString("siteCode", userInfo.Result.Site_code);
                return RedirectToAction("Index", "Menu"); // duong dan successs
            }
            else
            {
                ViewBag.Error = Resources.Messages.ERR_010;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            var Url = service.GetDomainUrl();

            //check null value
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = Resources.Messages.ERR_010;
                return View(model);
            }

            var check = AuthenticateUser(Url, model.UserName, model.Password);
            if (check)
            {                
                var userInfo = service.GetInfoUser(model.UserName);
                HttpContext.Session.SetString("userNo", userInfo.Result.User_no);
                HttpContext.Session.SetString("roleCode", userInfo.Result.Role_code);
                HttpContext.Session.SetString("groupCode", userInfo.Result.Group_code);
                HttpContext.Session.SetString("siteCode", userInfo.Result.Site_code);
                return RedirectToAction("Index", "Menu"); // duong dan successs
            }
            else
            {
                ViewBag.Error = Resources.Messages.ERR_010;
                return View(model);
            }
        }

        /// <summary>
        /// authenticate login information
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AuthenticateUser(string domainName, string userName, string password)
        {
            var ret = false;
            try
            {
                if (ModelState.IsValid)
                {
                    DirectoryEntry de = new DirectoryEntry("LDAP://" + domainName, userName, password);
                    DirectorySearcher dsearch = new DirectorySearcher(de);
                    SearchResult results = null;
                    results = dsearch.FindOne();
                    ret = true;
                }
            }
            catch
            {
                ret = false;
            }

            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CheckAutoLogin()
        {
            var check = false;
            UserPrincipal user = UserPrincipal.Current;
            string loginName = user.SamAccountName;
            var listUser = GetAllAdUsers();
            for (int i = 0; i < listUser.Count; i++)
            {
                if (listUser[i].Samaccountname == loginName)
                {
                    return true;
                }
            }
            return check;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserProfile> GetAllAdUsers()
        {
            List<UserProfile> adUsers = new List<UserProfile>();
            PrincipalContext context = new PrincipalContext(ContextType.Domain);
            PrincipalSearcher search = new PrincipalSearcher(new UserPrincipal(context));
            UserPrincipal userPrin = new UserPrincipal(context);

            //UserPrincipal user2 = UserPrincipal.FindByIdentity(context, model.UserName);
            var searcher = new PrincipalSearcher();
            searcher.QueryFilter = userPrin;
            var results = searcher.FindAll();
            foreach (UserPrincipal p in results)
            {
                adUsers.Add(new UserProfile
                {
                    DisplayName = p.DisplayName,
                    Samaccountname = p.SamAccountName
                });
            };

            return adUsers;
        }
        /// <summary>
        /// logout system
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userNo");
            HttpContext.Session.Remove("roleCode");
            return this.RedirectToAction("Index", "Login");
        }

    }
}
