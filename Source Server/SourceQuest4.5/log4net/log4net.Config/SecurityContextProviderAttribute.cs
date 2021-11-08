using log4net.Core;
using log4net.Repository;
using log4net.Util;
using System;
using System.Reflection;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute to configure the <see cref="T:log4net.Core.SecurityContextProvider" />.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute may only be used at the assembly scope and can only
	/// be used once per assembly.
	/// </para>
	/// <para>
	/// Use this attribute to configure the <see cref="T:log4net.Config.XmlConfigurator" />
	/// without calling one of the <see cref="M:log4net.Config.XmlConfigurator.Configure" />
	/// methods.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class SecurityContextProviderAttribute : ConfiguratorAttribute
	{
		private Type m_providerType = null;

		/// <summary>
		/// Gets or sets the type of the provider to use.
		/// </summary>
		/// <value>
		/// the type of the provider to use.
		/// </value>
		/// <remarks>
		/// <para>
		/// The provider specified must subclass the <see cref="T:log4net.Core.SecurityContextProvider" />
		/// class.
		/// </para>
		/// </remarks>
		public Type ProviderType
		{
			get
			{
				return m_providerType;
			}
			set
			{
				m_providerType = value;
			}
		}

		/// <summary>
		/// Construct provider attribute with type specified
		/// </summary>
		/// <param name="providerType">the type of the provider to use</param>
		/// <remarks>
		/// <para>
		/// The provider specified must subclass the <see cref="T:log4net.Core.SecurityContextProvider" />
		/// class.
		/// </para>
		/// </remarks>
		public SecurityContextProviderAttribute(Type providerType)
			: base(100)
		{
			m_providerType = providerType;
		}

		/// <summary>
		/// Configures the SecurityContextProvider
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		/// <remarks>
		/// <para>
		/// Creates a provider instance from the <see cref="P:log4net.Config.SecurityContextProviderAttribute.ProviderType" /> specified.
		/// Sets this as the default security context provider <see cref="P:log4net.Core.SecurityContextProvider.DefaultProvider" />.
		/// </para>
		/// </remarks>
		public override void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository)
		{
			if ((object)m_providerType == null)
			{
				LogLog.Error("SecurityContextProviderAttribute: Attribute specified on assembly [" + sourceAssembly.FullName + "] with null ProviderType.");
				return;
			}
			LogLog.Debug("SecurityContextProviderAttribute: Creating provider of type [" + m_providerType.FullName + "]");
			SecurityContextProvider securityContextProvider = Activator.CreateInstance(m_providerType) as SecurityContextProvider;
			if (securityContextProvider == null)
			{
				LogLog.Error("SecurityContextProviderAttribute: Failed to create SecurityContextProvider instance of type [" + m_providerType.Name + "].");
			}
			else
			{
				SecurityContextProvider.DefaultProvider = securityContextProvider;
			}
		}
	}
}
