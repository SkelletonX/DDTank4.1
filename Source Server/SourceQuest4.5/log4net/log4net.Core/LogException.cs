using System;
using System.Runtime.Serialization;

namespace log4net.Core
{
	/// <summary>
	/// Exception base type for log4net.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This type extends <see cref="T:System.ApplicationException" />. It
	/// does not add any new functionality but does differentiate the
	/// type of exception being thrown.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public class LogException : ApplicationException
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LogException" /> class.
		/// </para>
		/// </remarks>
		public LogException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LogException" /> class with
		/// the specified message.
		/// </para>
		/// </remarks>
		public LogException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">A message to include with the exception.</param>
		/// <param name="innerException">A nested exception to include.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LogException" /> class
		/// with the specified message and inner exception.
		/// </para>
		/// </remarks>
		public LogException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Serialization constructor
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Core.LogException" /> class 
		/// with serialized data.
		/// </para>
		/// </remarks>
		protected LogException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
