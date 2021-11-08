using System;
using System.Web.Caching;

namespace Ajax
{
	[AttributeUsage(AttributeTargets.Method)]
	public class AjaxMethodAttribute : Attribute
	{
		private string methodName = null;

		private bool isCacheEnabled = false;

		private TimeSpan cacheDuration = Cache.NoSlidingExpiration;

		private HttpSessionStateRequirement requireSessionState = HttpSessionStateRequirement.None;

		private HttpConnectionProtocolType httpConnectionProtocol = HttpConnectionProtocolType.Default;

		internal string MethodName
		{
			get
			{
				return methodName;
			}
			set
			{
				methodName = value;
			}
		}

		internal TimeSpan CacheDuration => cacheDuration;

		internal bool IsCacheEnabled => isCacheEnabled;

		internal HttpSessionStateRequirement RequireSessionState => requireSessionState;

		internal HttpConnectionProtocolType HttpConnectionProtocol => httpConnectionProtocol;

		public AjaxMethodAttribute()
		{
		}

		public AjaxMethodAttribute(string methodName)
		{
			this.methodName = methodName;
		}

		public AjaxMethodAttribute(HttpSessionStateRequirement requireSessionState)
		{
			this.requireSessionState = requireSessionState;
		}

		public AjaxMethodAttribute(TimeSpan cacheDuration)
		{
			isCacheEnabled = true;
			this.cacheDuration = cacheDuration;
		}

		public AjaxMethodAttribute(int cacheSeconds)
		{
			isCacheEnabled = true;
			cacheDuration = new TimeSpan(0, 0, 0, cacheSeconds, 0);
		}

		public AjaxMethodAttribute(string methodName, int cacheSeconds)
		{
			isCacheEnabled = true;
			cacheDuration = new TimeSpan(0, 0, 0, cacheSeconds, 0);
			this.methodName = methodName;
		}

		public AjaxMethodAttribute(int cacheSeconds, HttpSessionStateRequirement requireSessionState)
		{
			isCacheEnabled = true;
			cacheDuration = new TimeSpan(0, 0, 0, cacheSeconds, 0);
			this.requireSessionState = requireSessionState;
		}

		public AjaxMethodAttribute(string methodName, int cacheSeconds, HttpSessionStateRequirement requireSessionState)
		{
			this.methodName = methodName;
			isCacheEnabled = true;
			cacheDuration = new TimeSpan(0, 0, 0, cacheSeconds, 0);
			this.requireSessionState = requireSessionState;
		}

		public AjaxMethodAttribute(string methodName, HttpSessionStateRequirement requireSessionState)
		{
			this.methodName = methodName;
			this.requireSessionState = requireSessionState;
		}

		[Obsolete("Most browsers do not accept changing protocol type.")]
		public AjaxMethodAttribute(HttpConnectionProtocolType httpConnectionProtocol)
		{
			this.httpConnectionProtocol = httpConnectionProtocol;
		}

		[Obsolete("Most browsers do not accept changing protocol type.")]
		public AjaxMethodAttribute(string methodName, HttpConnectionProtocolType httpConnectionProtocol)
		{
			this.methodName = methodName;
			this.httpConnectionProtocol = httpConnectionProtocol;
		}

		[Obsolete("Most browsers do not accept changing protocol type.")]
		public AjaxMethodAttribute(HttpSessionStateRequirement requireSessionState, HttpConnectionProtocolType HttpConnectionProtocol)
		{
			this.requireSessionState = requireSessionState;
			httpConnectionProtocol = httpConnectionProtocol;
		}

		[Obsolete("Most browsers do not accept changing protocol type.")]
		public AjaxMethodAttribute(string methodName, HttpConnectionProtocolType httpConnectionProtocol, HttpSessionStateRequirement requireSessionState)
		{
			this.methodName = methodName;
			this.httpConnectionProtocol = httpConnectionProtocol;
			this.requireSessionState = requireSessionState;
		}
	}
}
