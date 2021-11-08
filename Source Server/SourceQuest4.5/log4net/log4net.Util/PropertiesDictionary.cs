using System;
using System.Collections;
using System.Runtime.Serialization;

namespace log4net.Util
{
	/// <summary>
	/// String keyed object map.
	/// </summary>
	/// <remarks>
	/// <para>
	/// While this collection is serializable only member 
	/// objects that are serializable will
	/// be serialized along with this collection.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public sealed class PropertiesDictionary : ReadOnlyPropertiesDictionary, ISerializable, IDictionary, ICollection, IEnumerable
	{
		/// <summary>
		/// Gets or sets the value of the  property with the specified key.
		/// </summary>
		/// <value>
		/// The value of the property with the specified key.
		/// </value>
		/// <param name="key">The key of the property to get or set.</param>
		/// <remarks>
		/// <para>
		/// The property value will only be serialized if it is serializable.
		/// If it cannot be serialized it will be silently ignored if
		/// a serialization operation is performed.
		/// </para>
		/// </remarks>
		public override object this[string key]
		{
			get
			{
				return base.InnerHashtable[key];
			}
			set
			{
				base.InnerHashtable[key] = value;
			}
		}

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.IsReadOnly" />
		/// </summary>
		/// <value>
		/// <c>false</c>
		/// </value>
		/// <remarks>
		/// <para>
		/// This collection is modifiable. This property always
		/// returns <c>false</c>.
		/// </para>
		/// </remarks>
		bool IDictionary.IsReadOnly => false;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Item(System.Object)" />
		/// </summary>
		/// <value>
		/// The value for the key specified.
		/// </value>
		/// <remarks>
		/// <para>
		/// Get or set a value for the specified <see cref="T:System.String" /> <paramref name="key" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentException">Thrown if the <paramref name="key" /> is not a string</exception>
		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
				{
					throw new ArgumentException("key must be a string", "key");
				}
				return base.InnerHashtable[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("key must be a string", "key");
				}
				base.InnerHashtable[key] = value;
			}
		}

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Values" />
		/// </summary>
		ICollection IDictionary.Values => base.InnerHashtable.Values;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Keys" />
		/// </summary>
		ICollection IDictionary.Keys => base.InnerHashtable.Keys;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.IsFixedSize" />
		/// </summary>
		bool IDictionary.IsFixedSize => false;

		/// <summary>
		/// See <see cref="P:System.Collections.ICollection.IsSynchronized" />
		/// </summary>
		bool ICollection.IsSynchronized => base.InnerHashtable.IsSynchronized;

		/// <summary>
		/// See <see cref="P:System.Collections.ICollection.SyncRoot" />
		/// </summary>
		object ICollection.SyncRoot => base.InnerHashtable.SyncRoot;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.PropertiesDictionary" /> class.
		/// </para>
		/// </remarks>
		public PropertiesDictionary()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="propertiesDictionary">properties to copy</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.PropertiesDictionary" /> class.
		/// </para>
		/// </remarks>
		public PropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
			: base(propertiesDictionary)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:log4net.Util.PropertiesDictionary" /> class 
		/// with serialized data.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Because this class is sealed the serialization constructor is private.
		/// </para>
		/// </remarks>
		private PropertiesDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Remove the entry with the specified key from this dictionary
		/// </summary>
		/// <param name="key">the key for the entry to remove</param>
		/// <remarks>
		/// <para>
		/// Remove the entry with the specified key from this dictionary
		/// </para>
		/// </remarks>
		public void Remove(string key)
		{
			base.InnerHashtable.Remove(key);
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.GetEnumerator" />
		/// </summary>
		/// <returns>an enumerator</returns>
		/// <remarks>
		/// <para>
		/// Returns a <see cref="T:System.Collections.IDictionaryEnumerator" /> over the contest of this collection.
		/// </para>
		/// </remarks>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return base.InnerHashtable.GetEnumerator();
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Remove(System.Object)" />
		/// </summary>
		/// <param name="key">the key to remove</param>
		/// <remarks>
		/// <para>
		/// Remove the entry with the specified key from this dictionary
		/// </para>
		/// </remarks>
		void IDictionary.Remove(object key)
		{
			base.InnerHashtable.Remove(key);
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Contains(System.Object)" />
		/// </summary>
		/// <param name="key">the key to lookup in the collection</param>
		/// <returns><c>true</c> if the collection contains the specified key</returns>
		/// <remarks>
		/// <para>
		/// Test if this collection contains a specified key.
		/// </para>
		/// </remarks>
		bool IDictionary.Contains(object key)
		{
			return base.InnerHashtable.Contains(key);
		}

		/// <summary>
		/// Remove all properties from the properties collection
		/// </summary>
		/// <remarks>
		/// <para>
		/// Remove all properties from the properties collection
		/// </para>
		/// </remarks>
		public override void Clear()
		{
			base.InnerHashtable.Clear();
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Add(System.Object,System.Object)" />
		/// </summary>
		/// <param name="key">the key</param>
		/// <param name="value">the value to store for the key</param>
		/// <remarks>
		/// <para>
		/// Store a value for the specified <see cref="T:System.String" /> <paramref name="key" />.
		/// </para>
		/// </remarks>
		/// <exception cref="T:System.ArgumentException">Thrown if the <paramref name="key" /> is not a string</exception>
		void IDictionary.Add(object key, object value)
		{
			if (!(key is string))
			{
				throw new ArgumentException("key must be a string", "key");
			}
			base.InnerHashtable.Add(key, value);
		}

		/// <summary>
		/// See <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void ICollection.CopyTo(Array array, int index)
		{
			base.InnerHashtable.CopyTo(array, index);
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IEnumerable.GetEnumerator" />
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)base.InnerHashtable).GetEnumerator();
		}
	}
}
