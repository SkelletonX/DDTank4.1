using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.Bson
{
	internal class BsonBinaryWriter
	{
		private static readonly Encoding Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

		private readonly BinaryWriter _writer;

		private byte[] _largeByteBuffer;

		public DateTimeKind DateTimeKindHandling
		{
			get;
			set;
		}

		public BsonBinaryWriter(BinaryWriter writer)
		{
			DateTimeKindHandling = DateTimeKind.Utc;
			_writer = writer;
		}

		public void Flush()
		{
			_writer.Flush();
		}

		public void Close()
		{
			_writer.Close();
		}

		public void WriteToken(BsonToken t)
		{
			CalculateSize(t);
			WriteTokenInternal(t);
		}

		private void WriteTokenInternal(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Undefined:
			case BsonType.Null:
				break;
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				_writer.Write(bsonObject.CalculatedSize);
				foreach (BsonProperty item in bsonObject)
				{
					_writer.Write((sbyte)item.Value.Type);
					WriteString((string)item.Name.Value, item.Name.ByteCount, null);
					WriteTokenInternal(item.Value);
				}
				_writer.Write((byte)0);
				break;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				_writer.Write(bsonArray.CalculatedSize);
				ulong num2 = 0uL;
				foreach (BsonToken item2 in bsonArray)
				{
					_writer.Write((sbyte)item2.Type);
					WriteString(num2.ToString(CultureInfo.InvariantCulture), MathUtils.IntLength(num2), null);
					WriteTokenInternal(item2);
					num2++;
				}
				_writer.Write((byte)0);
				break;
			}
			case BsonType.Integer:
			{
				BsonValue bsonValue3 = (BsonValue)t;
				_writer.Write(Convert.ToInt32(bsonValue3.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.Long:
			{
				BsonValue bsonValue4 = (BsonValue)t;
				_writer.Write(Convert.ToInt64(bsonValue4.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.Number:
			{
				BsonValue bsonValue5 = (BsonValue)t;
				_writer.Write(Convert.ToDouble(bsonValue5.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				WriteString((string)bsonString.Value, bsonString.ByteCount, bsonString.CalculatedSize - 4);
				break;
			}
			case BsonType.Boolean:
			{
				BsonValue bsonValue6 = (BsonValue)t;
				_writer.Write((bool)bsonValue6.Value);
				break;
			}
			case BsonType.Date:
			{
				BsonValue bsonValue2 = (BsonValue)t;
				long num = 0L;
				if (bsonValue2.Value is DateTime)
				{
					DateTime dateTime = (DateTime)bsonValue2.Value;
					if (DateTimeKindHandling == DateTimeKind.Utc)
					{
						dateTime = dateTime.ToUniversalTime();
					}
					else if (DateTimeKindHandling == DateTimeKind.Local)
					{
						dateTime = dateTime.ToLocalTime();
					}
					num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime, convertToUtc: false);
				}
				else
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)bsonValue2.Value;
					num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.UtcDateTime, dateTimeOffset.Offset);
				}
				_writer.Write(num);
				break;
			}
			case BsonType.Binary:
			{
				BsonBinary bsonBinary = (BsonBinary)t;
				byte[] array = (byte[])bsonBinary.Value;
				_writer.Write(array.Length);
				_writer.Write((byte)bsonBinary.BinaryType);
				_writer.Write(array);
				break;
			}
			case BsonType.Oid:
			{
				BsonValue bsonValue = (BsonValue)t;
				byte[] buffer = (byte[])bsonValue.Value;
				_writer.Write(buffer);
				break;
			}
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				WriteString((string)bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, null);
				WriteString((string)bsonRegex.Options.Value, bsonRegex.Options.ByteCount, null);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, t.Type));
			}
		}

		private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
		{
			if (calculatedlengthPrefix.HasValue)
			{
				_writer.Write(calculatedlengthPrefix.Value);
			}
			WriteUtf8Bytes(s, byteCount);
			_writer.Write((byte)0);
		}

		public void WriteUtf8Bytes(string s, int byteCount)
		{
			if (s != null)
			{
				if (_largeByteBuffer == null)
				{
					_largeByteBuffer = new byte[256];
				}
				if (byteCount <= 256)
				{
					Encoding.GetBytes(s, 0, s.Length, _largeByteBuffer, 0);
					_writer.Write(_largeByteBuffer, 0, byteCount);
				}
				else
				{
					byte[] bytes = Encoding.GetBytes(s);
					_writer.Write(bytes);
				}
			}
		}

		private int CalculateSize(int stringByteCount)
		{
			return stringByteCount + 1;
		}

		private int CalculateSizeWithLength(int stringByteCount, bool includeSize)
		{
			int num = (!includeSize) ? 1 : 5;
			return num + stringByteCount;
		}

		private int CalculateSize(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				int num6 = 4;
				foreach (BsonProperty item in bsonObject)
				{
					int num7 = 1;
					num7 += CalculateSize(item.Name);
					num7 += CalculateSize(item.Value);
					num6 += num7;
				}
				return bsonObject.CalculatedSize = num6 + 1;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				int num3 = 4;
				ulong num4 = 0uL;
				foreach (BsonToken item2 in bsonArray)
				{
					num3++;
					num3 += CalculateSize(MathUtils.IntLength(num4));
					num3 += CalculateSize(item2);
					num4++;
				}
				num3 = (bsonArray.CalculatedSize = num3 + 1);
				return bsonArray.CalculatedSize;
			}
			case BsonType.Integer:
				return 4;
			case BsonType.Long:
				return 8;
			case BsonType.Number:
				return 8;
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				string text = (string)bsonString.Value;
				bsonString.ByteCount = ((text != null) ? Encoding.GetByteCount(text) : 0);
				bsonString.CalculatedSize = CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
				return bsonString.CalculatedSize;
			}
			case BsonType.Boolean:
				return 1;
			case BsonType.Undefined:
			case BsonType.Null:
				return 0;
			case BsonType.Date:
				return 8;
			case BsonType.Binary:
			{
				BsonBinary bsonBinary = (BsonBinary)t;
				byte[] array = (byte[])bsonBinary.Value;
				bsonBinary.CalculatedSize = 5 + array.Length;
				return bsonBinary.CalculatedSize;
			}
			case BsonType.Oid:
				return 12;
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				int num = 0;
				num += CalculateSize(bsonRegex.Pattern);
				num = (bsonRegex.CalculatedSize = num + CalculateSize(bsonRegex.Options));
				return bsonRegex.CalculatedSize;
			}
			default:
				throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, t.Type));
			}
		}
	}
}
