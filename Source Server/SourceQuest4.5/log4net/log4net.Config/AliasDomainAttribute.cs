using System;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute that specifies a domain to alias to this assembly's repository.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <b>AliasDomainAttribute is obsolete. Use AliasRepositoryAttribute instead of AliasDomainAttribute.</b>
	/// </para>
	/// <para>
	/// An assembly's logger repository is defined by its <see cref="T:log4net.Config.DomainAttribute" />,
	/// however this can be overridden by an assembly loaded before the target assembly.
	/// </para>
	/// <para>
	/// An assembly can alias another assembly's domain to its repository by
	/// specifying this attribute with the name of the target domain.
	/// </para>
	/// <para>
	/// This attribute can only be specified on the assembly and may be used
	/// as many times as necessary to alias all the required domains.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	[Obsolete("Use AliasRepositoryAttribute instead of AliasDomainAttribute")]
	public sealed class AliasDomainAttribute : AliasRepositoryAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.AliasDomainAttribute" /> class with 
		/// the specified domain to alias to this assembly's repository.
		/// </summary>
		/// <param name="name">The domain to alias to this assemby's repository.</param>
		/// <remarks>
		/// <para>
		/// Obsolete. Use <see cref="T:log4net.Config.AliasRepositoryAttribute" /> instead of <see cref="T:log4net.Config.AliasDomainAttribute" />.
		/// </para>
		/// </remarks>
		public AliasDomainAttribute(string name)
			: base(name)
		{
		}
	}
}
