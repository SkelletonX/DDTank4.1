using log4net.Util;
using System;
using System.Collections;

namespace log4net.Core
{
	/// <summary>
	/// Mapping between string name and Level object
	/// </summary>
	/// <remarks>
	/// <para>
	/// Mapping between string name and <see cref="T:log4net.Core.Level" /> object.
	/// This mapping is held separately for each <see cref="T:log4net.Repository.ILoggerRepository" />.
	/// The level name is case insensitive.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class LevelMap
	{
		/// <summary>
		/// Mapping from level name to Level object. The
		/// level name is case insensitive
		/// </summary>
		private Hashtable m_mapName2Level = SystemInfo.CreateCaseInsensitiveHashtable();

		/// <summary>
		/// Lookup a <see cref="T:log4net.Core.Level" /> by name
		/// </summary>
		/// <param name="name">The name of the Level to lookup</param>
		/// <returns>a Level from the map with the name specified</returns>
		/// <remarks>
		/// <para>
		/// Returns the <see cref="T:log4net.Core.Level" /> from the
		/// map with the name specified. If the no level is
		/// found then <c>null</c> is returned.
		/// </para>
		/// </remarks>
		public Level this[string name]
		{
			get
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				lock (this)
				{
					return (Level)m_mapName2Level[name];
				}
			}
		}

		/// <summary>
		/// Return all possible levels as a list of Level objects.
		/// </summary>
		/// <returns>all possible levels as a list of Level objects</returns>
		/// <remarks>
		/// <para>
		/// Return all possible levels as a list of Level objects.
		/// </para>
		/// </remarks>
		public LevelCollection AllLevels
		{
			get
			{
				lock (this)
				{
					return new LevelCollection(m_mapName2Level.Values);
				}
			}
		}

		/// <summary>
		/// Clear the internal maps of all levels
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clear the internal maps of all levels
		/// </para>
		/// </remarks>
		public void Clear()
		{
			m_mapName2Level.Clear();
		}

		/// <summary>
		/// Create a new Level and add it to the map
		/// </summary>
		/// <param name="name">the string to display for the Level</param>
		/// <param name="value">the level value to give to the Level</param>
		/// <remarks>
		/// <para>
		/// Create a new Level and add it to the map
		/// </para>
		/// </remarks>
		/// <seealso cref="M:log4net.Core.LevelMap.Add(System.String,System.Int32,System.String)" />
		public void Add(string name, int value)
		{
			Add(name, value, null);
		}

		/// <summary>
		/// Create a new Level and add it to the map
		/// </summary>
		/// <param name="name">the string to display for the Level</param>
		/// <param name="value">the level value to give to the Level</param>
		/// <param name="displayName">the display name to give to the Level</param>
		/// <remarks>
		/// <para>
		/// Create a new Level and add it to the map
		/// </para>
		/// </remarks>
		public void Add(string name, int value, string displayName)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw SystemInfo.CreateArgumentOutOfRangeException("name", name, "Parameter: name, Value: [" + name + "] out of range. Level name must not be empty");
			}
			if (displayName == null || displayName.Length == 0)
			{
				displayName = name;
			}
			Add(new Level(value, name, displayName));
		}

		/// <summary>
		/// Add a Level to the map
		/// </summary>
		/// <param name="level">the Level to add</param>
		/// <remarks>
		/// <para>
		/// Add a Level to the map
		/// </para>
		/// </remarks>
		public void Add(Level level)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level");
			}
			lock (this)
			{
				m_mapName2Level[level.Name] = level;
			}
		}

		/// <summary>
		/// Lookup a named level from the map
		/// </summary>
		/// <param name="defaultLevel">the name of the level to lookup is taken from this level. 
		/// If the level is not set on the map then this level is added</param>
		/// <returns>the level in the map with the name specified</returns>
		/// <remarks>
		/// <para>
		/// Lookup a named level from the map. The name of the level to lookup is taken
		/// from the <see cref="P:log4net.Core.Level.Name" /> property of the <paramref name="defaultLevel" />
		/// argument.
		/// </para>
		/// <para>
		/// If no level with the specified name is found then the 
		/// <paramref name="defaultLevel" /> argument is added to the level map
		/// and returned.
		/// </para>
		/// </remarks>
		public Level LookupWithDefault(Level defaultLevel)
		{
			if (defaultLevel == null)
			{
				throw new ArgumentNullException("defaultLevel");
			}
			lock (this)
			{
				Level level = (Level)m_mapName2Level[defaultLevel.Name];
				if (level == null)
				{
					m_mapName2Level[defaultLevel.Name] = defaultLevel;
					return defaultLevel;
				}
				return level;
			}
		}
	}
}
