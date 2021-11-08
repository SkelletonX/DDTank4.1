using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Threading;

namespace log4net.Util
{
	/// <summary>
	/// Utility class for system specific information.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Utility class of static methods for system specific information.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Alexey Solofnenko</author>
	public sealed class SystemInfo
	{
		private const string DEFAULT_NULL_TEXT = "(null)";

		private const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";

		/// <summary>
		/// Gets an empty array of types.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <c>Type.EmptyTypes</c> field is not available on
		/// the .NET Compact Framework 1.0.
		/// </para>
		/// </remarks>
		public static readonly Type[] EmptyTypes;

		/// <summary>
		/// Cache the host name for the current machine
		/// </summary>
		private static string s_hostName;

		/// <summary>
		/// Cache the application friendly name
		/// </summary>
		private static string s_appFriendlyName;

		/// <summary>
		/// Text to output when a <c>null</c> is encountered.
		/// </summary>
		private static string s_nullText;

		/// <summary>
		/// Text to output when an unsupported feature is requested.
		/// </summary>
		private static string s_notAvailableText;

		/// <summary>
		/// Start time for the current process.
		/// </summary>
		private static DateTime s_processStartTime;

		/// <summary>
		/// Gets the system dependent line terminator.
		/// </summary>
		/// <value>
		/// The system dependent line terminator.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the system dependent line terminator.
		/// </para>
		/// </remarks>
		public static string NewLine => Environment.NewLine;

		/// <summary>
		/// Gets the base directory for this <see cref="T:System.AppDomain" />.
		/// </summary>
		/// <value>The base directory path for the current <see cref="T:System.AppDomain" />.</value>
		/// <remarks>
		/// <para>
		/// Gets the base directory for this <see cref="T:System.AppDomain" />.
		/// </para>
		/// <para>
		/// The value returned may be either a local file path or a URI.
		/// </para>
		/// </remarks>
		public static string ApplicationBaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

		/// <summary>
		/// Gets the path to the configuration file for the current <see cref="T:System.AppDomain" />.
		/// </summary>
		/// <value>The path to the configuration file for the current <see cref="T:System.AppDomain" />.</value>
		/// <remarks>
		/// <para>
		/// The .NET Compact Framework 1.0 does not have a concept of a configuration
		/// file. For this runtime, we use the entry assembly location as the root for
		/// the configuration file name.
		/// </para>
		/// <para>
		/// The value returned may be either a local file path or a URI.
		/// </para>
		/// </remarks>
		public static string ConfigurationFileLocation => AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

		/// <summary>
		/// Gets the path to the file that first executed in the current <see cref="T:System.AppDomain" />.
		/// </summary>
		/// <value>The path to the entry assembly.</value>
		/// <remarks>
		/// <para>
		/// Gets the path to the file that first executed in the current <see cref="T:System.AppDomain" />.
		/// </para>
		/// </remarks>
		public static string EntryAssemblyLocation => Assembly.GetEntryAssembly().Location;

		/// <summary>
		/// Gets the ID of the current thread.
		/// </summary>
		/// <value>The ID of the current thread.</value>
		/// <remarks>
		/// <para>
		/// On the .NET framework, the <c>AppDomain.GetCurrentThreadId</c> method
		/// is used to obtain the thread ID for the current thread. This is the 
		/// operating system ID for the thread.
		/// </para>
		/// <para>
		/// On the .NET Compact Framework 1.0 it is not possible to get the 
		/// operating system thread ID for the current thread. The native method 
		/// <c>GetCurrentThreadId</c> is implemented inline in a header file
		/// and cannot be called.
		/// </para>
		/// <para>
		/// On the .NET Framework 2.0 the <c>Thread.ManagedThreadId</c> is used as this
		/// gives a stable id unrelated to the operating system thread ID which may 
		/// change if the runtime is using fibers.
		/// </para>
		/// </remarks>
		public static int CurrentThreadId => Thread.CurrentThread.ManagedThreadId;

		/// <summary>
		/// Get the host name or machine name for the current machine
		/// </summary>
		/// <value>
		/// The hostname or machine name
		/// </value>
		/// <remarks>
		/// <para>
		/// Get the host name or machine name for the current machine
		/// </para>
		/// <para>
		/// The host name (<see cref="M:System.Net.Dns.GetHostName" />) or
		/// the machine name (<c>Environment.MachineName</c>) for
		/// the current machine, or if neither of these are available
		/// then <c>NOT AVAILABLE</c> is returned.
		/// </para>
		/// </remarks>
		public static string HostName
		{
			get
			{
				if (s_hostName == null)
				{
					try
					{
						s_hostName = Dns.GetHostName();
					}
					catch (SocketException)
					{
					}
					catch (SecurityException)
					{
					}
					if (s_hostName == null || s_hostName.Length == 0)
					{
						try
						{
							s_hostName = Environment.MachineName;
						}
						catch (InvalidOperationException)
						{
						}
						catch (SecurityException)
						{
						}
					}
					if (s_hostName == null || s_hostName.Length == 0)
					{
						s_hostName = s_notAvailableText;
					}
				}
				return s_hostName;
			}
		}

		/// <summary>
		/// Get this application's friendly name
		/// </summary>
		/// <value>
		/// The friendly name of this application as a string
		/// </value>
		/// <remarks>
		/// <para>
		/// If available the name of the application is retrieved from
		/// the <c>AppDomain</c> using <c>AppDomain.CurrentDomain.FriendlyName</c>.
		/// </para>
		/// <para>
		/// Otherwise the file name of the entry assembly is used.
		/// </para>
		/// </remarks>
		public static string ApplicationFriendlyName
		{
			get
			{
				if (s_appFriendlyName == null)
				{
					try
					{
						s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
					}
					catch (SecurityException)
					{
						LogLog.Debug("SystemInfo: Security exception while trying to get current domain friendly name. Error Ignored.");
					}
					if (s_appFriendlyName == null || s_appFriendlyName.Length == 0)
					{
						try
						{
							string entryAssemblyLocation = EntryAssemblyLocation;
							s_appFriendlyName = Path.GetFileName(entryAssemblyLocation);
						}
						catch (SecurityException)
						{
						}
					}
					if (s_appFriendlyName == null || s_appFriendlyName.Length == 0)
					{
						s_appFriendlyName = s_notAvailableText;
					}
				}
				return s_appFriendlyName;
			}
		}

		/// <summary>
		/// Get the start time for the current process.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is the time at which the log4net library was loaded into the
		/// AppDomain. Due to reports of a hang in the call to <c>System.Diagnostics.Process.StartTime</c>
		/// this is not the start time for the current process.
		/// </para>
		/// <para>
		/// The log4net library should be loaded by an application early during its
		/// startup, therefore this start time should be a good approximation for
		/// the actual start time.
		/// </para>
		/// <para>
		/// Note that AppDomains may be loaded and unloaded within the
		/// same process without the process terminating, however this start time
		/// will be set per AppDomain.
		/// </para>
		/// </remarks>
		public static DateTime ProcessStartTime => s_processStartTime;

		/// <summary>
		/// Text to output when a <c>null</c> is encountered.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Use this value to indicate a <c>null</c> has been encountered while
		/// outputting a string representation of an item.
		/// </para>
		/// <para>
		/// The default value is <c>(null)</c>. This value can be overridden by specifying
		/// a value for the <c>log4net.NullText</c> appSetting in the application's
		/// .config file.
		/// </para>
		/// </remarks>
		public static string NullText
		{
			get
			{
				return s_nullText;
			}
			set
			{
				s_nullText = value;
			}
		}

		/// <summary>
		/// Text to output when an unsupported feature is requested.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Use this value when an unsupported feature is requested.
		/// </para>
		/// <para>
		/// The default value is <c>NOT AVAILABLE</c>. This value can be overridden by specifying
		/// a value for the <c>log4net.NotAvailableText</c> appSetting in the application's
		/// .config file.
		/// </para>
		/// </remarks>
		public static string NotAvailableText
		{
			get
			{
				return s_notAvailableText;
			}
			set
			{
				s_notAvailableText = value;
			}
		}

		/// <summary>
		/// Private constructor to prevent instances.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only static methods are exposed from this type.
		/// </para>
		/// </remarks>
		private SystemInfo()
		{
		}

		/// <summary>
		/// Initialize default values for private static fields.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only static methods are exposed from this type.
		/// </para>
		/// </remarks>
		static SystemInfo()
		{
			EmptyTypes = new Type[0];
			s_processStartTime = DateTime.Now;
			string text = "(null)";
			string text2 = "NOT AVAILABLE";
			string appSetting = GetAppSetting("log4net.NullText");
			if (appSetting != null && appSetting.Length > 0)
			{
				LogLog.Debug("SystemInfo: Initializing NullText value to [" + appSetting + "].");
				text = appSetting;
			}
			string appSetting2 = GetAppSetting("log4net.NotAvailableText");
			if (appSetting2 != null && appSetting2.Length > 0)
			{
				LogLog.Debug("SystemInfo: Initializing NotAvailableText value to [" + appSetting2 + "].");
				text2 = appSetting2;
			}
			s_notAvailableText = text2;
			s_nullText = text;
		}

		/// <summary>
		/// Gets the assembly location path for the specified assembly.
		/// </summary>
		/// <param name="myAssembly">The assembly to get the location for.</param>
		/// <returns>The location of the assembly.</returns>
		/// <remarks>
		/// <para>
		/// This method does not guarantee to return the correct path
		/// to the assembly. If only tries to give an indication as to
		/// where the assembly was loaded from.
		/// </para>
		/// </remarks>
		public static string AssemblyLocationInfo(Assembly myAssembly)
		{
			if (myAssembly.GlobalAssemblyCache)
			{
				return "Global Assembly Cache";
			}
			try
			{
				return myAssembly.Location;
			}
			catch (SecurityException)
			{
				return "Location Permission Denied";
			}
		}

		/// <summary>
		/// Gets the fully qualified name of the <see cref="T:System.Type" />, including 
		/// the name of the assembly from which the <see cref="T:System.Type" /> was 
		/// loaded.
		/// </summary>
		/// <param name="type">The <see cref="T:System.Type" /> to get the fully qualified name for.</param>
		/// <returns>The fully qualified name for the <see cref="T:System.Type" />.</returns>
		/// <remarks>
		/// <para>
		/// This is equivalent to the <c>Type.AssemblyQualifiedName</c> property,
		/// but this method works on the .NET Compact Framework 1.0 as well as
		/// the full .NET runtime.
		/// </para>
		/// </remarks>
		public static string AssemblyQualifiedName(Type type)
		{
			return type.FullName + ", " + type.Assembly.FullName;
		}

		/// <summary>
		/// Gets the short name of the <see cref="T:System.Reflection.Assembly" />.
		/// </summary>
		/// <param name="myAssembly">The <see cref="T:System.Reflection.Assembly" /> to get the name for.</param>
		/// <returns>The short name of the <see cref="T:System.Reflection.Assembly" />.</returns>
		/// <remarks>
		/// <para>
		/// The short name of the assembly is the <see cref="P:System.Reflection.Assembly.FullName" /> 
		/// without the version, culture, or public key. i.e. it is just the 
		/// assembly's file name without the extension.
		/// </para>
		/// <para>
		/// Use this rather than <c>Assembly.GetName().Name</c> because that
		/// is not available on the Compact Framework.
		/// </para>
		/// <para>
		/// Because of a FileIOPermission security demand we cannot do
		/// the obvious Assembly.GetName().Name. We are allowed to get
		/// the <see cref="P:System.Reflection.Assembly.FullName" /> of the assembly so we 
		/// start from there and strip out just the assembly name.
		/// </para>
		/// </remarks>
		public static string AssemblyShortName(Assembly myAssembly)
		{
			string text = myAssembly.FullName;
			int num = text.IndexOf(',');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			return text.Trim();
		}

		/// <summary>
		/// Gets the file name portion of the <see cref="T:System.Reflection.Assembly" />, including the extension.
		/// </summary>
		/// <param name="myAssembly">The <see cref="T:System.Reflection.Assembly" /> to get the file name for.</param>
		/// <returns>The file name of the assembly.</returns>
		/// <remarks>
		/// <para>
		/// Gets the file name portion of the <see cref="T:System.Reflection.Assembly" />, including the extension.
		/// </para>
		/// </remarks>
		public static string AssemblyFileName(Assembly myAssembly)
		{
			return Path.GetFileName(myAssembly.Location);
		}

		/// <summary>
		/// Loads the type specified in the type string.
		/// </summary>
		/// <param name="relativeType">A sibling type to use to load the type.</param>
		/// <param name="typeName">The name of the type to load.</param>
		/// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
		/// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
		/// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>
		/// <remarks>
		/// <para>
		/// If the type name is fully qualified, i.e. if contains an assembly name in 
		/// the type name, the type will be loaded from the system using 
		/// <see cref="M:System.Type.GetType(System.String,System.Boolean)" />.
		/// </para>
		/// <para>
		/// If the type name is not fully qualified, it will be loaded from the assembly
		/// containing the specified relative type. If the type is not found in the assembly 
		/// then all the loaded assemblies will be searched for the type.
		/// </para>
		/// </remarks>
		public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase)
		{
			return GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);
		}

		/// <summary>
		/// Loads the type specified in the type string.
		/// </summary>
		/// <param name="typeName">The name of the type to load.</param>
		/// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
		/// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
		/// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>		
		/// <remarks>
		/// <para>
		/// If the type name is fully qualified, i.e. if contains an assembly name in 
		/// the type name, the type will be loaded from the system using 
		/// <see cref="M:System.Type.GetType(System.String,System.Boolean)" />.
		/// </para>
		/// <para>
		/// If the type name is not fully qualified it will be loaded from the
		/// assembly that is directly calling this method. If the type is not found 
		/// in the assembly then all the loaded assemblies will be searched for the type.
		/// </para>
		/// </remarks>
		public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
		{
			return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
		}

		/// <summary>
		/// Loads the type specified in the type string.
		/// </summary>
		/// <param name="relativeAssembly">An assembly to load the type from.</param>
		/// <param name="typeName">The name of the type to load.</param>
		/// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
		/// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
		/// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>
		/// <remarks>
		/// <para>
		/// If the type name is fully qualified, i.e. if contains an assembly name in 
		/// the type name, the type will be loaded from the system using 
		/// <see cref="M:System.Type.GetType(System.String,System.Boolean)" />.
		/// </para>
		/// <para>
		/// If the type name is not fully qualified it will be loaded from the specified
		/// assembly. If the type is not found in the assembly then all the loaded assemblies 
		/// will be searched for the type.
		/// </para>
		/// </remarks>
		public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
		{
			if (typeName.IndexOf(',') == -1)
			{
				Type type = relativeAssembly.GetType(typeName, throwOnError: false, ignoreCase);
				if ((object)type != null)
				{
					return type;
				}
				Assembly[] array = null;
				try
				{
					array = AppDomain.CurrentDomain.GetAssemblies();
				}
				catch (SecurityException)
				{
				}
				if (array != null)
				{
					Assembly[] array2 = array;
					foreach (Assembly assembly in array2)
					{
						type = assembly.GetType(typeName, throwOnError: false, ignoreCase);
						if ((object)type != null)
						{
							LogLog.Debug("SystemInfo: Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
							return type;
						}
					}
				}
				if (throwOnError)
				{
					throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
				}
				return null;
			}
			return Type.GetType(typeName, throwOnError, ignoreCase);
		}

		/// <summary>
		/// Generate a new guid
		/// </summary>
		/// <returns>A new Guid</returns>
		/// <remarks>
		/// <para>
		/// Generate a new guid
		/// </para>
		/// </remarks>
		public static Guid NewGuid()
		{
			return Guid.NewGuid();
		}

		/// <summary>
		/// Create an <see cref="T:System.ArgumentOutOfRangeException" />
		/// </summary>
		/// <param name="parameterName">The name of the parameter that caused the exception</param>
		/// <param name="actualValue">The value of the argument that causes this exception</param>
		/// <param name="message">The message that describes the error</param>
		/// <returns>the ArgumentOutOfRangeException object</returns>
		/// <remarks>
		/// <para>
		/// Create a new instance of the <see cref="T:System.ArgumentOutOfRangeException" /> class 
		/// with a specified error message, the parameter name, and the value 
		/// of the argument.
		/// </para>
		/// <para>
		/// The Compact Framework does not support the 3 parameter constructor for the
		/// <see cref="T:System.ArgumentOutOfRangeException" /> type. This method provides an
		/// implementation that works for all platforms.
		/// </para>
		/// </remarks>
		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string parameterName, object actualValue, string message)
		{
			return new ArgumentOutOfRangeException(parameterName, actualValue, message);
		}

		/// <summary>
		/// Parse a string into an <see cref="T:System.Int32" /> value
		/// </summary>
		/// <param name="s">the string to parse</param>
		/// <param name="val">out param where the parsed value is placed</param>
		/// <returns><c>true</c> if the string was able to be parsed into an integer</returns>
		/// <remarks>
		/// <para>
		/// Attempts to parse the string into an integer. If the string cannot
		/// be parsed then this method returns <c>false</c>. The method does not throw an exception.
		/// </para>
		/// </remarks>
		public static bool TryParse(string s, out int val)
		{
			val = 0;
			try
			{
				if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out double result))
				{
					val = Convert.ToInt32(result);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		/// <summary>
		/// Parse a string into an <see cref="T:System.Int64" /> value
		/// </summary>
		/// <param name="s">the string to parse</param>
		/// <param name="val">out param where the parsed value is placed</param>
		/// <returns><c>true</c> if the string was able to be parsed into an integer</returns>
		/// <remarks>
		/// <para>
		/// Attempts to parse the string into an integer. If the string cannot
		/// be parsed then this method returns <c>false</c>. The method does not throw an exception.
		/// </para>
		/// </remarks>
		public static bool TryParse(string s, out long val)
		{
			val = 0L;
			try
			{
				if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out double result))
				{
					val = Convert.ToInt64(result);
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		/// <summary>
		/// Lookup an application setting
		/// </summary>
		/// <param name="key">the application settings key to lookup</param>
		/// <returns>the value for the key, or <c>null</c></returns>
		/// <remarks>
		/// <para>
		/// Configuration APIs are not supported under the Compact Framework
		/// </para>
		/// </remarks>
		public static string GetAppSetting(string key)
		{
			try
			{
				return ConfigurationManager.AppSettings[key];
			}
			catch (Exception exception)
			{
				LogLog.Error("DefaultRepositorySelector: Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", exception);
			}
			return null;
		}

		/// <summary>
		/// Convert a path into a fully qualified local file path.
		/// </summary>
		/// <param name="path">The path to convert.</param>
		/// <returns>The fully qualified path.</returns>
		/// <remarks>
		/// <para>
		/// Converts the path specified to a fully
		/// qualified path. If the path is relative it is
		/// taken as relative from the application base 
		/// directory.
		/// </para>
		/// <para>
		/// The path specified must be a local file path, a URI is not supported.
		/// </para>
		/// </remarks>
		public static string ConvertToFullPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			string text = "";
			try
			{
				string applicationBaseDirectory = ApplicationBaseDirectory;
				if (applicationBaseDirectory != null)
				{
					Uri uri = new Uri(applicationBaseDirectory);
					if (uri.IsFile)
					{
						text = uri.LocalPath;
					}
				}
			}
			catch
			{
			}
			if (text != null && text.Length > 0)
			{
				return Path.GetFullPath(Path.Combine(text, path));
			}
			return Path.GetFullPath(path);
		}

		/// <summary>
		/// Creates a new case-insensitive instance of the <see cref="T:System.Collections.Hashtable" /> class with the default initial capacity. 
		/// </summary>
		/// <returns>A new case-insensitive instance of the <see cref="T:System.Collections.Hashtable" /> class with the default initial capacity</returns>
		/// <remarks>
		/// <para>
		/// The new Hashtable instance uses the default load factor, the CaseInsensitiveHashCodeProvider, and the CaseInsensitiveComparer.
		/// </para>
		/// </remarks>
		public static Hashtable CreateCaseInsensitiveHashtable()
		{
			return CollectionsUtil.CreateCaseInsensitiveHashtable();
		}
	}
}
