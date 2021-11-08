using Ajax.JSON;
using Ajax.JSON.HtmlControls;
using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Ajax
{
	public sealed class Utility
	{
		private static AjaxSettings m_Settings = null;

		public static string HandlerExtension = ".ashx";

		public static string HandlerPath = "ajax";

		internal static bool ConverterRegistered = false;

		internal static Hashtable AjaxConverters = null;

		internal static AjaxSettings Settings
		{
			get
			{
				if (m_Settings != null)
				{
					return m_Settings;
				}
				try
				{
					m_Settings = (AjaxSettings)ConfigurationSettings.GetConfig("ajaxNet/ajaxSettings");
					return m_Settings;
				}
				catch
				{
					m_Settings = new AjaxSettings();
				}
				return null;
			}
		}

		internal static void RegisterCommonAjax()
		{
			if (!ConverterRegistered)
			{
				RegisterConverterForAjax(null);
			}
			if (Settings != null && Settings.CommonScript != null)
			{
				((Page)HttpContext.Current.Handler).RegisterClientScriptBlock("common", "<script type=\"text/" + Settings.ScriptLanguage + "\" src=\"" + Path.GetDirectoryName(Settings.CommonScript) + "/" + ((HttpContext.Current.Session != null && HttpContext.Current.Session.IsCookieless) ? ("(" + HttpContext.Current.Session.SessionID + ")/") : "") + Path.GetFileName(Settings.CommonScript) + "\"></script>");
			}
			else
			{
				((Page)HttpContext.Current.Handler).RegisterClientScriptBlock("common", "<script type=\"text/javascript\" src=\"" + HttpContext.Current.Request.ApplicationPath + (HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? "" : "/") + HandlerPath + "/" + ((HttpContext.Current.Session != null && HttpContext.Current.Session.IsCookieless) ? ("(" + HttpContext.Current.Session.SessionID + ")/") : "") + "common" + HandlerExtension + "\"></script>");
			}
		}

		public static void RegisterTypeForAjax(Type t)
		{
			RegisterCommonAjax();
			Page page = (Page)HttpContext.Current.Handler;
			string text = t.FullName + "," + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(","));
			if (Settings != null && Settings.UrlNamespaceMappings.ContainsValue(text))
			{
				foreach (string key in Settings.UrlNamespaceMappings.Keys)
				{
					if (Settings.UrlNamespaceMappings[key].ToString() == text)
					{
						text = key;
						break;
					}
				}
			}
			page.RegisterClientScriptBlock(t.FullName, "<script type=\"text/javascript\" src=\"" + HttpContext.Current.Request.ApplicationPath + (HttpContext.Current.Request.ApplicationPath.EndsWith("/") ? "" : "/") + HandlerPath + "/" + ((HttpContext.Current.Session != null && HttpContext.Current.Session.IsCookieless) ? ("(" + HttpContext.Current.Session.SessionID + ")/") : "") + text + HandlerExtension + "\"></script>");
		}

		public static void RegisterConverterForAjax(IAjaxObjectConverter converter)
		{
			RegisterConverterForAjax(converter, overrideExisting: true);
		}

		internal static void RegisterConverterForAjax(IAjaxObjectConverter converter, bool overrideExisting)
		{
			if (AjaxConverters == null || converter == null)
			{
				HttpContext.Current.Trace.Write("Ajax.NET", "Begin Register Converter for Ajax.NET");
				AjaxConverters = new Hashtable();
				RegisterConverterForAjax(new ImageConverter());
				RegisterConverterForAjax(new DataRowConverter());
				RegisterConverterForAjax(new DataSetConverter());
				RegisterConverterForAjax(new DataTableConverter());
				RegisterConverterForAjax(new DateTimeConverter());
				RegisterConverterForAjax(new TimeSpanConverter());
				RegisterConverterForAjax(new ArrayListConverter());
				RegisterConverterForAjax(new ICollectionConverter());
				RegisterConverterForAjax(new HtmlAnchorConverter());
				RegisterConverterForAjax(new HtmlButtonConverter());
				RegisterConverterForAjax(new HtmlImageConverter());
				RegisterConverterForAjax(new HtmlInputButtonConverter());
				RegisterConverterForAjax(new HtmlInputCheckBoxConverter());
				RegisterConverterForAjax(new HtmlInputRadioButtonConverter());
				RegisterConverterForAjax(new HtmlInputTextConverter());
				RegisterConverterForAjax(new HtmlSelectConverter());
				RegisterConverterForAjax(new HtmlTableCellConverter());
				RegisterConverterForAjax(new HtmlTableConverter());
				RegisterConverterForAjax(new HtmlTableRowConverter());
				RegisterConverterForAjax(new HtmlTextAreaConverter());
				RegisterConverterForAjax(new HtmlControlConverter());
				AjaxConverterConfiguration ajaxConverterConfiguration = null;
				try
				{
					ajaxConverterConfiguration = (AjaxConverterConfiguration)ConfigurationSettings.GetConfig("ajaxNet/ajaxConverters");
				}
				catch
				{
				}
				if (ajaxConverterConfiguration != null)
				{
					foreach (AjaxConverterItem item in ajaxConverterConfiguration)
					{
						if (item.Action == AjaxConverterConfigurationAction.Add)
						{
							IAjaxObjectConverter ajaxObjectConverter = null;
							try
							{
								Type converterType = item.ConverterType;
								object[] args = new object[0];
								object obj2 = Activator.CreateInstance(converterType, args);
								ajaxObjectConverter = (IAjaxObjectConverter)obj2;
							}
							catch
							{
								ajaxObjectConverter = null;
							}
							if (ajaxObjectConverter != null)
							{
								RegisterConverterForAjax(ajaxObjectConverter);
							}
						}
						else if (AjaxConverters.Contains(item.ConverterType))
						{
							AjaxConverters.Remove(item.ConverterType);
						}
					}
				}
				HttpContext.Current.Trace.Write("Ajax.NET", "End Register Converter for Ajax.NET");
			}
			if (converter != null)
			{
				AjaxConverters.Contains(converter.GetType());
				if (AjaxConverters.Contains(converter.GetType()))
				{
					if (overrideExisting)
					{
						AjaxConverters[converter.GetType()] = converter;
					}
				}
				else
				{
					AjaxConverters.Add(converter.GetType(), converter);
				}
			}
			ConverterRegistered = true;
		}
	}
}
