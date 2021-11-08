using System.Runtime.Remoting.Messaging;

namespace log4net.Util
{
	/// <summary>
	/// Implementation of Properties collection for the <see cref="T:log4net.LogicalThreadContext" />
	/// </summary>
	/// <remarks>
	/// <para>
	/// Class implements a collection of properties that is specific to each thread.
	/// The class is not synchronized as each thread has its own <see cref="T:log4net.Util.PropertiesDictionary" />.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	public sealed class LogicalThreadContextProperties : ContextPropertiesBase
	{
		/// <summary>
		/// Gets or sets the value of a property
		/// </summary>
		/// <value>
		/// The value for the property with the specified key
		/// </value>
		/// <remarks>
		/// <para>
		/// Get or set the property value for the <paramref name="key" /> specified.
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
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.LogicalThreadContextProperties" /> class.
		/// </para>
		/// </remarks>
		internal LogicalThreadContextProperties()
		{
		}

		/// <summary>
		/// Remove a property
		/// </summary>
		/// <param name="key">the key for the entry to remove</param>
		/// <remarks>
		/// <para>
		/// Remove the value for the specified <paramref name="key" /> from the context.
		/// </para>
		/// </remarks>
		public void Remove(string key)
		{
			GetProperties(create: false)?.Remove(key);
		}

		/// <summary>
		/// Clear all the context properties
		/// </summary>
		/// <remarks>
		/// <para>
		/// Clear all the context properties
		/// </para>
		/// </remarks>
		public void Clear()
		{
			GetProperties(create: false)?.Clear();
		}

		/// <summary>
		/// Get the PropertiesDictionary stored in the LocalDataStoreSlot for this thread.
		/// </summary>
		/// <param name="create">create the dictionary if it does not exist, otherwise return null if is does not exist</param>
		/// <returns>the properties for this thread</returns>
		/// <remarks>
		/// <para>
		/// The collection returned is only to be used on the calling thread. If the
		/// caller needs to share the collection between different threads then the 
		/// caller must clone the collection before doings so.
		/// </para>
		/// </remarks>
		internal PropertiesDictionary GetProperties(bool create)
		{
			PropertiesDictionary propertiesDictionary = (PropertiesDictionary)CallContext.GetData("log4net.Util.LogicalThreadContextProperties");
			if (propertiesDictionary == null && create)
			{
				propertiesDictionary = new PropertiesDictionary();
				CallContext.SetData("log4net.Util.LogicalThreadContextProperties", propertiesDictionary);
			}
			return propertiesDictionary;
		}
	}
}
