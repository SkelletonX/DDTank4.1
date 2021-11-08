using System;

namespace log4net.Core
{
	/// <summary>
	/// Flags passed to the <see cref="P:log4net.Core.LoggingEvent.Fix" /> property
	/// </summary>
	/// <remarks>
	/// <para>
	/// Flags passed to the <see cref="P:log4net.Core.LoggingEvent.Fix" /> property
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	[Flags]
	public enum FixFlags
	{
		/// <summary>
		/// Fix the MDC
		/// </summary>
		[Obsolete("Replaced by composite Properties")]
		Mdc = 0x1,
		/// <summary>
		/// Fix the NDC
		/// </summary>
		Ndc = 0x2,
		/// <summary>
		/// Fix the rendered message
		/// </summary>
		Message = 0x4,
		/// <summary>
		/// Fix the thread name
		/// </summary>
		ThreadName = 0x8,
		/// <summary>
		/// Fix the callers location information
		/// </summary>
		/// <remarks>
		/// CAUTION: Very slow to generate
		/// </remarks>
		LocationInfo = 0x10,
		/// <summary>
		/// Fix the callers windows user name
		/// </summary>
		/// <remarks>
		/// CAUTION: Slow to generate
		/// </remarks>
		UserName = 0x20,
		/// <summary>
		/// Fix the domain friendly name
		/// </summary>
		Domain = 0x40,
		/// <summary>
		/// Fix the callers principal name
		/// </summary>
		/// <remarks>
		/// CAUTION: May be slow to generate
		/// </remarks>
		Identity = 0x80,
		/// <summary>
		/// Fix the exception text
		/// </summary>
		Exception = 0x100,
		/// <summary>
		/// Fix the event properties
		/// </summary>
		Properties = 0x200,
		/// <summary>
		/// No fields fixed
		/// </summary>
		None = 0x0,
		/// <summary>
		/// All fields fixed
		/// </summary>
		All = 0xFFFFFFF,
		/// <summary>
		/// Partial fields fixed
		/// </summary>
		/// <remarks>
		/// <para>
		/// This set of partial fields gives good performance. The following fields are fixed:
		/// </para>
		/// <list type="bullet">
		/// <item><description><see cref="F:log4net.Core.FixFlags.Message" /></description></item>
		/// <item><description><see cref="F:log4net.Core.FixFlags.ThreadName" /></description></item>
		/// <item><description><see cref="F:log4net.Core.FixFlags.Exception" /></description></item>
		/// <item><description><see cref="F:log4net.Core.FixFlags.Domain" /></description></item>
		/// <item><description><see cref="F:log4net.Core.FixFlags.Properties" /></description></item>
		/// </list>
		/// </remarks>
		Partial = 0x34C
	}
}
