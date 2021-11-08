using ProtoBuf.Meta;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace ProtoBuf.ServiceModel
{
	public sealed class XmlProtoSerializer : XmlObjectSerializer
	{
		private readonly TypeModel model;

		private readonly int key;

		private readonly bool isList;

		private readonly bool isEnum;

		private readonly Type type;

		private const string PROTO_ELEMENT = "proto";

		internal XmlProtoSerializer(TypeModel model, int key, Type type, bool isList)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (key < 0)
			{
				throw new ArgumentOutOfRangeException("key");
			}
			if (type == null)
			{
				throw new ArgumentOutOfRangeException("type");
			}
			this.model = model;
			this.key = key;
			this.isList = isList;
			this.type = type;
			isEnum = Helpers.IsEnum(type);
		}

		public static XmlProtoSerializer TryCreate(TypeModel model, Type type)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			bool isList;
			int key = GetKey(model, ref type, out isList);
			if (key >= 0)
			{
				return new XmlProtoSerializer(model, key, type, isList);
			}
			return null;
		}

		public XmlProtoSerializer(TypeModel model, Type type)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			key = GetKey(model, ref type, out isList);
			this.model = model;
			this.type = type;
			isEnum = Helpers.IsEnum(type);
			if (key < 0)
			{
				throw new ArgumentOutOfRangeException("type", "Type not recognised by the model: " + type.FullName);
			}
		}

		private static int GetKey(TypeModel model, ref Type type, out bool isList)
		{
			if (model != null && type != null)
			{
				int key2 = model.GetKey(ref type);
				if (key2 >= 0)
				{
					isList = false;
					return key2;
				}
				Type itemType = TypeModel.GetListItemType(model, type);
				if (itemType != null)
				{
					key2 = model.GetKey(ref itemType);
					if (key2 >= 0)
					{
						isList = true;
						return key2;
					}
				}
			}
			isList = false;
			return -1;
		}

		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteEndElement();
		}

		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteStartElement("proto");
		}

		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (graph == null)
			{
				writer.WriteAttributeString("nil", "true");
			}
			else
			{
				using (MemoryStream ms = new MemoryStream())
				{
					if (isList)
					{
						model.Serialize(ms, graph, null);
					}
					else
					{
						using (ProtoWriter protoWriter = new ProtoWriter(ms, model, null))
						{
							model.Serialize(key, graph, protoWriter);
						}
					}
					byte[] buffer = ms.GetBuffer();
					writer.WriteBase64(buffer, 0, (int)ms.Length);
				}
			}
		}

		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			reader.MoveToContent();
			if (reader.NodeType == XmlNodeType.Element)
			{
				return reader.Name == "proto";
			}
			return false;
		}

		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			reader.MoveToContent();
			bool isSelfClosed = reader.IsEmptyElement;
			bool isNil = reader.GetAttribute("nil") == "true";
			reader.ReadStartElement("proto");
			if (isNil)
			{
				if (!isSelfClosed)
				{
					reader.ReadEndElement();
				}
				return null;
			}
			if (isSelfClosed)
			{
				if (isList || isEnum)
				{
					return model.Deserialize(Stream.Null, null, type, null);
				}
				ProtoReader protoReader2 = null;
				try
				{
					protoReader2 = ProtoReader.Create(Stream.Null, model, null, -1L);
					return model.Deserialize(key, null, protoReader2);
				}
				finally
				{
					ProtoReader.Recycle(protoReader2);
				}
			}
			object result;
			using (MemoryStream ms = new MemoryStream(reader.ReadContentAsBase64()))
			{
				if (isList || isEnum)
				{
					result = model.Deserialize(ms, null, type, null);
				}
				else
				{
					ProtoReader protoReader = null;
					try
					{
						protoReader = ProtoReader.Create(ms, model, null, -1L);
						result = model.Deserialize(key, null, protoReader);
					}
					finally
					{
						ProtoReader.Recycle(protoReader);
					}
				}
			}
			reader.ReadEndElement();
			return result;
		}
	}
}
