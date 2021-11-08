using ProtoBuf.Meta;
using System;
using System.IO;
using System.Text;

namespace ProtoBuf
{
	public sealed class ProtoWriter : IDisposable
	{
		private Stream dest;

		private TypeModel model;

		private readonly NetObjectCache netCache = new NetObjectCache();

		private int fieldNumber;

		private int flushLock;

		private WireType wireType;

		private int depth;

		private const int RecursionCheckDepth = 25;

		private MutableList recursionStack;

		private readonly SerializationContext context;

		private byte[] ioBuffer;

		private int ioIndex;

		private long position64;

		private static readonly UTF8Encoding encoding = new UTF8Encoding();

		private int packedFieldNumber;

		internal NetObjectCache NetCache => netCache;

		internal WireType WireType => wireType;

		public SerializationContext Context => context;

		public TypeModel Model => model;

		public static void WriteObject(object value, int key, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			SubItemToken token = StartSubItem(value, writer);
			if (key >= 0)
			{
				writer.model.Serialize(key, value, writer);
			}
			else if (writer.model == null || !writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, isInsideList: false, null))
			{
				TypeModel.ThrowUnexpectedType(value.GetType());
			}
			EndSubItem(token, writer);
		}

		public static void WriteRecursionSafeObject(object value, int key, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			SubItemToken token = StartSubItem(null, writer);
			writer.model.Serialize(key, value, writer);
			EndSubItem(token, writer);
		}

		internal static void WriteObject(object value, int key, ProtoWriter writer, PrefixStyle style, int fieldNumber)
		{
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			if (writer.wireType != WireType.None)
			{
				throw CreateException(writer);
			}
			switch (style)
			{
			case PrefixStyle.Base128:
				writer.wireType = WireType.String;
				writer.fieldNumber = fieldNumber;
				if (fieldNumber > 0)
				{
					WriteHeaderCore(fieldNumber, WireType.String, writer);
				}
				break;
			case PrefixStyle.Fixed32:
			case PrefixStyle.Fixed32BigEndian:
				writer.fieldNumber = 0;
				writer.wireType = WireType.Fixed32;
				break;
			default:
				throw new ArgumentOutOfRangeException("style");
			}
			SubItemToken token = StartSubItem(value, writer, allowFixed: true);
			if (key < 0)
			{
				if (!writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, isInsideList: false, null))
				{
					TypeModel.ThrowUnexpectedType(value.GetType());
				}
			}
			else
			{
				writer.model.Serialize(key, value, writer);
			}
			EndSubItem(token, writer, style);
		}

		internal int GetTypeKey(ref Type type)
		{
			return model.GetKey(ref type);
		}

		public static void WriteFieldHeader(int fieldNumber, WireType wireType, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.wireType != WireType.None)
			{
				throw new InvalidOperationException("Cannot write a " + wireType.ToString() + " header until the " + writer.wireType.ToString() + " data has been written");
			}
			if (fieldNumber < 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (writer.packedFieldNumber == 0)
			{
				writer.fieldNumber = fieldNumber;
				writer.wireType = wireType;
				WriteHeaderCore(fieldNumber, wireType, writer);
				return;
			}
			if (writer.packedFieldNumber == fieldNumber)
			{
				if ((uint)wireType > 1u && wireType != WireType.Fixed32 && wireType != WireType.SignedVariant)
				{
					throw new InvalidOperationException("Wire-type cannot be encoded as packed: " + wireType);
				}
				writer.fieldNumber = fieldNumber;
				writer.wireType = wireType;
				return;
			}
			throw new InvalidOperationException("Field mismatch during packed encoding; expected " + writer.packedFieldNumber + " but received " + fieldNumber);
		}

		internal static void WriteHeaderCore(int fieldNumber, WireType wireType, ProtoWriter writer)
		{
			uint header = (uint)((fieldNumber << 3) | (int)(wireType & (WireType)7));
			WriteUInt32Variant(header, writer);
		}

		public static void WriteBytes(byte[] data, ProtoWriter writer)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			WriteBytes(data, 0, data.Length, writer);
		}

		public static void WriteBytes(byte[] data, int offset, int length, ProtoWriter writer)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed32:
				if (length != 4)
				{
					throw new ArgumentException("length");
				}
				break;
			case WireType.Fixed64:
				if (length != 8)
				{
					throw new ArgumentException("length");
				}
				break;
			case WireType.String:
				WriteUInt32Variant((uint)length, writer);
				writer.wireType = WireType.None;
				if (length == 0)
				{
					return;
				}
				if (writer.flushLock == 0 && length > writer.ioBuffer.Length)
				{
					Flush(writer);
					writer.dest.Write(data, offset, length);
					writer.position64 += length;
					return;
				}
				break;
			default:
				throw CreateException(writer);
			}
			DemandSpace(length, writer);
			Helpers.BlockCopy(data, offset, writer.ioBuffer, writer.ioIndex, length);
			IncrementedAndReset(length, writer);
		}

		private static void CopyRawFromStream(Stream source, ProtoWriter writer)
		{
			byte[] buffer = writer.ioBuffer;
			int space = buffer.Length - writer.ioIndex;
			int bytesRead3 = 1;
			while (space > 0 && (bytesRead3 = source.Read(buffer, writer.ioIndex, space)) > 0)
			{
				writer.ioIndex += bytesRead3;
				writer.position64 += bytesRead3;
				space -= bytesRead3;
			}
			if (bytesRead3 <= 0)
			{
				return;
			}
			if (writer.flushLock == 0)
			{
				Flush(writer);
				while ((bytesRead3 = source.Read(buffer, 0, buffer.Length)) > 0)
				{
					writer.dest.Write(buffer, 0, bytesRead3);
					writer.position64 += bytesRead3;
				}
				return;
			}
			while (true)
			{
				DemandSpace(128, writer);
				if ((bytesRead3 = source.Read(writer.ioBuffer, writer.ioIndex, writer.ioBuffer.Length - writer.ioIndex)) > 0)
				{
					writer.position64 += bytesRead3;
					writer.ioIndex += bytesRead3;
					continue;
				}
				break;
			}
		}

		private static void IncrementedAndReset(int length, ProtoWriter writer)
		{
			writer.ioIndex += length;
			writer.position64 += length;
			writer.wireType = WireType.None;
		}

		public static SubItemToken StartSubItem(object instance, ProtoWriter writer)
		{
			return StartSubItem(instance, writer, allowFixed: false);
		}

		private void CheckRecursionStackAndPush(object instance)
		{
			int hitLevel;
			if (recursionStack == null)
			{
				recursionStack = new MutableList();
			}
			else if (instance != null && (hitLevel = recursionStack.IndexOfReference(instance)) >= 0)
			{
				throw new ProtoException("Possible recursion detected (offset: " + (recursionStack.Count - hitLevel) + " level(s)): " + instance.ToString());
			}
			recursionStack.Add(instance);
		}

		private void PopRecursionStack()
		{
			recursionStack.RemoveLast();
		}

		private static SubItemToken StartSubItem(object instance, ProtoWriter writer, bool allowFixed)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (++writer.depth > 25)
			{
				writer.CheckRecursionStackAndPush(instance);
			}
			if (writer.packedFieldNumber != 0)
			{
				throw new InvalidOperationException("Cannot begin a sub-item while performing packed encoding");
			}
			switch (writer.wireType)
			{
			case WireType.StartGroup:
				writer.wireType = WireType.None;
				return new SubItemToken((long)(-writer.fieldNumber));
			case WireType.String:
				writer.wireType = WireType.None;
				DemandSpace(32, writer);
				writer.flushLock++;
				writer.position64++;
				return new SubItemToken((long)writer.ioIndex++);
			case WireType.Fixed32:
			{
				if (!allowFixed)
				{
					throw CreateException(writer);
				}
				DemandSpace(32, writer);
				writer.flushLock++;
				SubItemToken token = new SubItemToken((long)writer.ioIndex);
				IncrementedAndReset(4, writer);
				return token;
			}
			default:
				throw CreateException(writer);
			}
		}

		public static void EndSubItem(SubItemToken token, ProtoWriter writer)
		{
			EndSubItem(token, writer, PrefixStyle.Base128);
		}

		private static void EndSubItem(SubItemToken token, ProtoWriter writer, PrefixStyle style)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.wireType != WireType.None)
			{
				throw CreateException(writer);
			}
			int value = (int)token.value64;
			if (writer.depth <= 0)
			{
				throw CreateException(writer);
			}
			if (writer.depth-- > 25)
			{
				writer.PopRecursionStack();
			}
			writer.packedFieldNumber = 0;
			if (value < 0)
			{
				WriteHeaderCore(-value, WireType.EndGroup, writer);
				writer.wireType = WireType.None;
				return;
			}
			switch (style)
			{
			case PrefixStyle.Fixed32:
			{
				int len = writer.ioIndex - value - 4;
				WriteInt32ToBuffer(len, writer.ioBuffer, value);
				break;
			}
			case PrefixStyle.Fixed32BigEndian:
			{
				int len = writer.ioIndex - value - 4;
				byte[] buffer = writer.ioBuffer;
				WriteInt32ToBuffer(len, buffer, value);
				byte b2 = buffer[value];
				buffer[value] = buffer[value + 3];
				buffer[value + 3] = b2;
				b2 = buffer[value + 1];
				buffer[value + 1] = buffer[value + 2];
				buffer[value + 2] = b2;
				break;
			}
			case PrefixStyle.Base128:
			{
				int len = writer.ioIndex - value - 1;
				int offset = 0;
				uint tmp2 = (uint)len;
				while ((tmp2 >>= 7) != 0)
				{
					offset++;
				}
				if (offset == 0)
				{
					writer.ioBuffer[value] = (byte)(len & 0x7F);
					break;
				}
				DemandSpace(offset, writer);
				byte[] blob = writer.ioBuffer;
				Helpers.BlockCopy(blob, value + 1, blob, value + 1 + offset, len);
				tmp2 = (uint)len;
				do
				{
					blob[value++] = (byte)((tmp2 & 0x7F) | 0x80);
				}
				while ((tmp2 >>= 7) != 0);
				blob[value - 1] = (byte)(blob[value - 1] & -129);
				writer.position64 += offset;
				writer.ioIndex += offset;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("style");
			}
			if (--writer.flushLock == 0 && writer.ioIndex >= 1024)
			{
				Flush(writer);
			}
		}

		public ProtoWriter(Stream dest, TypeModel model, SerializationContext context)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (!dest.CanWrite)
			{
				throw new ArgumentException("Cannot write to stream", "dest");
			}
			this.dest = dest;
			ioBuffer = BufferPool.GetBuffer();
			this.model = model;
			wireType = WireType.None;
			if (context == null)
			{
				context = SerializationContext.Default;
			}
			else
			{
				context.Freeze();
			}
			this.context = context;
		}

		void IDisposable.Dispose()
		{
			Dispose();
		}

		private void Dispose()
		{
			if (dest != null)
			{
				Flush(this);
				dest = null;
			}
			model = null;
			BufferPool.ReleaseBufferToPool(ref ioBuffer);
		}

		internal static long GetLongPosition(ProtoWriter writer)
		{
			return writer.position64;
		}

		internal static int GetPosition(ProtoWriter writer)
		{
			return checked((int)writer.position64);
		}

		private static void DemandSpace(int required, ProtoWriter writer)
		{
			if (writer.ioBuffer.Length - writer.ioIndex >= required)
			{
				return;
			}
			if (writer.flushLock == 0)
			{
				Flush(writer);
				if (writer.ioBuffer.Length - writer.ioIndex >= required)
				{
					return;
				}
			}
			BufferPool.ResizeAndFlushLeft(ref writer.ioBuffer, required + writer.ioIndex, 0, writer.ioIndex);
		}

		public void Close()
		{
			if (depth != 0 || flushLock != 0)
			{
				throw new InvalidOperationException("Unable to close stream in an incomplete state");
			}
			Dispose();
		}

		internal void CheckDepthFlushlock()
		{
			if (depth != 0 || flushLock != 0)
			{
				throw new InvalidOperationException("The writer is in an incomplete state");
			}
		}

		internal static void Flush(ProtoWriter writer)
		{
			if (writer.flushLock == 0 && writer.ioIndex != 0)
			{
				writer.dest.Write(writer.ioBuffer, 0, writer.ioIndex);
				writer.ioIndex = 0;
			}
		}

		private static void WriteUInt32Variant(uint value, ProtoWriter writer)
		{
			DemandSpace(5, writer);
			int count = 0;
			do
			{
				writer.ioBuffer[writer.ioIndex++] = (byte)((value & 0x7F) | 0x80);
				count++;
			}
			while ((value >>= 7) != 0);
			writer.ioBuffer[writer.ioIndex - 1] &= 127;
			writer.position64 += count;
		}

		internal static uint Zig(int value)
		{
			return (uint)((value << 1) ^ (value >> 31));
		}

		internal static ulong Zig(long value)
		{
			return (ulong)((value << 1) ^ (value >> 63));
		}

		private static void WriteUInt64Variant(ulong value, ProtoWriter writer)
		{
			DemandSpace(10, writer);
			int count = 0;
			do
			{
				writer.ioBuffer[writer.ioIndex++] = (byte)((value & 0x7F) | 0x80);
				count++;
			}
			while ((value >>= 7) != 0L);
			writer.ioBuffer[writer.ioIndex - 1] &= 127;
			writer.position64 += count;
		}

		public static void WriteString(string value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.wireType != WireType.String)
			{
				throw CreateException(writer);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				WriteUInt32Variant(0u, writer);
				writer.wireType = WireType.None;
				return;
			}
			int predicted = encoding.GetByteCount(value);
			WriteUInt32Variant((uint)predicted, writer);
			DemandSpace(predicted, writer);
			int actual = encoding.GetBytes(value, 0, value.Length, writer.ioBuffer, writer.ioIndex);
			IncrementedAndReset(actual, writer);
		}

		public static void WriteUInt64(ulong value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed64:
				WriteInt64((long)value, writer);
				break;
			case WireType.Variant:
				WriteUInt64Variant(value, writer);
				writer.wireType = WireType.None;
				break;
			case WireType.Fixed32:
				WriteUInt32(checked((uint)value), writer);
				break;
			default:
				throw CreateException(writer);
			}
		}

		public static void WriteInt64(long value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed64:
			{
				DemandSpace(8, writer);
				byte[] buffer = writer.ioBuffer;
				int index = writer.ioIndex;
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				buffer[index + 4] = (byte)(value >> 32);
				buffer[index + 5] = (byte)(value >> 40);
				buffer[index + 6] = (byte)(value >> 48);
				buffer[index + 7] = (byte)(value >> 56);
				IncrementedAndReset(8, writer);
				break;
			}
			case WireType.SignedVariant:
				WriteUInt64Variant(Zig(value), writer);
				writer.wireType = WireType.None;
				break;
			case WireType.Variant:
			{
				if (value >= 0)
				{
					WriteUInt64Variant((ulong)value, writer);
					writer.wireType = WireType.None;
					break;
				}
				DemandSpace(10, writer);
				byte[] buffer = writer.ioBuffer;
				int index = writer.ioIndex;
				buffer[index] = (byte)(value | 0x80);
				buffer[index + 1] = (byte)((int)(value >> 7) | 0x80);
				buffer[index + 2] = (byte)((int)(value >> 14) | 0x80);
				buffer[index + 3] = (byte)((int)(value >> 21) | 0x80);
				buffer[index + 4] = (byte)((int)(value >> 28) | 0x80);
				buffer[index + 5] = (byte)((int)(value >> 35) | 0x80);
				buffer[index + 6] = (byte)((int)(value >> 42) | 0x80);
				buffer[index + 7] = (byte)((int)(value >> 49) | 0x80);
				buffer[index + 8] = (byte)((int)(value >> 56) | 0x80);
				buffer[index + 9] = 1;
				IncrementedAndReset(10, writer);
				break;
			}
			case WireType.Fixed32:
				WriteInt32(checked((int)value), writer);
				break;
			default:
				throw CreateException(writer);
			}
		}

		public static void WriteUInt32(uint value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed32:
				WriteInt32((int)value, writer);
				break;
			case WireType.Fixed64:
				WriteInt64((int)value, writer);
				break;
			case WireType.Variant:
				WriteUInt32Variant(value, writer);
				writer.wireType = WireType.None;
				break;
			default:
				throw CreateException(writer);
			}
		}

		public static void WriteInt16(short value, ProtoWriter writer)
		{
			WriteInt32(value, writer);
		}

		public static void WriteUInt16(ushort value, ProtoWriter writer)
		{
			WriteUInt32(value, writer);
		}

		public static void WriteByte(byte value, ProtoWriter writer)
		{
			WriteUInt32(value, writer);
		}

		public static void WriteSByte(sbyte value, ProtoWriter writer)
		{
			WriteInt32(value, writer);
		}

		private static void WriteInt32ToBuffer(int value, byte[] buffer, int index)
		{
			buffer[index] = (byte)value;
			buffer[index + 1] = (byte)(value >> 8);
			buffer[index + 2] = (byte)(value >> 16);
			buffer[index + 3] = (byte)(value >> 24);
		}

		public static void WriteInt32(int value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed32:
				DemandSpace(4, writer);
				WriteInt32ToBuffer(value, writer.ioBuffer, writer.ioIndex);
				IncrementedAndReset(4, writer);
				break;
			case WireType.Fixed64:
			{
				DemandSpace(8, writer);
				byte[] buffer = writer.ioBuffer;
				int index = writer.ioIndex;
				buffer[index] = (byte)value;
				buffer[index + 1] = (byte)(value >> 8);
				buffer[index + 2] = (byte)(value >> 16);
				buffer[index + 3] = (byte)(value >> 24);
				byte[] array4 = buffer;
				int num4 = index + 4;
				byte[] array5 = buffer;
				int num5 = index + 5;
				byte b;
				buffer[index + 6] = (b = (buffer[index + 7] = 0));
				array5[num5] = (b = b);
				array4[num4] = b;
				IncrementedAndReset(8, writer);
				break;
			}
			case WireType.SignedVariant:
				WriteUInt32Variant(Zig(value), writer);
				writer.wireType = WireType.None;
				break;
			case WireType.Variant:
			{
				if (value >= 0)
				{
					WriteUInt32Variant((uint)value, writer);
					writer.wireType = WireType.None;
					break;
				}
				DemandSpace(10, writer);
				byte[] buffer = writer.ioBuffer;
				int index = writer.ioIndex;
				buffer[index] = (byte)(value | 0x80);
				buffer[index + 1] = (byte)((value >> 7) | 0x80);
				buffer[index + 2] = (byte)((value >> 14) | 0x80);
				buffer[index + 3] = (byte)((value >> 21) | 0x80);
				buffer[index + 4] = (byte)((value >> 28) | 0x80);
				byte[] array = buffer;
				int num = index + 5;
				byte[] array2 = buffer;
				int num2 = index + 6;
				byte[] array3 = buffer;
				int num3 = index + 7;
				byte b;
				buffer[index + 8] = (b = byte.MaxValue);
				array3[num3] = (b = b);
				array2[num2] = (b = b);
				array[num] = b;
				buffer[index + 9] = 1;
				IncrementedAndReset(10, writer);
				break;
			}
			default:
				throw CreateException(writer);
			}
		}

		public unsafe static void WriteDouble(double value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed32:
			{
				float f = (float)value;
				if (Helpers.IsInfinity(f) && !Helpers.IsInfinity(value))
				{
					throw new OverflowException();
				}
				WriteSingle(f, writer);
				break;
			}
			case WireType.Fixed64:
				WriteInt64(*(long*)(&value), writer);
				break;
			default:
				throw CreateException(writer);
			}
		}

		public unsafe static void WriteSingle(float value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed32:
				WriteInt32(*(int*)(&value), writer);
				break;
			case WireType.Fixed64:
				WriteDouble(value, writer);
				break;
			default:
				throw CreateException(writer);
			}
		}

		public static void ThrowEnumException(ProtoWriter writer, object enumValue)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			string rhs = (enumValue == null) ? "<null>" : (enumValue.GetType().FullName + "." + enumValue.ToString());
			throw new ProtoException("No wire-value is mapped to the enum " + rhs + " at position " + writer.position64);
		}

		internal static Exception CreateException(ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			return new ProtoException("Invalid serialization operation with wire-type " + writer.wireType.ToString() + " at position " + writer.position64);
		}

		public static void WriteBoolean(bool value, ProtoWriter writer)
		{
			WriteUInt32(value ? 1u : 0u, writer);
		}

		public static void AppendExtensionData(IExtensible instance, ProtoWriter writer)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.wireType != WireType.None)
			{
				throw CreateException(writer);
			}
			IExtension extn = instance.GetExtensionObject(createIfMissing: false);
			if (extn != null)
			{
				Stream source = extn.BeginQuery();
				try
				{
					CopyRawFromStream(source, writer);
				}
				finally
				{
					extn.EndQuery(source);
				}
			}
		}

		public static void SetPackedField(int fieldNumber, ProtoWriter writer)
		{
			if (fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.packedFieldNumber = fieldNumber;
		}

		public static void ClearPackedField(int fieldNumber, ProtoWriter writer)
		{
			if (fieldNumber != writer.packedFieldNumber)
			{
				throw new InvalidOperationException("Field mismatch during packed encoding; expected " + writer.packedFieldNumber + " but received " + fieldNumber);
			}
			writer.packedFieldNumber = 0;
		}

		public static void WritePackedPrefix(int elementCount, WireType wireType, ProtoWriter writer)
		{
			if (writer.WireType != WireType.String)
			{
				throw new InvalidOperationException("Invalid wire-type: " + writer.WireType);
			}
			if (elementCount < 0)
			{
				throw new ArgumentOutOfRangeException("elementCount");
			}
			ulong bytes;
			switch (wireType)
			{
			case WireType.Fixed32:
				bytes = (ulong)((long)elementCount << 2);
				break;
			case WireType.Fixed64:
				bytes = (ulong)((long)elementCount << 3);
				break;
			default:
				throw new ArgumentOutOfRangeException("wireType", "Invalid wire-type: " + wireType);
			}
			WriteUInt64Variant(bytes, writer);
			writer.wireType = WireType.None;
		}

		internal string SerializeType(Type type)
		{
			return TypeModel.SerializeType(model, type);
		}

		public void SetRootObject(object value)
		{
			NetCache.SetKeyedObject(0, value);
		}

		public static void WriteType(Type value, ProtoWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			WriteString(writer.SerializeType(value), writer);
		}
	}
}
