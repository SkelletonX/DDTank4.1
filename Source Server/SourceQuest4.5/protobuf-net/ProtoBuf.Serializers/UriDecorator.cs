using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class UriDecorator : ProtoDecoratorBase
	{
		private static readonly Type expectedType = typeof(Uri);

		public override Type ExpectedType => expectedType;

		public override bool RequiresOldValue => false;

		public override bool ReturnsValue => true;

		public UriDecorator(TypeModel model, IProtoSerializer tail)
			: base(tail)
		{
		}

		public override void Write(object value, ProtoWriter dest)
		{
			Tail.Write(((Uri)value).OriginalString, dest);
		}

		public override object Read(object value, ProtoReader source)
		{
			string s = (string)Tail.Read(null, source);
			if (s.Length != 0)
			{
				return new Uri(s, UriKind.RelativeOrAbsolute);
			}
			return null;
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.LoadValue(valueFrom);
			ctx.LoadValue(typeof(Uri).GetProperty("OriginalString"));
			Tail.EmitWrite(ctx, null);
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			Tail.EmitRead(ctx, valueFrom);
			ctx.CopyValue();
			CodeLabel nonEmpty = ctx.DefineLabel();
			CodeLabel end = ctx.DefineLabel();
			ctx.LoadValue(typeof(string).GetProperty("Length"));
			ctx.BranchIfTrue(nonEmpty, @short: true);
			ctx.DiscardValue();
			ctx.LoadNullRef();
			ctx.Branch(end, @short: true);
			ctx.MarkLabel(nonEmpty);
			ctx.LoadValue(0);
			ctx.EmitCtor(ctx.MapType(typeof(Uri)), ctx.MapType(typeof(string)), ctx.MapType(typeof(UriKind)));
			ctx.MarkLabel(end);
		}
	}
}
