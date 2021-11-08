using log4net.Util;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;

namespace log4net.Core
{
	/// <summary>
	/// The internal representation of caller location information.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class uses the <c>System.Diagnostics.StackTrace</c> class to generate
	/// a call stack. The caller's information is then extracted from this stack.
	/// </para>
	/// <para>
	/// The <c>System.Diagnostics.StackTrace</c> class is not supported on the 
	/// .NET Compact Framework 1.0 therefore caller location information is not
	/// available on that framework.
	/// </para>
	/// <para>
	/// The <c>System.Diagnostics.StackTrace</c> class has this to say about Release builds:
	/// </para>
	/// <para>
	/// "StackTrace information will be most informative with Debug build configurations. 
	/// By default, Debug builds include debug symbols, while Release builds do not. The 
	/// debug symbols contain most of the file, method name, line number, and column 
	/// information used in constructing StackFrame and StackTrace objects. StackTrace 
	/// might not report as many method calls as expected, due to code transformations 
	/// that occur during optimization."
	/// </para>
	/// <para>
	/// This means that in a Release build the caller information may be incomplete or may 
	/// not exist at all! Therefore caller location information cannot be relied upon in a Release build.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public class LocationInfo
	{
		/// <summary>
		/// When location information is not available the constant
		/// <c>NA</c> is returned. Current value of this string
		/// constant is <b>?</b>.
		/// </summary>
		private const string NA = "?";

		private readonly string m_className;

		private readonly string m_fileName;

		private readonly string m_lineNumber;

		private readonly string m_methodName;

		private readonly string m_fullInfo;

		/// <summary>
		/// Gets the fully qualified class name of the caller making the logging 
		/// request.
		/// </summary>
		/// <value>
		/// The fully qualified class name of the caller making the logging 
		/// request.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the fully qualified class name of the caller making the logging 
		/// request.
		/// </para>
		/// </remarks>
		public string ClassName => m_className;

		/// <summary>
		/// Gets the file name of the caller.
		/// </summary>
		/// <value>
		/// The file name of the caller.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the file name of the caller.
		/// </para>
		/// </remarks>
		public string FileName => m_fileName;

		/// <summary>
		/// Gets the line number of the caller.
		/// </summary>
		/// <value>
		/// The line number of the caller.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the line number of the caller.
		/// </para>
		/// </remarks>
		public string LineNumber => m_lineNumber;

		/// <summary>
		/// Gets the method name of the caller.
		/// </summary>
		/// <value>
		/// The method name of the caller.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the method name of the caller.
		/// </para>
		/// </remarks>
		public string MethodName => m_methodName;

		/// <summary>
		/// Gets all available caller information
		/// </summary>
		/// <value>
		/// All available caller information, in the format
		/// <c>fully.qualified.classname.of.caller.methodName(Filename:line)</c>
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets all available caller information, in the format
		/// <c>fully.qualified.classname.of.caller.methodName(Filename:line)</c>
		/// </para>
		/// </remarks>
		public string FullInfo => m_fullInfo;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="callerStackBoundaryDeclaringType">The declaring type of the method that is
		/// the stack boundary into the logging system for this call.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LocationInfo" />
		/// class based on the current thread.
		/// </para>
		/// </remarks>
		public LocationInfo(Type callerStackBoundaryDeclaringType)
		{
			m_className = "?";
			m_fileName = "?";
			m_lineNumber = "?";
			m_methodName = "?";
			m_fullInfo = "?";
			if ((object)callerStackBoundaryDeclaringType != null)
			{
				try
				{
					StackTrace stackTrace = new StackTrace(fNeedFileInfo: true);
					int i;
					for (i = 0; i < stackTrace.FrameCount; i++)
					{
						StackFrame frame = stackTrace.GetFrame(i);
						if (frame != null && (object)frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType)
						{
							break;
						}
					}
					for (; i < stackTrace.FrameCount; i++)
					{
						StackFrame frame = stackTrace.GetFrame(i);
						if (frame != null && (object)frame.GetMethod().DeclaringType != callerStackBoundaryDeclaringType)
						{
							break;
						}
					}
					if (i < stackTrace.FrameCount)
					{
						StackFrame frame2 = stackTrace.GetFrame(i);
						if (frame2 != null)
						{
							MethodBase method = frame2.GetMethod();
							if ((object)method != null)
							{
								m_methodName = method.Name;
								if ((object)method.DeclaringType != null)
								{
									m_className = method.DeclaringType.FullName;
								}
							}
							m_fileName = frame2.GetFileName();
							m_lineNumber = frame2.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
							m_fullInfo = m_className + '.' + m_methodName + '(' + m_fileName + ':' + m_lineNumber + ')';
						}
					}
				}
				catch (SecurityException)
				{
					LogLog.Debug("LocationInfo: Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="className">The fully qualified class name.</param>
		/// <param name="methodName">The method name.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="lineNumber">The line number of the method within the file.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LocationInfo" />
		/// class with the specified data.
		/// </para>
		/// </remarks>
		public LocationInfo(string className, string methodName, string fileName, string lineNumber)
		{
			m_className = className;
			m_fileName = fileName;
			m_lineNumber = lineNumber;
			m_methodName = methodName;
			m_fullInfo = m_className + '.' + m_methodName + '(' + m_fileName + ':' + m_lineNumber + ')';
		}
	}
}
