using System;
using System.Web;

namespace MS.Web
{
	internal class PageHandler : IHttpHandler
	{
		private Request m_Request;

		private string m_Assembly;

		private string m_HandlerUri;

		public Request Request
		{
			get
			{
				return m_Request;
			}
			set
			{
				m_Request = value;
			}
		}

		public string Assembly
		{
			get
			{
				return m_Assembly;
			}
			set
			{
				m_Assembly = value;
			}
		}

		public string HandlerUri
		{
			get
			{
				return m_HandlerUri;
			}
			set
			{
				m_HandlerUri = value;
			}
		}

		public virtual bool IsReusable => true;

		internal PageHandler()
		{
		}

		public virtual void ProcessRequest(HttpContext context)
		{
		}

		public void Write(string text)
		{
			if (HttpContext.Current == null)
			{
				throw new NullReferenceException("The context is not set correct in ProcessRequest().");
			}
			HttpContext.Current.Response.Write(text);
		}

		public void WriteLine(string line)
		{
			Write(line);
			Write("\r\n");
		}
	}
}
