using System.Threading;

namespace log4net.Util
{
	/// <summary>
	/// Defines a lock that supports single writers and multiple readers
	/// </summary>
	/// <remarks>
	/// <para>
	/// <c>ReaderWriterLock</c> is used to synchronize access to a resource. 
	/// At any given time, it allows either concurrent read access for 
	/// multiple threads, or write access for a single thread. In a 
	/// situation where a resource is changed infrequently, a 
	/// <c>ReaderWriterLock</c> provides better throughput than a simple 
	/// one-at-a-time lock, such as <see cref="T:System.Threading.Monitor" />.
	/// </para>
	/// <para>
	/// If a platform does not support a <c>System.Threading.ReaderWriterLock</c> 
	/// implementation then all readers and writers are serialized. Therefore 
	/// the caller must not rely on multiple simultaneous readers.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class ReaderWriterLock
	{
		private System.Threading.ReaderWriterLock m_lock;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.ReaderWriterLock" /> class.
		/// </para>
		/// </remarks>
		public ReaderWriterLock()
		{
			m_lock = new System.Threading.ReaderWriterLock();
		}

		/// <summary>
		/// Acquires a reader lock
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="M:log4net.Util.ReaderWriterLock.AcquireReaderLock" /> blocks if a different thread has the writer 
		/// lock, or if at least one thread is waiting for the writer lock.
		/// </para>
		/// </remarks>
		public void AcquireReaderLock()
		{
			m_lock.AcquireReaderLock(-1);
		}

		/// <summary>
		/// Decrements the lock count
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="M:log4net.Util.ReaderWriterLock.ReleaseReaderLock" /> decrements the lock count. When the count 
		/// reaches zero, the lock is released.
		/// </para>
		/// </remarks>
		public void ReleaseReaderLock()
		{
			m_lock.ReleaseReaderLock();
		}

		/// <summary>
		/// Acquires the writer lock
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method blocks if another thread has a reader lock or writer lock.
		/// </para>
		/// </remarks>
		public void AcquireWriterLock()
		{
			m_lock.AcquireWriterLock(-1);
		}

		/// <summary>
		/// Decrements the lock count on the writer lock
		/// </summary>
		/// <remarks>
		/// <para>
		/// ReleaseWriterLock decrements the writer lock count. 
		/// When the count reaches zero, the writer lock is released.
		/// </para>
		/// </remarks>
		public void ReleaseWriterLock()
		{
			m_lock.ReleaseWriterLock();
		}
	}
}
