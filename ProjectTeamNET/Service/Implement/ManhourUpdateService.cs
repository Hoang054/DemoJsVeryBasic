using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectTeamNET.Models;
using ProjectTeamNET.Models.Response;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Resources;

namespace ProjectTeamNET.Service.Implement
{
    public class ManhourUpdateService : IManhourUpdateService
    {

        private readonly IBaseRepository<Manhour> manhourRepository;
        private readonly IBaseRepository<Group> groupRepository;
        private readonly IBaseRepository<SalesObject> saleRepository;        
        private readonly IBaseRepository<UserScreenItem> screenRepository;
        private readonly DbSet<WorkContents> dbWorkContents;
        private readonly DbSet<User> dbUsers;
        private readonly DbSet<Theme> dbTheme;
        private readonly IBaseRepository<UserScreenItem> userScreenItemRepository;
        const string HEADER = "年,月,ユーザNo,ユーザ名,テーマＮｏ,テーマ名,内容コード,内容名,内容詳細コード,合計," +
                              "1日,2日,3日,4日,5日,6日,7日,8日,9日,10日,11日,12日,13日,14日,15日,16日,17日,18日,19日," +
                              "20日,21日,22日,23日,24日,25日,26日,27日,28日,29日,30日,31日";
        public ManhourUpdateService(ProjectDbContext context,
                                    IBaseRepository<UserScreenItem> userScreenItemRepository,                                    
                                    IBaseRepository<Manhour> manhourRepository,
                                    IBaseRepository<Group> groupRepository,
                                    IBaseRepository<SalesObject> saleRepository,
                                    IBaseRepository<UserScreenItem> screenRepository)
        {
            this.dbUsers = context.Set<User>();
            this.dbWorkContents = context.Set<WorkContents>();
            this.dbTheme = context.Set<Theme>();
            this.userScreenItemRepository = userScreenItemRepository;            
            this.manhourRepository = manhourRepository;
            this.groupRepository = groupRepository;
            this.saleRepository = saleRepository;
            this.screenRepository = screenRepository;
        }
        public async Task<ManhourUpdate> GetGroupAndUser(string userId)
        {
            ManhourUpdate model = new ManhourUpdate();
            string functionClass = await GetFunctionClassByUserNo(userId);
            if (functionClass != "0")
            {
                DateTime today = DateTime.Now;
                model.today = today.ToString("yyyy/MM/dd").Substring(0, 7);              
                // get group_code no history
                string groupId = dbUsers.FirstOrDefault(e => e.User_no == userId).Group_code;
                // get History
                List<UserScreenItem> historySearch = await GetHistorySearchByUserNo(userId);
                List<Group> groups = await groupRepository.Gets();
                List<SalesObject> salesObjects = await saleRepository.Gets();               
                // SelectListItem
                List<SelectListItem> groupSelectList = new List<SelectListItem>();
                List<SelectListItem> userSelectList = new List<SelectListItem>();
                List<SelectListItem> saleObjSelectlist = new List<SelectListItem>();
                List<SelectListItem> groupSelectListTheme = new List<SelectListItem>();               
                if(functionClass == "2")
                {
                    foreach (var item in groups)
                    { 
                        if(item.Group_code == groupId)
                        {
                            groupSelectList.Add(new SelectListItem()
                            {
                                Value = item.Group_code,
                                Text = item.Group_code + "[" + item.Group_name + "]"
                            });
                            groupSelectListTheme.Add(new SelectListItem()
                            {
                                Value = item.Accounting_group_code,
                                Text = item.Accounting_group_code + "[" + item.Accounting_group_name + "]"
                            });
                        }
                       
                    }                   
                }
                else
                {
                    foreach (var item in groups)
                    {
                        groupSelectList.Add(new SelectListItem()
                        {
                            Value = item.Group_code,
                            Text = item.Group_code + "[" + item.Group_name + "]"
                        });
                        groupSelectListTheme.Add(new SelectListItem()
                        {
                            Value = item.Accounting_group_code,
                            Text = item.Accounting_group_code + "[" + item.Accounting_group_name + "]"
                        });
                    }

                }              
                foreach (var item in salesObjects)
                {
                    saleObjSelectlist.Add(new SelectListItem()
                    {
                        Value = item.Sales_object_code,
                        Text = item.Sales_object_code + "[" + item.Sales_object_name + "]"
                    });

                }
               
                model.groupThemes = groupSelectListTheme;
                model.salesObject = saleObjSelectlist;
                model.groups = groupSelectList;

                // if have history
                if (historySearch.Count != 0)
                {
                    string userNo = null;
                    string groupCode = null;

                    foreach (UserScreenItem item in historySearch)
                    {
                        if (item.Screen_item == "GROUP_CODE")
                        {
                            groupCode = item.Screen_input;
                        }
                        if (item.Screen_item == "USER_NO")
                        {
                            userNo = item.Screen_input;
                        }
                    }
                    List<User> users = dbUsers.Where(e => e.Group_code == groupCode).ToList();

                    foreach (var item in users)
                    {
                        userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
                    }
                    model.groupId = groupCode;
                    model.userId = userNo;
                    model.users = userSelectList;
                    return model;
                }
                // if not have history
                else
                {
                    List<User> users = dbUsers.Where(e => e.Group_code == groupId).ToList();
                    foreach (var item in users)
                    {
                        userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
                    }
                    model.groupId = groupId;
                    model.userId = userId;
                    model.users = userSelectList;
                    return model;
                }
            }
            return null;
        }

        public async Task<ManHourUpdateSearchModel> Search(ManhourUpdateSearch keySearch, string userId, string siteCode)
        {
            string[] screenItem = { "GROUP_CODE", "USER_NO", "SITE_CODE", "YEAR", "MONTH" };
            string[] screenInput = { keySearch.Group, keySearch.User, siteCode, keySearch.Year, keySearch.Month };
            DateTime today = DateTime.Now;
            string surrogateUserKey = userId + today.ToString("yyyyMMddHHmmss") + "00";
            List<UserScreenItem> history = await GetHistorySearchByUserNo(userId);
            if (history.Count != 0)
            {                
                foreach (var item in history)
                {
                    await screenRepository.Delete(item.Surrogate_key);
                }                               
            }
            for (int i = 0; i < 5; i++)
            {
                UserScreenItem userScreenItem = new UserScreenItem()
                {
                    Surrogate_key = surrogateUserKey + i.ToString(),
                    User_no = userId,
                    Screen_url = "ManhourUpdate",
                    Screen_item = screenItem[i],
                    Screen_input = screenInput[i],
                    Save_name = ""
                };
                screenRepository.Create(userScreenItem);
            }
            ManHourUpdateSearchModel model = new ManHourUpdateSearchModel();
            model.models = await GetDataSearchByKeySearch(keySearch);
            model.holiday = await GetHolidayByKeySearch(keySearch);
            return model;
        }

        public async Task<ExportModel> ExportCSV(string user, string group)
        {
            ExportModel exportModel = new ExportModel();
            DateTime today = DateTime.Now;
            ManhourUpdateSearch key = new ManhourUpdateSearch()
            {
                Year = today.Year.ToString(),
                Month = today.Month.ToString(),
                Group = group,
                User = user
            };
            List<ManhourUpdateViewModel> result = await GetDataSearchByKeySearch(key);
            string name = "DL Manhour " + user + " " + today.ToString("yyyyMMddHHmmss") + ".csv";
            StringBuilder buider = new StringBuilder();
            buider.AppendLine(HEADER);
            foreach (var item in result)
            {
                buider.AppendLine($"{item.Year}, {item.Month}, {item.User_no}, {item.User_name}, {item.Theme_no}, {item.Theme_name1}, " +
                                  $"{item.Work_contents_code},{item.Work_contents_code_name},{item.Work_contents_detail}, " +
                                  $"{item.Total:0.0},{item.Day1:0.0}, {item.Day2:0.0}, {item.Day3:0.0},{item.Day4:0.0}, " +
                                  $"{item.Day5:0.0}, {item.Day6:0.0}, {item.Day7:0.0}, " + $"{item.Day8:0.0}, {item.Day9:0.0}, {item.Day10:0.0}, " +
                                  $"{item.Day11:0.0}, {item.Day12:0.0}, {item.Day13:0.0}," + $"{item.Day14:0.0}, {item.Day15:0.0}, {item.Day16:0.0}, " +
                                  $"{item.Day17:0.0}, {item.Day18:0.0}, {item.Day19:0.0}," + $"{item.Day20:0.0}, {item.Day21:0.0}, {item.Day22:0.0}," +
                                  $"{item.Day23:0.0},{item.Day24:0.0}, {item.Day25:0.0}," + $"{item.Day26:0.0}, {item.Day27:0.0}, {item.Day28:0.0}," +
                                  $"{item.Day29:0.0}, {item.Day30:0.0}, {item.Day31:0.0}");
            }
            exportModel.builder = buider;
            exportModel.nameFile = name;
            return exportModel;
        }

        public async Task<string> ImportCSV(IFormFile files)
        {           
            List<Manhour> dataExport = new List<Manhour>();            
            using (var sreader = new StreamReader(files.OpenReadStream()))
            {
                string headers = sreader.ReadLine();
                bool checkHeader = IsCheckHeaderFileCSV(headers);
                if (checkHeader)
                {
                    while (!sreader.EndOfStream)                          //get all the content in rows 
                    {
                        string[] dataCSV = sreader.ReadLine().Split(',');
                        // check if key is empty
                        if (IsCheckNumber(dataCSV[0].Trim()) == false)
                        {
                            return string.Format(Messages.ERR_004, "Year ");
                        }
                        if (IsCheckNumber(dataCSV[1].Trim()) == false)
                        {
                            return string.Format(Messages.ERR_004, "Month ");
                        }
                        if (dbUsers.FirstOrDefault(e => e.User_no == dataCSV[2].Trim()) == null)
                        {
                            return string.Format(Messages.ERR_004, "User ");
                        }                      
                        string WorkContentsClass = await GetWordConteClass(dataCSV[4]);
                        if (WorkContentsClass == null || WorkContentsClass.Length > 2)
                        {
                            return string.Format(Messages.ERR_004, "Work Contents ");
                        }
                        if (dbTheme.FirstOrDefault(e => e.Theme_no == dataCSV[4].Trim()) == null)
                        {
                            return string.Format(Messages.ERR_004, "Theme ");
                        }
                        if (dbWorkContents.FirstOrDefault(e => e.Work_contents_code == dataCSV[6].Trim()) == null)
                        {
                            return string.Format(Messages.ERR_004, "Work contents code ");
                        }
                        if(dataCSV[8].Trim().Length == 0 || dataCSV[8].Trim().Length > 2)
                        {
                            return string.Format(Messages.ERR_004, "Work contents detail ");
                        }                    
                        Manhour CsvData = new Manhour();
                        CsvData.Year = Int16.Parse(dataCSV[0]);
                        CsvData.Month = Int16.Parse(dataCSV[1]);                      
                        CsvData.User_no = dataCSV[2].Trim();
                        CsvData.Theme_no = dataCSV[4].Trim();
                        CsvData.Work_contents_code = dataCSV[6].Trim();
                        CsvData.Work_contents_class = WorkContentsClass.Trim();
                        CsvData.Work_contents_detail = dataCSV[8].Trim();
                        if (IsCheckNumber(dataCSV[9].Trim()) == false)
                        {
                            return string.Format(Messages.ERR_004, "Total ");
                        }
                        CsvData.Total = Double.Parse(dataCSV[9]);
                        for (var i = 1; i <= 31; i++)
                        {
                            int j = i + 9;
                            //Check is Number
                            if (IsCheckNumber(dataCSV[j]))
                            {
                                if (Double.Parse(dataCSV[j]) < 0 )
                                {
                                    string day = "day " + i.ToString();
                                    return string.Format(Messages.ERR_004,day);
                                }
                                else if(Double.Parse(dataCSV[j]) > 24)
                                {
                                    return Messages.ERR_018;
                                }
                                else
                                {
                                    CsvData.GetType().GetProperty("Day" + i).SetValue(CsvData, Double.Parse(dataCSV[j]));
                                }                               
                            }
                            else
                            {                                
                                return Messages.ERR_005;
                            }
                        }                       
                        User user = await GetUser(CsvData.User_no);
                        CsvData.Group_code = user.Group_code.Trim();
                        CsvData.Site_code = user.Site_code.Trim();
                        CsvData.Fix_date = DateTime.Now.ToString("yyyyMMdd");
                        CsvData.Pin_flg = false;
                        dataExport.Add(CsvData);
                       
                    }    
                    // Update and Create if no error
                    foreach(Manhour item in dataExport)
                    {
                        bool check = IsExistManhour(item);
                        if (check)
                        {
                            UpdateManhours(item);
                        }
                        else
                        {
                            manhourRepository.Create(item);
                        }                          
                    }
                    return "CSVアップロードが正常終了しました";                    
                }
                //if header does not match
                else
                {
                    return Messages.ERR_005;
                }
            }
            
        }

        public bool IsCheckNumber(string day)
        {
            double price ;
            bool isDouble = Double.TryParse(day, out price);
            return isDouble;
        }

        public bool Save(ManhourUpdateSave saveData)
        {
            var result = 0;
            if (saveData.save.Count != 0)
            {
                foreach (Manhour mh in saveData.save)
                {                  
                    bool check = IsExistManhour(mh);
                    if (check)
                    {
                       UpdateManhours(mh);                       
                    }
                    else
                    {
                        manhourRepository.Create(mh);
                    }

                }
            }         
            //Delete list record rest from DB
            if (saveData.delete.Count != 0)
            {
                result = DeleteManhours(saveData.delete);
            }
            return result > 0;
        }

        public ManhourUpdateUserSelectList GetUserInGroup(string groupId)
        {
            ManhourUpdateUserSelectList resultUser = new ManhourUpdateUserSelectList();
            List<User> listUsers = (List<User>)dbUsers.Where(e => e.Group_code == groupId).ToList();
            List<SelectListItem> userSelectList = new List<SelectListItem>();
            foreach (var item in listUsers)
            {
                userSelectList.Add(new SelectListItem() { Value = item.User_no, Text = item.User_no + "[" + item.User_name + "]" });
            }
            resultUser.listUser = userSelectList;
            return resultUser;
        }

        public async Task<string> GetFunctionClassByUserNo(string userId)
        {
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectFunctionClass");
            List<string> functionClass = await manhourRepository.Search<string>(query, new { user_no = userId });
            return functionClass.FirstOrDefault();
        }

        public async Task<List<int>> GetHolidayByKeySearch(ManhourUpdateSearch keySearch)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(keySearch.Year.ToString()))
            {
                list.Add("Year");
            }
            if (!string.IsNullOrEmpty(keySearch.Month.ToString()))
            {
                list.Add("Month");
            }
            if (!string.IsNullOrEmpty(keySearch.User))
            {
                list.Add("User");
            }
            if (!string.IsNullOrEmpty(keySearch.Group))
            {
                list.Add("Group");
            }
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectHoliday", list);

            if (string.IsNullOrEmpty(keySearch.User))
            {
                keySearch.User = "";
            }
            var param = new
            {
                Year = Int32.Parse(keySearch.Year.Trim()),
                Month = Int32.Parse(keySearch.Month.Trim()),
                Group = keySearch.Group.ToString().Trim(),
                User = keySearch.User.ToString().Trim()
            };
            List<int> resultSearch = await manhourRepository.Search<int>(query, param);
            return resultSearch;
        }

        public async Task<List<ManhourUpdateViewModel>> GetDataSearchByKeySearch(ManhourUpdateSearch keySearch)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(keySearch.Year.ToString()))
            {
                list.Add("Year");
            }
            if (!string.IsNullOrEmpty(keySearch.Month.ToString()))
            {
                list.Add("Month");
            }
            if (!string.IsNullOrEmpty(keySearch.User))
            {
                list.Add("User");
            }
            if (!string.IsNullOrEmpty(keySearch.Group))
            {
                list.Add("Group");
            }
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectManhourData", list);

            if (string.IsNullOrEmpty(keySearch.User))
            {
                keySearch.User = "";
            }
            var param = new
            {
                Year = Int32.Parse(keySearch.Year.Trim()),
                Month = Int32.Parse(keySearch.Month.Trim()),
                Group = keySearch.Group.ToString().Trim(),
                User = keySearch.User.ToString().Trim()
            };          
            List<ManhourUpdateViewModel> resultSearch = await manhourRepository.Search<ManhourUpdateViewModel>(query, param);
            // sum day1 => day 31
            foreach(var item in resultSearch){
                item.Total =  item.Day1 + item.Day2 + item.Day3 + item.Day4 + item.Day5 + item.Day6 + item.Day7 + item.Day8 + item.Day9 + item.Day10
                            + item.Day11 + item.Day12 + item.Day13 + item.Day14 + item.Day15 + item.Day16 + item.Day17 + item.Day18 + item.Day19 + item.Day20
                            + item.Day21 + item.Day22 + item.Day23 + item.Day24 + item.Day25 + item.Day26 + item.Day27 + item.Day28 + item.Day29 + item.Day30 + item.Day31;
            }
            return resultSearch;
        }

        //Get History search
        public async Task<List<UserScreenItem>> GetHistorySearchByUserNo(string userId)
        {
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectHistoryManhourUpdate");
           
            List<UserScreenItem> userScreenItems = await manhourRepository.Search<UserScreenItem>(query, new { user_no = userId });            
            
            return userScreenItems;
        }                  

        // check header file csv duplicate header data
        public bool IsCheckHeaderFileCSV(string header)
        {
            string[] listHeaderCSV = header.Split(",");
            string[] listHeader = HEADER.Split(",");

            if (listHeader.Length == listHeaderCSV.Length)
            {
                for (int i = 0; i < listHeaderCSV.Length; i++)
                {
                    if (listHeader[i] != listHeaderCSV[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }      

        public int UpdateManhours(Manhour manhours)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "UpdateManhour");
            //Set param for update query
            var param = new
            {
                Year = manhours.Year,
                Month = manhours.Month,
                User_no = manhours.User_no,
                Theme_no = manhours.Theme_no,
                Work_contents_class = manhours.Work_contents_class,
                Work_contents_code = manhours.Work_contents_code,
                Work_contents_detail = manhours.Work_contents_detail,
                Pin_flg = manhours.Pin_flg,
                Total = manhours.Total,
                Day1 = manhours.Day1,
                Day2 = manhours.Day2,
                Day3 = manhours.Day3,
                Day4 = manhours.Day4,
                Day5 = manhours.Day5,
                Day6 = manhours.Day6,
                Day7 = manhours.Day7,
                Day8 = manhours.Day8,
                Day9 = manhours.Day9,
                Day10 = manhours.Day10,
                Day11 = manhours.Day11,
                Day12 = manhours.Day12,
                Day13 = manhours.Day13,
                Day14 = manhours.Day14,
                Day15 = manhours.Day15,
                Day16 = manhours.Day16,
                Day17 = manhours.Day17,
                Day18 = manhours.Day18,
                Day19 = manhours.Day19,
                Day20 = manhours.Day20,
                Day21 = manhours.Day21,
                Day22 = manhours.Day22,
                Day23 = manhours.Day23,
                Day24 = manhours.Day24,
                Day25 = manhours.Day25,
                Day26 = manhours.Day26,
                Day27 = manhours.Day27,
                Day28 = manhours.Day28,
                Day29 = manhours.Day29,
                Day30 = manhours.Day30,
                Day31 = manhours.Day31,
                Fix_date = manhours.Fix_date
            };
            result =  manhourRepository.Update<Manhour>(query, param);
          
            return result;
        }

        public int DeleteManhours(List<ManhourUpdateKey> manhours)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "DeleteManhour");

            foreach (ManhourUpdateKey mh in manhours)
            {
                //Set keys param for where of delete query
                var keys = new
                {
                    Year = mh.Year,
                    Month = mh.Month,
                    User_no = mh.User_no,
                    Theme_no = mh.Theme_no,
                    Work_contents_class = mh.Work_contents_class,
                    Work_contents_code = mh.Work_contents_code,
                    Work_contents_detail = mh.Work_contents_detail
                };
                result =  manhourRepository.Update<Manhour>(query, keys);
            }
            return result;
        }       

        public bool IsExistManhour(Manhour manhour)
        {
          
            List<string> list = new List<string>();
            var keys = new
            {
                Year = manhour.Year,
                Month = manhour.Month,
                User_no = manhour.User_no.Trim(),
                Theme_no = manhour.Theme_no.Trim(),
                Work_contents_class = manhour.Work_contents_class.Trim(),
                Work_contents_code = manhour.Work_contents_code.Trim(),
                Work_contents_detail = manhour.Work_contents_detail.Trim()

            };
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "CheckExist");
            int result = manhourRepository.Select<ManhourUpdate>(query, keys).Result.Count;
            if (result != 0)
            {
                return true;
            }
            return false;
        }

        public async Task<SelectThemeModel> SearchThemes(SearchThemeParam param, string user_no)
        {
            var addListTheme = new List<string>();

            //update screen item
            var updateScreenItem = QueryLoader.GetQuery("ManhourUpdateQuery", "UpdateUserScreenItemTheme");

            List<string> items = new List<string>
            {
                    "ThemeNo",
                    "AccountingGroupCode",
                    "ThemeName",
                    "SalesObjectCode",
                    "SoldFlg"
            };

            //update new item input in to user screen item
            foreach (string item in items)
            {
                string screenInput = (string)param.GetType().GetProperty(item).GetValue(param);

                var updateParam = new
                {
                    ScreenItem = item,
                    ScreenUrl = "SelectTheme",
                    UserNo = user_no,
                    ScreenInput = screenInput
                };

                screenRepository.Update(updateScreenItem, updateParam);
            }

            // check condition for where to get data
            if (!string.IsNullOrEmpty(param.ThemeNo))
            {
                addListTheme.Add("ThemeNo");
            }
            if (!string.IsNullOrEmpty(param.ThemeName))
            {
                addListTheme.Add("ThemeName");
            }
            if (!string.IsNullOrEmpty(param.SalesObjectCode))
            {
                addListTheme.Add("ObjectCode");
            }
            if (!string.IsNullOrEmpty(param.AccountingGroupCode))
            {
                addListTheme.Add("GroupCode");
            }
            if (!string.IsNullOrEmpty(param.SoldFlg) && !param.SoldFlg.Equals("全て"))
            {
                addListTheme.Add("SoldFlg");
            }
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectHistoryThemeSelected", addListTheme);

            //if sold flag is null or empty set default value is empty
            if (string.IsNullOrEmpty(param.SoldFlg))
            {
                param.SoldFlg = "";
            }

            var paramQuery = new
            {
                ThemeNo = param.ThemeNo,
                ThemeName = param.ThemeName,
                ObjectCode = param.SalesObjectCode,
                GroupCode = param.AccountingGroupCode,
                SoldFlg = param.SoldFlg.Equals("未売上") ? false : true
            };

            SelectThemeModel model = new SelectThemeModel();
            model.Themes = await manhourRepository.Select<Theme>(query, paramQuery);
            // set history input to return view 
            model.HistoryInput = param;

            return model;
        }

        public async Task<List<WorkContents>> GetWorkContents()
        {
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectWorkContent");
            List<WorkContents> result = await groupRepository.Select<WorkContents>(query);
            return result;
        }

        public async Task<string> GetWordConteClass(string themeNo)
        {
            var key = new
            {
                themeNo = themeNo.Trim()
            };
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectWordContenClass");
            List<string> result = await manhourRepository.Select<string>(query, key);
            if(result.Count == 0)
            {
                return null;
            }
            return result[0];
        }

        public async Task<User> GetUser(string userNo)
        {
            var key = new
            {
                userNo = userNo.Trim()
            };
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectUser");
            List<User> result = await manhourRepository.Select<User>(query, key);
            return result[0];
        }
    }

}
