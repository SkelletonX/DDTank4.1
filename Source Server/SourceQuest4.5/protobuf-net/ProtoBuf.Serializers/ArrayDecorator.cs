using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class ArrayDecorator : ProtoDecoratorBase
	{
		private readonly int fieldNumber;

		private const byte OPTIONS_WritePacked = 1;

		private const byte OPTIONS_OverwriteList = 2;

		private const byte OPTIONS_SupportNull = 4;

		private readonly byte options;

		private readonly WireType packedWireType;

		private readonly Type arrayType;

		private readonly Type itemType;

		public override Type ExpectedType => arrayType;

		public override bool RequiresOldValue => AppendToCollection;

		public override bool ReturnsValue => true;

		private bool AppendToCollection => (options & 2) == 0;

		private bool SupportNull => (options & 4) != 0;

		public ArrayDecorator(TypeModel model, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, Type arrayType, bool overwriteList, bool supportNull)
			: base(tail)
		{
			itemType = arrayType.GetElementType();
			Type underlyingItemType = supportNull ? itemType : (Helpers.GetUnderlyingType(itemType) ?? itemType);
			if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (!ListDecorator.CanPack(packedWireType))
			{
				if (writePacked)
				{
					throw new InvalidOperationException("Only simple data-types can use packed encoding");
				}
				packedWireType = WireType.None;
			}
			this.fieldNumber = fieldNumber;
			this.packedWireType = packedWireType;
			if (writePacked)
			{
				options |= 1;
			}
			if (overwriteList)
			{
				options |= 2;
			}
			if (supportNull)
			{
				options |= 4;
			}
			this.arrayType = arrayType;
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			using (Local arr = ctx.GetLocalWithValue(arrayType, valueFrom))
			{
				using (Local i = new Local(ctx, ctx.MapType(typeof(int))))
				{
					bool writePacked = (options & 1) != 0;
					bool fixedLengthPacked = writePacked && CanUsePackedPrefix();
					using (Local token = (writePacked && !fixedLengthPacked) ? new Local(ctx, ctx.MapType(typeof(SubItemToken))) : null)
					{
						Type mappedWriter = ctx.MapType(typeof(ProtoWriter));
						if (writePacked)
						{
							ctx.LoadValue(fieldNumber);
							ctx.LoadValue(2);
							ctx.LoadReaderWriter();
							ctx.EmitCall(mappedWriter.GetMethod("WriteFieldHeader"));
							if (fixedLengthPacked)
							{
								ctx.LoadLength(arr, zeroIfNull: false);
								ctx.LoadValue((int)packedWireType);
								ctx.LoadReaderWriter();
								ctx.EmitCall(mappedWriter.GetMethod("WritePackedPrefix"));
							}
							else
							{
								ctx.LoadValue(arr);
								ctx.LoadReaderWriter();
								ctx.EmitCall(mappedWriter.GetMethod("StartSubItem"));
								ctx.StoreValue(token);
							}
							ctx.LoadValue(fieldNumber);
							ctx.LoadReaderWriter();
							ctx.EmitCall(mappedWriter.GetMethod("SetPackedField"));
						}
						EmitWriteArrayLoop(ctx, i, arr);
						if (writePacked)
						{
							if (fixedLengthPacked)
							{
								ctx.LoadValue(fieldNumber);
								ctx.LoadReaderWriter();
								ctx.EmitCall(mappedWriter.GetMethod("ClearPackedField"));
							}
							else
							{
								ctx.LoadValue(token);
								ctx.LoadReaderWriter();
								ctx.EmitCall(mappedWriter.GetMethod("EndSubItem"));
							}
						}
					}
				}
			}
		}

		private bool CanUsePackedPrefix()
		{
			return CanUsePackedPrefix(packedWireType, itemType);
		}

		internal static bool CanUsePackedPrefix(WireType packedWireType, Type itemType)
		{
			if (packedWireType != WireType.Fixed64 && packedWireType != WireType.Fixed32)
			{
				return false;
			}
			if (!Helpers.IsValueType(itemType))
			{
				return false;
			}
			return Helpers.GetUnderlyingType(itemType) == null;
		}

		private void EmitWriteArrayLoop(CompilerContext ctx, Local i, Local arr)
		{
			ctx.LoadValue(0);
			ctx.StoreValue(i);
			CodeLabel loopTest = ctx.DefineLabel();
			CodeLabel processItem = ctx.DefineLabel();
			ctx.Branch(loopTest, @short: false);
			ctx.MarkLabel(processItem);
			ctx.LoadArrayValue(arr, i);
			if (SupportNull)
			{
				Tail.EmitWrite(ctx, null);
			}
			else
			{
				ctx.WriteNullCheckedTail(itemType, Tail, null);
			}
			ctx.LoadValue(i);
			ctx.LoadValue(1);
			ctx.Add();
			ctx.StoreValue(i);
			ctx.MarkLabel(loopTest);
			ctx.LoadValue(i);
			ctx.LoadLength(arr, zeroIfNull: false);
			ctx.BranchIfLess(processItem, @short: false);
		}

		public override void Write(object value, ProtoWriter dest)
		{
			IList arr = (IList)value;
			int len = arr.Count;
			bool writePacked = (options & 1) != 0;
			bool fixedLengthPacked = writePacked && CanUsePackedPrefix();
			SubItemToken token;
			if (writePacked)
			{
				ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, dest);
				if (fixedLengthPacked)
				{
					ProtoWriter.WritePackedPrefix(arr.Count, packedWireType, dest);
					token = default(SubItemToken);
				}
				else
				{
					token = ProtoWriter.StartSubItem(value, dest);
				}
				ProtoWriter.SetPackedField(fieldNumber, dest);
			}
			else
			{
				token = default(SubItemToken);
			}
			bool checkForNull = !SupportNull;
			for (int i = 0; i < len; i++)
			{
				object obj = arr[i];
				if (checkForNull && obj == null)
				{
					throw new NullReferenceException();
				}
				Tail.Write(obj, dest);
			}
			if (writePacked)
			{
				if (fixedLengthPacked)
				{
					ProtoWriter.ClearPackedField(fieldNumber, dest);
				}
				else
				{
					ProtoWriter.EndSubItem(token, dest);
				}
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			int field = source.FieldNumber;
			BasicList list = new BasicList();
			if (packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				while (ProtoReader.HasSubValue(packedWireType, source))
				{
					list.Add(Tail.Read(null, source));
				}
				ProtoReader.EndSubItem(token, source);
			}
			else
			{
				do
				{
					list.Add(Tail.Read(null, source));
				}
				while (source.TryReadFieldHeader(field));
			}
			int oldLen = AppendToCollection ? ((value != null) ? ((Array)value).Length : 0) : 0;
			Array result = Array.CreateInstance(itemType, oldLen + list.Count);
			if (oldLen != 0)
			{
				((Array)value).CopyTo(result, 0);
			}
			list.CopyTo(result, oldLen);
			return result;
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			Type listType = ctx.MapType(typeof(List<>)).MakeGenericType(itemType);
			Type expected = ExpectedType;
			using (Local oldArr = AppendToCollection ? ctx.GetLocalWithValue(expected, valueFrom) : null)
			{
				using (Local newArr = new Local(ctx, expected))
				{
					using (Local list = new Local(ctx, listType))
					{
						ctx.EmitCtor(listType);
						ctx.StoreValue(list);
						ListDecorator.EmitReadList(ctx, list, Tail, listType.GetMethod("Add"), packedWireType, castListForAdd: false);
						using (Local oldLen = AppendToCollection ? new Local(ctx, ctx.MapType(typeof(int))) : null)
						{
							Type[] copyToArrayInt32Args = new Type[2]
							{
								ctx.MapType(typeof(Array)),
								ctx.MapType(typeof(int))
							};
							if (AppendToCollection)
							{
								ctx.LoadLength(oldArr, zeroIfNull: true);
								ctx.CopyValue();
								ctx.StoreValue(oldLen);
								ctx.LoadAddress(list, listType);
								ctx.LoadValue(listType.GetProperty("Count"));
								ctx.Add();
								ctx.CreateArray(itemType, null);
								ctx.StoreValue(newArr);
								ctx.LoadValue(oldLen);
								CodeLabel nothingToCopy = ctx.DefineLabel();
								ctx.BranchIfFalse(nothingToCopy, @short: true);
								ctx.LoadValue(oldArr);
								ctx.LoadValue(newArr);
								ctx.LoadValue(0);
								ctx.EmitCall(expected.GetMethod("CopyTo", copyToArrayInt32Args));
								ctx.MarkLabel(nothingToCopy);
								ctx.LoadValue(list);
								ctx.LoadValue(newArr);
								ctx.LoadValue(oldLen);
							}
							else
							{
								ctx.LoadAddress(list, listType);
								ctx.LoadValue(listType.GetProperty("Count"));
								ctx.CreateArray(itemType, null);
								ctx.StoreValue(newArr);
								ctx.LoadAddress(list, listType);
								ctx.LoadValue(newArr);
								ctx.LoadValue(0);
							}
							copyToArrayInt32Args[0] = expected;
							MethodInfo copyTo = listType.GetMethod("CopyTo", copyToArrayInt32Args);
							if (copyTo == null)
							{
								copyToArrayInt32Args[1] = ctx.MapType(typeof(Array));
								copyTo = listType.GetMethod("CopyTo", copyToArrayInt32Args);
							}
							ctx.EmitCall(copyTo);
						}
						ctx.LoadValue(newArr);
					}
				}
			}
		}
	}
}
