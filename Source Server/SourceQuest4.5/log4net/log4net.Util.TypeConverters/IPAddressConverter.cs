using System;
using System.Net;

namespace log4net.Util.TypeConverters
{
	/// <summary>
	/// Supports conversion from string to <see cref="T:System.Net.IPAddress" /> type.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Supports conversion from string to <see cref="T:System.Net.IPAddress" /> type.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:log4net.Util.TypeConverters.ConverterRegistry" />
	/// <seealso cref="T:log4net.Util.TypeConverters.IConvertFrom" />
	/// <author>Nicko Cadell</author>
	internal class IPAddressConverter : IConvertFrom
	{
		/// <summary>
		/// Valid characters in an IPv4 or IPv6 address string. (Does not support subnets)
		/// </summary>
		private static readonly char[] validIpAddressChars = new char[27]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'x',
			'X',
			'.',
			':',
			'%'
		};

		/// <summary>
		/// Can the source type be converted to the type supported by this object
		/// </summary>
		/// <param name="sourceType">the type to convert</param>
		/// <returns>true if the conversion is possible</returns>
		/// <remarks>
		/// <para>
		/// Returns <c>true</c> if the <paramref name="sourceType" /> is
		/// the <see cref="T:System.String" /> type.
		/// </para>
		/// </remarks>
		public bool CanConvertFrom(Type sourceType)
		{
			return (object)sourceType == typeof(string);
		}

		/// <summary>
		/// Overrides the ConvertFrom method of IConvertFrom.
		/// </summary>
		/// <param name="source">the object to convert to an IPAddress</param>
		/// <returns>the IPAddress</returns>
		/// <remarks>
		/// <para>
		/// Uses the <see cref="M:System.Net.IPAddress.Parse(System.String)" /> method to convert the
		/// <see cref="T:System.String" /> argument to an <see cref="T:System.Net.IPAddress" />.
		/// If that fails then the string is resolved as a DNS hostname.
		/// </para>
		/// </remarks>
		/// <exception cref="T:log4net.Util.TypeConverters.ConversionNotSupportedException">
		/// The <paramref name="source" /> object cannot be converted to the
		/// target type. To check for this condition use the <see cref="M:log4net.Util.TypeConverters.IPAddressConverter.CanConvertFrom(System.Type)" />
		/// method.
		/// </exception>
		public object ConvertFrom(object source)
		{
			string text = source as string;
			if (text != null && text.Length > 0)
			{
				try
				{
					IPHostEntry hostEntry = Dns.GetHostEntry(text);
					if (hostEntry != null && hostEntry.AddressList != null && hostEntry.AddressList.Length > 0 && hostEntry.AddressList[0] != null)
					{
						return hostEntry.AddressList[0];
					}
				}
				catch (Exception innerException)
				{
					throw ConversionNotSupportedException.Create(typeof(IPAddress), source, innerException);
				}
			}
			throw ConversionNotSupportedException.Create(typeof(IPAddress), source);
		}
	}
}
