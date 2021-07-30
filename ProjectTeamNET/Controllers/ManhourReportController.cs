using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectTeamNET.Controllers
{
    public class ManhourReportController : Controller
    {
        private readonly IManhourReportService manhourReportService;

        private const string ERROR = "Something wrong! Please try later";
        private string user = "";
        public ManhourReportController(IManhourReportService manhourReportService)
        {
            this.manhourReportService = manhourReportService;
        }
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userNo")))
            {
                user = HttpContext.Session.GetString("userNo").ToUpper();
                var model = manhourReportService.Init(user);
                return View(model);
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpGet("/ManhourReport/AddTheme/{count}")]
        public IActionResult AddTheme(int count)
        {
            var result = PartialView("AddTheme", count);
            return result;
        }

        [HttpPost("/ManhourReport/CheckReport")]
        public OkObjectResult CheckReport(ManHourReportSearch data)
        {
            var result = manhourReportService.ValidateReport(data);
            return Ok(result);
        }
        [HttpPost("/ManhourReport/CheckSave")]
        public OkObjectResult CheckSave(ManHourReportSearch data)
        {
            var result = manhourReportService.ValidateSaveName(data);
            return Ok(result);
        }

        [HttpPost("/ManhourReport/GetDataCSV")]
        public async Task<IActionResult> GetDataCSV(ManHourReportSearch data)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userNo")))
            {
                user = HttpContext.Session.GetString("userNo").ToUpper();
                var exportModel = await manhourReportService.GetDataReportCSV(data);
                if (exportModel.nameFile == "")
                {
                    return Ok(new { messenge = Resources.Messages.ERR_005 });
                }
                return Ok(new { data = exportModel.builder.ToString(), fileName = exportModel.nameFile, messenge = ""});
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost("/ManhourReport/SaveScreen")]
        public IActionResult SaveScreen(ManHourReportSearch data)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userNo")))
            {
                user = HttpContext.Session.GetString("userNo").ToUpper();
                var result = manhourReportService.SaveScreen(data, user);
                return Ok(result);
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpGet("/ManhourReport/GetsScreen")]
        public IActionResult GetsScreen()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userNo")))
            {
                user = HttpContext.Session.GetString("userNo").ToUpper();
                var result = manhourReportService.GetsScreen(user);
                return Ok(result);
            }
            return RedirectToAction("Index", "Login");
        }

        [HttpPost("/ManhourReport/CreateData")]
        public async Task<OkObjectResult> CreateData(ManHourReportSearch data)
        {
            var result = await manhourReportService.SetManhourReport(data);
            if(result.Count >0)
                return Ok(new {data = result, messenge = "" });
            return Ok(new { messenge = Resources.Messages.ERR_005 });
        }

        public IActionResult ShowReport()
        {
            return View();
        }

        [HttpDelete("/ManhourReport/DeleteScreenUser/{surrogate}")]
        public async Task<OkObjectResult> DeleteScreenUser(string surrogate)
        {
            var result = await manhourReportService.Delete(surrogate);
            if(result > 0)
            {
                return Ok(string.Format(Resources.Messages.INF_001, "Save Name"));
            }
            return Ok(ERROR);
        }

        [HttpGet("/ManhourReport/GetsUserName/{GroupCode}")]
        public async Task<OkObjectResult> GetsUserName(string groupCode)
        {
            var result = await manhourReportService.GetsUserName(groupCode);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsGroupName/{GroupCode}")]
        public async Task<OkObjectResult> GetsGroupName(string groupCode)
        {
            var result = await manhourReportService.GetsGroupName(groupCode);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsThemeName")]
        public async Task<OkObjectResult> GetsThemeName()
        {
            var result = await manhourReportService.GetsThemeName();
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetsGroupName")]
        public async Task<OkObjectResult> GetsGroupName()
        {
            var result = await manhourReportService.GetsGroupName();
            return Ok(result);
        }

        [HttpGet("/ManhourReport/WorkContents/{themeNo}")]
        public async Task<OkObjectResult> WorkContents(string themeNo)
        {
            var result = await manhourReportService.GetsWorkContent(themeNo);
            return Ok(result);
        }

        [HttpGet("/ManhourReport/GetManhourReport/{surrogatekey}")]
        public async Task<OkObjectResult> GetManhourReport(string surrogatekey)
        {
            var result = await manhourReportService.GetsManHourReportSaved(surrogatekey);
            return Ok(result);
        }
    }
}
