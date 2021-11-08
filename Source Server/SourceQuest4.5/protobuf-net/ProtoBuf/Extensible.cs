using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProtoBuf
{
	public abstract class Extensible : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return GetExtensionObject(createIfMissing);
		}

		protected virtual IExtension GetExtensionObject(bool createIfMissing)
		{
			return GetExtensionObject(ref extensionObject, createIfMissing);
		}

		public static IExtension GetExtensionObject(ref IExtension extensionObject, bool createIfMissing)
		{
			if (createIfMissing && extensionObject == null)
			{
				extensionObject = new BufferExtension();
			}
			return extensionObject;
		}

		public static void AppendValue<TValue>(IExtensible instance, int tag, TValue value)
		{
			AppendValue(instance, tag, DataFormat.Default, value);
		}

		public static void AppendValue<TValue>(IExtensible instance, int tag, DataFormat format, TValue value)
		{
			ExtensibleUtil.AppendExtendValue(RuntimeTypeModel.Default, instance, tag, format, value);
		}

		public static TValue GetValue<TValue>(IExtensible instance, int tag)
		{
			return GetValue<TValue>(instance, tag, DataFormat.Default);
		}

		public static TValue GetValue<TValue>(IExtensible instance, int tag, DataFormat format)
		{
			TryGetValue(instance, tag, format, out TValue value);
			return value;
		}

		public static bool TryGetValue<TValue>(IExtensible instance, int tag, out TValue value)
		{
			return TryGetValue(instance, tag, DataFormat.Default, out value);
		}

		public static bool TryGetValue<TValue>(IExtensible instance, int tag, DataFormat format, out TValue value)
		{
			return TryGetValue(instance, tag, format, allowDefinedTag: false, out value);
		}

		public static bool TryGetValue<TValue>(IExtensible instance, int tag, DataFormat format, bool allowDefinedTag, out TValue value)
		{
			value = default(TValue);
			bool set = false;
			foreach (TValue extendedValue in ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, singleton: true, allowDefinedTag))
			{
				TValue val = value = extendedValue;
				set = true;
			}
			return set;
		}

		public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag)
		{
			return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, DataFormat.Default, singleton: false, allowDefinedTag: false);
		}

		public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag, DataFormat format)
		{
			return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, singleton: false, allowDefinedTag: false);
		}

		public static bool TryGetValue(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool allowDefinedTag, out object value)
		{
			value = null;
			bool set = false;
			foreach (object extendedValue in ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, singleton: true, allowDefinedTag))
			{
				object val = value = extendedValue;
				set = true;
			}
			return set;
		}

		public static IEnumerable GetValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format)
		{
			return ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, singleton: false, allowDefinedTag: false);
		}

		public static void AppendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
		}
	}
}
