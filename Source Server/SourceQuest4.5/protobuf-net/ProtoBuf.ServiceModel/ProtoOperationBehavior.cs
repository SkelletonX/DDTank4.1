using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml;

namespace ProtoBuf.ServiceModel
{
	public sealed class ProtoOperationBehavior : DataContractSerializerOperationBehavior
	{
		private TypeModel model;

		public TypeModel Model
		{
			get
			{
				return model;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				model = value;
			}
		}

		public ProtoOperationBehavior(OperationDescription operation)
			: base(operation)
		{
			model = RuntimeTypeModel.Default;
		}

		public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
		{
			if (model == null)
			{
				throw new InvalidOperationException("No Model instance has been assigned to the ProtoOperationBehavior");
			}
			return XmlProtoSerializer.TryCreate(model, type) ?? base.CreateSerializer(type, name, ns, knownTypes);
		}
	}
}
