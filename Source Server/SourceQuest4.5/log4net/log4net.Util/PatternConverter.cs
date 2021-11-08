using log4net.Repository;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace log4net.Util
{
	/// <summary>
	/// Abstract class that provides the formatting functionality that 
	/// derived classes need.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Conversion specifiers in a conversion patterns are parsed to
	/// individual PatternConverters. Each of which is responsible for
	/// converting a logging event in a converter specific manner.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public abstract class PatternConverter
	{
		/// <summary>
		/// Initial buffer size
		/// </summary>
		private const int c_renderBufferSize = 256;

		/// <summary>
		/// Maximum buffer size before it is recycled
		/// </summary>
		private const int c_renderBufferMaxCapacity = 1024;

		private static readonly string[] SPACES = new string[6]
		{
			" ",
			"  ",
			"    ",
			"        ",
			"                ",
			"                                "
		};

		private PatternConverter m_next;

		private int m_min = -1;

		private int m_max = int.MaxValue;

		private bool m_leftAlign = false;

		/// <summary>
		/// The option string to the converter
		/// </summary>
		private string m_option = null;

		private ReusableStringWriter m_formatWriter = new ReusableStringWriter(CultureInfo.InvariantCulture);

		/// <summary>
		/// Get the next pattern converter in the chain
		/// </summary>
		/// <value>
		/// the next pattern converter in the chain
		/// </value>
		/// <remarks>
		/// <para>
		/// Get the next pattern converter in the chain
		/// </para>
		/// </remarks>
		public virtual PatternConverter Next => m_next;

		/// <summary>
		/// Gets or sets the formatting info for this converter
		/// </summary>
		/// <value>
		/// The formatting info for this converter
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the formatting info for this converter
		/// </para>
		/// </remarks>
		public virtual FormattingInfo FormattingInfo
		{
			get
			{
				return new FormattingInfo(m_min, m_max, m_leftAlign);
			}
			set
			{
				m_min = value.Min;
				m_max = value.Max;
				m_leftAlign = value.LeftAlign;
			}
		}

		/// <summary>
		/// Gets or sets the option value for this converter
		/// </summary>
		/// <summary>
		/// The option for this converter
		/// </summary>
		/// <remarks>
		/// <para>
		/// Gets or sets the option value for this converter
		/// </para>
		/// </remarks>
		public virtual string Option
		{
			get
			{
				return m_option;
			}
			set
			{
				m_option = value;
			}
		}

		/// <summary>
		/// Evaluate this pattern converter and write the output to a writer.
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="state">The state object on which the pattern converter should be executed.</param>
		/// <remarks>
		/// <para>
		/// Derived pattern converters must override this method in order to
		/// convert conversion specifiers in the appropriate way.
		/// </para>
		/// </remarks>
		protected abstract void Convert(TextWriter writer, object state);

		/// <summary>
		/// Set the next pattern converter in the chains
		/// </summary>
		/// <param name="patternConverter">the pattern converter that should follow this converter in the chain</param>
		/// <returns>the next converter</returns>
		/// <remarks>
		/// <para>
		/// The PatternConverter can merge with its neighbor during this method (or a sub class).
		/// Therefore the return value may or may not be the value of the argument passed in.
		/// </para>
		/// </remarks>
		public virtual PatternConverter SetNext(PatternConverter patternConverter)
		{
			m_next = patternConverter;
			return m_next;
		}

		/// <summary>
		/// Write the pattern converter to the writer with appropriate formatting
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> that will receive the formatted result.</param>
		/// <param name="state">The state object on which the pattern converter should be executed.</param>
		/// <remarks>
		/// <para>
		/// This method calls <see cref="M:log4net.Util.PatternConverter.Convert(System.IO.TextWriter,System.Object)" /> to allow the subclass to perform
		/// appropriate conversion of the pattern converter. If formatting options have
		/// been specified via the <see cref="P:log4net.Util.PatternConverter.FormattingInfo" /> then this method will
		/// apply those formattings before writing the output.
		/// </para>
		/// </remarks>
		public virtual void Format(TextWriter writer, object state)
		{
			if (m_min < 0 && m_max == int.MaxValue)
			{
				Convert(writer, state);
				return;
			}
			m_formatWriter.Reset(1024, 256);
			Convert(m_formatWriter, state);
			StringBuilder stringBuilder = m_formatWriter.GetStringBuilder();
			int length = stringBuilder.Length;
			if (length > m_max)
			{
				writer.Write(stringBuilder.ToString(length - m_max, m_max));
			}
			else if (length < m_min)
			{
				if (m_leftAlign)
				{
					writer.Write(stringBuilder.ToString());
					SpacePad(writer, m_min - length);
				}
				else
				{
					SpacePad(writer, m_min - length);
					writer.Write(stringBuilder.ToString());
				}
			}
			else
			{
				writer.Write(stringBuilder.ToString());
			}
		}

		/// <summary>
		/// Fast space padding method.
		/// </summary>
		/// <param name="writer"><see cref="T:System.IO.TextWriter" /> to which the spaces will be appended.</param>
		/// <param name="length">The number of spaces to be padded.</param>
		/// <remarks>
		/// <para>
		/// Fast space padding method.
		/// </para>
		/// </remarks>
		protected static void SpacePad(TextWriter writer, int length)
		{
			while (length >= 32)
			{
				writer.Write(SPACES[5]);
				length -= 32;
			}
			for (int num = 4; num >= 0; num--)
			{
				if ((length & (1 << num)) != 0)
				{
					writer.Write(SPACES[num]);
				}
			}
		}

		/// <summary>
		/// Write an dictionary to a <see cref="T:System.IO.TextWriter" />
		/// </summary>
		/// <param name="writer">the writer to write to</param>
		/// <param name="repository">a <see cref="T:log4net.Repository.ILoggerRepository" /> to use for object conversion</param>
		/// <param name="value">the value to write to the writer</param>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="T:System.Collections.IDictionary" /> to a writer in the form:
		/// </para>
		/// <code>
		/// {key1=value1, key2=value2, key3=value3}
		/// </code>
		/// <para>
		/// If the <see cref="T:log4net.Repository.ILoggerRepository" /> specified
		/// is not null then it is used to render the key and value to text, otherwise
		/// the object's ToString method is called.
		/// </para>
		/// </remarks>
		protected static void WriteDictionary(TextWriter writer, ILoggerRepository repository, IDictionary value)
		{
			writer.Write("{");
			bool flag = true;
			foreach (DictionaryEntry item in value)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					writer.Write(", ");
				}
				WriteObject(writer, repository, item.Key);
				writer.Write("=");
				WriteObject(writer, repository, item.Value);
			}
			writer.Write("}");
		}

		/// <summary>
		/// Write an object to a <see cref="T:System.IO.TextWriter" />
		/// </summary>
		/// <param name="writer">the writer to write to</param>
		/// <param name="repository">a <see cref="T:log4net.Repository.ILoggerRepository" /> to use for object conversion</param>
		/// <param name="value">the value to write to the writer</param>
		/// <remarks>
		/// <para>
		/// Writes the Object to a writer. If the <see cref="T:log4net.Repository.ILoggerRepository" /> specified
		/// is not null then it is used to render the object to text, otherwise
		/// the object's ToString method is called.
		/// </para>
		/// </remarks>
		protected static void WriteObject(TextWriter writer, ILoggerRepository repository, object value)
		{
			if (repository != null)
			{
				repository.RendererMap.FindAndRender(value, writer);
			}
			else if (value == null)
			{
				writer.Write(SystemInfo.NullText);
			}
			else
			{
				writer.Write(value.ToString());
			}
		}
	}
}
