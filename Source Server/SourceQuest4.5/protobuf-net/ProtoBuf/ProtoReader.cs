using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProtoBuf
{
	public sealed class ProtoReader : IDisposable
	{
		private Stream source;

		private byte[] ioBuffer;

		private TypeModel model;

		private int fieldNumber;

		private int depth;

		private int ioIndex;

		private int available;

		private long position64;

		private long blockEnd64;

		private long dataRemaining64;

		private WireType wireType;

		private bool isFixedLength;

		private bool internStrings;

		private NetObjectCache netCache;

		private uint trapCount;

		internal const long TO_EOF = -1L;

		private SerializationContext context;

		private const long Int64Msb = long.MinValue;

		private const int Int32Msb = int.MinValue;

		private Dictionary<string, string> stringInterner;

		private static readonly UTF8Encoding encoding = new UTF8Encoding();

		private static readonly byte[] EmptyBlob = new byte[0];

		[ThreadStatic]
		private static ProtoReader lastReader;

		public int FieldNumber => fieldNumber;

		public WireType WireType => wireType;

		public bool InternStrings
		{
			get
			{
				return internStrings;
			}
			set
			{
				internStrings = value;
			}
		}

		public SerializationContext Context => context;

		public int Position => checked((int)position64);

		public long LongPosition => position64;

		public TypeModel Model => model;

		internal NetObjectCache NetCache => netCache;

		public ProtoReader(Stream source, TypeModel model, SerializationContext context)
		{
			Init(this, source, model, context, -1L);
		}

		public ProtoReader(Stream source, TypeModel model, SerializationContext context, int length)
		{
			Init(this, source, model, context, length);
		}

		public ProtoReader(Stream source, TypeModel model, SerializationContext context, long length)
		{
			Init(this, source, model, context, length);
		}

		private static void Init(ProtoReader reader, Stream source, TypeModel model, SerializationContext context, long length)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!source.CanRead)
			{
				throw new ArgumentException("Cannot read from stream", "source");
			}
			reader.source = source;
			reader.ioBuffer = BufferPool.GetBuffer();
			reader.model = model;
			reader.dataRemaining64 = ((reader.isFixedLength = (length >= 0)) ? length : 0);
			if (context == null)
			{
				context = SerializationContext.Default;
			}
			else
			{
				context.Freeze();
			}
			reader.context = context;
			reader.position64 = 0L;
			reader.available = (reader.depth = (reader.fieldNumber = (reader.ioIndex = 0)));
			reader.blockEnd64 = long.MaxValue;
			reader.internStrings = true;
			reader.wireType = WireType.None;
			reader.trapCount = 1u;
			if (reader.netCache == null)
			{
				reader.netCache = new NetObjectCache();
			}
		}

		public void Dispose()
		{
			source = null;
			model = null;
			BufferPool.ReleaseBufferToPool(ref ioBuffer);
			if (stringInterner != null)
			{
				stringInterner.Clear();
				stringInterner = null;
			}
			if (netCache != null)
			{
				netCache.Clear();
			}
		}

		internal int TryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
		{
			if (available < 10)
			{
				Ensure(10, strict: false);
			}
			if (available == 0)
			{
				value = 0u;
				return 0;
			}
			int readPos6 = ioIndex;
			value = ioBuffer[readPos6++];
			if ((value & 0x80) == 0)
			{
				return 1;
			}
			value &= 127u;
			if (available == 1)
			{
				throw EoF(this);
			}
			uint chunk4 = ioBuffer[readPos6++];
			value |= (chunk4 & 0x7F) << 7;
			if ((chunk4 & 0x80) == 0)
			{
				return 2;
			}
			if (available == 2)
			{
				throw EoF(this);
			}
			chunk4 = ioBuffer[readPos6++];
			value |= (chunk4 & 0x7F) << 14;
			if ((chunk4 & 0x80) == 0)
			{
				return 3;
			}
			if (available == 3)
			{
				throw EoF(this);
			}
			chunk4 = ioBuffer[readPos6++];
			value |= (chunk4 & 0x7F) << 21;
			if ((chunk4 & 0x80) == 0)
			{
				return 4;
			}
			if (available == 4)
			{
				throw EoF(this);
			}
			chunk4 = ioBuffer[readPos6];
			value |= chunk4 << 28;
			if ((chunk4 & 0xF0) == 0)
			{
				return 5;
			}
			if (trimNegative && (chunk4 & 0xF0) == 240 && available >= 10 && ioBuffer[++readPos6] == byte.MaxValue && ioBuffer[++readPos6] == byte.MaxValue && ioBuffer[++readPos6] == byte.MaxValue && ioBuffer[++readPos6] == byte.MaxValue && ioBuffer[++readPos6] == 1)
			{
				return 10;
			}
			throw AddErrorData(new OverflowException(), this);
		}

		private uint ReadUInt32Variant(bool trimNegative)
		{
			uint value;
			int read = TryReadUInt32VariantWithoutMoving(trimNegative, out value);
			if (read > 0)
			{
				ioIndex += read;
				available -= read;
				position64 += read;
				return value;
			}
			throw EoF(this);
		}

		private bool TryReadUInt32Variant(out uint value)
		{
			int read = TryReadUInt32VariantWithoutMoving(trimNegative: false, out value);
			if (read > 0)
			{
				ioIndex += read;
				available -= read;
				position64 += read;
				return true;
			}
			return false;
		}

		public uint ReadUInt32()
		{
			switch (wireType)
			{
			case WireType.Variant:
				return ReadUInt32Variant(trimNegative: false);
			case WireType.Fixed32:
				if (available < 4)
				{
					Ensure(4, strict: true);
				}
				position64 += 4L;
				available -= 4;
				return (uint)(ioBuffer[ioIndex++] | (ioBuffer[ioIndex++] << 8) | (ioBuffer[ioIndex++] << 16) | (ioBuffer[ioIndex++] << 24));
			case WireType.Fixed64:
			{
				ulong val = ReadUInt64();
				return checked((uint)val);
			}
			default:
				throw CreateWireTypeException();
			}
		}

		internal void Ensure(int count, bool strict)
		{
			if (count > ioBuffer.Length)
			{
				BufferPool.ResizeAndFlushLeft(ref ioBuffer, count, ioIndex, available);
				ioIndex = 0;
			}
			else if (ioIndex + count >= ioBuffer.Length)
			{
				Helpers.BlockCopy(ioBuffer, ioIndex, ioBuffer, 0, available);
				ioIndex = 0;
			}
			count -= available;
			int writePos = ioIndex + available;
			int canRead = ioBuffer.Length - writePos;
			if (isFixedLength && dataRemaining64 < canRead)
			{
				canRead = (int)dataRemaining64;
			}
			int bytesRead;
			while (count > 0 && canRead > 0 && (bytesRead = source.Read(ioBuffer, writePos, canRead)) > 0)
			{
				available += bytesRead;
				count -= bytesRead;
				canRead -= bytesRead;
				writePos += bytesRead;
				if (isFixedLength)
				{
					dataRemaining64 -= bytesRead;
				}
			}
			if (strict && count > 0)
			{
				throw EoF(this);
			}
		}

		public short ReadInt16()
		{
			return checked((short)ReadInt32());
		}

		public ushort ReadUInt16()
		{
			return checked((ushort)ReadUInt32());
		}

		public byte ReadByte()
		{
			return checked((byte)ReadUInt32());
		}

		public sbyte ReadSByte()
		{
			return checked((sbyte)ReadInt32());
		}

		public int ReadInt32()
		{
			switch (wireType)
			{
			case WireType.Variant:
				return (int)ReadUInt32Variant(trimNegative: true);
			case WireType.Fixed32:
				if (available < 4)
				{
					Ensure(4, strict: true);
				}
				position64 += 4L;
				available -= 4;
				return ioBuffer[ioIndex++] | (ioBuffer[ioIndex++] << 8) | (ioBuffer[ioIndex++] << 16) | (ioBuffer[ioIndex++] << 24);
			case WireType.Fixed64:
			{
				long i = ReadInt64();
				return checked((int)i);
			}
			case WireType.SignedVariant:
				return Zag(ReadUInt32Variant(trimNegative: true));
			default:
				throw CreateWireTypeException();
			}
		}

		private static int Zag(uint ziggedValue)
		{
			return (int)(0 - (ziggedValue & 1)) ^ (((int)ziggedValue >> 1) & int.MaxValue);
		}

		private static long Zag(ulong ziggedValue)
		{
			return (long)(0L - (ziggedValue & 1)) ^ (((long)ziggedValue >> 1) & long.MaxValue);
		}

		public long ReadInt64()
		{
			switch (wireType)
			{
			case WireType.Variant:
				return (long)ReadUInt64Variant();
			case WireType.Fixed32:
				return ReadInt32();
			case WireType.Fixed64:
				if (available < 8)
				{
					Ensure(8, strict: true);
				}
				position64 += 8L;
				available -= 8;
				return (long)(ioBuffer[ioIndex++] | ((ulong)ioBuffer[ioIndex++] << 8) | ((ulong)ioBuffer[ioIndex++] << 16) | ((ulong)ioBuffer[ioIndex++] << 24) | ((ulong)ioBuffer[ioIndex++] << 32) | ((ulong)ioBuffer[ioIndex++] << 40) | ((ulong)ioBuffer[ioIndex++] << 48) | ((ulong)ioBuffer[ioIndex++] << 56));
			case WireType.SignedVariant:
				return Zag(ReadUInt64Variant());
			default:
				throw CreateWireTypeException();
			}
		}

		private int TryReadUInt64VariantWithoutMoving(out ulong value)
		{
			if (available < 10)
			{
				Ensure(10, strict: false);
			}
			if (available == 0)
			{
				value = 0uL;
				return 0;
			}
			int readPos = ioIndex;
			value = ioBuffer[readPos++];
			if ((value & 0x80) == 0L)
			{
				return 1;
			}
			value &= 127uL;
			if (available == 1)
			{
				throw EoF(this);
			}
			ulong chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 7;
			if ((chunk3 & 0x80) == 0L)
			{
				return 2;
			}
			if (available == 2)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 14;
			if ((chunk3 & 0x80) == 0L)
			{
				return 3;
			}
			if (available == 3)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 21;
			if ((chunk3 & 0x80) == 0L)
			{
				return 4;
			}
			if (available == 4)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 28;
			if ((chunk3 & 0x80) == 0L)
			{
				return 5;
			}
			if (available == 5)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 35;
			if ((chunk3 & 0x80) == 0L)
			{
				return 6;
			}
			if (available == 6)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 42;
			if ((chunk3 & 0x80) == 0L)
			{
				return 7;
			}
			if (available == 7)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 49;
			if ((chunk3 & 0x80) == 0L)
			{
				return 8;
			}
			if (available == 8)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos++];
			value |= (chunk3 & 0x7F) << 56;
			if ((chunk3 & 0x80) == 0L)
			{
				return 9;
			}
			if (available == 9)
			{
				throw EoF(this);
			}
			chunk3 = ioBuffer[readPos];
			value |= chunk3 << 63;
			if (((long)chunk3 & -2L) != 0L)
			{
				throw AddErrorData(new OverflowException(), this);
			}
			return 10;
		}

		private ulong ReadUInt64Variant()
		{
			ulong value;
			int read = TryReadUInt64VariantWithoutMoving(out value);
			if (read > 0)
			{
				ioIndex += read;
				available -= read;
				position64 += read;
				return value;
			}
			throw EoF(this);
		}

		private string Intern(string value)
		{
			if (value == null)
			{
				return null;
			}
			if (value.Length == 0)
			{
				return "";
			}
			string found;
			if (stringInterner == null)
			{
				stringInterner = new Dictionary<string, string>();
				stringInterner.Add(value, value);
			}
			else if (stringInterner.TryGetValue(value, out found))
			{
				value = found;
			}
			else
			{
				stringInterner.Add(value, value);
			}
			return value;
		}

		public string ReadString()
		{
			if (wireType == WireType.String)
			{
				int bytes = (int)ReadUInt32Variant(trimNegative: false);
				if (bytes == 0)
				{
					return "";
				}
				if (available < bytes)
				{
					Ensure(bytes, strict: true);
				}
				string s = encoding.GetString(ioBuffer, ioIndex, bytes);
				if (internStrings)
				{
					s = Intern(s);
				}
				available -= bytes;
				position64 += bytes;
				ioIndex += bytes;
				return s;
			}
			throw CreateWireTypeException();
		}

		public void ThrowEnumException(Type type, int value)
		{
			string desc = (type == null) ? "<null>" : type.FullName;
			throw AddErrorData(new ProtoException("No " + desc + " enum is mapped to the wire-value " + value), this);
		}

		private Exception CreateWireTypeException()
		{
			return CreateException("Invalid wire-type; this usually means you have over-written a file without truncating or setting the length; see http://stackoverflow.com/q/2152978/23354");
		}

		private Exception CreateException(string message)
		{
			return AddErrorData(new ProtoException(message), this);
		}

		public unsafe double ReadDouble()
		{
			switch (wireType)
			{
			case WireType.Fixed32:
				return ReadSingle();
			case WireType.Fixed64:
			{
				long value = ReadInt64();
				return *(double*)(&value);
			}
			default:
				throw CreateWireTypeException();
			}
		}

		public static object ReadObject(object value, int key, ProtoReader reader)
		{
			return ReadTypedObject(value, key, reader, null);
		}

		internal static object ReadTypedObject(object value, int key, ProtoReader reader, Type type)
		{
			if (reader.model == null)
			{
				throw AddErrorData(new InvalidOperationException("Cannot deserialize sub-objects unless a model is provided"), reader);
			}
			SubItemToken token = StartSubItem(reader);
			if (key >= 0)
			{
				value = reader.model.Deserialize(key, value, reader);
			}
			else if (!(type != null) || !reader.model.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, skipOtherFields: true, asListItem: false, autoCreate: true, insideList: false, null))
			{
				TypeModel.ThrowUnexpectedType(type);
			}
			EndSubItem(token, reader);
			return value;
		}

		public static void EndSubItem(SubItemToken token, ProtoReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			long value64 = token.value64;
			WireType wireType = reader.wireType;
			if (wireType == WireType.EndGroup)
			{
				if (value64 >= 0)
				{
					throw AddErrorData(new ArgumentException("token"), reader);
				}
				if (-(int)value64 != reader.fieldNumber)
				{
					throw reader.CreateException("Wrong group was ended");
				}
				reader.wireType = WireType.None;
				reader.depth--;
			}
			else
			{
				if (value64 < reader.position64)
				{
					throw reader.CreateException($"Sub-message not read entirely; expected {value64}, was {reader.position64}");
				}
				if (reader.blockEnd64 != reader.position64 && reader.blockEnd64 != long.MaxValue)
				{
					throw reader.CreateException("Sub-message not read correctly");
				}
				reader.blockEnd64 = value64;
				reader.depth--;
			}
		}

		public static SubItemToken StartSubItem(ProtoReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			switch (reader.wireType)
			{
			case WireType.StartGroup:
				reader.wireType = WireType.None;
				reader.depth++;
				return new SubItemToken((long)(-reader.fieldNumber));
			case WireType.String:
			{
				long len = (long)reader.ReadUInt64Variant();
				if (len < 0)
				{
					throw AddErrorData(new InvalidOperationException(), reader);
				}
				long lastEnd = reader.blockEnd64;
				reader.blockEnd64 = reader.position64 + len;
				reader.depth++;
				return new SubItemToken(lastEnd);
			}
			default:
				throw reader.CreateWireTypeException();
			}
		}

		public int ReadFieldHeader()
		{
			if (blockEnd64 <= position64 || wireType == WireType.EndGroup)
			{
				return 0;
			}
			if (TryReadUInt32Variant(out uint tag) && tag != 0)
			{
				wireType = (WireType)(tag & 7);
				fieldNumber = (int)(tag >> 3);
				if (fieldNumber < 1)
				{
					throw new ProtoException("Invalid field in source data: " + fieldNumber);
				}
			}
			else
			{
				wireType = WireType.None;
				fieldNumber = 0;
			}
			if (wireType == WireType.EndGroup)
			{
				if (depth > 0)
				{
					return 0;
				}
				throw new ProtoException("Unexpected end-group in source data; this usually means the source data is corrupt");
			}
			return fieldNumber;
		}

		public bool TryReadFieldHeader(int field)
		{
			if (blockEnd64 <= position64 || wireType == WireType.EndGroup)
			{
				return false;
			}
			uint tag;
			int read = TryReadUInt32VariantWithoutMoving(trimNegative: false, out tag);
			WireType tmpWireType;
			if (read > 0 && (int)tag >> 3 == field && (tmpWireType = (WireType)(tag & 7)) != WireType.EndGroup)
			{
				wireType = tmpWireType;
				fieldNumber = field;
				position64 += read;
				ioIndex += read;
				available -= read;
				return true;
			}
			return false;
		}

		public void Hint(WireType wireType)
		{
			if (this.wireType != wireType && (wireType & (WireType)7) == this.wireType)
			{
				this.wireType = wireType;
			}
		}

		public void Assert(WireType wireType)
		{
			if (this.wireType != wireType)
			{
				if ((wireType & (WireType)7) != this.wireType)
				{
					throw CreateWireTypeException();
				}
				this.wireType = wireType;
			}
		}

		public void SkipField()
		{
			switch (wireType)
			{
			case WireType.Fixed32:
				if (available < 4)
				{
					Ensure(4, strict: true);
				}
				available -= 4;
				ioIndex += 4;
				position64 += 4L;
				break;
			case WireType.Fixed64:
				if (available < 8)
				{
					Ensure(8, strict: true);
				}
				available -= 8;
				ioIndex += 8;
				position64 += 8L;
				break;
			case WireType.String:
			{
				long len2 = (long)ReadUInt64Variant();
				if (len2 <= available)
				{
					available -= (int)len2;
					ioIndex += (int)len2;
					position64 += len2;
					break;
				}
				position64 += len2;
				len2 -= available;
				ioIndex = (available = 0);
				if (isFixedLength)
				{
					if (len2 > dataRemaining64)
					{
						throw EoF(this);
					}
					dataRemaining64 -= len2;
				}
				Seek(source, len2, ioBuffer);
				break;
			}
			case WireType.Variant:
			case WireType.SignedVariant:
				ReadUInt64Variant();
				break;
			case WireType.StartGroup:
			{
				int originalFieldNumber = fieldNumber;
				depth++;
				while (ReadFieldHeader() > 0)
				{
					SkipField();
				}
				depth--;
				if (wireType == WireType.EndGroup && fieldNumber == originalFieldNumber)
				{
					wireType = WireType.None;
					break;
				}
				throw CreateWireTypeException();
			}
			default:
				throw CreateWireTypeException();
			}
		}

		public ulong ReadUInt64()
		{
			switch (wireType)
			{
			case WireType.Variant:
				return ReadUInt64Variant();
			case WireType.Fixed32:
				return ReadUInt32();
			case WireType.Fixed64:
				if (available < 8)
				{
					Ensure(8, strict: true);
				}
				position64 += 8L;
				available -= 8;
				return ioBuffer[ioIndex++] | ((ulong)ioBuffer[ioIndex++] << 8) | ((ulong)ioBuffer[ioIndex++] << 16) | ((ulong)ioBuffer[ioIndex++] << 24) | ((ulong)ioBuffer[ioIndex++] << 32) | ((ulong)ioBuffer[ioIndex++] << 40) | ((ulong)ioBuffer[ioIndex++] << 48) | ((ulong)ioBuffer[ioIndex++] << 56);
			default:
				throw CreateWireTypeException();
			}
		}

		public unsafe float ReadSingle()
		{
			switch (wireType)
			{
			case WireType.Fixed32:
			{
				int value = ReadInt32();
				return *(float*)(&value);
			}
			case WireType.Fixed64:
			{
				double value2 = ReadDouble();
				float f = (float)value2;
				if (Helpers.IsInfinity(f) && !Helpers.IsInfinity(value2))
				{
					throw AddErrorData(new OverflowException(), this);
				}
				return f;
			}
			default:
				throw CreateWireTypeException();
			}
		}

		public bool ReadBoolean()
		{
			switch (ReadUInt32())
			{
			case 0u:
				return false;
			case 1u:
				return true;
			default:
				throw CreateException("Unexpected boolean value");
			}
		}

		public static byte[] AppendBytes(byte[] value, ProtoReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			switch (reader.wireType)
			{
			case WireType.String:
			{
				int len = (int)reader.ReadUInt32Variant(trimNegative: false);
				reader.wireType = WireType.None;
				if (len == 0)
				{
					if (value != null)
					{
						return value;
					}
					return EmptyBlob;
				}
				int offset;
				if (value == null || value.Length == 0)
				{
					offset = 0;
					value = new byte[len];
				}
				else
				{
					offset = value.Length;
					byte[] tmp = new byte[value.Length + len];
					Helpers.BlockCopy(value, 0, tmp, 0, value.Length);
					value = tmp;
				}
				reader.position64 += len;
				while (len > reader.available)
				{
					if (reader.available > 0)
					{
						Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, offset, reader.available);
						len -= reader.available;
						offset += reader.available;
						reader.ioIndex = (reader.available = 0);
					}
					int count = (len > reader.ioBuffer.Length) ? reader.ioBuffer.Length : len;
					if (count > 0)
					{
						reader.Ensure(count, strict: true);
					}
				}
				if (len > 0)
				{
					Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, offset, len);
					reader.ioIndex += len;
					reader.available -= len;
				}
				return value;
			}
			case WireType.Variant:
				return new byte[0];
			default:
				throw reader.CreateWireTypeException();
			}
		}

		private static int ReadByteOrThrow(Stream source)
		{
			int val = source.ReadByte();
			if (val < 0)
			{
				throw EoF(null);
			}
			return val;
		}

		public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber)
		{
			int bytesRead;
			return ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead);
		}

		public static int DirectReadLittleEndianInt32(Stream source)
		{
			return ReadByteOrThrow(source) | (ReadByteOrThrow(source) << 8) | (ReadByteOrThrow(source) << 16) | (ReadByteOrThrow(source) << 24);
		}

		public static int DirectReadBigEndianInt32(Stream source)
		{
			return (ReadByteOrThrow(source) << 24) | (ReadByteOrThrow(source) << 16) | (ReadByteOrThrow(source) << 8) | ReadByteOrThrow(source);
		}

		public static int DirectReadVarintInt32(Stream source)
		{
			ulong val;
			int bytes = TryReadUInt64Variant(source, out val);
			if (bytes <= 0)
			{
				throw EoF(null);
			}
			return checked((int)val);
		}

		public static void DirectReadBytes(Stream source, byte[] buffer, int offset, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			int read;
			while (count > 0 && (read = source.Read(buffer, offset, count)) > 0)
			{
				count -= read;
				offset += read;
			}
			if (count > 0)
			{
				throw EoF(null);
			}
		}

		public static byte[] DirectReadBytes(Stream source, int count)
		{
			byte[] buffer = new byte[count];
			DirectReadBytes(source, buffer, 0, count);
			return buffer;
		}

		public static string DirectReadString(Stream source, int length)
		{
			byte[] buffer = new byte[length];
			DirectReadBytes(source, buffer, 0, length);
			return Encoding.UTF8.GetString(buffer, 0, length);
		}

		public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber, out int bytesRead)
		{
			if (style == PrefixStyle.None)
			{
				bytesRead = (fieldNumber = 0);
				return int.MaxValue;
			}
			long len64 = ReadLongLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead);
			return checked((int)len64);
		}

		public static long ReadLongLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber, out int bytesRead)
		{
			fieldNumber = 0;
			switch (style)
			{
			case PrefixStyle.None:
				bytesRead = 0;
				return long.MaxValue;
			case PrefixStyle.Base128:
			{
				bytesRead = 0;
				ulong val;
				int tmpBytesRead3;
				if (expectHeader)
				{
					tmpBytesRead3 = TryReadUInt64Variant(source, out val);
					bytesRead += tmpBytesRead3;
					if (tmpBytesRead3 > 0)
					{
						if ((val & 7) != 2)
						{
							throw new InvalidOperationException();
						}
						fieldNumber = (int)(val >> 3);
						tmpBytesRead3 = TryReadUInt64Variant(source, out val);
						bytesRead += tmpBytesRead3;
						if (bytesRead == 0)
						{
							throw EoF(null);
						}
						return (long)val;
					}
					bytesRead = 0;
					return -1L;
				}
				tmpBytesRead3 = TryReadUInt64Variant(source, out val);
				bytesRead += tmpBytesRead3;
				if (bytesRead >= 0)
				{
					return (long)val;
				}
				return -1L;
			}
			case PrefixStyle.Fixed32:
			{
				int b = source.ReadByte();
				if (b < 0)
				{
					bytesRead = 0;
					return -1L;
				}
				bytesRead = 4;
				return b | (ReadByteOrThrow(source) << 8) | (ReadByteOrThrow(source) << 16) | (ReadByteOrThrow(source) << 24);
			}
			case PrefixStyle.Fixed32BigEndian:
			{
				int b2 = source.ReadByte();
				if (b2 < 0)
				{
					bytesRead = 0;
					return -1L;
				}
				bytesRead = 4;
				return (b2 << 24) | (ReadByteOrThrow(source) << 16) | (ReadByteOrThrow(source) << 8) | ReadByteOrThrow(source);
			}
			default:
				throw new ArgumentOutOfRangeException("style");
			}
		}

		private static int TryReadUInt64Variant(Stream source, out ulong value)
		{
			value = 0uL;
			int b3 = source.ReadByte();
			if (b3 < 0)
			{
				return 0;
			}
			value = (uint)b3;
			if ((value & 0x80) == 0L)
			{
				return 1;
			}
			value &= 127uL;
			int bytesRead3 = 1;
			int shift = 7;
			while (bytesRead3 < 9)
			{
				b3 = source.ReadByte();
				if (b3 < 0)
				{
					throw EoF(null);
				}
				value |= (ulong)(((long)b3 & 127L) << shift);
				shift += 7;
				if ((b3 & 0x80) == 0)
				{
					return ++bytesRead3;
				}
			}
			b3 = source.ReadByte();
			if (b3 < 0)
			{
				throw EoF(null);
			}
			if ((b3 & 1) == 0)
			{
				value |= (ulong)(((long)b3 & 127L) << shift);
				return ++bytesRead3;
			}
			throw new OverflowException();
		}

		internal static void Seek(Stream source, long count, byte[] buffer)
		{
			if (source.CanSeek)
			{
				source.Seek(count, SeekOrigin.Current);
				count = 0L;
			}
			else if (buffer != null)
			{
				int bytesRead4;
				while (count > buffer.Length && (bytesRead4 = source.Read(buffer, 0, buffer.Length)) > 0)
				{
					count -= bytesRead4;
				}
				while (count > 0 && (bytesRead4 = source.Read(buffer, 0, (int)count)) > 0)
				{
					count -= bytesRead4;
				}
			}
			else
			{
				buffer = BufferPool.GetBuffer();
				try
				{
					int bytesRead2;
					while (count > buffer.Length && (bytesRead2 = source.Read(buffer, 0, buffer.Length)) > 0)
					{
						count -= bytesRead2;
					}
					while (count > 0 && (bytesRead2 = source.Read(buffer, 0, (int)count)) > 0)
					{
						count -= bytesRead2;
					}
				}
				finally
				{
					BufferPool.ReleaseBufferToPool(ref buffer);
				}
			}
			if (count > 0)
			{
				throw EoF(null);
			}
		}

		internal static Exception AddErrorData(Exception exception, ProtoReader source)
		{
			if (exception != null && source != null && !exception.Data.Contains("protoSource"))
			{
				exception.Data.Add("protoSource", $"tag={source.fieldNumber}; wire-type={source.wireType}; offset={source.position64}; depth={source.depth}");
			}
			return exception;
		}

		private static Exception EoF(ProtoReader source)
		{
			return AddErrorData(new EndOfStreamException(), source);
		}

		public void AppendExtensionData(IExtensible instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			IExtension extn = instance.GetExtensionObject(createIfMissing: true);
			bool commit = false;
			Stream dest = extn.BeginAppend();
			try
			{
				using (ProtoWriter writer = new ProtoWriter(dest, model, null))
				{
					AppendExtensionField(writer);
					writer.Close();
				}
				commit = true;
			}
			finally
			{
				extn.EndAppend(dest, commit);
			}
		}

		private void AppendExtensionField(ProtoWriter writer)
		{
			ProtoWriter.WriteFieldHeader(fieldNumber, wireType, writer);
			switch (wireType)
			{
			case WireType.Fixed32:
				ProtoWriter.WriteInt32(ReadInt32(), writer);
				break;
			case WireType.Variant:
			case WireType.Fixed64:
			case WireType.SignedVariant:
				ProtoWriter.WriteInt64(ReadInt64(), writer);
				break;
			case WireType.String:
				ProtoWriter.WriteBytes(AppendBytes(null, this), writer);
				break;
			case WireType.StartGroup:
			{
				SubItemToken readerToken = StartSubItem(this);
				SubItemToken writerToken = ProtoWriter.StartSubItem(null, writer);
				while (ReadFieldHeader() > 0)
				{
					AppendExtensionField(writer);
				}
				EndSubItem(readerToken, this);
				ProtoWriter.EndSubItem(writerToken, writer);
				break;
			}
			default:
				throw CreateWireTypeException();
			}
		}

		public static bool HasSubValue(WireType wireType, ProtoReader source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source.blockEnd64 <= source.position64 || wireType == WireType.EndGroup)
			{
				return false;
			}
			source.wireType = wireType;
			return true;
		}

		internal int GetTypeKey(ref Type type)
		{
			return model.GetKey(ref type);
		}

		internal Type DeserializeType(string value)
		{
			return TypeModel.DeserializeType(model, value);
		}

		internal void SetRootObject(object value)
		{
			netCache.SetKeyedObject(0, value);
			trapCount--;
		}

		public static void NoteObject(object value, ProtoReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (reader.trapCount != 0)
			{
				reader.netCache.RegisterTrappedObject(value);
				reader.trapCount--;
			}
		}

		public Type ReadType()
		{
			return TypeModel.DeserializeType(model, ReadString());
		}

		internal void TrapNextObject(int newObjectKey)
		{
			trapCount++;
			netCache.SetKeyedObject(newObjectKey, null);
		}

		internal void CheckFullyConsumed()
		{
			if (isFixedLength)
			{
				if (dataRemaining64 != 0L)
				{
					throw new ProtoException("Incorrect number of bytes consumed");
				}
			}
			else if (available != 0)
			{
				throw new ProtoException("Unconsumed data left in the buffer; this suggests corrupt input");
			}
		}

		public static object Merge(ProtoReader parent, object from, object to)
		{
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			TypeModel model = parent.Model;
			SerializationContext ctx = parent.Context;
			if (model == null)
			{
				throw new InvalidOperationException("Types cannot be merged unless a type-model has been specified");
			}
			using (MemoryStream ms = new MemoryStream())
			{
				model.Serialize(ms, from, ctx);
				ms.Position = 0L;
				return model.Deserialize(ms, to, null);
			}
		}

		internal static ProtoReader Create(Stream source, TypeModel model, SerializationContext context, int len)
		{
			return Create(source, model, context, (long)len);
		}

		internal static ProtoReader Create(Stream source, TypeModel model, SerializationContext context, long len)
		{
			ProtoReader reader = GetRecycled();
			if (reader == null)
			{
				return new ProtoReader(source, model, context, len);
			}
			Init(reader, source, model, context, len);
			return reader;
		}

		private static ProtoReader GetRecycled()
		{
			ProtoReader tmp = lastReader;
			lastReader = null;
			return tmp;
		}

		internal static void Recycle(ProtoReader reader)
		{
			if (reader != null)
			{
				reader.Dispose();
				lastReader = reader;
			}
		}
	}
}
