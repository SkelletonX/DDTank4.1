using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;

namespace Ajax.JSON
{
	internal sealed class ImageConverter : IAjaxObjectConverter
	{
		public string ClientScriptIdentifier => "AjaxImage";

		public Type[] SupportedTypes => new Type[1]
		{
			typeof(Bitmap)
		};

		public bool IncludeSubclasses => true;

		public void RenderClientScript(ref StringBuilder sb)
		{
			sb.Append("function AjaxImage(url){var img=new Image();img.src=url;return img;}\r\n");
		}

		public object FromString(string s, Type t)
		{
			throw new NotImplementedException();
		}

		public void ToJSON(ref StringBuilder sb, object o)
		{
			if ((object)o.GetType() == typeof(Bitmap))
			{
				try
				{
					string text = HttpContext.Current.Server.MapPath(Utility.Settings.TemporaryFiles.Path);
					if (!Directory.Exists(text))
					{
						sb.Append("null");
					}
					else
					{
						string text2 = Guid.NewGuid().ToString() + ".jpg";
						Bitmap bitmap = (Bitmap)o;
						bitmap.Save(Path.Combine(text, text2), ImageFormat.Jpeg);
						if (Utility.Settings.TemporaryFiles.Path.StartsWith("~/") || Utility.Settings.TemporaryFiles.Path.StartsWith("~\\"))
						{
							sb.Append("new AjaxImage('" + HttpContext.Current.Request.ApplicationPath + "/" + Utility.Settings.TemporaryFiles.Path.Substring(1).Replace("\\", "/"));
						}
						else
						{
							sb.Append("new AjaxImage('" + Utility.Settings.TemporaryFiles.Path.Replace("\\", "/"));
						}
						if (!Utility.Settings.TemporaryFiles.Path.EndsWith("\\") && !Utility.Settings.TemporaryFiles.Path.EndsWith("/"))
						{
							sb.Append("/");
						}
						sb.Append(text2 + "');");
						try
						{
							DateTime t = DateTime.Now.AddMinutes(-1 * Utility.Settings.TemporaryFiles.DeleteAfter);
							string[] files = Directory.GetFiles(text, "*.jpg");
							foreach (string text3 in files)
							{
								FileInfo fileInfo = new FileInfo(text3);
								if (fileInfo.CreationTime < t)
								{
									File.Delete(text3);
								}
							}
						}
						catch (Exception)
						{
						}
					}
				}
				catch (Exception)
				{
					sb.Append("null");
				}
			}
		}
	}
}
