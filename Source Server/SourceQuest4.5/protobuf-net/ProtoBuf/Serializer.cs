using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace ProtoBuf
{
	public static class Serializer
	{
		public static class NonGeneric
		{
			public static object DeepClone(object instance)
			{
				if (instance != null)
				{
					return RuntimeTypeModel.Default.DeepClone(instance);
				}
				return null;
			}

			public static void Serialize(Stream dest, object instance)
			{
				if (instance != null)
				{
					RuntimeTypeModel.Default.Serialize(dest, instance);
				}
			}

			public static object Deserialize(Type type, Stream source)
			{
				return RuntimeTypeModel.Default.Deserialize(source, null, type);
			}

			public static object Merge(Stream source, object instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				return RuntimeTypeModel.Default.Deserialize(source, instance, instance.GetType(), null);
			}

			public static void SerializeWithLengthPrefix(Stream destination, object instance, PrefixStyle style, int fieldNumber)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				RuntimeTypeModel model = RuntimeTypeModel.Default;
				model.SerializeWithLengthPrefix(destination, instance, model.MapType(instance.GetType()), style, fieldNumber);
			}

			public static bool TryDeserializeWithLengthPrefix(Stream source, PrefixStyle style, TypeResolver resolver, out object value)
			{
				value = RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, null, null, style, 0, resolver);
				return value != null;
			}

			public static bool CanSerialize(Type type)
			{
				return RuntimeTypeModel.Default.IsDefined(type);
			}
		}

		public static class GlobalOptions
		{
			[Obsolete("Please use RuntimeTypeModel.Default.InferTagFromNameDefault instead (or on a per-model basis)", false)]
			public static bool InferTagFromName
			{
				get
				{
					return RuntimeTypeModel.Default.InferTagFromNameDefault;
				}
				set
				{
					RuntimeTypeModel.Default.InferTagFromNameDefault = value;
				}
			}
		}

		public delegate Type TypeResolver(int fieldNumber);

		private const string ProtoBinaryField = "proto";

		public const int ListItemTag = 1;

		public static string GetProto<T>()
		{
			return GetProto<T>(ProtoSyntax.Proto2);
		}

		public static string GetProto<T>(ProtoSyntax syntax)
		{
			return RuntimeTypeModel.Default.GetSchema(RuntimeTypeModel.Default.MapType(typeof(T)), syntax);
		}

		public static T DeepClone<T>(T instance)
		{
			if (instance != null)
			{
				return (T)RuntimeTypeModel.Default.DeepClone(instance);
			}
			return instance;
		}

		public static T Merge<T>(Stream source, T instance)
		{
			return (T)RuntimeTypeModel.Default.Deserialize(source, instance, typeof(T));
		}

		public static T Deserialize<T>(Stream source)
		{
			return (T)RuntimeTypeModel.Default.Deserialize(source, null, typeof(T));
		}

		public static object Deserialize(Type type, Stream source)
		{
			return RuntimeTypeModel.Default.Deserialize(source, null, type);
		}

		public static void Serialize<T>(Stream destination, T instance)
		{
			if (instance != null)
			{
				RuntimeTypeModel.Default.Serialize(destination, instance);
			}
		}

		public static TTo ChangeType<TFrom, TTo>(TFrom instance)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				Serialize(ms, instance);
				ms.Position = 0L;
				return Deserialize<TTo>(ms);
			}
		}

		public static void Serialize<T>(SerializationInfo info, T instance) where T : class, ISerializable
		{
			Serialize(info, new StreamingContext(StreamingContextStates.Persistence), instance);
		}

		public static void Serialize<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (instance.GetType() != typeof(T))
			{
				throw new ArgumentException("Incorrect type", "instance");
			}
			using (MemoryStream ms = new MemoryStream())
			{
				RuntimeTypeModel.Default.Serialize(ms, instance, context);
				info.AddValue("proto", ms.ToArray());
			}
		}

		public static void Serialize<T>(XmlWriter writer, T instance) where T : IXmlSerializable
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			using (MemoryStream ms = new MemoryStream())
			{
				Serialize(ms, instance);
				writer.WriteBase64(Helpers.GetBuffer(ms), 0, (int)ms.Length);
			}
		}

		public static void Merge<T>(XmlReader reader, T instance) where T : IXmlSerializable
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			byte[] buffer = new byte[4096];
			using (MemoryStream ms = new MemoryStream())
			{
				int depth = reader.Depth;
				while (reader.Read() && reader.Depth > depth)
				{
					if (reader.NodeType == XmlNodeType.Text)
					{
						int read;
						while ((read = reader.ReadContentAsBase64(buffer, 0, 4096)) > 0)
						{
							ms.Write(buffer, 0, read);
						}
						if (reader.Depth <= depth)
						{
							break;
						}
					}
				}
				ms.Position = 0L;
				Merge(ms, instance);
			}
		}

		public static void Merge<T>(SerializationInfo info, T instance) where T : class, ISerializable
		{
			Merge(info, new StreamingContext(StreamingContextStates.Persistence), instance);
		}

		public static void Merge<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (instance.GetType() != typeof(T))
			{
				throw new ArgumentException("Incorrect type", "instance");
			}
			byte[] buffer = (byte[])info.GetValue("proto", typeof(byte[]));
			using (MemoryStream ms = new MemoryStream(buffer))
			{
				T result = (T)RuntimeTypeModel.Default.Deserialize(ms, instance, typeof(T), context);
				if (result != instance)
				{
					throw new ProtoException("Deserialization changed the instance; cannot succeed.");
				}
			}
		}

		public static void PrepareSerializer<T>()
		{
			RuntimeTypeModel model = RuntimeTypeModel.Default;
			model[model.MapType(typeof(T))].CompileInPlace();
		}

		public static IFormatter CreateFormatter<T>()
		{
			return RuntimeTypeModel.Default.CreateFormatter(typeof(T));
		}

		public static IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int fieldNumber)
		{
			return RuntimeTypeModel.Default.DeserializeItems<T>(source, style, fieldNumber);
		}

		public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style)
		{
			return DeserializeWithLengthPrefix<T>(source, style, 0);
		}

		public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style, int fieldNumber)
		{
			RuntimeTypeModel model = RuntimeTypeModel.Default;
			return (T)model.DeserializeWithLengthPrefix(source, null, model.MapType(typeof(T)), style, fieldNumber);
		}

		public static T MergeWithLengthPrefix<T>(Stream source, T instance, PrefixStyle style)
		{
			RuntimeTypeModel model = RuntimeTypeModel.Default;
			return (T)model.DeserializeWithLengthPrefix(source, instance, model.MapType(typeof(T)), style, 0);
		}

		public static void SerializeWithLengthPrefix<T>(Stream destination, T instance, PrefixStyle style)
		{
			SerializeWithLengthPrefix(destination, instance, style, 0);
		}

		public static void SerializeWithLengthPrefix<T>(Stream destination, T instance, PrefixStyle style, int fieldNumber)
		{
			RuntimeTypeModel model = RuntimeTypeModel.Default;
			model.SerializeWithLengthPrefix(destination, instance, model.MapType(typeof(T)), style, fieldNumber);
		}

		public static bool TryReadLengthPrefix(Stream source, PrefixStyle style, out int length)
		{
			length = ProtoReader.ReadLengthPrefix(source, expectHeader: false, style, out int _, out int bytesRead);
			return bytesRead > 0;
		}

		public static bool TryReadLengthPrefix(byte[] buffer, int index, int count, PrefixStyle style, out int length)
		{
			using (Stream source = new MemoryStream(buffer, index, count))
			{
				return TryReadLengthPrefix(source, style, out length);
			}
		}

		public static void FlushPool()
		{
			BufferPool.Flush();
		}
	}
}
