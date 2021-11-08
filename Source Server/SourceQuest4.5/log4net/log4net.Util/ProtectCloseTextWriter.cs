using System.IO;

namespace log4net.Util
{
	/// <summary>
	/// A <see cref="T:System.IO.TextWriter" /> that ignores the <see cref="M:log4net.Util.ProtectCloseTextWriter.Close" /> message
	/// </summary>
	/// <remarks>
	/// <para>
	/// This writer is used in special cases where it is necessary 
	/// to protect a writer from being closed by a client.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public class ProtectCloseTextWriter : TextWriterAdapter
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="writer">the writer to actually write to</param>
		/// <remarks>
		/// <para>
		/// Create a new ProtectCloseTextWriter using a writer
		/// </para>
		/// </remarks>
		public ProtectCloseTextWriter(TextWriter writer)
			: base(writer)
		{
		}

		/// <summary>
		/// Attach this instance to a different underlying <see cref="T:System.IO.TextWriter" />
		/// </summary>
		/// <param name="writer">the writer to attach to</param>
		/// <remarks>
		/// <para>
		/// Attach this instance to a different underlying <see cref="T:System.IO.TextWriter" />
		/// </para>
		/// </remarks>
		public void Attach(TextWriter writer)
		{
			base.Writer = writer;
		}

		/// <summary>
		/// Does not close the underlying output writer.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Does not close the underlying output writer.
		/// This method does nothing.
		/// </para>
		/// </remarks>
		public override void Close()
		{
		}
	}
}
