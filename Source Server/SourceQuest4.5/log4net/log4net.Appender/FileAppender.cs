using log4net.Core;
using log4net.Layout;
using log4net.Util;
using System;
using System.IO;
using System.Text;

namespace log4net.Appender
{
	/// <summary>
	/// Appends logging events to a file.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Logging events are sent to the file specified by
	/// the <see cref="P:log4net.Appender.FileAppender.File" /> property.
	/// </para>
	/// <para>
	/// The file can be opened in either append or overwrite mode 
	/// by specifying the <see cref="P:log4net.Appender.FileAppender.AppendToFile" /> property.
	/// If the file path is relative it is taken as relative from 
	/// the application base directory. The file encoding can be
	/// specified by setting the <see cref="P:log4net.Appender.FileAppender.Encoding" /> property.
	/// </para>
	/// <para>
	/// The layout's <see cref="P:log4net.Layout.ILayout.Header" /> and <see cref="P:log4net.Layout.ILayout.Footer" />
	/// values will be written each time the file is opened and closed
	/// respectively. If the <see cref="P:log4net.Appender.FileAppender.AppendToFile" /> property is <see langword="true" />
	/// then the file may contain multiple copies of the header and footer.
	/// </para>
	/// <para>
	/// This appender will first try to open the file for writing when <see cref="M:log4net.Appender.FileAppender.ActivateOptions" />
	/// is called. This will typically be during configuration.
	/// If the file cannot be opened for writing the appender will attempt
	/// to open the file again each time a message is logged to the appender.
	/// If the file cannot be opened for writing when a message is logged then
	/// the message will be discarded by this appender.
	/// </para>
	/// <para>
	/// The <see cref="T:log4net.Appender.FileAppender" /> supports pluggable file locking models via
	/// the <see cref="P:log4net.Appender.FileAppender.LockingModel" /> property.
	/// The default behavior, implemented by <see cref="T:log4net.Appender.FileAppender.ExclusiveLock" /> 
	/// is to obtain an exclusive write lock on the file until this appender is closed.
	/// The alternative model, <see cref="T:log4net.Appender.FileAppender.MinimalLock" />, only holds a
	/// write lock while the appender is writing a logging event.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	/// <author>Rodrigo B. de Oliveira</author>
	/// <author>Douglas de la Torre</author>
	/// <author>Niall Daley</author>
	public class FileAppender : TextWriterAppender
	{
		/// <summary>
		/// Write only <see cref="T:System.IO.Stream" /> that uses the <see cref="T:log4net.Appender.FileAppender.LockingModelBase" /> 
		/// to manage access to an underlying resource.
		/// </summary>
		private sealed class LockingStream : Stream, IDisposable
		{
			public sealed class LockStateException : LogException
			{
				public LockStateException(string message)
					: base(message)
				{
				}
			}

			private Stream m_realStream = null;

			private LockingModelBase m_lockingModel = null;

			private int m_readTotal = -1;

			private int m_lockLevel = 0;

			public override bool CanRead => false;

			public override bool CanSeek
			{
				get
				{
					AssertLocked();
					return m_realStream.CanSeek;
				}
			}

			public override bool CanWrite
			{
				get
				{
					AssertLocked();
					return m_realStream.CanWrite;
				}
			}

			public override long Length
			{
				get
				{
					AssertLocked();
					return m_realStream.Length;
				}
			}

			public override long Position
			{
				get
				{
					AssertLocked();
					return m_realStream.Position;
				}
				set
				{
					AssertLocked();
					m_realStream.Position = value;
				}
			}

			public LockingStream(LockingModelBase locking)
			{
				if (locking == null)
				{
					throw new ArgumentException("Locking model may not be null", "locking");
				}
				m_lockingModel = locking;
			}

			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				AssertLocked();
				IAsyncResult asyncResult = m_realStream.BeginRead(buffer, offset, count, callback, state);
				m_readTotal = EndRead(asyncResult);
				return asyncResult;
			}

			/// <summary>
			/// True asynchronous writes are not supported, the implementation forces a synchronous write.
			/// </summary>
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
			{
				AssertLocked();
				IAsyncResult asyncResult = m_realStream.BeginWrite(buffer, offset, count, callback, state);
				EndWrite(asyncResult);
				return asyncResult;
			}

			public override void Close()
			{
				m_lockingModel.CloseFile();
			}

			public override int EndRead(IAsyncResult asyncResult)
			{
				AssertLocked();
				return m_readTotal;
			}

			public override void EndWrite(IAsyncResult asyncResult)
			{
			}

			public override void Flush()
			{
				AssertLocked();
				m_realStream.Flush();
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return m_realStream.Read(buffer, offset, count);
			}

			public override int ReadByte()
			{
				return m_realStream.ReadByte();
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				AssertLocked();
				return m_realStream.Seek(offset, origin);
			}

			public override void SetLength(long value)
			{
				AssertLocked();
				m_realStream.SetLength(value);
			}

			void IDisposable.Dispose()
			{
				Close();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				AssertLocked();
				m_realStream.Write(buffer, offset, count);
			}

			public override void WriteByte(byte value)
			{
				AssertLocked();
				m_realStream.WriteByte(value);
			}

			private void AssertLocked()
			{
				if (m_realStream == null)
				{
					throw new LockStateException("The file is not currently locked");
				}
			}

			public bool AcquireLock()
			{
				bool result = false;
				lock (this)
				{
					if (m_lockLevel == 0)
					{
						m_realStream = m_lockingModel.AcquireLock();
					}
					if (m_realStream != null)
					{
						m_lockLevel++;
						result = true;
					}
				}
				return result;
			}

			public void ReleaseLock()
			{
				lock (this)
				{
					m_lockLevel--;
					if (m_lockLevel == 0)
					{
						m_lockingModel.ReleaseLock();
						m_realStream = null;
					}
				}
			}
		}

		/// <summary>
		/// Locking model base class
		/// </summary>
		/// <remarks>
		/// <para>
		/// Base class for the locking models available to the <see cref="T:log4net.Appender.FileAppender" /> derived loggers.
		/// </para>
		/// </remarks>
		public abstract class LockingModelBase
		{
			private FileAppender m_appender = null;

			/// <summary>
			/// Gets or sets the <see cref="T:log4net.Appender.FileAppender" /> for this LockingModel
			/// </summary>
			/// <value>
			/// The <see cref="T:log4net.Appender.FileAppender" /> for this LockingModel
			/// </value>
			/// <remarks>
			/// <para>
			/// The file appender this locking model is attached to and working on
			/// behalf of.
			/// </para>
			/// <para>
			/// The file appender is used to locate the security context and the error handler to use.
			/// </para>
			/// <para>
			/// The value of this property will be set before <see cref="M:log4net.Appender.FileAppender.LockingModelBase.OpenFile(System.String,System.Boolean,System.Text.Encoding)" /> is
			/// called.
			/// </para>
			/// </remarks>
			public FileAppender CurrentAppender
			{
				get
				{
					return m_appender;
				}
				set
				{
					m_appender = value;
				}
			}

			/// <summary>
			/// Open the output file
			/// </summary>
			/// <param name="filename">The filename to use</param>
			/// <param name="append">Whether to append to the file, or overwrite</param>
			/// <param name="encoding">The encoding to use</param>
			/// <remarks>
			/// <para>
			/// Open the file specified and prepare for logging. 
			/// No writes will be made until <see cref="M:log4net.Appender.FileAppender.LockingModelBase.AcquireLock" /> is called.
			/// Must be called before any calls to <see cref="M:log4net.Appender.FileAppender.LockingModelBase.AcquireLock" />,
			/// <see cref="M:log4net.Appender.FileAppender.LockingModelBase.ReleaseLock" /> and <see cref="M:log4net.Appender.FileAppender.LockingModelBase.CloseFile" />.
			/// </para>
			/// </remarks>
			public abstract void OpenFile(string filename, bool append, Encoding encoding);

			/// <summary>
			/// Close the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Close the file. No further writes will be made.
			/// </para>
			/// </remarks>
			public abstract void CloseFile();

			/// <summary>
			/// Acquire the lock on the file
			/// </summary>
			/// <returns>A stream that is ready to be written to.</returns>
			/// <remarks>
			/// <para>
			/// Acquire the lock on the file in preparation for writing to it. 
			/// Return a stream pointing to the file. <see cref="M:log4net.Appender.FileAppender.LockingModelBase.ReleaseLock" />
			/// must be called to release the lock on the output file.
			/// </para>
			/// </remarks>
			public abstract Stream AcquireLock();

			/// <summary>
			/// Release the lock on the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Release the lock on the file. No further writes will be made to the 
			/// stream until <see cref="M:log4net.Appender.FileAppender.LockingModelBase.AcquireLock" /> is called again.
			/// </para>
			/// </remarks>
			public abstract void ReleaseLock();
		}

		/// <summary>
		/// Hold an exclusive lock on the output file
		/// </summary>
		/// <remarks>
		/// <para>
		/// Open the file once for writing and hold it open until <see cref="M:log4net.Appender.FileAppender.ExclusiveLock.CloseFile" /> is called. 
		/// Maintains an exclusive lock on the file during this time.
		/// </para>
		/// </remarks>
		public class ExclusiveLock : LockingModelBase
		{
			private Stream m_stream = null;

			/// <summary>
			/// Open the file specified and prepare for logging.
			/// </summary>
			/// <param name="filename">The filename to use</param>
			/// <param name="append">Whether to append to the file, or overwrite</param>
			/// <param name="encoding">The encoding to use</param>
			/// <remarks>
			/// <para>
			/// Open the file specified and prepare for logging. 
			/// No writes will be made until <see cref="M:log4net.Appender.FileAppender.ExclusiveLock.AcquireLock" /> is called.
			/// Must be called before any calls to <see cref="M:log4net.Appender.FileAppender.ExclusiveLock.AcquireLock" />,
			/// <see cref="M:log4net.Appender.FileAppender.ExclusiveLock.ReleaseLock" /> and <see cref="M:log4net.Appender.FileAppender.ExclusiveLock.CloseFile" />.
			/// </para>
			/// </remarks>
			public override void OpenFile(string filename, bool append, Encoding encoding)
			{
				try
				{
					using (base.CurrentAppender.SecurityContext.Impersonate(this))
					{
						string directoryName = Path.GetDirectoryName(filename);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						FileMode mode = append ? FileMode.Append : FileMode.Create;
						m_stream = new FileStream(filename, mode, FileAccess.Write, FileShare.Read);
					}
				}
				catch (Exception ex)
				{
					base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + filename + ". " + ex.Message);
				}
			}

			/// <summary>
			/// Close the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Close the file. No further writes will be made.
			/// </para>
			/// </remarks>
			public override void CloseFile()
			{
				using (base.CurrentAppender.SecurityContext.Impersonate(this))
				{
					m_stream.Close();
				}
			}

			/// <summary>
			/// Acquire the lock on the file
			/// </summary>
			/// <returns>A stream that is ready to be written to.</returns>
			/// <remarks>
			/// <para>
			/// Does nothing. The lock is already taken
			/// </para>
			/// </remarks>
			public override Stream AcquireLock()
			{
				return m_stream;
			}

			/// <summary>
			/// Release the lock on the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Does nothing. The lock will be released when the file is closed.
			/// </para>
			/// </remarks>
			public override void ReleaseLock()
			{
			}
		}

		/// <summary>
		/// Acquires the file lock for each write
		/// </summary>
		/// <remarks>
		/// <para>
		/// Opens the file once for each <see cref="M:log4net.Appender.FileAppender.MinimalLock.AcquireLock" />/<see cref="M:log4net.Appender.FileAppender.MinimalLock.ReleaseLock" /> cycle, 
		/// thus holding the lock for the minimal amount of time. This method of locking
		/// is considerably slower than <see cref="T:log4net.Appender.FileAppender.ExclusiveLock" /> but allows 
		/// other processes to move/delete the log file whilst logging continues.
		/// </para>
		/// </remarks>
		public class MinimalLock : LockingModelBase
		{
			private string m_filename;

			private bool m_append;

			private Stream m_stream = null;

			/// <summary>
			/// Prepares to open the file when the first message is logged.
			/// </summary>
			/// <param name="filename">The filename to use</param>
			/// <param name="append">Whether to append to the file, or overwrite</param>
			/// <param name="encoding">The encoding to use</param>
			/// <remarks>
			/// <para>
			/// Open the file specified and prepare for logging. 
			/// No writes will be made until <see cref="M:log4net.Appender.FileAppender.MinimalLock.AcquireLock" /> is called.
			/// Must be called before any calls to <see cref="M:log4net.Appender.FileAppender.MinimalLock.AcquireLock" />,
			/// <see cref="M:log4net.Appender.FileAppender.MinimalLock.ReleaseLock" /> and <see cref="M:log4net.Appender.FileAppender.MinimalLock.CloseFile" />.
			/// </para>
			/// </remarks>
			public override void OpenFile(string filename, bool append, Encoding encoding)
			{
				m_filename = filename;
				m_append = append;
			}

			/// <summary>
			/// Close the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Close the file. No further writes will be made.
			/// </para>
			/// </remarks>
			public override void CloseFile()
			{
			}

			/// <summary>
			/// Acquire the lock on the file
			/// </summary>
			/// <returns>A stream that is ready to be written to.</returns>
			/// <remarks>
			/// <para>
			/// Acquire the lock on the file in preparation for writing to it. 
			/// Return a stream pointing to the file. <see cref="M:log4net.Appender.FileAppender.MinimalLock.ReleaseLock" />
			/// must be called to release the lock on the output file.
			/// </para>
			/// </remarks>
			public override Stream AcquireLock()
			{
				if (m_stream == null)
				{
					try
					{
						using (base.CurrentAppender.SecurityContext.Impersonate(this))
						{
							string directoryName = Path.GetDirectoryName(m_filename);
							if (!Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							FileMode mode = m_append ? FileMode.Append : FileMode.Create;
							m_stream = new FileStream(m_filename, mode, FileAccess.Write, FileShare.Read);
							m_append = true;
						}
					}
					catch (Exception ex)
					{
						base.CurrentAppender.ErrorHandler.Error("Unable to acquire lock on file " + m_filename + ". " + ex.Message);
					}
				}
				return m_stream;
			}

			/// <summary>
			/// Release the lock on the file
			/// </summary>
			/// <remarks>
			/// <para>
			/// Release the lock on the file. No further writes will be made to the 
			/// stream until <see cref="M:log4net.Appender.FileAppender.MinimalLock.AcquireLock" /> is called again.
			/// </para>
			/// </remarks>
			public override void ReleaseLock()
			{
				using (base.CurrentAppender.SecurityContext.Impersonate(this))
				{
					m_stream.Close();
					m_stream = null;
				}
			}
		}

		/// <summary>
		/// Flag to indicate if we should append to the file
		/// or overwrite the file. The default is to append.
		/// </summary>
		private bool m_appendToFile = true;

		/// <summary>
		/// The name of the log file.
		/// </summary>
		private string m_fileName = null;

		/// <summary>
		/// The encoding to use for the file stream.
		/// </summary>
		private Encoding m_encoding = Encoding.Default;

		/// <summary>
		/// The security context to use for privileged calls
		/// </summary>
		private SecurityContext m_securityContext;

		/// <summary>
		/// The stream to log to. Has added locking semantics
		/// </summary>
		private LockingStream m_stream = null;

		/// <summary>
		/// The locking model to use
		/// </summary>
		private LockingModelBase m_lockingModel = new ExclusiveLock();

		/// <summary>
		/// Gets or sets the path to the file that logging will be written to.
		/// </summary>
		/// <value>
		/// The path to the file that logging will be written to.
		/// </value>
		/// <remarks>
		/// <para>
		/// If the path is relative it is taken as relative from 
		/// the application base directory.
		/// </para>
		/// </remarks>
		public virtual string File
		{
			get
			{
				return m_fileName;
			}
			set
			{
				m_fileName = value;
			}
		}

		/// <summary>
		/// Gets or sets a flag that indicates whether the file should be
		/// appended to or overwritten.
		/// </summary>
		/// <value>
		/// Indicates whether the file should be appended to or overwritten.
		/// </value>
		/// <remarks>
		/// <para>
		/// If the value is set to false then the file will be overwritten, if 
		/// it is set to true then the file will be appended to.
		/// </para>
		/// The default value is true.
		/// </remarks>
		public bool AppendToFile
		{
			get
			{
				return m_appendToFile;
			}
			set
			{
				m_appendToFile = value;
			}
		}

		/// <summary>
		/// Gets or sets <see cref="P:log4net.Appender.FileAppender.Encoding" /> used to write to the file.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.FileAppender.Encoding" /> used to write to the file.
		/// </value>
		/// <remarks>
		/// <para>
		/// The default encoding set is <see cref="P:System.Text.Encoding.Default" />
		/// which is the encoding for the system's current ANSI code page.
		/// </para>
		/// </remarks>
		public Encoding Encoding
		{
			get
			{
				return m_encoding;
			}
			set
			{
				m_encoding = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Appender.FileAppender.SecurityContext" /> used to write to the file.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.FileAppender.SecurityContext" /> used to write to the file.
		/// </value>
		/// <remarks>
		/// <para>
		/// Unless a <see cref="P:log4net.Appender.FileAppender.SecurityContext" /> specified here for this appender
		/// the <see cref="P:log4net.Core.SecurityContextProvider.DefaultProvider" /> is queried for the
		/// security context to use. The default behavior is to use the security context
		/// of the current thread.
		/// </para>
		/// </remarks>
		public SecurityContext SecurityContext
		{
			get
			{
				return m_securityContext;
			}
			set
			{
				m_securityContext = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="P:log4net.Appender.FileAppender.LockingModel" /> used to handle locking of the file.
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Appender.FileAppender.LockingModel" /> used to lock the file.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the <see cref="P:log4net.Appender.FileAppender.LockingModel" /> used to handle locking of the file.
		/// </para>
		/// <para>
		/// There are two built in locking models, <see cref="T:log4net.Appender.FileAppender.ExclusiveLock" /> and <see cref="T:log4net.Appender.FileAppender.MinimalLock" />.
		/// The former locks the file from the start of logging to the end and the 
		/// later lock only for the minimal amount of time when logging each message.
		/// </para>
		/// <para>
		/// The default locking model is the <see cref="T:log4net.Appender.FileAppender.ExclusiveLock" />.
		/// </para>
		/// </remarks>
		public LockingModelBase LockingModel
		{
			get
			{
				return m_lockingModel;
			}
			set
			{
				m_lockingModel = value;
			}
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Default constructor
		/// </para>
		/// </remarks>
		public FileAppender()
		{
		}

		/// <summary>
		/// Construct a new appender using the layout, file and append mode.
		/// </summary>
		/// <param name="layout">the layout to use with this appender</param>
		/// <param name="filename">the full path to the file to write to</param>
		/// <param name="append">flag to indicate if the file should be appended to</param>
		/// <remarks>
		/// <para>
		/// Obsolete constructor.
		/// </para>
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout, File & AppendToFile properties")]
		public FileAppender(ILayout layout, string filename, bool append)
		{
			Layout = layout;
			File = filename;
			AppendToFile = append;
			ActivateOptions();
		}

		/// <summary>
		/// Construct a new appender using the layout and file specified.
		/// The file will be appended to.
		/// </summary>
		/// <param name="layout">the layout to use with this appender</param>
		/// <param name="filename">the full path to the file to write to</param>
		/// <remarks>
		/// <para>
		/// Obsolete constructor.
		/// </para>
		/// </remarks>
		[Obsolete("Instead use the default constructor and set the Layout & File properties")]
		public FileAppender(ILayout layout, string filename)
			: this(layout, filename, append: true)
		{
		}

		/// <summary>
		/// Activate the options on the file appender. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Appender.FileAppender.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Appender.FileAppender.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Appender.FileAppender.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// This will cause the file to be opened.
		/// </para>
		/// </remarks>
		public override void ActivateOptions()
		{
			base.ActivateOptions();
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			if (m_lockingModel == null)
			{
				m_lockingModel = new ExclusiveLock();
			}
			m_lockingModel.CurrentAppender = this;
			using (SecurityContext.Impersonate(this))
			{
				m_fileName = ConvertToFullPath(m_fileName.Trim());
			}
			if (m_fileName != null)
			{
				SafeOpenFile(m_fileName, m_appendToFile);
				return;
			}
			LogLog.Warn("FileAppender: File option not set for appender [" + base.Name + "].");
			LogLog.Warn("FileAppender: Are you using FileAppender instead of ConsoleAppender?");
		}

		/// <summary>
		/// Closes any previously opened file and calls the parent's <see cref="M:log4net.Appender.TextWriterAppender.Reset" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Resets the filename and the file stream.
		/// </para>
		/// </remarks>
		protected override void Reset()
		{
			base.Reset();
			m_fileName = null;
		}

		/// <summary>
		/// Called to initialize the file writer
		/// </summary>
		/// <remarks>
		/// <para>
		/// Will be called for each logged message until the file is
		/// successfully opened.
		/// </para>
		/// </remarks>
		protected override void PrepareWriter()
		{
			SafeOpenFile(m_fileName, m_appendToFile);
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent)" />
		/// method. 
		/// </summary>
		/// <param name="loggingEvent">The event to log.</param>
		/// <remarks>
		/// <para>
		/// Writes a log statement to the output stream if the output stream exists 
		/// and is writable.  
		/// </para>
		/// <para>
		/// The format of the output will depend on the appender's layout.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent loggingEvent)
		{
			if (m_stream.AcquireLock())
			{
				try
				{
					base.Append(loggingEvent);
				}
				finally
				{
					m_stream.ReleaseLock();
				}
			}
		}

		/// <summary>
		/// This method is called by the <see cref="M:log4net.Appender.AppenderSkeleton.DoAppend(log4net.Core.LoggingEvent[])" />
		/// method. 
		/// </summary>
		/// <param name="loggingEvents">The array of events to log.</param>
		/// <remarks>
		/// <para>
		/// Acquires the output file locks once before writing all the events to
		/// the stream.
		/// </para>
		/// </remarks>
		protected override void Append(LoggingEvent[] loggingEvents)
		{
			if (m_stream.AcquireLock())
			{
				try
				{
					base.Append(loggingEvents);
				}
				finally
				{
					m_stream.ReleaseLock();
				}
			}
		}

		/// <summary>
		/// Writes a footer as produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Footer" /> property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes a footer as produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Footer" /> property.
		/// </para>
		/// </remarks>
		protected override void WriteFooter()
		{
			if (m_stream != null)
			{
				m_stream.AcquireLock();
				try
				{
					base.WriteFooter();
				}
				finally
				{
					m_stream.ReleaseLock();
				}
			}
		}

		/// <summary>
		/// Writes a header produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Header" /> property.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes a header produced by the embedded layout's <see cref="P:log4net.Layout.ILayout.Header" /> property.
		/// </para>
		/// </remarks>
		protected override void WriteHeader()
		{
			if (m_stream != null && m_stream.AcquireLock())
			{
				try
				{
					base.WriteHeader();
				}
				finally
				{
					m_stream.ReleaseLock();
				}
			}
		}

		/// <summary>
		/// Closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Closes the underlying <see cref="T:System.IO.TextWriter" />.
		/// </para>
		/// </remarks>
		protected override void CloseWriter()
		{
			if (m_stream != null)
			{
				m_stream.AcquireLock();
				try
				{
					base.CloseWriter();
				}
				finally
				{
					m_stream.ReleaseLock();
				}
			}
		}

		/// <summary>
		/// Closes the previously opened file.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="P:log4net.Layout.ILayout.Footer" /> to the file and then
		/// closes the file.
		/// </para>
		/// </remarks>
		protected void CloseFile()
		{
			WriteFooterAndCloseWriter();
		}

		/// <summary>
		/// Sets and <i>opens</i> the file where the log output will go. The specified file must be writable.
		/// </summary>
		/// <param name="fileName">The path to the log file. Must be a fully qualified path.</param>
		/// <param name="append">If true will append to fileName. Otherwise will truncate fileName</param>
		/// <remarks>
		/// <para>
		/// Calls <see cref="M:log4net.Appender.FileAppender.OpenFile(System.String,System.Boolean)" /> but guarantees not to throw an exception.
		/// Errors are passed to the <see cref="P:log4net.Appender.TextWriterAppender.ErrorHandler" />.
		/// </para>
		/// </remarks>
		protected virtual void SafeOpenFile(string fileName, bool append)
		{
			try
			{
				OpenFile(fileName, append);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("OpenFile(" + fileName + "," + append + ") call failed.", e, ErrorCode.FileOpenFailure);
			}
		}

		/// <summary>
		/// Sets and <i>opens</i> the file where the log output will go. The specified file must be writable.
		/// </summary>
		/// <param name="fileName">The path to the log file. Must be a fully qualified path.</param>
		/// <param name="append">If true will append to fileName. Otherwise will truncate fileName</param>
		/// <remarks>
		/// <para>
		/// If there was already an opened file, then the previous file
		/// is closed first.
		/// </para>
		/// <para>
		/// This method will ensure that the directory structure
		/// for the <paramref name="fileName" /> specified exists.
		/// </para>
		/// </remarks>
		protected virtual void OpenFile(string fileName, bool append)
		{
			if (LogLog.IsErrorEnabled)
			{
				bool flag = false;
				using (SecurityContext.Impersonate(this))
				{
					flag = Path.IsPathRooted(fileName);
				}
				if (!flag)
				{
					LogLog.Error("FileAppender: INTERNAL ERROR. OpenFile(" + fileName + "): File name is not fully qualified.");
				}
			}
			lock (this)
			{
				Reset();
				LogLog.Debug("FileAppender: Opening file for writing [" + fileName + "] append [" + append + "]");
				m_fileName = fileName;
				m_appendToFile = append;
				LockingModel.CurrentAppender = this;
				LockingModel.OpenFile(fileName, append, m_encoding);
				m_stream = new LockingStream(LockingModel);
				if (m_stream != null)
				{
					m_stream.AcquireLock();
					try
					{
						SetQWForFiles(new StreamWriter(m_stream, m_encoding));
					}
					finally
					{
						m_stream.ReleaseLock();
					}
				}
				WriteHeader();
			}
		}

		/// <summary>
		/// Sets the quiet writer used for file output
		/// </summary>
		/// <param name="fileStream">the file stream that has been opened for writing</param>
		/// <remarks>
		/// <para>
		/// This implementation of <see cref="M:log4net.Appender.FileAppender.SetQWForFiles(System.IO.Stream)" /> creates a <see cref="T:System.IO.StreamWriter" />
		/// over the <paramref name="fileStream" /> and passes it to the 
		/// <see cref="M:log4net.Appender.FileAppender.SetQWForFiles(System.IO.TextWriter)" /> method.
		/// </para>
		/// <para>
		/// This method can be overridden by sub classes that want to wrap the
		/// <see cref="T:System.IO.Stream" /> in some way, for example to encrypt the output
		/// data using a <c>System.Security.Cryptography.CryptoStream</c>.
		/// </para>
		/// </remarks>
		protected virtual void SetQWForFiles(Stream fileStream)
		{
			SetQWForFiles(new StreamWriter(fileStream, m_encoding));
		}

		/// <summary>
		/// Sets the quiet writer being used.
		/// </summary>
		/// <param name="writer">the writer over the file stream that has been opened for writing</param>
		/// <remarks>
		/// <para>
		/// This method can be overridden by sub classes that want to
		/// wrap the <see cref="T:System.IO.TextWriter" /> in some way.
		/// </para>
		/// </remarks>
		protected virtual void SetQWForFiles(TextWriter writer)
		{
			base.QuietWriter = new QuietTextWriter(writer, ErrorHandler);
		}

		/// <summary>
		/// Convert a path into a fully qualified path.
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
		/// </remarks>
		protected static string ConvertToFullPath(string path)
		{
			return SystemInfo.ConvertToFullPath(path);
		}
	}
}
