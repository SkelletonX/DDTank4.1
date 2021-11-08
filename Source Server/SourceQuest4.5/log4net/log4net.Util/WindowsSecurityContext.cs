using log4net.Core;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace log4net.Util
{
	/// <summary>
	/// Impersonate a Windows Account
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="T:log4net.Core.SecurityContext" /> impersonates a Windows account.
	/// </para>
	/// <para>
	/// How the impersonation is done depends on the value of <see cref="M:log4net.Util.WindowsSecurityContext.Impersonate(System.Object)" />.
	/// This allows the context to either impersonate a set of user credentials specified 
	/// using username, domain name and password or to revert to the process credentials.
	/// </para>
	/// </remarks>
	public class WindowsSecurityContext : SecurityContext, IOptionHandler
	{
		/// <summary>
		/// The impersonation modes for the <see cref="T:log4net.Util.WindowsSecurityContext" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// See the <see cref="P:log4net.Util.WindowsSecurityContext.Credentials" /> property for
		/// details.
		/// </para>
		/// </remarks>
		public enum ImpersonationMode
		{
			/// <summary>
			/// Impersonate a user using the credentials supplied
			/// </summary>
			User,
			/// <summary>
			/// Revert this the thread to the credentials of the process
			/// </summary>
			Process
		}

		/// <summary>
		/// Adds <see cref="T:System.IDisposable" /> to <see cref="T:System.Security.Principal.WindowsImpersonationContext" />
		/// </summary>
		/// <remarks>
		/// <para>
		/// Helper class to expose the <see cref="T:System.Security.Principal.WindowsImpersonationContext" />
		/// through the <see cref="T:System.IDisposable" /> interface.
		/// </para>
		/// </remarks>
		private sealed class DisposableImpersonationContext : IDisposable
		{
			private readonly WindowsImpersonationContext m_impersonationContext;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="impersonationContext">the impersonation context being wrapped</param>
			/// <remarks>
			/// <para>
			/// Constructor
			/// </para>
			/// </remarks>
			public DisposableImpersonationContext(WindowsImpersonationContext impersonationContext)
			{
				m_impersonationContext = impersonationContext;
			}

			/// <summary>
			/// Revert the impersonation
			/// </summary>
			/// <remarks>
			/// <para>
			/// Revert the impersonation
			/// </para>
			/// </remarks>
			public void Dispose()
			{
				m_impersonationContext.Undo();
			}
		}

		private ImpersonationMode m_impersonationMode = ImpersonationMode.User;

		private string m_userName;

		private string m_domainName = Environment.MachineName;

		private string m_password;

		private WindowsIdentity m_identity;

		/// <summary>
		/// Gets or sets the impersonation mode for this security context
		/// </summary>
		/// <value>
		/// The impersonation mode for this security context
		/// </value>
		/// <remarks>
		/// <para>
		/// Impersonate either a user with user credentials or
		/// revert this thread to the credentials of the process.
		/// The value is one of the <see cref="T:log4net.Util.WindowsSecurityContext.ImpersonationMode" />
		/// enum.
		/// </para>
		/// <para>
		/// The default value is <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.User" />
		/// </para>
		/// <para>
		/// When the mode is set to <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.User" />
		/// the user's credentials are established using the
		/// <see cref="P:log4net.Util.WindowsSecurityContext.UserName" />, <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> and <see cref="P:log4net.Util.WindowsSecurityContext.Password" />
		/// values.
		/// </para>
		/// <para>
		/// When the mode is set to <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.Process" />
		/// no other properties need to be set. If the calling thread is 
		/// impersonating then it will be reverted back to the process credentials.
		/// </para>
		/// </remarks>
		public ImpersonationMode Credentials
		{
			get
			{
				return m_impersonationMode;
			}
			set
			{
				m_impersonationMode = value;
			}
		}

		/// <summary>
		/// Gets or sets the Windows username for this security context
		/// </summary>
		/// <value>
		/// The Windows username for this security context
		/// </value>
		/// <remarks>
		/// <para>
		/// This property must be set if <see cref="P:log4net.Util.WindowsSecurityContext.Credentials" />
		/// is set to <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.User" /> (the default setting).
		/// </para>
		/// </remarks>
		public string UserName
		{
			get
			{
				return m_userName;
			}
			set
			{
				m_userName = value;
			}
		}

		/// <summary>
		/// Gets or sets the Windows domain name for this security context
		/// </summary>
		/// <value>
		/// The Windows domain name for this security context
		/// </value>
		/// <remarks>
		/// <para>
		/// The default value for <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> is the local machine name
		/// taken from the <see cref="P:System.Environment.MachineName" /> property.
		/// </para>
		/// <para>
		/// This property must be set if <see cref="P:log4net.Util.WindowsSecurityContext.Credentials" />
		/// is set to <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.User" /> (the default setting).
		/// </para>
		/// </remarks>
		public string DomainName
		{
			get
			{
				return m_domainName;
			}
			set
			{
				m_domainName = value;
			}
		}

		/// <summary>
		/// Sets the password for the Windows account specified by the <see cref="P:log4net.Util.WindowsSecurityContext.UserName" /> and <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> properties.
		/// </summary>
		/// <value>
		/// The password for the Windows account specified by the <see cref="P:log4net.Util.WindowsSecurityContext.UserName" /> and <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> properties.
		/// </value>
		/// <remarks>
		/// <para>
		/// This property must be set if <see cref="P:log4net.Util.WindowsSecurityContext.Credentials" />
		/// is set to <see cref="F:log4net.Util.WindowsSecurityContext.ImpersonationMode.User" /> (the default setting).
		/// </para>
		/// </remarks>
		public string Password
		{
			set
			{
				m_password = value;
			}
		}

		/// <summary>
		/// Initialize the SecurityContext based on the options set.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is part of the <see cref="T:log4net.Core.IOptionHandler" /> delayed object
		/// activation scheme. The <see cref="M:log4net.Util.WindowsSecurityContext.ActivateOptions" /> method must 
		/// be called on this object after the configuration properties have
		/// been set. Until <see cref="M:log4net.Util.WindowsSecurityContext.ActivateOptions" /> is called this
		/// object is in an undefined state and must not be used. 
		/// </para>
		/// <para>
		/// If any of the configuration properties are modified then 
		/// <see cref="M:log4net.Util.WindowsSecurityContext.ActivateOptions" /> must be called again.
		/// </para>
		/// <para>
		/// The security context will try to Logon the specified user account and
		/// capture a primary token for impersonation.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The required <see cref="P:log4net.Util.WindowsSecurityContext.UserName" />, 
		/// <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> or <see cref="P:log4net.Util.WindowsSecurityContext.Password" /> properties were not specified.</exception>
		public void ActivateOptions()
		{
			if (m_impersonationMode == ImpersonationMode.User)
			{
				if (m_userName == null)
				{
					throw new ArgumentNullException("m_userName");
				}
				if (m_domainName == null)
				{
					throw new ArgumentNullException("m_domainName");
				}
				if (m_password == null)
				{
					throw new ArgumentNullException("m_password");
				}
				m_identity = LogonUser(m_userName, m_domainName, m_password);
			}
		}

		/// <summary>
		/// Impersonate the Windows account specified by the <see cref="P:log4net.Util.WindowsSecurityContext.UserName" /> and <see cref="P:log4net.Util.WindowsSecurityContext.DomainName" /> properties.
		/// </summary>
		/// <param name="state">caller provided state</param>
		/// <returns>
		/// An <see cref="T:System.IDisposable" /> instance that will revoke the impersonation of this SecurityContext
		/// </returns>
		/// <remarks>
		/// <para>
		/// Depending on the <see cref="P:log4net.Util.WindowsSecurityContext.Credentials" /> property either
		/// impersonate a user using credentials supplied or revert 
		/// to the process credentials.
		/// </para>
		/// </remarks>
		public override IDisposable Impersonate(object state)
		{
			if (m_impersonationMode == ImpersonationMode.User)
			{
				if (m_identity != null)
				{
					return new DisposableImpersonationContext(m_identity.Impersonate());
				}
			}
			else if (m_impersonationMode == ImpersonationMode.Process)
			{
				return new DisposableImpersonationContext(WindowsIdentity.Impersonate(IntPtr.Zero));
			}
			return null;
		}

		/// <summary>
		/// Create a <see cref="T:System.Security.Principal.WindowsIdentity" /> given the userName, domainName and password.
		/// </summary>
		/// <param name="userName">the user name</param>
		/// <param name="domainName">the domain name</param>
		/// <param name="password">the password</param>
		/// <returns>the <see cref="T:System.Security.Principal.WindowsIdentity" /> for the account specified</returns>
		/// <remarks>
		/// <para>
		/// Uses the Windows API call LogonUser to get a principal token for the account. This
		/// token is used to initialize the WindowsIdentity.
		/// </para>
		/// </remarks>
		private static WindowsIdentity LogonUser(string userName, string domainName, string password)
		{
			IntPtr phToken = IntPtr.Zero;
			if (!LogonUser(userName, domainName, password, 2, 0, ref phToken))
			{
				NativeError lastError = NativeError.GetLastError();
				throw new Exception("Failed to LogonUser [" + userName + "] in Domain [" + domainName + "]. Error: " + lastError.ToString());
			}
			IntPtr DuplicateTokenHandle = IntPtr.Zero;
			if (!DuplicateToken(phToken, 2, ref DuplicateTokenHandle))
			{
				NativeError lastError = NativeError.GetLastError();
				if (phToken != IntPtr.Zero)
				{
					CloseHandle(phToken);
				}
				throw new Exception("Failed to DuplicateToken after LogonUser. Error: " + lastError.ToString());
			}
			WindowsIdentity result = new WindowsIdentity(DuplicateTokenHandle);
			if (DuplicateTokenHandle != IntPtr.Zero)
			{
				CloseHandle(DuplicateTokenHandle);
			}
			if (phToken != IntPtr.Zero)
			{
				CloseHandle(phToken);
			}
			return result;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern bool CloseHandle(IntPtr handle);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DuplicateToken(IntPtr ExistingTokenHandle, int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
	}
}
