using ProjectTeamNET.Models.Entity;
using ProjectTeamNET.Models.Request;
using ProjectTeamNET.Models.Response;
using ProjectTeamNET.Repository.Interface;
using ProjectTeamNET.Service.Interface;
using ProjectTeamNET.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTeamNET.Service.Implement
{
    /// <summary>
    /// Handle events ManhourInput
    /// </summary>
    public class ManhourInputService : IManhourInputService
    {
        const string FORMATDATE = "yyyy/MM/dd";
        const string HEADER     = "年,月,ユーザNo,所属コード,サイトコード,テーマNO,作業内容区分,作業内容コード,作業内容詳細,ピン止めフラグ,合計," +
                                  "1日,2日,3日,4日,5日,6日,7日,8日,9日,10日,11日,12日,13日,14日,15日,16日,17日,18日,19日," +
                                  "20日,21日,22日,23日,24日,25日,26日,27日,28日,29日,30日,31日,確定日付";
        private readonly IBaseRepository<Manhour> manhourRepository;
        private readonly IBaseRepository<SalesObject> saleRepository;
        private readonly IBaseRepository<UserScreenItem> screenRepository;
        public ManhourInputService(IBaseRepository<Manhour> manhourRepository, 
                                   IBaseRepository<SalesObject> saleRepository, 
                                   IBaseRepository<UserScreenItem> screenRepository)
        {
            this.manhourRepository      =   manhourRepository;
            this.saleRepository         =   saleRepository;
            this.screenRepository       =   screenRepository;
        }
        /// <summary>
        /// Get first data to load page
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public async Task<InitDataModel> Init(InputParamModel pModel)
        { 
            DateTime date = DateTime.Parse(pModel.DateStr);
           
            InitDataModel model = new InitDataModel();
            model.Groups = await GetGroups();
            model.Objects = await saleRepository.Gets();
            model.DateSelect = date.ToString(FORMATDATE);
            model.pageHistory = null;
            //Check screen item history button to return view Day/week/month
            List<UserScreenItem> pageHistory = await GetsScreenItemHistory(pModel.UserNo, "ManhourInput");
            if (pageHistory.Count != 0)
            {
                model.pageHistory = pageHistory.FirstOrDefault().Screen_item;
            }
            return model;
        }
        /// <summary>
        /// Get Manhour data from database and insert pintheme if not exist
        /// </summary>
        /// <param name="paramSt">dateSt + user_no</param>
        /// <returns></returns>
        public async Task<ManhourInputModel> GetManhourData(InputParamModel pModel)
        {
            DateTime date = DateTime.Parse(pModel.DateStr);
            HolidayParam hparam = new HolidayParam();
            hparam.Year = date.Year;
            hparam.Month = date.Month;
            hparam.SiteCode = pModel.SiteCode;

            //set data for manhourinput model
            ManhourInputModel model = new ManhourInputModel();
            model.ManhourDatas   = await GetManhours(pModel);
            model.DateSelect     = date.ToString(FORMATDATE);
            model.ListDateOfWeek = GetListDayOfWeek(date);
            model.Holidays      = await GetHoliday(hparam);

            //Check pin themes and insert if not exits in manhour
            List<ManhourInput> pinThemes = await GetPinTheme(pModel);
            if (pinThemes.Count() != 0)
            {
                foreach (ManhourInput pinManhour in pinThemes) 
                {
                    var invalidNum = 0;
                    var numManhour = 0;
                    foreach (ManhourInput manhour in model.ManhourDatas) 
                    {
                        //increatement row number approved
                        numManhour++;

                        //Check exit PinTheme in Manhour table by keys
                        if (   pinManhour.Year == manhour.Year
                            && pinManhour.User_no.Equals(manhour.User_no)
                            && pinManhour.Theme_no.Equals(manhour.Theme_no)
                            && pinManhour.Work_contents_class.Equals(manhour.Work_contents_class)
                            && pinManhour.Work_contents_code.Equals(manhour.Work_contents_code)
                            && pinManhour.Work_contents_detail.Equals(manhour.Work_contents_detail)
                          )
                          {
                                //if exist 
                                break;
                          }

                        //increatement row invalid
                        invalidNum++;
                    }

                    //If row not exits in manhour -> Insert to manhour table
                    if (invalidNum == numManhour)
                    {
                        Manhour pinMh = new Manhour();
                        pinMh.Year = pinManhour.Year;
                        pinMh.Month = (Int16)(pinManhour.Month + 1);
                        pinMh.User_no = pinManhour.User_no;
                        pinMh.Theme_no = pinManhour.Theme_no;
                        pinMh.Group_code = pinManhour.Group_code;
                        pinMh.Pin_flg = false; //set default value is false for Pin
                        pinMh.Site_code = pinManhour.Site_code;
                        pinMh.Work_contents_class = pinManhour.Work_contents_class;
                        pinMh.Work_contents_code = pinManhour.Work_contents_code;
                        pinMh.Work_contents_detail = pinManhour.Work_contents_detail;

                        manhourRepository.Create(pinMh);
                        
                        var query = QueryLoader.GetQuery("ManhourInput", "UpdatePinTheme");
                        var keys = new
                        {
                            pinManhour.Year,
                            pinManhour.Month,
                            pinManhour.User_no,
                            pinManhour.Theme_no,
                            pinManhour.Work_contents_class,
                            pinManhour.Work_contents_code,
                            pinManhour.Work_contents_detail
                        };

                        int result =  manhourRepository.Update(query, keys);
                    }
                }

                // Select again manhour data if had insert new record
                model.ManhourDatas = await GetManhours(pModel);
            }
           
            return model;
        }
        /// <summary>
        /// Get screen item by user_no and screen_url
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public async Task<List<UserScreenItem>> GetsScreenItemHistory(string user_no, string screen_url)
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectScreenItem");
            var pQuery = new
            {
                User_no = user_no,
                Screen_url = screen_url
            };
            List<UserScreenItem> result = await manhourRepository.Select<UserScreenItem>(query, pQuery);

            return result;
        }
        /// <summary>
        /// Get pin theme by param 
        /// </summary>
        /// <param name="paramSt"></param>
        /// <returns></returns>
        public async Task<List<ManhourInput>> GetPinTheme(InputParamModel pModel)
        {
            //get date and user_no from paramSt
            DateTime date = DateTime.Parse(pModel.DateStr);

            //get query by xml file
            var query = QueryLoader.GetQuery("ManhourInput","SelectPinTheme");

            //add param for query
            var pQuery = new
            {
                Year = date.Year,
                Month = date.Month-1,
                User_no = pModel.UserNo
            };
            List<ManhourInput> results = await manhourRepository.Select<ManhourInput>(query, pQuery);
            return results;
        }
        /// <summary>
        /// Get param search theme history
        /// </summary>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public async Task<SearchThemeParam> GetHistoryThemes(string user_no)
        {
            // Get History Theme selected
            List<UserScreenItem> searchHistory = await GetsScreenItemHistory(user_no,"SelectTheme");
           
            if(searchHistory.Count == 0)
            {
                List<string> items = new List<string>
                {
                    "ThemeNo",
                    "AccountingGroupCode",
                    "ThemeName",
                    "SalesObjectCode",
                    "SoldFlg"
                };
                
                //if not exist insert new  user screen item select theme
                var i = 0;
                foreach (string item in items)
                {
                    string key = user_no + DateTime.Now.ToString("yyyyMMddHHmmss") + i.ToString("D3");   
                    UserScreenItem user = new UserScreenItem 
                    {
                        Surrogate_key = key,
                        User_no       = user_no,
                        Screen_url    = "SelectTheme",
                        Screen_item   = item,
                        Screen_input  = null,
                        Save_name     = null
                    };  
                    screenRepository.Create(user);
                    i++;
                }
                return null;
            }

            SearchThemeParam historyParma = new SearchThemeParam();
            // Get value exist in db for search param
            foreach (UserScreenItem item in searchHistory)
            {
                if (item.Screen_item.Equals("ThemeNo"))
                    historyParma.ThemeNo = item.Screen_input;
                if (item.Screen_item.Equals("ThemeName"))
                    historyParma.ThemeName = item.Screen_input;
                if (item.Screen_item.Equals("AccountingGroupCode"))
                    historyParma.AccountingGroupCode = item.Screen_input;
                if (item.Screen_item.Equals("SalesObjectCode"))
                    historyParma.SalesObjectCode = item.Screen_input;
                if (item.Screen_item.Equals("SoldFlg"))
                    historyParma.SoldFlg = item.Screen_input;
            }

            return historyParma;              
        }
        /// <summary>
        /// Search Theme by param 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SelectThemeModel> SearchThemes(SearchThemeParam pSearchQuery, string user_no)
        {
            var addListTheme = new List<string>();

            //update screen item
            var updateScreenItem = QueryLoader.GetQuery("ManhourInput", "UpdateUserScreenItem");

            List<string> items = new List<string>
            {
                    "ThemeNo",
                    "AccountingGroupCode",
                    "ThemeName",
                    "SalesObjectCode",
                    "SoldFlg"
            };
            
            //update new item input in to user screen item
            foreach(string item in items)
            {
                string screenInput =(string)pSearchQuery.GetType().GetProperty(item).GetValue(pSearchQuery);

                var updateParam = new
                {
                    ScreenItem = item,
                    ScreenUrl = "SelectTheme",
                    UserNo = user_no,
                    ScreenInput = screenInput
                };

                manhourRepository.Update(updateScreenItem, updateParam);
            }
                         
            // check condition for where to get data
            if (!string.IsNullOrEmpty(pSearchQuery.ThemeNo))
            {
                addListTheme.Add("ThemeNo");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.ThemeName))
            {
                addListTheme.Add("ThemeName");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.SalesObjectCode))
            {
                addListTheme.Add("ObjectCode");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.AccountingGroupCode))
            {
                addListTheme.Add("GroupCode");
            }
            if (!string.IsNullOrEmpty(pSearchQuery.SoldFlg) && !pSearchQuery.SoldFlg.Equals("全て"))
            {
                addListTheme.Add("SoldFlg");
            }

            var query = QueryLoader.GetQuery("ManhourInput", "SelectHistoryTheme", addListTheme);

            //if sold flag is null or empty set default value is empty
            if( string.IsNullOrEmpty(pSearchQuery.SoldFlg))
            {
                pSearchQuery.SoldFlg = "";
            }

            var paramQuery = new
            {
                ThemeNo = pSearchQuery.ThemeNo,
                ThemeName = pSearchQuery.ThemeName,
                ObjectCode = pSearchQuery.SalesObjectCode,
                GroupCode = pSearchQuery.AccountingGroupCode,
                SoldFlg = pSearchQuery.SoldFlg.Equals("未売上") ? false : true
            };

            SelectThemeModel model = new SelectThemeModel();
            model.Themes = await manhourRepository.Select<Theme>(query, paramQuery);
            // set history input to return view 
            model.HistoryInput = pSearchQuery;

            return model;
        }
        /// <summary>
        /// Handle save data form client to db
        /// </summary>
        /// <param name="screenItems"></param>
        public bool Save(SaveData saveDatas, UserInfo user)
        {
            var result = 0;
            //Insert new record in save list 
            if (saveDatas.Insert.Count != 0)
            {
                result = Create(saveDatas.Insert,user);
                if (result < 0)
                {
                    return false;
                }
            }
            //Update theme change
            if(saveDatas.NeedUpdate.Count !=0 && saveDatas.ForUpdate.Count != 0)
            {
                result =  ChangeManhour(saveDatas.NeedUpdate, saveDatas.ForUpdate, user.User_no);
            }
            //update information 
            if (saveDatas.Update.Count != 0)
            {
                result =  UpdateManhours(saveDatas.Update,user.User_no);
                if (result < 0)
                {
                    return false;
                }
            }
            //Delete in list delete
            if (saveDatas.Delete.Count != 0)
            {
                result =  DeleteManhours(saveDatas.Delete,user.User_no);
                if (result < 0)
                {
                    return false;
                }
            }
            return result >= 0;
        }
        /// <summary>
        /// Return list dayofweek need get
        /// </summary>
        /// <param name="dateSt"></param>
        /// <returns></returns>
        public List<string> GetListDayOfWeek(DateTime date)
        {
            DateTime dateSelected = date;
            int currentDayOfWeek = (int) dateSelected.DayOfWeek;
         
            DateTime sunday = dateSelected.AddDays(-currentDayOfWeek);

            //get list date form monday to sunday
            var dates = Enumerable.Range(0, 7).Select(days => sunday.AddDays(days)).ToList();
            List<string> dateOfWeekList = new List<string>();

            foreach (var data in dates)
            {
                //if month of this date is in month selected
                if (DateTime.Parse(data.ToString()).Month == dateSelected.Month)
                {
                    dateOfWeekList.Add(String.Format("{0:yyyy/MM/dd}", data));
                }
            }
            return dateOfWeekList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public int Create(List<Manhour> manhours, UserInfo user)
        {
            var result = 0;
            foreach (Manhour mh in manhours)
            {
                Manhour newMh = new Manhour
                {
                    Year = mh.Year,
                    Month = mh.Month,
                    User_no = user.User_no,
                    Group_code = user.Group_code,
                    Site_code = user.Site_code,
                    Theme_no = mh.Theme_no,
                    Work_contents_class = mh.Work_contents_class,
                    Work_contents_code = mh.Work_contents_code,
                    Work_contents_detail = mh.Work_contents_detail
                };

                result =  manhourRepository.Create(newMh);
            }
            return result;
        }
        /// <summary>
        /// Update records in manhour table
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public  int UpdateManhours(List<Manhour> manhours, string user_no)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "UpdateManhour");

            foreach (Manhour mh in manhours)
            {
                //Set param for update query
                var param = new
                {
                    mh.Year,mh.Month,User_no=user_no,mh.Theme_no,mh.Work_contents_class,mh.Work_contents_code,
                    mh.Work_contents_detail,mh.Pin_flg,mh.Total,
                    mh.Day1,mh.Day2,mh.Day3,mh.Day4,mh.Day5,mh.Day6,mh.Day7,mh.Day8,mh.Day9,mh.Day10,
                    mh.Day11,mh.Day12,mh.Day13,mh.Day14,mh.Day15,mh.Day16,mh.Day17,mh.Day18,mh.Day19,
                    mh.Day20,mh.Day21,mh.Day22,mh.Day23,mh.Day24,mh.Day25,mh.Day26,mh.Day27,mh.Day28,
                    mh.Day29,mh.Day30,mh.Day31,mh.Fix_date
                };
                result =  manhourRepository.Update(query, param);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manhours"></param>
        /// <returns></returns>
        public int DeleteManhours(List<Manhour> manhours, string user_no)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "DeleteManhour");

            foreach (Manhour mh in manhours)
            {
                //Set keys param for where of delete query
                var keys = new
                {
                    mh.Year,
                    mh.Month,
                    User_no = user_no,
                    mh.Theme_no,
                    mh.Work_contents_class,
                    mh.Work_contents_code,
                    mh.Work_contents_detail
                };
                result =  manhourRepository.Update(query, keys);
            }
            return result;
        }
        /// <summary>
        /// Get list of manhour data
        /// </summary>
        /// <param name="paramModel"></param>
        /// <returns></returns>
        public async Task<List<ManhourInput>> GetManhours(InputParamModel paramModel)
        { 
            DateTime date = DateTime.Parse(paramModel.DateStr);

            //add list for where select if value of condition not null or empty
            var addList = new List<string>();
            if (!string.IsNullOrEmpty(date.Year.ToString()))
            {
                addList.Add("Year");
            }
            if (!string.IsNullOrEmpty(date.Month.ToString()))
            {
                addList.Add("Month");
            }
            if (!string.IsNullOrEmpty(paramModel.UserNo))
            {
                addList.Add("User_no");
            }

            // Get query select 
            var query = string.Format(QueryLoader.GetQuery("ManhourInput", "SelectManhourDatas", addList));

            // Set param for manhour query
            var param = new
            {
                Year = date.Year,
                Month = date.Month,
                User_no = paramModel.UserNo
            };
            return await manhourRepository.Select<ManhourInput>(query, param);
        }
        /// <summary>
        /// get Accounting group for combobox
        /// </summary>
        /// <returns></returns>
        public async Task<List<Group>> GetGroups()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectGroupCode");
            List<Group> result = await manhourRepository.Select<Group>(query);
            return result;
        }
        /// <summary>
        /// get work contents for combobox
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkContents>> GetWorkContents()
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectWorkContent");
            List<WorkContents> result = await manhourRepository.Select<WorkContents>(query);
            return result;
        }
        /// <summary>
        /// Save screen item cliked
        /// </summary>
        /// <param name="pModel"></param>
        public bool SavePageHistory(InputParamModel pModel)
        {
            //delete user screen in database
            var delScreenItem = QueryLoader.GetQuery("ManhourInput", "DeleteScreenItem");
            var delParam = new
            {
                ScreenUrl = "ManhourInput",
                pModel.UserNo,
            };

            var result = screenRepository.Update(delScreenItem, delParam);
            if (result < 0)
            {
                return false;
            }
            //Insert new a user screen item into database
            string key = pModel.UserNo + DateTime.Now.ToString("yyyyMMddHHmmss");
            key += 0.ToString("D3");
            UserScreenItem user = new UserScreenItem
            {
                Surrogate_key = key,
                User_no = pModel.UserNo,
                Screen_url = "ManhourInput",
                Screen_item = pModel.PageName,
                Screen_input = null,
                Save_name = null
            };
            result=screenRepository.Create(user);

            return result >= 0;
        }
        /// <summary>
        /// get list holiday by sitecode month and year
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<int>> GetHoliday(HolidayParam param)
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectHorliday");
            List<int> horlidays = await manhourRepository.Search<int>(query, param);
            return horlidays;
        }
        /// <summary>
        /// get list workcontent by class code
        /// </summary>
        /// <param name="classCode"></param>
        /// <returns></returns>2
        public async Task<List<WorkContents>> GetWorkContentsByClass(string classCode)
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectByWorkClass");
            var param = new {
                Work_contents_class = classCode
            };
            return  await manhourRepository.Select<WorkContents>(query, param);
        }
        /// <summary>
        /// Export manhour data of month selected
        /// </summary>
        /// <param name="pModel"></param>
        /// <returns></returns>
        public async Task<ExportModel> ExportCSV(string userNo, string dateStr)
        {
            ExportModel exportModel = new ExportModel();
            DateTime date = DateTime.Parse(dateStr);

            List<ManhourUpdateViewModel> result = await GetDataExport(userNo, date);
            string name = "DL Manhour " + userNo + " " + date.ToString("yyyyMMddHHmmss") + ".csv";
            StringBuilder buider = new StringBuilder();
            buider.AppendLine(HEADER);

            foreach (var item in result)
            {
                buider.AppendLine($"{item.Year}, {item.Month}, {item.User_no},{item.Group_code},{item.Site_code}, {item.Theme_no}," +
                                   $"{item.Work_contents_class},{item.Work_contents_code},{item.Work_contents_detail},{item.Pin_flg}, " +
                                   $"{item.Total:0.0},{item.Day1:0.0}, {item.Day2:0.0}, " +
                                   $"{item.Day3:0.0},{item.Day4:0.0}, " +
                                   $"{item.Day5:0.0}, {item.Day6:0.0}, {item.Day7:0.0}, " +
                                   $"{item.Day8:0.0}, {item.Day9:0.0}, {item.Day10:0.0}, " +
                                   $"{item.Day11:0.0}, {item.Day12:0.0}, {item.Day13:0.0}," +
                                   $"{item.Day14:0.0}, {item.Day15:0.0}, {item.Day16:0.0}, " +
                                   $"{item.Day17:0.0}, {item.Day18:0.0}, {item.Day19:0.0}," +
                                   $"{item.Day20:0.0}, {item.Day21:0.0}, {item.Day22:0.0}," +
                                   $"{item.Day23:0.0},{item.Day24:0.0}, {item.Day25:0.0}," +
                                   $"{item.Day26:0.0}, {item.Day27:0.0}, {item.Day28:0.0}," +
                                   $"{item.Day29:0.0}, {item.Day30:0.0}, {item.Day31:0.0},{item.Fix_date}");
            }

            exportModel.builder = buider;
            exportModel.nameFile = name;
            return exportModel;
        }
        /// <summary>
        /// Export CSV file
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<ManhourUpdateViewModel>> GetDataExport(string userNo, DateTime date)
        {
            List<string> addlist = new List<string>{"Year","Month","User"};
            var param = new
            {
                date.Year,
                date.Month,
                User = userNo,
            };
            var query = QueryLoader.GetQuery("ManhourUpdateQuery", "SelectManhourData", addlist);
            List<ManhourUpdateViewModel> result = await manhourRepository.Search<ManhourUpdateViewModel>(query, param);
            return result;
        }
        /// <summary>
        /// Check them exist
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<bool> CheckThemeExist(ManhourKeys keys)
        {
            var query = QueryLoader.GetQuery("ManhourInput", "SelectManhour");
            var param = new
            {
                keys.Year,
                keys.Month,
                keys.User_no,
                keys.Theme_no,
            };
            List<Manhour> result = await manhourRepository.Select<Manhour>(query, param);
            return result.Count > 0;
        }
        /// <summary>
        /// Change theme in manhour
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="newData"></param>
        /// <param name="user_no"></param>
        /// <returns></returns>
        public int ChangeManhour(List<Manhour> oldData, List<Manhour> newData,string user_no)
        {
            var result = 0;
            var query = QueryLoader.GetQuery("ManhourInput", "ChangeManhour");

            for (int i = 0; i < oldData.Count; i++)
            {
                //Set param for update query
                var param = new
                {
                    oldData[i].Year,
                    oldData[i].Month,
                    User_no = user_no,
                    oldData[i].Theme_no,
                    oldData[i].Work_contents_class,
                    oldData[i].Work_contents_code,
                    oldData[i].Work_contents_detail,
                    NewTheme_no = newData[i].Theme_no,
                    NewWork_contents_class = newData[i].Work_contents_class,
                    Newwork_contents_code = newData[i].Work_contents_code,
                    NewWork_contents_detail = newData[i].Work_contents_detail,
                };
                result =  manhourRepository.Update(query, param);
            }
            return result;
        }
    }
}
