using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

namespace ProtoBuf.Serializers
{
	internal sealed class EnumSerializer : IProtoSerializer
	{
		public struct EnumPair
		{
			public readonly object RawValue;

			public readonly Enum TypedValue;

			public readonly int WireValue;

			public EnumPair(int wireValue, object raw, Type type)
			{
				WireValue = wireValue;
				RawValue = raw;
				TypedValue = (Enum)Enum.ToObject(type, raw);
			}
		}

		private readonly Type enumType;

		private readonly EnumPair[] map;

		public Type ExpectedType => enumType;

		bool IProtoSerializer.RequiresOldValue => false;

		bool IProtoSerializer.ReturnsValue => true;

		public EnumSerializer(Type enumType, EnumPair[] map)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			this.enumType = enumType;
			this.map = map;
			if (map == null)
			{
				return;
			}
			for (int i = 1; i < map.Length; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (map[i].WireValue == map[j].WireValue && !object.Equals(map[i].RawValue, map[j].RawValue))
					{
						throw new ProtoException("Multiple enums with wire-value " + map[i].WireValue);
					}
					if (object.Equals(map[i].RawValue, map[j].RawValue) && map[i].WireValue != map[j].WireValue)
					{
						throw new ProtoException("Multiple enums with deserialized-value " + map[i].RawValue);
					}
				}
			}
		}

		private ProtoTypeCode GetTypeCode()
		{
			Type type = Helpers.GetUnderlyingType(enumType);
			if (type == null)
			{
				type = enumType;
			}
			return Helpers.GetTypeCode(type);
		}

		private int EnumToWire(object value)
		{
			switch (GetTypeCode())
			{
			case ProtoTypeCode.Byte:
				return (byte)value;
			case ProtoTypeCode.SByte:
				return (sbyte)value;
			case ProtoTypeCode.Int16:
				return (short)value;
			case ProtoTypeCode.Int32:
				return (int)value;
			case ProtoTypeCode.Int64:
				return (int)(long)value;
			case ProtoTypeCode.UInt16:
				return (ushort)value;
			case ProtoTypeCode.UInt32:
				return (int)(uint)value;
			case ProtoTypeCode.UInt64:
				return (int)(ulong)value;
			default:
				throw new InvalidOperationException();
			}
		}

		private object WireToEnum(int value)
		{
			switch (GetTypeCode())
			{
			case ProtoTypeCode.Byte:
				return Enum.ToObject(enumType, (byte)value);
			case ProtoTypeCode.SByte:
				return Enum.ToObject(enumType, (sbyte)value);
			case ProtoTypeCode.Int16:
				return Enum.ToObject(enumType, (short)value);
			case ProtoTypeCode.Int32:
				return Enum.ToObject(enumType, value);
			case ProtoTypeCode.Int64:
				return Enum.ToObject(enumType, (long)value);
			case ProtoTypeCode.UInt16:
				return Enum.ToObject(enumType, (ushort)value);
			case ProtoTypeCode.UInt32:
				return Enum.ToObject(enumType, (uint)value);
			case ProtoTypeCode.UInt64:
				return Enum.ToObject(enumType, (ulong)value);
			default:
				throw new InvalidOperationException();
			}
		}

		public object Read(object value, ProtoReader source)
		{
			int wireValue = source.ReadInt32();
			if (map == null)
			{
				return WireToEnum(wireValue);
			}
			for (int i = 0; i < map.Length; i++)
			{
				if (map[i].WireValue == wireValue)
				{
					return map[i].TypedValue;
				}
			}
			source.ThrowEnumException(ExpectedType, wireValue);
			return null;
		}

		public void Write(object value, ProtoWriter dest)
		{
			if (map == null)
			{
				ProtoWriter.WriteInt32(EnumToWire(value), dest);
				return;
			}
			for (int i = 0; i < map.Length; i++)
			{
				if (object.Equals(map[i].TypedValue, value))
				{
					ProtoWriter.WriteInt32(map[i].WireValue, dest);
					return;
				}
			}
			ProtoWriter.ThrowEnumException(dest, value);
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ProtoTypeCode typeCode = GetTypeCode();
			if (map == null)
			{
				ctx.LoadValue(valueFrom);
				ctx.ConvertToInt32(typeCode, uint32Overflow: false);
				ctx.EmitBasicWrite("WriteInt32", null);
			}
			else
			{
				using (Local loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
				{
					CodeLabel @continue = ctx.DefineLabel();
					for (int i = 0; i < map.Length; i++)
					{
						CodeLabel tryNextValue = ctx.DefineLabel();
						CodeLabel processThisValue = ctx.DefineLabel();
						ctx.LoadValue(loc);
						WriteEnumValue(ctx, typeCode, map[i].RawValue);
						ctx.BranchIfEqual(processThisValue, @short: true);
						ctx.Branch(tryNextValue, @short: true);
						ctx.MarkLabel(processThisValue);
						ctx.LoadValue(map[i].WireValue);
						ctx.EmitBasicWrite("WriteInt32", null);
						ctx.Branch(@continue, @short: false);
						ctx.MarkLabel(tryNextValue);
					}
					ctx.LoadReaderWriter();
					ctx.LoadValue(loc);
					ctx.CastToObject(ExpectedType);
					ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("ThrowEnumException"));
					ctx.MarkLabel(@continue);
				}
			}
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			ProtoTypeCode typeCode = GetTypeCode();
			if (map == null)
			{
				ctx.EmitBasicRead("ReadInt32", ctx.MapType(typeof(int)));
				ctx.ConvertFromInt32(typeCode, uint32Overflow: false);
				return;
			}
			int[] wireValues = new int[map.Length];
			object[] values = new object[map.Length];
			for (int k = 0; k < map.Length; k++)
			{
				wireValues[k] = map[k].WireValue;
				values[k] = map[k].RawValue;
			}
			using (Local result = new Local(ctx, ExpectedType))
			{
				using (Local wireValue = new Local(ctx, ctx.MapType(typeof(int))))
				{
					ctx.EmitBasicRead("ReadInt32", ctx.MapType(typeof(int)));
					ctx.StoreValue(wireValue);
					CodeLabel @continue = ctx.DefineLabel();
					BasicList.NodeEnumerator enumerator = BasicList.GetContiguousGroups(wireValues, values).GetEnumerator();
					while (enumerator.MoveNext())
					{
						BasicList.Group group = (BasicList.Group)enumerator.Current;
						CodeLabel tryNextGroup = ctx.DefineLabel();
						int groupItemCount = group.Items.Count;
						if (groupItemCount == 1)
						{
							ctx.LoadValue(wireValue);
							ctx.LoadValue(group.First);
							CodeLabel processThisValue = ctx.DefineLabel();
							ctx.BranchIfEqual(processThisValue, @short: true);
							ctx.Branch(tryNextGroup, @short: false);
							WriteEnumValue(ctx, typeCode, processThisValue, @continue, group.Items[0], result);
						}
						else
						{
							ctx.LoadValue(wireValue);
							ctx.LoadValue(group.First);
							ctx.Subtract();
							CodeLabel[] jmp = new CodeLabel[groupItemCount];
							for (int j = 0; j < groupItemCount; j++)
							{
								jmp[j] = ctx.DefineLabel();
							}
							ctx.Switch(jmp);
							ctx.Branch(tryNextGroup, @short: false);
							for (int i = 0; i < groupItemCount; i++)
							{
								WriteEnumValue(ctx, typeCode, jmp[i], @continue, group.Items[i], result);
							}
						}
						ctx.MarkLabel(tryNextGroup);
					}
					ctx.LoadReaderWriter();
					ctx.LoadValue(ExpectedType);
					ctx.LoadValue(wireValue);
					ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("ThrowEnumException"));
					ctx.MarkLabel(@continue);
					ctx.LoadValue(result);
				}
			}
		}

		private static void WriteEnumValue(CompilerContext ctx, ProtoTypeCode typeCode, object value)
		{
			switch (typeCode)
			{
			case ProtoTypeCode.Byte:
				ctx.LoadValue((byte)value);
				break;
			case ProtoTypeCode.SByte:
				ctx.LoadValue((sbyte)value);
				break;
			case ProtoTypeCode.Int16:
				ctx.LoadValue((short)value);
				break;
			case ProtoTypeCode.Int32:
				ctx.LoadValue((int)value);
				break;
			case ProtoTypeCode.Int64:
				ctx.LoadValue((long)value);
				break;
			case ProtoTypeCode.UInt16:
				ctx.LoadValue((ushort)value);
				break;
			case ProtoTypeCode.UInt32:
				ctx.LoadValue((int)(uint)value);
				break;
			case ProtoTypeCode.UInt64:
				ctx.LoadValue((long)(ulong)value);
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		private static void WriteEnumValue(CompilerContext ctx, ProtoTypeCode typeCode, CodeLabel handler, CodeLabel @continue, object value, Local local)
		{
			ctx.MarkLabel(handler);
			WriteEnumValue(ctx, typeCode, value);
			ctx.StoreValue(local);
			ctx.Branch(@continue, @short: false);
		}
	}
}
