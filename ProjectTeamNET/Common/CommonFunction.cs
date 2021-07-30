using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using ProjectTeamNET.Models.Response;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectTeamNET
{
    
    public static class CommonFunction
    {
        /// <summary>
        /// author: Hieunv
        /// Kiểm tra chuỗi truyền vào có phải là 1 container string
        /// </summary>
        /// <param name="code"> Container string</param>
        /// <returns></returns>

        public static bool IsContainerCode(string code)
        {
            try
            {
                int size = code.Length;

                // kiểm tra độ dài của chuỗi truyền vào
                if (size != 11)
                {
                    return false;
                }

                string ownerCode = code.Substring(0, 3);
                string category = code.Substring(3, 1);
                string serialNum = code.Substring(4, 6);

                int chekDigit = Int32.Parse(code.Substring(10, 1));

                // kiểm tra ký tự của category nằm trong 'U', 'Z', 'J'
                if (!(category.Equals("U") || category.Equals("Z") || category.Equals("J")))
                {
                    return false;
                }

                // kiểm tra serial number is số
                Regex regexSerialNum = new Regex(@"^[0-9]+$");
                if (!regexSerialNum.IsMatch(serialNum))
                {
                    return false;
                }

                // kiểm tra Owner Code là chử trong khoảng A-Z
                Regex regexOwnerCode = new Regex(@"^[A-Z]+$");
                if (!regexOwnerCode.IsMatch(ownerCode))
                {
                    return false;
                }

                // Thêm key value vào  một dictionary ( A,0)
                Dictionary<string, int> dicCharValue = new Dictionary<string, int>();
                int value = 10;
                for (int key = 0; key < 26; key++)
                {
                    if (value == 11 || value == 22 || value == 33)
                    {
                        value ++;
                    }
                    dicCharValue.Add(Convert.ToChar(key + 65).ToString(), value);
                    value ++;
                }

                //Tính tổng ban đầu của chuổi truyền vào
                int originalSum = 0;
    
                int multiplier = 1;
                for (int i = 0; i < size - 1; i++)
                {
                    //Nếu là chữ cộng bằng value của dictionary đã thêm
                    if (dicCharValue.ContainsKey(code[i].ToString()))
                    {
                        originalSum += multiplier * dicCharValue[code[i].ToString()];
                    }
                    //Là số
                    else
                    {
                        originalSum += multiplier * Int32.Parse(code[i].ToString());
                    }
                    multiplier *= 2;
                }

                //sô sau khi chia 11 và làm tròn và nhân với 11
                int subtrahend = (int)Math.Round(double.Parse((originalSum / 11).ToString()), MidpointRounding.AwayFromZero) * 11;


                //kiểm tra hiệu của tổng ban đầu và số bi trừ với check digit
                if (!(originalSum - subtrahend == chekDigit))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// author: Hieunv 
        /// convert password string by MD5
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string MD5Encode(string pwd)
        {
            try {

                var md5 = new MD5CryptoServiceProvider();
                var originalBytes = Encoding.Default.GetBytes(pwd);
                var encodedBytes = md5.ComputeHash(originalBytes);

                return BitConverter.ToString(encodedBytes);

            } catch {

                return "Convert failed";

            }
            
        }

        /// <summary>
        /// author: Nhanvt
        /// Get day of week by japanese
        /// </summary>
        /// <param name="dateStr">Date String</param>
        /// <returns></returns>
        public static string GetJapaneseDayOfWeek(string dateSt)
        {
            DateTime dt;
            string day = string.Empty;
            try
            {
                if (DateTime.TryParse(dateSt, out dt))
                {
                    // Chuyển thứ vừa lấy được sang tiếng nhật
                    switch (dt.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            day = "日";
                            break;
                        case DayOfWeek.Monday:
                            day = "月";
                            break;
                        case DayOfWeek.Tuesday:
                            day = "火";
                            break;
                        case DayOfWeek.Wednesday:
                            day = "水";
                            break;
                        case DayOfWeek.Thursday:
                            day = "木";
                            break;
                        case DayOfWeek.Friday:
                            day = "金";
                            break;
                        case DayOfWeek.Saturday:
                            day = "土";
                            break;
                        default:
                            break;
                    }
                }
                return day;
            }
            catch (Exception e)
            {
                e.StackTrace.ToString();
                return day;
            }
        }

        /// <summary>
        /// author: Nhanvt
        /// Lấy số tháng giữa 2 ngày truyền vào
        /// </summary>
        /// <param name="fromDate"> Từ ngày </param>
        /// <param name="toDate"> Đến ngày</param>
        /// <returns></returns>
        public static int GetMonthBetWeenTwoDate(DateTime fromDate, DateTime toDate)
        {

            var betweenMonth = ((toDate.Year - fromDate.Year) * 12) + toDate.Month - fromDate.Month;
            if (betweenMonth >= 0)
            {
                return betweenMonth;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// author: Nhanvt
        /// Convert decimal to string and round by number decimal need take
        /// </summary>
        /// <param name="numDecimal"> số decimal</param>
        /// <param name="numTake"> Số thập phân cần lấy</param>
        /// <returns></returns>
        public static string ConvertDecimalToString(Double decimalNum, int numTake)
        {
            if(numTake < 0)
            {
                return "Take number decimal < 0";
            }
            if (decimalNum >= 0)
            {
                return Math.Round(decimalNum, numTake, MidpointRounding.AwayFromZero).ToString();
            }
            else
            {
                var power = Convert.ToDouble(Math.Pow(10, numTake));
                return (-Math.Floor(-decimalNum * power) / power).ToString();

            }
        }

        /// <summary>
        /// author: Thuannv
        /// </summary>
        /// <param name="fileName"> Tên file muốn đọc</param>
        /// <returns></returns>
        public static List<string> ReadCSVFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    var fileExt = Path.GetExtension(fileName).Substring(1).ToLower();
                    if (fileExt == "csv")
                    {
                        string[] lines = File.ReadAllLines(fileName);
                        List<string> Data = new List<string>();
                        foreach (var line in lines)
                        {
                            Data.Add(line);
                        }
                        return Data;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }                  

        }

        /// <summary>
        /// author: Thuannv
        /// </summary>
        /// <param name="fileName"> Tên file muốn lưu</param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool WriteSCVFile(string fileName, List<string> Data)
        {
            // valid đuôi file
            try
            {
                var fileExt = Path.GetExtension(fileName).Substring(1).ToLower();
                if (File.Exists(fileName) && fileExt == "csv")
                {
                    foreach (var item in Data)
                    {
                        string items = item + "\n";
                        File.AppendAllText(fileName, items);
                    }
                    return true;
                }
                else
                    return false;
            } catch {

                return false;
            }
          
        }

        /// <summary>
        /// author: Hoangtx
        /// in ra ngày các ngày trong tháng theo từng thứ trong tuan
        /// </summary>
        /// <param name="dateSt"></param>   
        /// <returns></returns>
        public static Calendar PrintCalendar(DateTime date)
        {
            var year = date.Year;
            var month = date.Month;

            // Ngày đâu tiên của tháng
            var firstDay = new DateTime(year, month, 1);
            //Thứ đầu tiên của tháng
            var dayOfWeek = firstDay.DayOfWeek;

            int daysInMoth = System.DateTime.DaysInMonth(year, month);

            var result = new Calendar { Index = (int)dayOfWeek, TotalDay = daysInMoth };
            return result;
        }

        /// <summary>
        /// author: Hoangtx
        /// coppy 2 object của cùng một class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"> Truyền vào một object</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj)
        {
            try {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);
                    ms.Position = 0;

                    return (T)formatter.Deserialize(ms);
                }
            } catch {
                return obj;
            }
            
        }

        /// <summary>
        /// author: Thuannv
        /// Coppy property of object( class A) to object( class B)
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        public static class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
            
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        //Nếu property có cùng tên và cùng kiểu dữ liệu thì sẽ 
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Display format number to decimal
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static string ToFixed(this double number, uint decimals)
        {
            return number.ToString("N" + decimals);
        }

    }
}
