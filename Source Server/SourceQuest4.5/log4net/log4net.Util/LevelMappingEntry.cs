using log4net.Core;

namespace log4net.Util
{
	/// <summary>
	/// An entry in the <see cref="T:log4net.Util.LevelMapping" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// This is an abstract base class for types that are stored in the
	/// <see cref="T:log4net.Util.LevelMapping" /> object.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public abstract class LevelMappingEntry : IOptionHandler
	{
		private Level m_level;

		/// <summary>
		/// The level that is the key for this mapping 
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Util.LevelMappingEntry.Level" /> that is the key for this mapping 
		/// </value>
		/// <remarks>
		/// <para>
		/// Get or set the <see cref="P:log4net.Util.LevelMappingEntry.Level" /> that is the key for this
		/// mapping subclass.
		/// </para>
		/// </remarks>
		public Level Level
		{
			get
			{
				return m_level;
			}
			set
			{
				m_level = value;
			}
		}

		/// <summary>
		/// Initialize any options defined on this entry
		/// </summary>
		/// <remarks>
		/// <para>
		/// Should be overridden by any classes that need to initialise based on their options
		/// </para>
		/// </remarks>
		public virtual void ActivateOptions()
		{
		}
	}
}
