using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace log4net.Appender
{
	/// <summary>
	/// Appends logging events to the console.
	/// </summary>
	/// <remarks>
	/// <para>
	/// ColoredConsoleAppender appends log events to the standard output stream
	/// or the error output stream using a layout specified by the 
	/// user. It also allows the color of a specific type of message to be set.
	/// </para>
	/// <para>
	/// By default, all output is written to the console's standard output stream.
	/// The <see cref="P:log4net.Appender.ColoredConsoleAppender.Target" /> property can be set to direct the output to the
	/// error stream.
	/// </para>
	/// <para>
	/// NOTE: This appender writes directly to the application's attached console
	/// not to the <c>System.Console.Out</c> or <c>System.Console.Error</c> <c>TextWriter</c>.
	/// The <c>System.Console.Out</c> and <c>System.Console.Error</c> streams can be
	/// programmatically redirected (for example NUnit does this to capture program output).
	/// This appender will ignore these redirections because it needs to use Win32
	/// API calls to colorize the output. To respect these redirections the <see cref="T:log4net.Appender.ConsoleAppender" />
	/// must be used.
	/// </para>
	/// <para>
	/// When configuring the colored console appender, mapping should be
	/// specified to map a logging level to a color. For example:
	/// </para>
	/// <code lang="XML" escaped="true">
	/// <mapping>
	/// 	<level value="ERROR" />
	/// 	<foreColor value="White" />
	/// 	<backColor value="Red, HighIntensity" />
	/// </mapping>
	/// <mapping>
	/// 	<level value="DEBUG" />
	/// 	<backColor value="Green" />
	/// </mapping>
	/// </code>
	/// <para>
	/// The Level is the standard log4net logging level and ForeColor and BackColor can be any
	/// combination of the following values:
	/// <list type="bullet">
	/// <item><term>Blue</term><description></description></item>
	/// <item><term>Green</term><description></description></item>
	/// <item><term>Red</term><description></description></item>
	/// <item><term>White</term><description></description></item>
	/// <item><term>Yellow</term><description></description></item>
	/// <item><term>Purple</term><description></description></item>
	/// <item><term>Cyan</term><description></description></item>
	/// <item><term>HighIntensity</term><description></description></item>
	/// </list>
	/// </para>
	/// </remarks>
	/// <author>Rick Hobbs</author>
	/// <author>Nicko Cadell</author>
	public class ColoredConsoleAppender : AppenderSkeleton
	{
		/// <summary>
		/// The enum of possible color values for use with the color mapping method
		/// </summary>
		/// <remarks>
		/// <para>
		/// The following flags can be combined together to
		/// form the colors.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Appender.ColoredConsoleAppender" />
		[Flags]
		public enum Colors
		{
			/// <summary>
			/// color is blue
			/// </summary>
			Blue = 0x1,
			/// <summary>
			/// color is green
			/// </summary>
			Green = 0x2,
			/// <summary>
			/// color is red
			/// </summary>
			Red = 0x4,
			/// <summary>
			/// color is white
			/// </summary>
			White = 0x7,
			/// <summary>
			/// color is yellow
			/// </summary>
			Yellow = 0x6,
			/// <summary>
			/// color is purple
			/// </summary>
			Purple = 0x5,
			/// <summary>
			/// color is cyan
			/// </summary>
			Cyan = 0x3,
			/// <summary>
			/// color is intensified
			/// </summary>
			HighIntensity = 0x8
		}

		private struct COORD
		{
			public ushort x;

			public ushort y;
		}

		private struct SMALL_RECT
		{
			public ushort Left;

			public ushort Top;

			public ushort Right;

			public ushort Bottom;
		}

		private struct CONSOLE_SCREEN_BUFFER_INFO
		{
			public COORD dwSize;

			public COORD dwCursorPosition;

			public ushort wAttributes;

			public SMALL_RECT srWindow;

			public COORD dwMaximumWindowSize;
		}

		/// <summary>
		/// A class to act as a mapping between the level that a logging call is made at and
		/// the color it should be displayed as.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Defines the mapping between a level and the color it should be displayed in.
		/// </para>
		/// </remarks>
		public class LevelColors : LevelMappingEntry
		{
			private Colors m_foreColor;

			private Colors m_backColor;

			private ushort m_combinedColor = 0;

			/// <summary>
			/// The mapped foreground color for the specified level
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The mapped foreground color for the specified level.
			/// </para>
			/// </remarks>
			public Colors ForeColor
			{
				get
				{
					return m_foreColor;
				}
				set
				{
					m_foreColor = value;
				}
			}

			/// <summary>
			/// The mapped background color for the specified level
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The mapped background color for the specified level.
			/// </para>
			/// </remarks>
			public Colors BackColor
			{
				get
				{
					return m_backColor;
				}
				set
				{
					m_backColor = value;
				}
			}

			/// <summary>
			/// The combined <see cref="P:log4net.Appender.ColoredConsoleAppender.LevelColors.ForeColor" /> and <see cref="P:log4net.Appender.ColoredConsoleAppender.LevelColors.BackColor" /> suitable for 
			/// setting the console color.
			/// </summary>
			internal ushort CombinedColor => m_combinedColor;

			/// <summary>
			/// Initialize the options for the object
			/// </summary>
			/// <remarks>
			/// <para>
			/// Combine the <see cref="P:log4net.Appender.ColoredConsoleAppender.LevelColors.ForeColor" /> and <see cref="P:log4net.Appender.ColoredConsoleAppender.LevelColors.BackColor" /> together.
			/// </para>
			/// </remarks>
			public override void ActivateOptions()
			{
				base.ActivateOptions();
				m_combinedColor = (ushort)(m_foreColor + ((int)m_backColor << 4));
			}
		}

		/// <summary>
		/// The <see cref="P:log4net.Appender.ColoredConsoleAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.ColoredConsoleAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleOut = "Console.Out";

		/// <summary>
		/// The <see cref="P:log4net.Appender.ColoredConsoleAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.ColoredConsoleAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleError = "Console.Error";

		private const uint STD_OUTPUT_HANDLE = 4294967285u;

		private const uint STD_ERROR_HANDLE = 4294967284u;

		private static readonly char[] s_windowsNewline = new char[2]
		{
			'\r',
			'\n'
		};

		/// <summary>
		/// Flag to write output to the error stream rather than the standard output stream
		/// </summary>
		private bool m_writeToErrorStream = false;

		/// <summary>
		/// Mapping from level object to color value
		/// </summary>
		private LevelMapping m_levelMapping = new LevelMapping();

		/// <summary>
		/// The console output stream writer to write to
		/// </summary>
		/// <remarks>
		/// <para>
		/// This writer is not thread safe.
		/// </para>
		/// </remarks>
		private StreamWriter m_consoleOutputWriter = null;

		/// <summary>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </summary>
		/// <value>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Target is the value of the console output stream.
		/// This is either <c>"Console.Out"</c> or <c>"Console.Error"</c>.
		/// </para>
		/// </remarks>
		public virtual string Target
		{
			get
			{
				return m_writeToErrorStream ? "Console.Error" : "Console.Out";
			}
			set
			{
				string strB = value.Trim();
				if (string.Compare("Console.Error", strB, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
				{
					m_writeToErrorStream = true;
				}
				else
				{
					m_writeToErrorStream = false;
				}
			}
		}

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
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ColoredConsoleAppender" /> class.
		/// </summary>
		/// <remarks>
		/// The instance of the <see cref="T:log4net.Appender.ColoredConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		public ColoredConsoleAppender()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ColoredConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <remarks>
		/// The instance of the <see cref="T:log4net.Appender.ColoredConsoleAppender" /> class is set up to write 
		/// to the standard output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout property")]
		public ColoredConsoleAppender(ILayout layout)
			: this(layout, writeToErrorStream: false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Appender.ColoredConsoleAppender" /> class
		/// with the specified layout.
		/// </summary>
		/// <param name="layout">the layout to use for this appender</param>
		/// <param name="writeToErrorStream">flag set to <c>true</c> to write to the console error stream</param>
		/// <remarks>
		/// When <paramref name="writeToErrorStream" /> is set to <c>true</c>, output is written to
		/// the standard error output stream.  Otherwise, output is written to the standard
		/// output stream.
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & Target properties")]
		public ColoredConsoleAppender(ILayout layout, bool writeToErrorStream)
		{
			Layout = layout;
			m_writeToErrorStream = writeToErrorStream;
		}

		/// <summary>
		/// Add a mapping of level to color - done by the config file
		/// </summary>
		/// <param name="mapping">The mapping to add</param>
		/// <remarks>
		/// <para>
		/// Add a <see cref="T:log4net.Appender.ColoredConsoleAppender.LevelColors" /> mapping to this appender.
		/// Each mapping defines the foreground and background colors
		/// for a level.
		/// </para>
		/// </remarks>
		public void AddMapping(LevelColors mapping)
		{
			m_levelMapping.Add(mapping);
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" /> method.
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes the event to the console.
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_consoleOutputWriter != null)
			{
				IntPtr zero = IntPtr.Zero;
				zero = ((!m_writeToErrorStream) ? GetStdHandle(4294967285u) : GetStdHandle(4294967284u));
				ushort attributes = 7;
				LevelColors levelColors = m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
				if (levelColors != null)
				{
					attributes = levelColors.CombinedColor;
				}
				string text = RenderLoggingEvent(loggingEvent);
				GetConsoleScreenBufferInfo(zero, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);
				SetConsoleTextAttribute(zero, attributes);
				char[] array = text.ToCharArray();
				int num = array.Length;
				bool flag = false;
				if (num > 1 && array[num - 2] == '\r' && array[num - 1] == '\n')
				{
					num -= 2;
					flag = true;
				}
				m_consoleOutputWriter.Write(array, 0, num);
				SetConsoleTextAttribute(zero, bufferInfo.wAttributes);
				if (flag)
				{
					m_consoleOutputWriter.Write(s_windowsNewline, 0, 2);
				}
			}
		}

		/// <summary>
		/// Initialize the options for this appender
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initialize the level to color mappings set on this appender.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_levelMapping.ActivateOptions();
			Stream stream = null;
			stream = ((!m_writeToErrorStream) ? Console.OpenStandardOutput() : Console.OpenStandardError());
			Encoding encoding = Encoding.GetEncoding(GetConsoleOutputCP());
			m_consoleOutputWriter = new StreamWriter(stream, encoding, 256);
			m_consoleOutputWriter.AutoFlush = true;
			GC.SuppressFinalize(m_consoleOutputWriter);
		}

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetConsoleOutputCP();

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool SetConsoleTextAttribute(IntPtr consoleHandle, ushort attributes);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool GetConsoleScreenBufferInfo(IntPtr consoleHandle, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetStdHandle(uint type);
	}
}
