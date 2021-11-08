using log4net.Core;
using System.Web;

namespace log4net.Appender
{
	/// <summary>
	/// <para>
	/// Appends log events to the ASP.NET <see cref="T:System.Web.TraceContext" /> system.
	/// </para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// Diagnostic information and tracing messages that you specify are appended to the output 
	/// of the page that is sent to the requesting browser. Optionally, you can view this information
	/// from a separate trace viewer (Trace.axd) that displays trace information for every page in a 
	/// given application.
	/// </para>
	/// <para>
	/// Trace statements are processed and displayed only when tracing is enabled. You can control 
	/// whether tracing is displayed to a page, to the trace viewer, or both.
	/// </para>
	/// <para>
	/// The logging event is passed to the <see cref="M:System.Web.TraceContext.Write(System.String)" /> or 
	/// <see cref="M:System.Web.TraceContext.Warn(System.String)" /> method depending on the level of the logging event.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class AspNetTraceAppender : AppenderSkeleton
	{
		/// <summary>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </summary>
		/// <value><c>true</c></value>
		/// <remarks>
		/// <para>
		/// This appender requires a <see cref="N:log4net.Layout" /> to be set.
		/// </para>
		/// </remarks>
		protected override bool RequiresLayout => true;

		/// <summary>
		/// Write the logging event to the ASP.NET trace
		/// </summary>
		/// <param name="loggingEvent">the event to log</param>
		/// <remarks>
		/// <para>
		/// Write the logging event to the ASP.NET trace
		/// <c>HttpContext.Current.Trace</c> 
		/// (<see cref="T:System.Web.TraceContext" />).
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (HttpContext.Current != null && HttpContext.Current.Trace.IsEnabled)
			{
				if (loggingEvent.Level >= Level.Warn)
				{
					HttpContext.Current.Trace.Warn(loggingEvent.LoggerName, RenderLoggingEvent(loggingEvent));
				}
				else
				{
					HttpContext.Current.Trace.Write(loggingEvent.LoggerName, RenderLoggingEvent(loggingEvent));
				}
			}
		}
	}
}
