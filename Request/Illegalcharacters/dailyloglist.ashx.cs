using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using log4net;
using System.Reflection;
using Bussiness;
using SqlDataProvider.Data;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for dailyloglist
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class dailyloglist : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";
            XElement result = new XElement("Result");

            try
            {
                string key = context.Request["key"];
                int id = int.Parse(context.Request["selfid"]);
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    DailyLogListInfo info = db.GetDailyLogListSingle(id);
                    if (info == null)
                    {
                        info = new DailyLogListInfo();
                        info.UserID = id;
                        info.DayLog = "";
                        info.UserAwardLog = 0;
                        info.LastDate = DateTime.Now;
                    }
                    string dayLog = info.DayLog;
                    int userAwardLog = info.UserAwardLog;
                    DateTime lastDate = info.LastDate;
                    int countday = dayLog.Split(',').Length;
                    int currentMonth = DateTime.Now.Month;
                    int curentYear = DateTime.Now.Year;
                    int curentDay = DateTime.Now.Day;
                    int dayofmonth = DateTime.DaysInMonth(curentYear, currentMonth);

                    if (currentMonth != lastDate.Month || curentYear != lastDate.Year)
                    {
                        dayLog = "";
                        userAwardLog = 0;
                        lastDate = DateTime.Now;
                    }
                    if (countday < dayofmonth)
                    {
                        if (string.IsNullOrEmpty(dayLog) && countday > 1)
                        {
                            dayLog = "False";
                        }
                        for (int i = countday; i < curentDay - 1; i++)
                        {
                            dayLog += ",False";
                        }

                    }

                    info.DayLog = dayLog;
                    info.UserAwardLog = userAwardLog;
                    info.LastDate = lastDate;
                    db.UpdateDailyLogList(info);

                    XElement node = new XElement("DailyLogList",
                        new XAttribute("UserAwardLog", userAwardLog),
                        new XAttribute("DayLog", dayLog),
                        new XAttribute("luckyNum", 0),
                        new XAttribute("myLuckyNum", 0));
                    result.Add(node);

                }

                value = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                log.Error("dailyloglist", ex);
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            result.Add(new XAttribute("nowDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));//nowDate="2012-08-16 16:24:17"
            context.Response.ContentType = "text/plain";
            //context.Response.Write(result.ToString(false));
            context.Response.BinaryWrite(StaticFunction.Compress(result.ToString(false)));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}