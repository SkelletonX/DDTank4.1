using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using SqlDataProvider.Data;
using Bussiness;
using log4net;
using System.Reflection;
using System.Configuration;
using Tank.Request.Illegalcharacters;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class NickNameCheck : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static FileSystem fileIllegal = new FileSystem(HttpContext.Current.Server.MapPath(IllegalCharacters), HttpContext.Current.Server.MapPath(IllegalDirectory), "*.txt");

        public static string IllegalCharacters
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["IllegalCharacters"];
            }
        }

        public static string IllegalDirectory
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["IllegalDirectory"];
            }
        }

        private static string CharacterAllow = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ.-_";

        private bool CheckCharacterAllow(string text)
        {
            bool isAllow = true;
            foreach(char value in text.ToCharArray())
            {
                if(!CharacterAllow.ToCharArray().Contains(value))
                {
                    isAllow = false;
                    break;
                }
            }

            return isAllow;
        }

        public void ProcessRequest(HttpContext context)
        {
            LanguageMgr.Setup(ConfigurationManager.AppSettings["ReqPath"]);
            bool value = false;
            //string message = "Name is Exist!";
            string message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Exist");
            XElement result = new XElement("Result");

            try
            {
                string nickName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
                if (System.Text.Encoding.Default.GetByteCount(nickName) <= 14)
                {
                    if (!fileIllegal.checkIllegalChar(nickName))
                    {
                        // scan name have special character
                        //if(CheckCharacterAllow(nickName))
                        //{
                            if (!string.IsNullOrEmpty(nickName))
                            {
                                using (PlayerBussiness db = new PlayerBussiness())
                                {
                                    if (db.GetUserSingleByNickName(nickName) == null)
                                    {
                                        value = true;
                                        //message = "You can register!";
                                        message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Right");
                                    }
                                }
                            }
                        //}
                        //else
                        //{
                            //message = "Tên chỉ được phép chứa Tiếng Việt và số";
                        //}
                    }
                    else
                    {
                        message = LanguageMgr.GetTranslation("Tank.Request.VisualizeRegister.Illegalcharacters");
                    }
                }
                else
                {
                    message = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Long");
                }
            }
            catch(Exception ex)
            {
                log.Error("NickNameCheck", ex);
                value = false;
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(result.ToString(false));
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
