using ProtoBuf.Compiler;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal interface IProtoTypeSerializer : IProtoSerializer
	{
		bool HasCallbacks(TypeModel.CallbackType callbackType);

		bool CanCreateInstance();

		object CreateInstance(ProtoReader source);

		void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);

		void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType);

		void EmitCreateInstance(CompilerContext ctx);
	}
}
