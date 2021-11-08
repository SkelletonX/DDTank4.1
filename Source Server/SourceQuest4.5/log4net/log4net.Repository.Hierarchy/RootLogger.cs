using log4net.Core;
using log4net.Util;

namespace log4net.Repository.Hierarchy
{
	/// <summary>
	/// The <see cref="T:log4net.Repository.Hierarchy.RootLogger" /> sits at the root of the logger hierarchy tree. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="T:log4net.Repository.Hierarchy.RootLogger" /> is a regular <see cref="T:log4net.Repository.Hierarchy.Logger" /> except 
	/// that it provides several guarantees.
	/// </para>
	/// <para>
	/// First, it cannot be assigned a <c>null</c>
	/// level. Second, since the root logger cannot have a parent, the
	/// <see cref="P:log4net.Repository.Hierarchy.RootLogger.EffectiveLevel" /> property always returns the value of the
	/// level field without walking the hierarchy.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class RootLogger : Logger
	{
		/// <summary>
		/// Gets the assigned level value without walking the logger hierarchy.
		/// </summary>
		/// <value>The assigned level value without walking the logger hierarchy.</value>
		/// <remarks>
		/// <para>
		/// Because the root logger cannot have a parent and its level
		/// must not be <c>null</c> this property just returns the
		/// value of <see cref="P:log4net.Repository.Hierarchy.Logger.Level" />.
		/// </para>
		/// </remarks>
		public override Level EffectiveLevel => base.Level;

		/// <summary>
		/// Gets or sets the assigned <see cref="P:log4net.Repository.Hierarchy.RootLogger.Level" /> for the root logger.  
		/// </summary>
		/// <value>
		/// The <see cref="P:log4net.Repository.Hierarchy.RootLogger.Level" /> of the root logger.
		/// </value>
		/// <remarks>
		/// <para>
		/// Setting the level of the root logger to a <c>null</c> reference
		/// may have catastrophic results. We prevent this here.
		/// </para>
		/// </remarks>
		public override Level Level
		{
			get
			{
				return base.Level;
			}
			set
			{
				if (value == null)
				{
					LogLog.Error("RootLogger: You have tried to set a null level to root.", new LogException());
				}
				else
				{
					base.Level = value;
				}
			}
		}

		/// <summary>
		/// Construct a <see cref="T:log4net.Repository.Hierarchy.RootLogger" />
		/// </summary>
		/// <param name="level">The level to assign to the root logger.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Repository.Hierarchy.RootLogger" /> class with
		/// the specified logging level.
		/// </para>
		/// <para>
		/// The root logger names itself as "root". However, the root
		/// logger cannot be retrieved by name.
		/// </para>
		/// </remarks>
		public RootLogger(Level level)
			: base("root")
		{
			Level = level;
		}
	}
}
