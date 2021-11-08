// Decompiled with JetBrains decompiler
// Type: Bussiness.Interface.BaseInterface
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using Bussiness.CenterService;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Security;

namespace Bussiness.Interface
{
  public abstract class BaseInterface
  {
    protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public static string GetInterName
    {
      get
      {
        return ConfigurationManager.AppSettings["InterName"].ToLower();
      }
    }

    public static string GetLoginKey
    {
      get
      {
        return ConfigurationManager.AppSettings["LoginKey"];
      }
    }

    public static string GetChargeKey
    {
      get
      {
        return ConfigurationManager.AppSettings["ChargeKey"];
      }
    }

    public static string LoginUrl
    {
      get
      {
        return ConfigurationManager.AppSettings[nameof (LoginUrl)];
      }
    }

    public virtual int ActiveGold
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings["DefaultGold"]);
      }
    }

    public virtual int ActiveMoney
    {
      get
      {
        return int.Parse(ConfigurationManager.AppSettings["DefaultMoney"]);
      }
    }

    public static string GetNameBySite(string user, string site)
    {
      if (!string.IsNullOrEmpty(site) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings[string.Format("LoginKey_{0}", (object) site)]))
        user = string.Format("{0}_{1}", (object) site, (object) user);
      return user;
    }

    public static DateTime ConvertIntDateTime(double d)
    {
      return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(d);
    }

    public static int ConvertDateTimeInt(DateTime time)
    {
      DateTime localTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
      return (int) (time - localTime).TotalSeconds;
    }

    public static string md5(string str)
    {
      return FormsAuthentication.HashPasswordForStoringInConfigFile(str, nameof (md5)).ToLower();
    }

    public static string RequestContent(string Url)
    {
      return BaseInterface.RequestContent(Url, 2560);
    }

    public static string RequestContent(string Url, int byteLength)
    {
      byte[] numArray = new byte[byteLength];
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
      httpWebRequest.ContentType = "text/plain";
      Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
      int count = responseStream.Read(numArray, 0, numArray.Length);
      string str = Encoding.UTF8.GetString(numArray, 0, count);
      responseStream.Close();
      return str;
    }

    public static string RequestContent(string Url, string param, string code)
    {
      Encoding encoding = Encoding.GetEncoding(code);
      byte[] bytes = encoding.GetBytes(param);
      encoding.GetString(bytes);
      byte[] numArray = new byte[2560];
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(Url);
      httpWebRequest.ServicePoint.Expect100Continue = false;
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/x-www-form-urlencoded";
      httpWebRequest.ContentLength = (long) bytes.Length;
      using (Stream requestStream = httpWebRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
      using (WebResponse response = httpWebRequest.GetResponse())
      {
        int count = response.GetResponseStream().Read(numArray, 0, numArray.Length);
        return Encoding.UTF8.GetString(numArray, 0, count);
      }
    }

    public static BaseInterface CreateInterface()
    {
      string getInterName = BaseInterface.GetInterName;
      if (getInterName == "qunying")
        return (BaseInterface) new QYInterface();
      if (getInterName == "sevenroad")
        return (BaseInterface) new SRInterface();
      if (!(getInterName == "duowan"))
        return (BaseInterface) null;
      return (BaseInterface) new DWInterface();
    }

    public virtual PlayerInfo CreateLogin(
      string name,
      string password,
      int zoneId,
      ref string message,
      ref int isFirst,
      string IP,
      ref bool isError,
      bool firstValidate,
      ref bool isActive,
      string site,
      string nickname)
    {
      try
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          bool isExist = true;
          DateTime now = DateTime.Now;
          PlayerInfo player = playerBussiness.LoginGame(name, ref isFirst, ref isExist, ref isError, firstValidate, ref now, nickname);
          if (player == null)
          {
            if (!playerBussiness.ActivePlayer(ref player, name, password, true, this.ActiveGold, this.ActiveMoney, IP, site))
            {
              player = (PlayerInfo) null;
              message = LanguageMgr.GetTranslation("BaseInterface.LoginAndUpdate.Fail");
            }
            else
            {
              isActive = true;
              using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                centerServiceClient.ActivePlayer(true);
            }
          }
          else if (isExist)
          {
            using (CenterServiceClient centerServiceClient = new CenterServiceClient())
              centerServiceClient.CreatePlayer(player.ID, name, password, isFirst == 0);
          }
          else
          {
            message = LanguageMgr.GetTranslation("ManageBussiness.Forbid1", (object) now.Year, (object) now.Month, (object) now.Day, (object) now.Hour, (object) now.Minute);
            return (PlayerInfo) null;
          }
          return player;
        }
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) "LoginAndUpdate", ex);
      }
      return (PlayerInfo) null;
    }

    public virtual PlayerInfo LoginGame(string name, string pass, ref bool isFirst)
    {
      try
      {
        using (CenterServiceClient centerServiceClient = new CenterServiceClient())
        {
          int userID = 0;
          if (centerServiceClient.ValidateLoginAndGetID(name, pass, ref userID, ref isFirst))
            return new PlayerInfo()
            {
              ID = userID,
              UserName = name
            };
        }
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) nameof (LoginGame), ex);
      }
      return (PlayerInfo) null;
    }

    public virtual string[] UnEncryptLogin(string content, ref int result, string site)
    {
      try
      {
        string str = string.Empty;
        if (!string.IsNullOrEmpty(site))
          str = ConfigurationManager.AppSettings[string.Format("LoginKey_{0}", (object) site)];
        if (string.IsNullOrEmpty(str))
          str = BaseInterface.GetLoginKey;
        if (!string.IsNullOrEmpty(str))
        {
          string[] strArray = content.Split('|');
          if (strArray.Length > 3)
          {
            if (BaseInterface.md5(strArray[0] + strArray[1] + strArray[2] + str) == strArray[3].ToLower())
              return strArray;
            result = 5;
          }
          else
            result = 2;
        }
        else
          result = 4;
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) nameof (UnEncryptLogin), ex);
      }
      return new string[0];
    }

    public virtual string[] UnEncryptCharge(string content, ref int result, string site)
    {
      try
      {
        string str1 = string.Empty;
        if (!string.IsNullOrEmpty(site))
          str1 = ConfigurationManager.AppSettings[string.Format("ChargeKey_{0}", (object) site)];
        if (string.IsNullOrEmpty(str1))
          str1 = BaseInterface.GetChargeKey;
        if (!string.IsNullOrEmpty(str1))
        {
          string[] strArray = content.Split('|');
          string str2 = BaseInterface.md5(strArray[0] + strArray[1] + strArray[2] + strArray[3] + strArray[4] + str1);
          if (strArray.Length > 5)
          {
            if (str2 == strArray[5].ToLower())
              return strArray;
            result = 7;
          }
          else
            result = 8;
        }
        else
          result = 6;
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) nameof (UnEncryptCharge), ex);
      }
      return new string[0];
    }

    public virtual string[] UnEncryptSentReward(string content, ref int result, string key)
    {
      try
      {
        string[] strArray = content.Split('#');
        if (strArray.Length == 8)
        {
          string appSetting = ConfigurationManager.AppSettings["SentRewardTimeSpan"];
          int num = int.Parse(string.IsNullOrEmpty(appSetting) ? "1" : appSetting);
          TimeSpan timeSpan = string.IsNullOrEmpty(strArray[6]) ? new TimeSpan(1, 1, 1) : DateTime.Now - BaseInterface.ConvertIntDateTime(double.Parse(strArray[6]));
          if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes < num)
          {
            if (string.IsNullOrEmpty(key))
              return strArray;
            if (BaseInterface.md5(strArray[2] + strArray[3] + strArray[4] + strArray[5] + strArray[6] + key) == strArray[7].ToLower())
              return strArray;
            result = 5;
          }
          else
            result = 7;
        }
        else
          result = 6;
      }
      catch (Exception ex)
      {
        BaseInterface.log.Error((object) nameof (UnEncryptSentReward), ex);
      }
      return new string[0];
    }

    public virtual bool GetUserSex(string name)
    {
      return true;
    }

    public static bool CheckRnd(string str)
    {
      return !string.IsNullOrEmpty(str);
    }
  }
}
