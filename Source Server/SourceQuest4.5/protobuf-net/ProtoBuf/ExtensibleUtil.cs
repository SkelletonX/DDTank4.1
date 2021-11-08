using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ProtoBuf
{
	internal static class ExtensibleUtil
	{
		internal static IEnumerable<TValue> GetExtendedValues<TValue>(IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
			foreach (TValue extendedValue in GetExtendedValues(RuntimeTypeModel.Default, typeof(TValue), instance, tag, format, singleton, allowDefinedTag))
			{
				yield return extendedValue;
			}
		}

		internal static IEnumerable GetExtendedValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (tag <= 0)
			{
				throw new ArgumentOutOfRangeException("tag");
			}
			IExtension extn = instance.GetExtensionObject(createIfMissing: false);
			if (extn != null)
			{
				Stream stream = extn.BeginQuery();
				object value = null;
				ProtoReader reader = null;
				try
				{
					SerializationContext ctx = new SerializationContext();
					reader = ProtoReader.Create(stream, model, ctx, -1L);
					while (model.TryDeserializeAuxiliaryType(reader, format, tag, type, ref value, skipOtherFields: true, asListItem: false, autoCreate: false, insideList: false, null) && value != null)
					{
						if (!singleton)
						{
							yield return value;
							value = null;
						}
					}
					if (singleton && value != null)
					{
						yield return value;
					}
				}
				finally
				{
					ProtoReader.Recycle(reader);
					extn.EndQuery(stream);
				}
			}
		}

		internal static void AppendExtendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			IExtension extn = instance.GetExtensionObject(createIfMissing: true);
			if (extn == null)
			{
				throw new InvalidOperationException("No extension object available; appended data would be lost.");
			}
			bool commit = false;
			Stream stream = extn.BeginAppend();
			try
			{
				using (ProtoWriter writer = new ProtoWriter(stream, model, null))
				{
					model.TrySerializeAuxiliaryType(writer, null, format, tag, value, isInsideList: false, null);
					writer.Close();
				}
				commit = true;
			}
			finally
			{
				extn.EndAppend(stream, commit);
			}
		}
	}
}
