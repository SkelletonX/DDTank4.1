using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace log4net.Util
{
	/// <summary>
	/// Represents a native error code and message.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Represents a Win32 platform native error.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public sealed class NativeError
	{
		private int m_number;

		private string m_message;

		/// <summary>
		/// Gets the number of the native error.
		/// </summary>
		/// <value>
		/// The number of the native error.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets the number of the native error.
		/// </para>
		/// </remarks>
		public int Number => m_number;

		/// <summary>
		/// Gets the message of the native error.
		/// </summary>
		/// <value>
		/// The message of the native error.
		/// </value>
		/// <remarks>
		/// <para>
		/// </para>
		/// Gets the message of the native error.
		/// </remarks>
		public string Message => m_message;

		/// <summary>
		/// Create an instance of the <see cref="T:log4net.Util.NativeError" /> class with the specified 
		/// error number and message.
		/// </summary>
		/// <param name="number">The number of the native error.</param>
		/// <param name="message">The message of the native error.</param>
		/// <remarks>
		/// <para>
		/// Create an instance of the <see cref="T:log4net.Util.NativeError" /> class with the specified 
		/// error number and message.
		/// </para>
		/// </remarks>
		private NativeError(int number, string message)
		{
			m_number = number;
			m_message = message;
		}

		/// <summary>
		/// Create a new instance of the <see cref="T:log4net.Util.NativeError" /> class for the last Windows error.
		/// </summary>
		/// <returns>
		/// An instance of the <see cref="T:log4net.Util.NativeError" /> class for the last windows error.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The message for the <see cref="M:System.Runtime.InteropServices.Marshal.GetLastWin32Error" /> error number is lookup up using the 
		/// native Win32 <c>FormatMessage</c> function.
		/// </para>
		/// </remarks>
		public static NativeError GetLastError()
		{
			int lastWin32Error = Marshal.GetLastWin32Error();
			return new NativeError(lastWin32Error, GetErrorMessage(lastWin32Error));
		}

		/// <summary>
		/// Create a new instance of the <see cref="T:log4net.Util.NativeError" /> class.
		/// </summary>
		/// <param name="number">the error number for the native error</param>
		/// <returns>
		/// An instance of the <see cref="T:log4net.Util.NativeError" /> class for the specified 
		/// error number.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The message for the specified error number is lookup up using the 
		/// native Win32 <c>FormatMessage</c> function.
		/// </para>
		/// </remarks>
		public static NativeError GetError(int number)
		{
			return new NativeError(number, GetErrorMessage(number));
		}

		/// <summary>
		/// Retrieves the message corresponding with a Win32 message identifier.
		/// </summary>
		/// <param name="messageId">Message identifier for the requested message.</param>
		/// <returns>
		/// The message corresponding with the specified message identifier.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The message will be searched for in system message-table resource(s)
		/// using the native <c>FormatMessage</c> function.
		/// </para>
		/// </remarks>
		public static string GetErrorMessage(int messageId)
		{
			int num = 256;
			int num2 = 512;
			int num3 = 4096;
			string lpBuffer = "";
			IntPtr lpSource = default(IntPtr);
			IntPtr arguments = default(IntPtr);
			if (messageId != 0)
			{
				int num4 = FormatMessage(num | num3 | num2, ref lpSource, messageId, 0, ref lpBuffer, 255, arguments);
				if (num4 > 0)
				{
					return lpBuffer.TrimEnd('\r', '\n');
				}
				return null;
			}
			return null;
		}

		/// <summary>
		/// Return error information string
		/// </summary>
		/// <returns>error information string</returns>
		/// <remarks>
		/// <para>
		/// Return error information string
		/// </para>
		/// </remarks>
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[1]
			{
				Number
			}) + ((Message != null) ? (": " + Message) : "");
		}

		/// <summary>
		/// Formats a message string.
		/// </summary>
		/// <param name="dwFlags">Formatting options, and how to interpret the <paramref name="lpSource" /> parameter.</param>
		/// <param name="lpSource">Location of the message definition.</param>
		/// <param name="dwMessageId">Message identifier for the requested message.</param>
		/// <param name="dwLanguageId">Language identifier for the requested message.</param>
		/// <param name="lpBuffer">If <paramref name="dwFlags" /> includes FORMAT_MESSAGE_ALLOCATE_BUFFER, the function allocates a buffer using the <c>LocalAlloc</c> function, and places the pointer to the buffer at the address specified in <paramref name="lpBuffer" />.</param>
		/// <param name="nSize">If the FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set, this parameter specifies the maximum number of TCHARs that can be stored in the output buffer. If FORMAT_MESSAGE_ALLOCATE_BUFFER is set, this parameter specifies the minimum number of TCHARs to allocate for an output buffer.</param>
		/// <param name="Arguments">Pointer to an array of values that are used as insert values in the formatted message.</param>
		/// <remarks>
		/// <para>
		/// The function requires a message definition as input. The message definition can come from a 
		/// buffer passed into the function. It can come from a message table resource in an 
		/// already-loaded module. Or the caller can ask the function to search the system's message 
		/// table resource(s) for the message definition. The function finds the message definition 
		/// in a message table resource based on a message identifier and a language identifier. 
		/// The function copies the formatted message text to an output buffer, processing any embedded 
		/// insert sequences if requested.
		/// </para>
		/// <para>
		/// To prevent the usage of unsafe code, this stub does not support inserting values in the formatted message.
		/// </para>
		/// </remarks>
		/// <returns>
		/// <para>
		/// If the function succeeds, the return value is the number of TCHARs stored in the output 
		/// buffer, excluding the terminating null character.
		/// </para>
		/// <para>
		/// If the function fails, the return value is zero. To get extended error information, 
		/// call <see cref="M:System.Runtime.InteropServices.Marshal.GetLastWin32Error" />.
		/// </para>
		/// </returns>
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId, ref string lpBuffer, int nSize, IntPtr Arguments);
	}
}
