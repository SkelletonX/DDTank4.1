using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml;

namespace log4net.Util
{
	/// <summary>
	/// String keyed object map that is read only.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This collection is readonly and cannot be modified.
	/// </para>
	/// <para>
	/// While this collection is serializable only member 
	/// objects that are serializable will
	/// be serialized along with this collection.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	[Serializable]
	public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary, ICollection, IEnumerable
	{
		/// <summary>
		/// The Hashtable used to store the properties data
		/// </summary>
		private Hashtable m_hashtable = new Hashtable();

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
		public virtual object this[string key]
		{
			get
			{
				return InnerHashtable[key];
			}
			set
			{
				throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
			}
		}

		/// <summary>
		/// The hashtable used to store the properties
		/// </summary>
		/// <value>
		/// The internal collection used to store the properties
		/// </value>
		/// <remarks>
		/// <para>
		/// The hashtable used to store the properties
		/// </para>
		/// </remarks>
		protected Hashtable InnerHashtable => m_hashtable;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.IsReadOnly" />
		/// </summary>
		bool IDictionary.IsReadOnly => true;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Item(System.Object)" />
		/// </summary>
		object IDictionary.this[object key]
		{
			get
			{
				if (!(key is string))
				{
					throw new ArgumentException("key must be a string");
				}
				return InnerHashtable[key];
			}
			set
			{
				throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
			}
		}

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Values" />
		/// </summary>
		ICollection IDictionary.Values => InnerHashtable.Values;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.Keys" />
		/// </summary>
		ICollection IDictionary.Keys => InnerHashtable.Keys;

		/// <summary>
		/// See <see cref="P:System.Collections.IDictionary.IsFixedSize" />
		/// </summary>
		bool IDictionary.IsFixedSize => InnerHashtable.IsFixedSize;

		/// <summary>
		/// See <see cref="P:System.Collections.ICollection.IsSynchronized" />
		/// </summary>
		bool ICollection.IsSynchronized => InnerHashtable.IsSynchronized;

		/// <summary>
		/// The number of properties in this collection
		/// </summary>
		public int Count => InnerHashtable.Count;

		/// <summary>
		/// See <see cref="P:System.Collections.ICollection.SyncRoot" />
		/// </summary>
		object ICollection.SyncRoot => InnerHashtable.SyncRoot;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.ReadOnlyPropertiesDictionary" /> class.
		/// </para>
		/// </remarks>
		public ReadOnlyPropertiesDictionary()
		{
		}

		/// <summary>
		/// Copy Constructor
		/// </summary>
		/// <param name="propertiesDictionary">properties to copy</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.ReadOnlyPropertiesDictionary" /> class.
		/// </para>
		/// </remarks>
		public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
		{
			foreach (DictionaryEntry item in (IEnumerable)propertiesDictionary)
			{
				InnerHashtable.Add(item.Key, item.Value);
			}
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="T:log4net.Util.ReadOnlyPropertiesDictionary" /> class 
		/// with serialized data.
		/// </para>
		/// </remarks>
		protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SerializationEntry current = enumerator.Current;
				InnerHashtable[XmlConvert.DecodeName(current.Name)] = current.Value;
			}
		}

		/// <summary>
		/// Gets the key names.
		/// </summary>
		/// <returns>An array of all the keys.</returns>
		/// <remarks>
		/// <para>
		/// Gets the key names.
		/// </para>
		/// </remarks>
		public string[] GetKeys()
		{
			string[] array = new string[InnerHashtable.Count];
			InnerHashtable.Keys.CopyTo(array, 0);
			return array;
		}

		/// <summary>
		/// Test if the dictionary contains a specified key
		/// </summary>
		/// <param name="key">the key to look for</param>
		/// <returns>true if the dictionary contains the specified key</returns>
		/// <remarks>
		/// <para>
		/// Test if the dictionary contains a specified key
		/// </para>
		/// </remarks>
		public bool Contains(string key)
		{
			return InnerHashtable.Contains(key);
		}

		/// <summary>
		/// Serializes this object into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> provided.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination for this serialization.</param>
		/// <remarks>
		/// <para>
		/// Serializes this object into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> provided.
		/// </para>
		/// </remarks>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			foreach (DictionaryEntry item in InnerHashtable)
			{
				string text = item.Key as string;
				object value = item.Value;
				if (text != null && value != null && value.GetType().IsSerializable)
				{
					info.AddValue(XmlConvert.EncodeLocalName(text), value);
				}
			}
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.GetEnumerator" />
		/// </summary>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return InnerHashtable.GetEnumerator();
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Remove(System.Object)" />
		/// </summary>
		/// <param name="key"></param>
		void IDictionary.Remove(object key)
		{
			throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Contains(System.Object)" />
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		bool IDictionary.Contains(object key)
		{
			return InnerHashtable.Contains(key);
		}

		/// <summary>
		/// Remove all properties from the properties collection
		/// </summary>
		public virtual void Clear()
		{
			throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IDictionary.Add(System.Object,System.Object)" />
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		void IDictionary.Add(object key, object value)
		{
			throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
		}

		/// <summary>
		/// See <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void ICollection.CopyTo(Array array, int index)
		{
			InnerHashtable.CopyTo(array, index);
		}

		/// <summary>
		/// See <see cref="M:System.Collections.IEnumerable.GetEnumerator" />
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)InnerHashtable).GetEnumerator();
		}
	}
}
