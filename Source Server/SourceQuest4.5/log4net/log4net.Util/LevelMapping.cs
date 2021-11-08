using log4net.Core;
using System;
using System.Collections;

namespace log4net.Util
{
	/// <summary>
	/// Manages a mapping from levels to <see cref="T:log4net.Util.LevelMappingEntry" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Manages an ordered mapping from <see cref="T:log4net.Core.Level" /> instances 
	/// to <see cref="T:log4net.Util.LevelMappingEntry" /> subclasses.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class LevelMapping : IOptionHandler
	{
		private Hashtable m_entriesMap = new Hashtable();

		private LevelMappingEntry[] m_entries = null;

		/// <summary>
		/// Add a <see cref="T:log4net.Util.LevelMappingEntry" /> to this mapping
		/// </summary>
		/// <param name="entry">the entry to add</param>
		/// <remarks>
		/// <para>
		/// If a <see cref="T:log4net.Util.LevelMappingEntry" /> has previously been added
		/// for the same <see cref="T:log4net.Core.Level" /> then that entry will be 
		/// overwritten.
		/// </para>
		/// </remarks>
		public void Add(LevelMappingEntry entry)
		{
			if (m_entriesMap.ContainsKey(entry.Level))
			{
				m_entriesMap.Remove(entry.Level);
			}
			m_entriesMap.Add(entry.Level, entry);
		}

		/// <summary>
		/// Lookup the mapping for the specified level
		/// </summary>
		/// <param name="level">the level to lookup</param>
		/// <returns>the <see cref="T:log4net.Util.LevelMappingEntry" /> for the level or <c>null</c> if no mapping found</returns>
		/// <remarks>
		/// <para>
		/// Lookup the value for the specified level. Finds the nearest
		/// mapping value for the level that is equal to or less than the
		/// <paramref name="level" /> specified.
		/// </para>
		/// <para>
		/// If no mapping could be found then <c>null</c> is returned.
		/// </para>
		/// </remarks>
		public LevelMappingEntry Lookup(Level level)
		{
			if (m_entries != null)
			{
				LevelMappingEntry[] entries = m_entries;
				foreach (LevelMappingEntry levelMappingEntry in entries)
				{
					if (level >= levelMappingEntry.Level)
					{
						return levelMappingEntry;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Initialize options
		/// </summary>
		/// <remarks>
		/// <para>
		/// Caches the sorted list of <see cref="T:log4net.Util.LevelMappingEntry" /> in an array
		/// </para>
		/// </remarks>
		public void ActivateOptions()
		{
			Level[] array = new Level[m_entriesMap.Count];
			LevelMappingEntry[] array2 = new LevelMappingEntry[m_entriesMap.Count];
			m_entriesMap.Keys.CopyTo(array, 0);
			m_entriesMap.Values.CopyTo(array2, 0);
			Array.Sort(array, array2, 0, array.Length, null);
			Array.Reverse(array2, 0, array2.Length);
			LevelMappingEntry[] array3 = array2;
			foreach (LevelMappingEntry levelMappingEntry in array3)
			{
				levelMappingEntry.ActivateOptions();
			}
			m_entries = array2;
		}
	}
}
