using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Service.Interface;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ProjectTeamNET.Controllers
{
    public class ManhourUpdateController : Controller
    {
        private readonly IManhourUpdateService manhourUpdateService;
        public ManhourUpdateController(IManhourUpdateService manhourUpdateService)
        {
            this.manhourUpdateService = manhourUpdateService;           
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userNo = HttpContext.Session.GetString("userNo");
            if (userNo == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ManhourUpdate model = await manhourUpdateService.GetGroupAndUser(userNo);
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Menu");
        }

        [HttpPost("/ManhourUpdate/Search")]
        public async Task<JsonResult> Search(ManhourUpdateSearch keySearch)
        {
            string siteCode = "";
            string userNo = HttpContext.Session.GetString("userNo").ToUpper();
            siteCode = HttpContext.Session.GetString("siteCode").ToUpper();
            if (userNo == null)
            {
                return Json(new { Url = "/Login" }); 
            }
            ManHourUpdateSearchModel result = new ManHourUpdateSearchModel();
            result = await manhourUpdateService.Search(keySearch, userNo, siteCode);
            return Json(new { data = result });
        }

        // get user in group
        [HttpPost("/ManhourUpdate/GetUser/{group}")]
        public JsonResult GetUser(string group)
        {
            ManhourUpdateUserSelectList user = manhourUpdateService.GetUserInGroup(group);
            return Json(new { data = user });
        }

        [HttpGet("/ManhourUpdate/ExportCSV")]
        public async Task<IActionResult> ExportCSV(string user, string group)
        {
            ExportModel exportModel = await manhourUpdateService.ExportCSV(user, group);
            return File(Encoding.UTF8.GetBytes(exportModel.builder.ToString()), "text/csv", exportModel.nameFile);
        }

        [HttpPost("/ManhourUpdate/ImportCSV")]
        public async Task<JsonResult> ImportCSV(IFormFile file)
        {
            
            if (file.FileName.EndsWith(".csv"))
            {
                string result = await manhourUpdateService.ImportCSV(file);             
                return Json(new { messages = result });               
            }
            return Json(new { messages = "ファイルはCSV形式（拡張子csv）のみ可能です" });

        }

        [HttpPost]
        public async Task<JsonResult> SearchThemes(SearchThemeParam param)
        {
            string userNo = HttpContext.Session.GetString("userNo");
            if (userNo == null)
            {
                return Json(new { Url = "/Login" }); 
            }
            SelectThemeModel data = await manhourUpdateService.SearchThemes(param, userNo);
            if (data.Themes.Count >= 1000)
            {
                return Json(Resources.Messages.ERR_001);
            }
            return Json(data);
        }

        [HttpPost("/ManhourUpdate/Save")]
        public Task<JsonResult> Save([FromBody] ManhourUpdateSave saveData)
        {
            if(saveData != null)
            {
                manhourUpdateService.Save(saveData);
                return Task.FromResult(Json("Save Successfuly!"));
            }
            return Task.FromResult(Json("Save Error"));

        }
    }
}
