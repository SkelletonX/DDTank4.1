using System.IO;
using System.Web;

namespace MS.Web
{
	internal class Request
	{
		private string m_RequestType;

		private string m_URL;

		private HttpContext context;

		public string Extension => System.IO.Path.GetExtension(m_URL).ToLower();

		public string FileName => System.IO.Path.GetFileName(m_URL).ToLower();

		public string FileNameWithoutExtension => System.IO.Path.GetFileNameWithoutExtension(m_URL).ToLower();

		public string ApplicationPath
		{
			get
			{
				string applicationPath = context.Request.ApplicationPath;
				if (applicationPath.Equals("/"))
				{
					return "";
				}
				return applicationPath;
			}
		}

		public string FilePath => context.Request.FilePath;

		public string FilePathWithoutExtension => FilePath.Substring(0, FilePath.Length - Extension.Length);

		public string VirtualFilePath => FilePathWithoutExtension.Substring(ApplicationPath.Length);

		public string PhysicalApplicationPath => context.Request.PhysicalApplicationPath;

		public string PhysicalPath => context.Request.PhysicalPath;

		public string PhysicalPathWithoutExtension => PhysicalPath.Substring(0, PhysicalPath.Length - Extension.Length);

		public string Path
		{
			get
			{
				string text = FilePath.Substring(ApplicationPath.Length, FilePath.Length - ApplicationPath.Length - FileName.Length);
				return text.Trim('/');
			}
		}

		public Request(HttpContext context, string requestType, string url)
		{
			this.context = context;
			m_RequestType = requestType;
			m_URL = url;
		}
	}
}
