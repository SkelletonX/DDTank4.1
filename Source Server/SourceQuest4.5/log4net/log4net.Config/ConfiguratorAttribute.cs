using log4net.Repository;
using System;
using System.Reflection;

namespace log4net.Config
{
	/// <summary>
	/// Base class for all log4net configuration attributes.
	/// </summary>
	/// <remarks>
	/// This is an abstract class that must be extended by 
	/// specific configurators. This attribute allows the
	/// configurator to be parameterized by an assembly level
	/// attribute.
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[AttributeUsage(AttributeTargets.Assembly)]
	public abstract class ConfiguratorAttribute : Attribute, IComparable
	{
		private int m_priority = 0;

		/// <summary>
		/// Constructor used by subclasses.
		/// </summary>
		/// <param name="priority">the ordering priority for this configurator</param>
		/// <remarks>
		/// <para>
		/// The <paramref name="priority" /> is used to order the configurator
		/// attributes before they are invoked. Higher priority configurators are executed
		/// before lower priority ones.
		/// </para>
		/// </remarks>
		protected ConfiguratorAttribute(int priority)
		{
			m_priority = priority;
		}

		/// <summary>
		/// Configures the <see cref="T:log4net.Repository.ILoggerRepository" /> for the specified assembly.
		/// </summary>
		/// <param name="sourceAssembly">The assembly that this attribute was defined on.</param>
		/// <param name="targetRepository">The repository to configure.</param>
		/// <remarks>
		/// <para>
		/// Abstract method implemented by a subclass. When this method is called
		/// the subclass should configure the <paramref name="targetRepository" />.
		/// </para>
		/// </remarks>
		public abstract void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository);

		/// <summary>
		/// Compare this instance to another ConfiguratorAttribute
		/// </summary>
		/// <param name="obj">the object to compare to</param>
		/// <returns>see <see cref="M:System.IComparable.CompareTo(System.Object)" /></returns>
		/// <remarks>
		/// <para>
		/// Compares the priorities of the two <see cref="T:log4net.Config.ConfiguratorAttribute" /> instances.
		/// Sorts by priority in descending order. Objects with the same priority are
		/// randomly ordered.
		/// </para>
		/// </remarks>
		public int CompareTo(object obj)
		{
			if (this == obj)
			{
				return 0;
			}
			int num = -1;
			ConfiguratorAttribute configuratorAttribute = obj as ConfiguratorAttribute;
			if (configuratorAttribute != null)
			{
				num = configuratorAttribute.m_priority.CompareTo(m_priority);
				if (num == 0)
				{
					num = -1;
				}
			}
			return num;
		}
	}
}
