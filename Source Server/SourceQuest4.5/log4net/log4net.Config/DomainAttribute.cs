using System;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute that specifies the logging domain for the assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <b>DomainAttribute is obsolete. Use RepositoryAttribute instead of DomainAttribute.</b>
	/// </para>
	/// <para>
	/// Assemblies are mapped to logging domains. Each domain has its own
	/// logging repository. This attribute specified on the assembly controls
	/// the configuration of the domain. The <see cref="P:log4net.Config.RepositoryAttribute.Name" /> property specifies the name
	/// of the domain that this assembly is a part of. The <see cref="P:log4net.Config.RepositoryAttribute.RepositoryType" />
	/// specifies the type of the repository objects to create for the domain. If 
	/// this attribute is not specified and a <see cref="P:log4net.Config.RepositoryAttribute.Name" /> is not specified
	/// then the assembly will be part of the default shared logging domain.
	/// </para>
	/// <para>
	/// This attribute can only be specified on the assembly and may only be used
	/// once per assembly.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	[Obsolete("Use RepositoryAttribute instead of DomainAttribute")]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class DomainAttribute : RepositoryAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.DomainAttribute" /> class.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Obsolete. Use RepositoryAttribute instead of DomainAttribute.
		/// </para>
		/// </remarks>
		public DomainAttribute()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="T:log4net.Config.DomainAttribute" /> class 
		/// with the name of the domain.
		/// </summary>
		/// <param name="name">The name of the domain.</param>
		/// <remarks>
		/// <para>
		/// Obsolete. Use RepositoryAttribute instead of DomainAttribute.
		/// </para>
		/// </remarks>
		public DomainAttribute(string name)
			: base(name)
		{
		}
	}
}
