using System;
using System.Runtime.Serialization;

namespace ProtoBuf
{
	public static class BclHelpers
	{
		[Flags]
		public enum NetObjectOptions : byte
		{
			None = 0x0,
			AsReference = 0x1,
			DynamicType = 0x2,
			UseConstructor = 0x4,
			LateSet = 0x8
		}

		private const int FieldTimeSpanValue = 1;

		private const int FieldTimeSpanScale = 2;

		private const int FieldTimeSpanKind = 3;

		internal static readonly DateTime[] EpochOrigin = new DateTime[3]
		{
			new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
			new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
			new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)
		};

		private static readonly DateTime TimestampEpoch = EpochOrigin[1];

		private const int FieldDecimalLow = 1;

		private const int FieldDecimalHigh = 2;

		private const int FieldDecimalSignScale = 3;

		private const int FieldGuidLow = 1;

		private const int FieldGuidHigh = 2;

		private const int FieldExistingObjectKey = 1;

		private const int FieldNewObjectKey = 2;

		private const int FieldExistingTypeKey = 3;

		private const int FieldNewTypeKey = 4;

		private const int FieldTypeName = 8;

		private const int FieldObject = 10;

		public static object GetUninitializedObject(Type type)
		{
			return FormatterServices.GetUninitializedObject(type);
		}

		public static void WriteTimeSpan(TimeSpan timeSpan, ProtoWriter dest)
		{
			WriteTimeSpanImpl(timeSpan, dest, DateTimeKind.Unspecified);
		}

		private static void WriteTimeSpanImpl(TimeSpan timeSpan, ProtoWriter dest, DateTimeKind kind)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			switch (dest.WireType)
			{
			case WireType.String:
			case WireType.StartGroup:
			{
				long value = timeSpan.Ticks;
				TimeSpanScale scale;
				if (timeSpan == TimeSpan.MaxValue)
				{
					value = 1L;
					scale = TimeSpanScale.MinMax;
				}
				else if (timeSpan == TimeSpan.MinValue)
				{
					value = -1L;
					scale = TimeSpanScale.MinMax;
				}
				else if (value % 864000000000L == 0L)
				{
					scale = TimeSpanScale.Days;
					value /= 864000000000L;
				}
				else if (value % 36000000000L == 0L)
				{
					scale = TimeSpanScale.Hours;
					value /= 36000000000L;
				}
				else if (value % 600000000 == 0L)
				{
					scale = TimeSpanScale.Minutes;
					value /= 600000000;
				}
				else if (value % 10000000 == 0L)
				{
					scale = TimeSpanScale.Seconds;
					value /= 10000000;
				}
				else if (value % 10000 == 0L)
				{
					scale = TimeSpanScale.Milliseconds;
					value /= 10000;
				}
				else
				{
					scale = TimeSpanScale.Ticks;
				}
				SubItemToken token = ProtoWriter.StartSubItem(null, dest);
				if (value != 0L)
				{
					ProtoWriter.WriteFieldHeader(1, WireType.SignedVariant, dest);
					ProtoWriter.WriteInt64(value, dest);
				}
				if (scale != 0)
				{
					ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
					ProtoWriter.WriteInt32((int)scale, dest);
				}
				if (kind != 0)
				{
					ProtoWriter.WriteFieldHeader(3, WireType.Variant, dest);
					ProtoWriter.WriteInt32((int)kind, dest);
				}
				ProtoWriter.EndSubItem(token, dest);
				break;
			}
			case WireType.Fixed64:
				ProtoWriter.WriteInt64(timeSpan.Ticks, dest);
				break;
			default:
				throw new ProtoException("Unexpected wire-type: " + dest.WireType);
			}
		}

		public static TimeSpan ReadTimeSpan(ProtoReader source)
		{
			DateTimeKind kind;
			long ticks = ReadTimeSpanTicks(source, out kind);
			switch (ticks)
			{
			case long.MinValue:
				return TimeSpan.MinValue;
			case long.MaxValue:
				return TimeSpan.MaxValue;
			default:
				return TimeSpan.FromTicks(ticks);
			}
		}

		public static TimeSpan ReadDuration(ProtoReader source)
		{
			long seconds = 0L;
			int nanos = 0;
			SubItemToken token = ProtoReader.StartSubItem(source);
			int fieldNumber;
			while ((fieldNumber = source.ReadFieldHeader()) > 0)
			{
				switch (fieldNumber)
				{
				case 1:
					seconds = source.ReadInt64();
					break;
				case 2:
					nanos = source.ReadInt32();
					break;
				default:
					source.SkipField();
					break;
				}
			}
			ProtoReader.EndSubItem(token, source);
			return FromDurationSeconds(seconds, nanos);
		}

		public static void WriteDuration(TimeSpan value, ProtoWriter dest)
		{
			int nanos;
			long seconds = ToDurationSeconds(value, out nanos);
			WriteSecondsNanos(seconds, nanos, dest);
		}

		private static void WriteSecondsNanos(long seconds, int nanos, ProtoWriter dest)
		{
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (seconds != 0L)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.Variant, dest);
				ProtoWriter.WriteInt64(seconds, dest);
			}
			if (nanos != 0)
			{
				ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
				ProtoWriter.WriteInt32(nanos, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		public static DateTime ReadTimestamp(ProtoReader source)
		{
			return TimestampEpoch + ReadDuration(source);
		}

		public static void WriteTimestamp(DateTime value, ProtoWriter dest)
		{
			int nanos;
			long seconds = ToDurationSeconds(value - TimestampEpoch, out nanos);
			if (nanos < 0)
			{
				seconds--;
				nanos += 1000000000;
			}
			WriteSecondsNanos(seconds, nanos, dest);
		}

		private static TimeSpan FromDurationSeconds(long seconds, int nanos)
		{
			checked
			{
				long ticks = seconds * 10000000 + unchecked(checked(unchecked((long)nanos) * 10000L) / 1000000);
				return TimeSpan.FromTicks(ticks);
			}
		}

		private static long ToDurationSeconds(TimeSpan value, out int nanos)
		{
			nanos = (int)(value.Ticks % 10000000 * 1000000 / 10000);
			return value.Ticks / 10000000;
		}

		public static DateTime ReadDateTime(ProtoReader source)
		{
			DateTimeKind kind;
			long ticks = ReadTimeSpanTicks(source, out kind);
			switch (ticks)
			{
			case long.MinValue:
				return DateTime.MinValue;
			case long.MaxValue:
				return DateTime.MaxValue;
			default:
				return EpochOrigin[(int)kind].AddTicks(ticks);
			}
		}

		public static void WriteDateTime(DateTime value, ProtoWriter dest)
		{
			WriteDateTimeImpl(value, dest, includeKind: false);
		}

		public static void WriteDateTimeWithKind(DateTime value, ProtoWriter dest)
		{
			WriteDateTimeImpl(value, dest, includeKind: true);
		}

		private static void WriteDateTimeImpl(DateTime value, ProtoWriter dest, bool includeKind)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			WireType wireType = dest.WireType;
			TimeSpan delta;
			if ((uint)(wireType - 2) <= 1u)
			{
				if (value == DateTime.MaxValue)
				{
					delta = TimeSpan.MaxValue;
					includeKind = false;
				}
				else if (value == DateTime.MinValue)
				{
					delta = TimeSpan.MinValue;
					includeKind = false;
				}
				else
				{
					delta = value - EpochOrigin[0];
				}
			}
			else
			{
				delta = value - EpochOrigin[0];
			}
			WriteTimeSpanImpl(delta, dest, includeKind ? value.Kind : DateTimeKind.Unspecified);
		}

		private static long ReadTimeSpanTicks(ProtoReader source, out DateTimeKind kind)
		{
			kind = DateTimeKind.Unspecified;
			switch (source.WireType)
			{
			case WireType.String:
			case WireType.StartGroup:
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				TimeSpanScale scale = TimeSpanScale.Days;
				long value = 0L;
				int fieldNumber;
				while ((fieldNumber = source.ReadFieldHeader()) > 0)
				{
					switch (fieldNumber)
					{
					case 2:
						scale = (TimeSpanScale)source.ReadInt32();
						break;
					case 1:
						source.Assert(WireType.SignedVariant);
						value = source.ReadInt64();
						break;
					case 3:
					{
						kind = (DateTimeKind)source.ReadInt32();
						DateTimeKind dateTimeKind = kind;
						if ((uint)dateTimeKind > 2u)
						{
							throw new ProtoException("Invalid date/time kind: " + kind);
						}
						break;
					}
					default:
						source.SkipField();
						break;
					}
				}
				ProtoReader.EndSubItem(token, source);
				switch (scale)
				{
				case TimeSpanScale.Days:
					return value * 864000000000L;
				case TimeSpanScale.Hours:
					return value * 36000000000L;
				case TimeSpanScale.Minutes:
					return value * 600000000;
				case TimeSpanScale.Seconds:
					return value * 10000000;
				case TimeSpanScale.Milliseconds:
					return value * 10000;
				case TimeSpanScale.Ticks:
					return value;
				case TimeSpanScale.MinMax:
					switch (value)
					{
					case 1L:
						return long.MaxValue;
					case -1L:
						return long.MinValue;
					default:
						throw new ProtoException("Unknown min/max value: " + value);
					}
				default:
					throw new ProtoException("Unknown timescale: " + scale);
				}
			}
			case WireType.Fixed64:
				return source.ReadInt64();
			default:
				throw new ProtoException("Unexpected wire-type: " + source.WireType);
			}
		}

		public static decimal ReadDecimal(ProtoReader reader)
		{
			ulong low = 0uL;
			uint high = 0u;
			uint signScale = 0u;
			SubItemToken token = ProtoReader.StartSubItem(reader);
			int fieldNumber;
			while ((fieldNumber = reader.ReadFieldHeader()) > 0)
			{
				switch (fieldNumber)
				{
				case 1:
					low = reader.ReadUInt64();
					break;
				case 2:
					high = reader.ReadUInt32();
					break;
				case 3:
					signScale = reader.ReadUInt32();
					break;
				default:
					reader.SkipField();
					break;
				}
			}
			ProtoReader.EndSubItem(token, reader);
			if (low == 0L && high == 0)
			{
				return 0m;
			}
			int lo = (int)(low & uint.MaxValue);
			int mid = (int)((low >> 32) & uint.MaxValue);
			int hi = (int)high;
			bool isNeg = (signScale & 1) == 1;
			byte scale = (byte)((signScale & 0x1FE) >> 1);
			return new decimal(lo, mid, hi, isNeg, scale);
		}

		public static void WriteDecimal(decimal value, ProtoWriter writer)
		{
			int[] bits = decimal.GetBits(value);
			ulong a = (ulong)((long)bits[1] << 32);
			ulong b = (ulong)(bits[0] & uint.MaxValue);
			ulong low = a | b;
			uint high = (uint)bits[2];
			uint signScale = (uint)(((bits[3] >> 15) & 0x1FE) | ((bits[3] >> 31) & 1));
			SubItemToken token = ProtoWriter.StartSubItem(null, writer);
			if (low != 0L)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
				ProtoWriter.WriteUInt64(low, writer);
			}
			if (high != 0)
			{
				ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
				ProtoWriter.WriteUInt32(high, writer);
			}
			if (signScale != 0)
			{
				ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
				ProtoWriter.WriteUInt32(signScale, writer);
			}
			ProtoWriter.EndSubItem(token, writer);
		}

		public static void WriteGuid(Guid value, ProtoWriter dest)
		{
			byte[] blob = value.ToByteArray();
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (value != Guid.Empty)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.Fixed64, dest);
				ProtoWriter.WriteBytes(blob, 0, 8, dest);
				ProtoWriter.WriteFieldHeader(2, WireType.Fixed64, dest);
				ProtoWriter.WriteBytes(blob, 8, 8, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		public static Guid ReadGuid(ProtoReader source)
		{
			ulong low = 0uL;
			ulong high = 0uL;
			SubItemToken token = ProtoReader.StartSubItem(source);
			int fieldNumber;
			while ((fieldNumber = source.ReadFieldHeader()) > 0)
			{
				switch (fieldNumber)
				{
				case 1:
					low = source.ReadUInt64();
					break;
				case 2:
					high = source.ReadUInt64();
					break;
				default:
					source.SkipField();
					break;
				}
			}
			ProtoReader.EndSubItem(token, source);
			if (low == 0L && high == 0L)
			{
				return Guid.Empty;
			}
			uint a = (uint)(low >> 32);
			uint b = (uint)low;
			uint c = (uint)(high >> 32);
			uint d = (uint)high;
			return new Guid((int)b, (short)a, (short)(a >> 16), (byte)d, (byte)(d >> 8), (byte)(d >> 16), (byte)(d >> 24), (byte)c, (byte)(c >> 8), (byte)(c >> 16), (byte)(c >> 24));
		}

		public static object ReadNetObject(object value, ProtoReader source, int key, Type type, NetObjectOptions options)
		{
			SubItemToken token = ProtoReader.StartSubItem(source);
			int newObjectKey = -1;
			int newTypeKey = -1;
			int fieldNumber;
			while ((fieldNumber = source.ReadFieldHeader()) > 0)
			{
				switch (fieldNumber)
				{
				case 1:
				{
					int tmp2 = source.ReadInt32();
					value = source.NetCache.GetKeyedObject(tmp2);
					break;
				}
				case 2:
					newObjectKey = source.ReadInt32();
					break;
				case 3:
				{
					int tmp2 = source.ReadInt32();
					type = (Type)source.NetCache.GetKeyedObject(tmp2);
					key = source.GetTypeKey(ref type);
					break;
				}
				case 4:
					newTypeKey = source.ReadInt32();
					break;
				case 8:
				{
					string typeName = source.ReadString();
					type = source.DeserializeType(typeName);
					if (type == null)
					{
						throw new ProtoException("Unable to resolve type: " + typeName + " (you can use the TypeModel.DynamicTypeFormatting event to provide a custom mapping)");
					}
					if (type == typeof(string))
					{
						key = -1;
						break;
					}
					key = source.GetTypeKey(ref type);
					if (key >= 0)
					{
						break;
					}
					throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
				}
				case 10:
				{
					bool isString = type == typeof(string);
					bool wasNull = value == null;
					bool lateSet = wasNull && (isString || (options & NetObjectOptions.LateSet) != 0);
					if (newObjectKey >= 0 && !lateSet)
					{
						if (value == null)
						{
							source.TrapNextObject(newObjectKey);
						}
						else
						{
							source.NetCache.SetKeyedObject(newObjectKey, value);
						}
						if (newTypeKey >= 0)
						{
							source.NetCache.SetKeyedObject(newTypeKey, type);
						}
					}
					object oldValue = value;
					value = ((!isString) ? ProtoReader.ReadTypedObject(oldValue, key, source, type) : source.ReadString());
					if (newObjectKey >= 0)
					{
						if (wasNull && !lateSet)
						{
							oldValue = source.NetCache.GetKeyedObject(newObjectKey);
						}
						if (lateSet)
						{
							source.NetCache.SetKeyedObject(newObjectKey, value);
							if (newTypeKey >= 0)
							{
								source.NetCache.SetKeyedObject(newTypeKey, type);
							}
						}
					}
					if (newObjectKey >= 0 && !lateSet && oldValue != value)
					{
						throw new ProtoException("A reference-tracked object changed reference during deserialization");
					}
					if (newObjectKey < 0 && newTypeKey >= 0)
					{
						source.NetCache.SetKeyedObject(newTypeKey, type);
					}
					break;
				}
				default:
					source.SkipField();
					break;
				}
			}
			if (newObjectKey >= 0 && (options & NetObjectOptions.AsReference) == 0)
			{
				throw new ProtoException("Object key in input stream, but reference-tracking was not expected");
			}
			ProtoReader.EndSubItem(token, source);
			return value;
		}

		public static void WriteNetObject(object value, ProtoWriter dest, int key, NetObjectOptions options)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			bool dynamicType = (options & NetObjectOptions.DynamicType) != 0;
			bool asReference = (options & NetObjectOptions.AsReference) != 0;
			WireType wireType = dest.WireType;
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			bool writeObject = true;
			if (asReference)
			{
				bool existing2;
				int objectKey = dest.NetCache.AddObjectKey(value, out existing2);
				ProtoWriter.WriteFieldHeader(existing2 ? 1 : 2, WireType.Variant, dest);
				ProtoWriter.WriteInt32(objectKey, dest);
				if (existing2)
				{
					writeObject = false;
				}
			}
			if (writeObject)
			{
				if (dynamicType)
				{
					Type type = value.GetType();
					if (!(value is string))
					{
						key = dest.GetTypeKey(ref type);
						if (key < 0)
						{
							throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
						}
					}
					bool existing;
					int typeKey = dest.NetCache.AddObjectKey(type, out existing);
					ProtoWriter.WriteFieldHeader(existing ? 3 : 4, WireType.Variant, dest);
					ProtoWriter.WriteInt32(typeKey, dest);
					if (!existing)
					{
						ProtoWriter.WriteFieldHeader(8, WireType.String, dest);
						ProtoWriter.WriteString(dest.SerializeType(type), dest);
					}
				}
				ProtoWriter.WriteFieldHeader(10, wireType, dest);
				if (value is string)
				{
					ProtoWriter.WriteString((string)value, dest);
				}
				else
				{
					ProtoWriter.WriteObject(value, key, dest);
				}
			}
			ProtoWriter.EndSubItem(token, dest);
		}
	}
}
