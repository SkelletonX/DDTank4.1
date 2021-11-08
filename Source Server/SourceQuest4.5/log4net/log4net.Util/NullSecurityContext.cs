using log4net.Core;
using System;

namespace log4net.Util
{
	/// <summary>
	/// A SecurityContext used when a SecurityContext is not required
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="T:log4net.Util.NullSecurityContext" /> is a no-op implementation of the
	/// <see cref="T:log4net.Core.SecurityContext" /> base class. It is used where a <see cref="T:log4net.Core.SecurityContext" />
	/// is required but one has not been provided.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class NullSecurityContext : SecurityContext
	{
		/// <summary>
		/// Singleton instance of <see cref="T:log4net.Util.NullSecurityContext" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// Singleton instance of <see cref="T:log4net.Util.NullSecurityContext" />
		/// </para>
		/// </remarks>
		public static readonly NullSecurityContext Instance = new NullSecurityContext();

		/// <summary>
		/// Private constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Private constructor for singleton pattern.
		/// </para>
		/// </remarks>
		private NullSecurityContext()
		{
		}

		/// <summary>
		/// Impersonate this SecurityContext
		/// </summary>
		/// <param name="state">State supplied by the caller</param>
		/// <returns><c>null</c></returns>
		/// <remarks>
		/// <para>
		/// No impersonation is done and <c>null</c> is always returned.
		/// </para>
		/// </remarks>
		public override IDisposable Impersonate(object state)
		{
			return null;
		}
	}
}
