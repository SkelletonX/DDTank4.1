using log4net.Core;
using System;
using System.IO;

namespace log4net.Util.PatternStringConverters
{
	/// <summary>
	/// A Pattern converter that generates a string of random characters
	/// </summary>
	/// <remarks>
	/// <para>
	/// The converter generates a string of random characters. By default
	/// the string is length 4. This can be changed by setting the <see cref="P:log4net.Util.PatternConverter.Option" />
	/// to the string value of the length required.
	/// </para>
	/// <para>
	/// The random characters in the string are limited to uppercase letters
	/// and numbers only.
	/// </para>
	/// <para>
	/// The random number generator used by this class is not cryptographically secure.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class RandomStringPatternConverter : PatternConverter, IOptionHandler
	{
		/// <summary>
		/// Shared random number generator
		/// </summary>
		private static readonly Random s_random = new Random();

		/// <summary>
		/// Length of random string to generate. Default length 4.
		/// </summary>
		private int m_length = 4;

		/// <summary>
		/// Initialize the converter options
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Util.PatternStringConverters.RandomStringPatternConverter.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Util.PatternStringConverters.RandomStringPatternConverter.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Util.PatternStringConverters.RandomStringPatternConverter.ActivateOptions" /> must be called again.
		/// </para>
		/// </remarks>
		public void ActivateOptions()
		{
			string option = Option;
			if (option != null && option.Length > 0)
			{
				if (SystemInfo.TryParse(option, out int val))
				{
					m_length = val;
				}
				else
				{
					LogLog.Error("RandomStringPatternConverter: Could not convert Option [" + option + "] to Length Int32");
				}
			}
		}

		/// <summary>
		/// Write a randoim string to the output
		/// </summary>
		/// <param name="writer">the writer to write to</param>
		/// <param name="state">null, state is not set</param>
		/// <remarks>
		/// <para>
		/// Write a randoim string to the output <paramref name="writer" />.
		/// </para>
		/// </remarks>
		protected override void Convert(TextWriter writer, object state)
		{
			try
			{
				lock (s_random)
				{
					for (int i = 0; i < m_length; i++)
					{
						int num = s_random.Next(36);
						if (num < 26)
						{
							char value = (char)(65 + num);
							writer.Write(value);
						}
						else if (num < 36)
						{
							char value = (char)(48 + (num - 26));
							writer.Write(value);
						}
						else
						{
							writer.Write('X');
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogLog.Error("RandomStringPatternConverter: Error occurred while converting.", exception);
			}
		}
	}
}
