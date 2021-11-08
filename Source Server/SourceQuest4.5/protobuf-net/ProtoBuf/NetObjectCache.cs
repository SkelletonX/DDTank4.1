using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProtoBuf
{
	internal sealed class NetObjectCache
	{
		private sealed class ReferenceComparer : IEqualityComparer<object>
		{
			public static readonly ReferenceComparer Default = new ReferenceComparer();

			private ReferenceComparer()
			{
			}

			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return x == y;
			}

			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}

		internal const int Root = 0;

		private MutableList underlyingList;

		private object rootObject;

		private int trapStartIndex;

		private Dictionary<string, int> stringKeys;

		private Dictionary<object, int> objectKeys;

		private MutableList List
		{
			get
			{
				if (underlyingList == null)
				{
					underlyingList = new MutableList();
				}
				return underlyingList;
			}
		}

		internal object GetKeyedObject(int key)
		{
			if (key-- == 0)
			{
				if (rootObject == null)
				{
					throw new ProtoException("No root object assigned");
				}
				return rootObject;
			}
			BasicList list = List;
			if (key < 0 || key >= list.Count)
			{
				throw new ProtoException("Internal error; a missing key occurred");
			}
			object tmp = list[key];
			if (tmp == null)
			{
				throw new ProtoException("A deferred key does not have a value yet");
			}
			return tmp;
		}

		internal void SetKeyedObject(int key, object value)
		{
			if (key-- == 0)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (rootObject != null && rootObject != value)
				{
					throw new ProtoException("The root object cannot be reassigned");
				}
				rootObject = value;
				return;
			}
			MutableList list = List;
			if (key < list.Count)
			{
				object oldVal = list[key];
				if (oldVal == null)
				{
					list[key] = value;
				}
				else if (oldVal != value)
				{
					throw new ProtoException("Reference-tracked objects cannot change reference");
				}
			}
			else if (key != list.Add(value))
			{
				throw new ProtoException("Internal error; a key mismatch occurred");
			}
		}

		internal int AddObjectKey(object value, out bool existing)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value == rootObject)
			{
				existing = true;
				return 0;
			}
			string s = value as string;
			BasicList list = List;
			int index;
			if (s == null)
			{
				if (objectKeys == null)
				{
					objectKeys = new Dictionary<object, int>(ReferenceComparer.Default);
					index = -1;
				}
				else if (!objectKeys.TryGetValue(value, out index))
				{
					index = -1;
				}
			}
			else if (stringKeys == null)
			{
				stringKeys = new Dictionary<string, int>();
				index = -1;
			}
			else if (!stringKeys.TryGetValue(s, out index))
			{
				index = -1;
			}
			if (!(existing = (index >= 0)))
			{
				index = list.Add(value);
				if (s == null)
				{
					objectKeys.Add(value, index);
				}
				else
				{
					stringKeys.Add(s, index);
				}
			}
			return index + 1;
		}

		internal void RegisterTrappedObject(object value)
		{
			if (rootObject == null)
			{
				rootObject = value;
			}
			else
			{
				if (underlyingList == null)
				{
					return;
				}
				int i = trapStartIndex;
				while (true)
				{
					if (i < underlyingList.Count)
					{
						trapStartIndex = i + 1;
						if (underlyingList[i] == null)
						{
							break;
						}
						i++;
						continue;
					}
					return;
				}
				underlyingList[i] = value;
			}
		}

		internal void Clear()
		{
			trapStartIndex = 0;
			rootObject = null;
			if (underlyingList != null)
			{
				underlyingList.Clear();
			}
			if (stringKeys != null)
			{
				stringKeys.Clear();
			}
			if (objectKeys != null)
			{
				objectKeys.Clear();
			}
		}
	}
}
