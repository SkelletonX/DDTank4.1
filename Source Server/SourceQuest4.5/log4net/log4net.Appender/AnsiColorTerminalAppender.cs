using log4net.Core;
using log4net.Util;
using System;
using System.Globalization;
using System.Text;

namespace log4net.Appender
{
	/// <summary>
	/// Appends logging events to the terminal using ANSI color escape sequences.
	/// </summary>
	/// <remarks>
	/// <para>
	/// AnsiColorTerminalAppender appends log events to the standard output stream
	/// or the error output stream using a layout specified by the 
	/// user. It also allows the color of a specific level of message to be set.
	/// </para>
	/// <note>
	/// This appender expects the terminal to understand the VT100 control set 
	/// in order to interpret the color codes. If the terminal or console does not
	/// understand the control codes the behavior is not defined.
	/// </note>
	/// <para>
	/// By default, all output is written to the console's standard output stream.
	/// The <see cref="P:log4net.Appender.AnsiColorTerminalAppender.Target" /> property can be set to direct the output to the
	/// error stream.
	/// </para>
	/// <para>
	/// NOTE: This appender writes each message to the <c>System.Console.Out</c> or 
	/// <c>System.Console.Error</c> that is set at the time the event is appended.
	/// Therefore it is possible to programmatically redirect the output of this appender 
	/// (for example NUnit does this to capture program output). While this is the desired
	/// behavior of this appender it may have security implications in your application. 
	/// </para>
	/// <para>
	/// When configuring the ANSI colored terminal appender, a mapping should be
	/// specified to map a logging level to a color. For example:
	/// </para>
	/// <code lang="XML" escaped="true">
	/// <mapping>
	/// 	<level value="ERROR" />
	/// 	<foreColor value="White" />
	/// 	<backColor value="Red" />
	///     <attributes value="Bright,Underscore" />
	/// </mapping>
	/// <mapping>
	/// 	<level value="DEBUG" />
	/// 	<backColor value="Green" />
	/// </mapping>
	/// </code>
	/// <para>
	/// The Level is the standard log4net logging level and ForeColor and BackColor can be any
	/// of the following values:
	/// <list type="bullet">
	/// <item><term>Blue</term><description></description></item>
	/// <item><term>Green</term><description></description></item>
	/// <item><term>Red</term><description></description></item>
	/// <item><term>White</term><description></description></item>
	/// <item><term>Yellow</term><description></description></item>
	/// <item><term>Purple</term><description></description></item>
	/// <item><term>Cyan</term><description></description></item>
	/// </list>
	/// These color values cannot be combined together to make new colors.
	/// </para>
	/// <para>
	/// The attributes can be any combination of the following:
	/// <list type="bullet">
	/// <item><term>Bright</term><description>foreground is brighter</description></item>
	/// <item><term>Dim</term><description>foreground is dimmer</description></item>
	/// <item><term>Underscore</term><description>message is underlined</description></item>
	/// <item><term>Blink</term><description>foreground is blinking (does not work on all terminals)</description></item>
	/// <item><term>Reverse</term><description>foreground and background are reversed</description></item>
	/// <item><term>Hidden</term><description>output is hidden</description></item>
	/// <item><term>Strikethrough</term><description>message has a line through it</description></item>
	/// </list>
	/// While any of these attributes may be combined together not all combinations
	/// work well together, for example setting both <i>Bright</i> and <i>Dim</i> attributes makes
	/// no sense.
	/// </para>
	/// </remarks>
	/// <author>Patrick Wagstrom</author>
	/// <author>Nicko Cadell</author>
	public class AnsiColorTerminalAppender : AppenderSkeleton
	{
		/// <summary>
		/// The enum of possible display attributes
		/// </summary>
		/// <remarks>
		/// <para>
		/// The following flags can be combined together to
		/// form the ANSI color attributes.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Appender.AnsiColorTerminalAppender" />
		[Flags]
		public enum AnsiAttributes
		{
			/// <summary>
			/// text is bright
			/// </summary>
			Bright = 0x1,
			/// <summary>
			/// text is dim
			/// </summary>
			Dim = 0x2,
			/// <summary>
			/// text is underlined
			/// </summary>
			Underscore = 0x4,
			/// <summary>
			/// text is blinking
			/// </summary>
			/// <remarks>
			/// Not all terminals support this attribute
			/// </remarks>
			Blink = 0x8,
			/// <summary>
			/// text and background colors are reversed
			/// </summary>
			Reverse = 0x10,
			/// <summary>
			/// text is hidden
			/// </summary>
			Hidden = 0x20,
			/// <summary>
			/// text is displayed with a strikethrough
			/// </summary>
			Strikethrough = 0x40
		}

		/// <summary>
		/// The enum of possible foreground or background color values for 
		/// use with the color mapping method
		/// </summary>
		/// <remarks>
		/// <para>
		/// The output can be in one for the following ANSI colors.
		/// </para>
		/// </remarks>
		/// <seealso cref="T:log4net.Appender.AnsiColorTerminalAppender" />
		public enum AnsiColor
		{
			/// <summary>
			/// color is black
			/// </summary>
			Black,
			/// <summary>
			/// color is red
			/// </summary>
			Red,
			/// <summary>
			/// color is green
			/// </summary>
			Green,
			/// <summary>
			/// color is yellow
			/// </summary>
			Yellow,
			/// <summary>
			/// color is blue
			/// </summary>
			Blue,
			/// <summary>
			/// color is magenta
			/// </summary>
			Magenta,
			/// <summary>
			/// color is cyan
			/// </summary>
			Cyan,
			/// <summary>
			/// color is white
			/// </summary>
			White
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
			private AnsiColor m_foreColor;

			private AnsiColor m_backColor;

			private AnsiAttributes m_attributes;

			private string m_combinedColor = "";

			/// <summary>
			/// The mapped foreground color for the specified level
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The mapped foreground color for the specified level
			/// </para>
			/// </remarks>
			public AnsiColor ForeColor
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
			/// The mapped background color for the specified level
			/// </para>
			/// </remarks>
			public AnsiColor BackColor
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
			/// The color attributes for the specified level
			/// </summary>
			/// <remarks>
			/// <para>
			/// Required property.
			/// The color attributes for the specified level
			/// </para>
			/// </remarks>
			public AnsiAttributes Attributes
			{
				get
				{
					return m_attributes;
				}
				set
				{
					m_attributes = value;
				}
			}

			/// <summary>
			/// The combined <see cref="P:log4net.Appender.AnsiColorTerminalAppender.LevelColors.ForeColor" />, <see cref="P:log4net.Appender.AnsiColorTerminalAppender.LevelColors.BackColor" /> and
			/// <see cref="P:log4net.Appender.AnsiColorTerminalAppender.LevelColors.Attributes" /> suitable for setting the ansi terminal color.
			/// </summary>
			internal string CombinedColor => m_combinedColor;

			/// <summary>
			/// Initialize the options for the object
			/// </summary>
			/// <remarks>
			/// <para>
			/// Combine the <see cref="P:log4net.Appender.AnsiColorTerminalAppender.LevelColors.ForeColor" /> and <see cref="P:log4net.Appender.AnsiColorTerminalAppender.LevelColors.BackColor" /> together
			/// and append the attributes.
			/// </para>
			/// </remarks>
			public override void ActivateOptions()
			{
				base.ActivateOptions();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("\u001b[0;");
				stringBuilder.Append((int)(30 + m_foreColor));
				stringBuilder.Append(';');
				stringBuilder.Append((int)(40 + m_backColor));
				if ((m_attributes & AnsiAttributes.Bright) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";1");
				}
				if ((m_attributes & AnsiAttributes.Dim) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";2");
				}
				if ((m_attributes & AnsiAttributes.Underscore) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";4");
				}
				if ((m_attributes & AnsiAttributes.Blink) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";5");
				}
				if ((m_attributes & AnsiAttributes.Reverse) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";7");
				}
				if ((m_attributes & AnsiAttributes.Hidden) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";8");
				}
				if ((m_attributes & AnsiAttributes.Strikethrough) > (AnsiAttributes)0)
				{
					stringBuilder.Append(";9");
				}
				stringBuilder.Append('m');
				m_combinedColor = stringBuilder.ToString();
			}
		}

		/// <summary>
		/// The <see cref="P:log4net.Appender.AnsiColorTerminalAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.AnsiColorTerminalAppender.Target" /> to use when writing to the Console 
		/// standard output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleOut = "Console.Out";

		/// <summary>
		/// The <see cref="P:log4net.Appender.AnsiColorTerminalAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The <see cref="P:log4net.Appender.AnsiColorTerminalAppender.Target" /> to use when writing to the Console 
		/// standard error output stream.
		/// </para>
		/// </remarks>
		public const string ConsoleError = "Console.Error";

		/// <summary>
		/// Ansi code to reset terminal
		/// </summary>
		private const string PostEventCodes = "\u001b[0m";

		/// <summary>
		/// Flag to write output to the error stream rather than the standard output stream
		/// </summary>
		private bool m_writeToErrorStream = false;

		/// <summary>
		/// Mapping from level object to color value
		/// </summary>
		private LevelMapping m_levelMapping = new LevelMapping();

		/// <summary>
		/// Target is the value of the console output stream.
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
		/// Add a mapping of level to color
		/// </summary>
		/// <param name="mapping">The mapping to add</param>
		/// <remarks>
		/// <para>
		/// Add a <see cref="T:log4net.Appender.AnsiColorTerminalAppender.LevelColors" /> mapping to this appender.
		/// Each mapping defines the foreground and background colours
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
			string text = RenderLoggingEvent(loggingEvent);
			LevelColors levelColors = m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;
			if (levelColors != null)
			{
				text = levelColors.CombinedColor + text;
			}
			text = ((text.Length > 1) ? ((text.EndsWith("\r\n") || text.EndsWith("\n\r")) ? text.Insert(text.Length - 2, "\u001b[0m") : ((!text.EndsWith("\n") && !text.EndsWith("\r")) ? (text + "\u001b[0m") : text.Insert(text.Length - 1, "\u001b[0m"))) : ((text[0] != '\n' && text[0] != '\r') ? (text + "\u001b[0m") : ("\u001b[0m" + text)));
			if (m_writeToErrorStream)
			{
				Console.Error.Write(text);
			}
			else
			{
				Console.Write(text);
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
		}
	}
}
