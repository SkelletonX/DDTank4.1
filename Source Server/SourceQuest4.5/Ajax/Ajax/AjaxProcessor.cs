using Ajax.JSON;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Ajax
{
	internal sealed class AjaxProcessor
	{
		private HttpContext context = null;

		internal void Write(string text)
		{
			if (context == null)
			{
				throw new NullReferenceException("The context is not set correct in ProcessRequest().");
			}
			context.Response.Write(text);
		}

		internal void WriteLine(string line)
		{
			Write(line);
			Write("\r\n");
		}

		internal void InitializeContext(ref HttpContext context)
		{
			this.context = context;
		}

		internal string GetMethodName()
		{
			if (context != null && context.Request["_method"] != null)
			{
				return context.Request["_method"];
			}
			return null;
		}

		internal void RetreiveParameters(ref StreamReader sr, ParameterInfo[] para, ref object[] po)
		{
			for (int i = 0; i < para.Length; i++)
			{
				po[i] = para[i].DefaultValue;
			}
			if (context.Request["_return"] != null && context.Request["_return"] == "xml")
			{
				for (int j = 0; j < para.Length; j++)
				{
					if (context.Request[para[j].Name] != null)
					{
						po[j] = DefaultConverter.FromString(HttpUtility.UrlDecode(context.Request[para[j].Name]), para[j].ParameterType);
					}
				}
				return;
			}
			Hashtable hashtable = new Hashtable();
			try
			{
				string text = sr.ReadLine();
				string text2 = null;
				while (text != null)
				{
					text = text.Replace("%26", "%").Replace("%3D", "=");
					if (text.IndexOf("=") > 0)
					{
						string text3 = text.Substring(0, text.IndexOf("="));
						string value = text.Substring(text.IndexOf("=") + 1);
						hashtable.Add(text3, value);
						text2 = text3;
					}
					else if (text2 != null)
					{
						hashtable[text2] = string.Concat(hashtable[text2], "\r\n", text);
					}
					text = sr.ReadLine();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				sr.Close();
			}
			for (int k = 0; k < para.Length; k++)
			{
				if (hashtable[para[k].Name] != null)
				{
					po[k] = DefaultConverter.FromString(hashtable[para[k].Name].ToString(), para[k].ParameterType);
				}
			}
		}

		internal void RenderCommonScript()
		{
			string str = "ajax.js";
			if (context.Request.UserAgent.IndexOf("Windows CE; PPC;") >= 0)
			{
				str = "ajax_mobile.js";
			}
			StreamReader streamReader = new StreamReader(Assembly.GetCallingAssembly().GetManifestResourceStream("Ajax." + str));
			Write(streamReader.ReadToEnd());
			streamReader.Close();
			Write("var ajaxVersion = '5.6.3.4'\r\n");
		}

		public void RenderClientScript(MethodInfo[] mi, Type type)
		{
			if (context.Cache[type.AssemblyQualifiedName] != null)
			{
				WriteLine("\r\n// cached javascript");
				Write((string)context.Cache[type.AssemblyQualifiedName]);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("var " + type.Name + " = {\r\n");
			foreach (MethodInfo methodInfo in mi)
			{
				if (methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), inherit: true).Length == 0)
				{
					continue;
				}
				object[] customAttributes = methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), inherit: true);
				AjaxMethodAttribute ajaxMethodAttribute = (AjaxMethodAttribute)customAttributes[0];
				ParameterInfo[] parameters = methodInfo.GetParameters();
				stringBuilder.Append(((ajaxMethodAttribute.MethodName != null) ? ajaxMethodAttribute.MethodName.Replace(" ", "_") : methodInfo.Name) + ":function(");
				for (int j = 0; j < parameters.Length; j++)
				{
					stringBuilder.Append(parameters[j].Name);
					stringBuilder.Append(",");
				}
				stringBuilder.Append("callback,context)");
				stringBuilder.Append("{");
				stringBuilder.Append("return new ajax_request(");
				if (ajaxMethodAttribute.HttpConnectionProtocol == HttpConnectionProtocolType.HTTP)
				{
					stringBuilder.Append("'http://" + context.Request.ServerVariables["SERVER_NAME"] + "' + ");
				}
				else if (ajaxMethodAttribute.HttpConnectionProtocol == HttpConnectionProtocolType.HTTPS)
				{
					stringBuilder.Append("'https://" + context.Request.ServerVariables["SERVER_NAME"] + "' + ");
				}
				stringBuilder.Append("this.url + '?_method=" + methodInfo.Name);
				if (ajaxMethodAttribute.RequireSessionState == HttpSessionStateRequirement.ReadWrite)
				{
					stringBuilder.Append("&_session=rw");
				}
				else if (ajaxMethodAttribute.RequireSessionState == HttpSessionStateRequirement.Read)
				{
					stringBuilder.Append("&_session=r");
				}
				else
				{
					stringBuilder.Append("&_session=no");
				}
				stringBuilder.Append("','");
				for (int k = 0; k < parameters.Length; k++)
				{
					if ((object)parameters[k].ParameterType == typeof(string[]))
					{
						stringBuilder.Append(parameters[k].Name + "=' + json_from_object(" + parameters[k].Name + ")");
					}
					else
					{
						stringBuilder.Append(parameters[k].Name + "=' + enc(" + parameters[k].Name + ")");
					}
					if (k < parameters.Length - 1)
					{
						stringBuilder.Append("+ '\\r\\n");
					}
				}
				if (parameters.Length == 0)
				{
					stringBuilder.Append("'");
				}
				stringBuilder.Append(",callback, context);");
				stringBuilder.Append("},\r\n");
			}
			string text = type.FullName + "," + type.Assembly.FullName.Substring(0, type.Assembly.FullName.IndexOf(","));
			if (Utility.Settings != null && Utility.Settings.UrlNamespaceMappings.ContainsValue(text))
			{
				foreach (string key in Utility.Settings.UrlNamespaceMappings.Keys)
				{
					if (Utility.Settings.UrlNamespaceMappings[key].ToString() == text)
					{
						text = key;
						break;
					}
				}
			}
			stringBuilder.Append("url:'" + context.Request.ApplicationPath + (context.Request.ApplicationPath.EndsWith("/") ? "" : "/") + Utility.HandlerPath + "/" + ((context.Session != null && context.Session.IsCookieless) ? ("(" + context.Session.SessionID + ")/") : "") + text + Utility.HandlerExtension + "'\r\n");
			stringBuilder.Append("}\r\n");
			CacheDependency dependencies = new CacheDependency(type.Assembly.Location);
			context.Cache.Add(type.AssemblyQualifiedName, stringBuilder.ToString(), dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
			Write(stringBuilder.ToString());
		}

		internal void HandleException(Exception ex, string message)
		{
			context.Trace.Warn("Ajax.NET", "HandleException", ex);
			string text = "new Object();r.error = new ajax_error('" + ((object)ex).GetType().FullName + "','" + ex.Message.Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n") + ((message != null) ? ("\\r\\n" + message) : "") + "',0)";
			if (context.Request["_return"] != null && context.Request["_return"] == "xml")
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml("<Ajax/>");
				xmlDocument.DocumentElement.InnerText = text;
				xmlDocument.Save(context.Response.OutputStream);
			}
			else
			{
				context.Response.Write(text);
			}
		}
	}
}
