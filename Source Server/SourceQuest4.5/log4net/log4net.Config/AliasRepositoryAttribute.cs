using System;

namespace log4net.Config
{
	/// <summary>
	/// Assembly level attribute that specifies a repository to alias to this assembly's repository.
	/// </summary>
	/// <remarks>
	/// <para>
	/// An assembly's logger repository is defined by its <see cref="T:log4net.Config.RepositoryAttribute" />,
	/// however this can be overridden by an assembly loaded before the target assembly.
	/// </para>
	/// <para>
	/// An assembly can alias another assembly's repository to its repository by
	/// specifying this attribute with the name of the target repository.
	/// </para>
	/// <para>
	/// This attribute can only be specified on the assembly and may be used
	/// as many times as necessary to alias all the required repositories.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class AliasRepositoryAttribute : Attribute
	{
		private string m_name = null;

		/// <summary>
		/// Gets or sets the repository to alias to this assemby's repository.
		/// </summary>
		/// <value>
		/// The repository to alias to this assemby's repository.
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of the repository to alias to this assemby's repository.
		/// </para>
		/// </remarks>
		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Config.AliasRepositoryAttribute" /> class with 
		/// the specified repository to alias to this assembly's repository.
		/// </summary>
		/// <param name="name">The repository to alias to this assemby's repository.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Config.AliasRepositoryAttribute" /> class with 
		/// the specified repository to alias to this assembly's repository.
		/// </para>
		/// </remarks>
		public AliasRepositoryAttribute(string name)
		{
			Name = name;
		}
	}
}
