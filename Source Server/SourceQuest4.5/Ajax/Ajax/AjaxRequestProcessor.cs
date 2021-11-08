using Ajax.JSON;
using MS.Utilities;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Ajax
{
	internal class AjaxRequestProcessor
	{
		private HttpContext context = null;

		internal AjaxRequestProcessor(HttpContext context)
		{
			this.context = context;
		}

		public void Run()
		{
			if (context.Trace.IsEnabled)
			{
				context.Trace.Write("Ajax.NET", "Begin ProcessRequest");
			}
			DateTime now = DateTime.Now;
			string text = Path.GetFileNameWithoutExtension(context.Request.FilePath);
			if (Utility.Settings != null && Utility.Settings.UrlNamespaceMappings.Contains(text))
			{
				text = Utility.Settings.UrlNamespaceMappings[text].ToString();
			}
			AjaxProcessor ajaxProcessor = new AjaxProcessor();
			ajaxProcessor.InitializeContext(ref context);
			if (text.ToLower() == "common")
			{
				if (context.Trace.IsEnabled)
				{
					context.Trace.Write("Ajax.NET", "Render common Javascript");
				}
				context.Response.Expires = 1;
				context.Response.ContentType = "text/plain";
				ajaxProcessor.RenderCommonScript();
				if (context.Trace.IsEnabled)
				{
					context.Trace.Write("Ajax.NET", "End ProcessRequest");
				}
				return;
			}
			context.Response.Expires = 0;
			context.Response.AddHeader("cache-control", "no-cache");
			MethodInfo[] array = null;
			string text2 = null;
			object[] array2 = new object[0];
			object[] array3 = array2;
			Type type = Type.GetType(text);
			array = type.GetMethods();
			text2 = ajaxProcessor.GetMethodName();
			string text3 = null;
			StreamReader sr = null;
			byte[] array4 = new byte[context.Request.ContentLength];
			if (context.Request.HttpMethod == "POST" && context.Request.InputStream.Read(array4, 0, array4.Length) >= 0)
			{
				text3 = MD5Helper.GetHash(array4);
				sr = new StreamReader(new MemoryStream(array4), Encoding.UTF8);
				if (context.Cache[type.FullName + "|" + text3] != null)
				{
					context.Response.Write(context.Cache[type.FullName + "|" + text3].ToString());
					if (context.Trace.IsEnabled)
					{
						context.Trace.Write("Ajax.NET", "End ProcessRequest");
					}
					return;
				}
			}
			if (text2 != null)
			{
				MethodInfo method = type.GetMethod(text2);
				if ((object)method != null)
				{
					object[] customAttributes = method.GetCustomAttributes(typeof(AjaxMethodAttribute), inherit: true);
					if (customAttributes.Length != 1)
					{
						ajaxProcessor.HandleException(new NotImplementedException("The method '" + context.Request["m"] + "' is not implemented or access refused."), null);
						if (context.Trace.IsEnabled)
						{
							context.Trace.Write("Ajax.NET", "End ProcessRequest");
						}
						return;
					}
					ParameterInfo[] parameters = method.GetParameters();
					array3 = new object[parameters.Length];
					try
					{
						ajaxProcessor.RetreiveParameters(ref sr, parameters, ref array3);
					}
					catch (Exception ex)
					{
						ajaxProcessor.HandleException(ex, "Could not retreive parameters from HTTP request.");
					}
					object obj = null;
					try
					{
						if (context.Trace.IsEnabled)
						{
							context.Trace.Write("Ajax.NET", "Invoking " + type.FullName + "." + method.Name);
						}
						if (method.IsStatic)
						{
							try
							{
								obj = type.InvokeMember(text2, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, array3);
							}
							catch (Exception ex2)
							{
								if (ex2.InnerException != null)
								{
									ajaxProcessor.HandleException(ex2.InnerException, null);
								}
								else
								{
									ajaxProcessor.HandleException(ex2, null);
								}
								if (context.Trace.IsEnabled)
								{
									context.Trace.Write("Ajax.NET", "End ProcessRequest");
								}
								return;
							}
						}
						else
						{
							try
							{
								Type type2 = Type.GetType(text);
								array2 = new object[0];
								object obj2 = Activator.CreateInstance(type2, array2);
								if (obj2 != null)
								{
									obj = method.Invoke(obj2, array3);
								}
							}
							catch (Exception ex3)
							{
								if (ex3.InnerException != null)
								{
									ajaxProcessor.HandleException(ex3.InnerException, null);
								}
								else
								{
									ajaxProcessor.HandleException(ex3, null);
								}
								if (context.Trace.IsEnabled)
								{
									context.Trace.Write("Ajax.NET", "End ProcessRequest");
								}
								return;
							}
						}
					}
					catch (Exception ex4)
					{
						if (ex4.InnerException != null)
						{
							ajaxProcessor.HandleException(ex4.InnerException, null);
						}
						else
						{
							ajaxProcessor.HandleException(ex4, null);
						}
						if (context.Trace.IsEnabled)
						{
							context.Trace.Write("Ajax.NET", "End ProcessRequest");
						}
						return;
					}
					if (!Utility.ConverterRegistered)
					{
						Utility.RegisterConverterForAjax(null);
					}
					try
					{
						if (obj != null && (object)obj.GetType() == typeof(XmlDocument))
						{
							context.Response.ContentType = "text/xml";
							((XmlDocument)obj).Save(context.Response.OutputStream);
							if (context.Trace.IsEnabled)
							{
								context.Trace.Write("Ajax.NET", "End ProcessRequest");
							}
						}
						else
						{
							StringBuilder sb = new StringBuilder();
							try
							{
								DefaultConverter.ToJSON(ref sb, obj);
							}
							catch (StackOverflowException ex5)
							{
								ajaxProcessor.HandleException(ex5, "The class you are returning is not supported.");
								return;
							}
							catch (Exception ex6)
							{
								ajaxProcessor.HandleException(ex6, "AjaxRequestProcessor throw exception while running ToJSON");
								return;
							}
							if (context.Request["_return"] != null && context.Request["_return"] == "xml")
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.LoadXml("<Ajax/>");
								xmlDocument.DocumentElement.InnerText = sb.ToString();
								xmlDocument.Save(context.Response.OutputStream);
							}
							else
							{
								if (text3 != null)
								{
									object[] customAttributes2 = method.GetCustomAttributes(typeof(AjaxMethodAttribute), inherit: true);
									if (customAttributes2.Length != 0)
									{
										AjaxMethodAttribute ajaxMethodAttribute = (AjaxMethodAttribute)customAttributes2[0];
										if (ajaxMethodAttribute.IsCacheEnabled)
										{
											context.Cache.Add(type.FullName + "|" + text3, sb.ToString(), null, now.AddSeconds(ajaxMethodAttribute.CacheDuration.TotalSeconds), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
										}
									}
								}
								if (context.Trace.IsEnabled)
								{
									context.Trace.Write("Ajax.NET", "JSON string: " + sb.ToString());
								}
								context.Response.Write(sb.ToString());
							}
							if (context.Trace.IsEnabled)
							{
								context.Trace.Write("Ajax.NET", "End ProcessRequest");
							}
						}
					}
					catch (Exception ex7)
					{
						ajaxProcessor.HandleException(ex7, "Error while converting object to JSON.");
						if (context.Trace.IsEnabled)
						{
							context.Trace.Write("Ajax.NET", "End ProcessRequest");
						}
					}
				}
				else
				{
					ajaxProcessor.HandleException(new NotImplementedException("The method '" + context.Request["m"] + "' is not implemented or access refused."), null);
					if (context.Trace.IsEnabled)
					{
						context.Trace.Write("Ajax.NET", "End ProcessRequest");
					}
				}
			}
			else
			{
				if (context.Trace.IsEnabled)
				{
					context.Trace.Write("Ajax.NET", "Render class proxy Javascript");
				}
				context.Response.ContentType = "text/plain";
				ajaxProcessor.RenderClientScript(array, type);
				StringBuilder sb2 = new StringBuilder();
				StringCollection stringCollection = new StringCollection();
				if (Utility.AjaxConverters != null)
				{
					foreach (Type key in Utility.AjaxConverters.Keys)
					{
						IAjaxObjectConverter ajaxObjectConverter = (IAjaxObjectConverter)Utility.AjaxConverters[key];
						if (ajaxObjectConverter.ClientScriptIdentifier == null || !stringCollection.Contains(ajaxObjectConverter.ClientScriptIdentifier))
						{
							ajaxObjectConverter.RenderClientScript(ref sb2);
							stringCollection.Add(ajaxObjectConverter.ClientScriptIdentifier);
						}
					}
				}
				if (sb2.Length > 0)
				{
					context.Response.Write(sb2.ToString());
				}
				if (context.Trace.IsEnabled)
				{
					context.Trace.Write("Ajax.NET", "End ProcessRequest");
				}
			}
		}
	}
}
