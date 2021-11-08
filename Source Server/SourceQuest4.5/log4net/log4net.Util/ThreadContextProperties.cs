using System;
using System.Threading;

namespace log4net.Util
{
	/// <summary>
	/// Implementation of Properties collection for the <see cref="T:log4net.ThreadContext" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Class implements a collection of properties that is specific to each thread.
	/// The class is not synchronized as each thread has its own <see cref="T:log4net.Util.PropertiesDictionary" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class ThreadContextProperties : ContextPropertiesBase
	{
		/// <summary>
		/// The thread local data slot to use to store a PropertiesDictionary.
		/// </summary>
		private static readonly LocalDataStoreSlot s_threadLocalSlot = Thread.AllocateDataSlot();

		/// <summary>
		/// Gets or sets the value of a property
		/// </summary>
		/// <value>
		/// The value for the property with the specified key
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the value of a property
		/// </para>
		/// </remarks>
		public override object this[string key]
		{
			get
			{
				return GetProperties(create: false)?[key];
			}
			set
			{
				GetProperties(create: true)[key] = value;
			}
		}

		/// <summary>
		/// Internal constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.ThreadContextProperties" /> class.
		/// </para>
		/// </remarks>
		internal ThreadContextProperties()
		{
		}

		/// <summary>
		/// Remove a property
		/// </summary>
		/// <param name="key">the key for the entry to remove</param>
		/// <remarks>
		/// <para>
		/// Remove a property
		/// </para>
		/// </remarks>
		public void Remove(string key)
		{
			GetProperties(create: false)?.Remove(key);
		}

		/// <summary>
		/// Clear all properties
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clear all properties
		/// </para>
		/// </remarks>
		public void Clear()
		{
			GetProperties(create: false)?.Clear();
		}

		/// <summary>
		/// Get the <c>PropertiesDictionary</c> for this thread.
		/// </summary>
		/// <param name="create">create the dictionary if it does not exist, otherwise return null if is does not exist</param>
		/// <returns>the properties for this thread</returns>
		/// <remarks>
		/// <para>
		/// The collection returned is only to be used on the calling thread. If the
		/// caller needs to share the collection between different threads then the 
		/// caller must clone the collection before doing so.
		/// </para>
		/// </remarks>
		internal PropertiesDictionary GetProperties(bool create)
		{
			PropertiesDictionary propertiesDictionary = (PropertiesDictionary)Thread.GetData(s_threadLocalSlot);
			if (propertiesDictionary == null && create)
			{
				propertiesDictionary = new PropertiesDictionary();
				Thread.SetData(s_threadLocalSlot, propertiesDictionary);
			}
			return propertiesDictionary;
		}
	}
}
