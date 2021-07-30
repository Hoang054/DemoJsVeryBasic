using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Service.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjectTeamNET.Controllers
{
    public class MenuController : Controller
    {

        private readonly IMenuService service;

        public MenuController(IMenuService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            string userNo = HttpContext.Session.GetString("userNo");
            if (userNo == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var result = await service.SendDataToController(userNo);
          
            return View(result);
        }


    }
}
