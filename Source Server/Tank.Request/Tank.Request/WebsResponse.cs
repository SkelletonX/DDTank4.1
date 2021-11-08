using System;
using System.IO;
using System.Net;
using System.Text;

namespace Tank.Request
{
	public class WebsResponse
	{
		public static string GetPage(string url, string postData, string encodeType, out string err)
		{
			Encoding encoding = Encoding.GetEncoding(encodeType);
			byte[] bytes = encoding.GetBytes(postData);
			try
			{
				HttpWebRequest obj = WebRequest.Create(url) as HttpWebRequest;
				obj.CookieContainer = new CookieContainer();
				obj.AllowAutoRedirect = true;
				obj.Method = "POST";
				obj.ContentType = "application/x-www-form-urlencoded";
				obj.ContentLength = bytes.Length;
				Stream requestStream = obj.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				string result = new StreamReader((obj.GetResponse() as HttpWebResponse).GetResponseStream(), encoding).ReadToEnd();
				err = string.Empty;
				return result;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				return string.Empty;
			}
		}
	}
}
