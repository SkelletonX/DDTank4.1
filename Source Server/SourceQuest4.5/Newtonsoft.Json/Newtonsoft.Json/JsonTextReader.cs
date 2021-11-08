using Newtonsoft.Json.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json
{
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		private const char UnicodeReplacementChar = '\ufffd';

		private const int MaximumJavascriptIntegerCharacterLength = 380;

		private readonly TextReader _reader;

		private char[] _chars;

		private int _charsUsed;

		private int _charPos;

		private int _lineStartPos;

		private int _lineNumber;

		private bool _isEndOfFile;

		private StringBuffer _buffer;

		private StringReference _stringReference;

		internal PropertyNameTable NameTable;

		public int LineNumber
		{
			get
			{
				if (base.CurrentState == State.Start && LinePosition == 0)
				{
					return 0;
				}
				return _lineNumber;
			}
		}

		public int LinePosition => _charPos - _lineStartPos;

		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			_reader = reader;
			_lineNumber = 1;
			_chars = new char[1025];
		}

		private StringBuffer GetBuffer()
		{
			if (_buffer == null)
			{
				_buffer = new StringBuffer(1025);
			}
			else
			{
				_buffer.Position = 0;
			}
			return _buffer;
		}

		private void OnNewLine(int pos)
		{
			_lineNumber++;
			_lineStartPos = pos - 1;
		}

		private void ParseString(char quote)
		{
			_charPos++;
			ShiftBufferIfNeeded();
			ReadStringIntoBuffer(quote);
			SetPostValueState(updateIndex: true);
			if (_readType == ReadType.ReadAsBytes)
			{
				byte[] value = (_stringReference.Length != 0) ? Convert.FromBase64CharArray(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length) : new byte[0];
				SetToken(JsonToken.Bytes, value, updateIndex: false);
				return;
			}
			if (_readType == ReadType.ReadAsString)
			{
				string value2 = _stringReference.ToString();
				SetToken(JsonToken.String, value2, updateIndex: false);
				_quoteChar = quote;
				return;
			}
			string text = _stringReference.ToString();
			if (_dateParseHandling != 0)
			{
				DateParseHandling dateParseHandling = (_readType == ReadType.ReadAsDateTime) ? DateParseHandling.DateTime : ((_readType != ReadType.ReadAsDateTimeOffset) ? _dateParseHandling : DateParseHandling.DateTimeOffset);
				if (DateTimeUtils.TryParseDateTime(text, dateParseHandling, base.DateTimeZoneHandling, base.DateFormatString, base.Culture, out object dt))
				{
					SetToken(JsonToken.Date, dt, updateIndex: false);
					return;
				}
			}
			SetToken(JsonToken.String, text, updateIndex: false);
			_quoteChar = quote;
		}

		private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
		}

		private void ShiftBufferIfNeeded()
		{
			int num = _chars.Length;
			if ((double)(num - _charPos) <= (double)num * 0.1)
			{
				int num2 = _charsUsed - _charPos;
				if (num2 > 0)
				{
					BlockCopyChars(_chars, _charPos, _chars, 0, num2);
				}
				_lineStartPos -= _charPos;
				_charPos = 0;
				_charsUsed = num2;
				_chars[_charsUsed] = '\0';
			}
		}

		private int ReadData(bool append)
		{
			return ReadData(append, 0);
		}

		private int ReadData(bool append, int charsRequired)
		{
			if (_isEndOfFile)
			{
				return 0;
			}
			if (_charsUsed + charsRequired >= _chars.Length - 1)
			{
				if (append)
				{
					int num = Math.Max(_chars.Length * 2, _charsUsed + charsRequired + 1);
					char[] array = new char[num];
					BlockCopyChars(_chars, 0, array, 0, _chars.Length);
					_chars = array;
				}
				else
				{
					int num2 = _charsUsed - _charPos;
					if (num2 + charsRequired + 1 >= _chars.Length)
					{
						char[] array2 = new char[num2 + charsRequired + 1];
						if (num2 > 0)
						{
							BlockCopyChars(_chars, _charPos, array2, 0, num2);
						}
						_chars = array2;
					}
					else if (num2 > 0)
					{
						BlockCopyChars(_chars, _charPos, _chars, 0, num2);
					}
					_lineStartPos -= _charPos;
					_charPos = 0;
					_charsUsed = num2;
				}
			}
			int count = _chars.Length - _charsUsed - 1;
			int num3 = _reader.Read(_chars, _charsUsed, count);
			_charsUsed += num3;
			if (num3 == 0)
			{
				_isEndOfFile = true;
			}
			_chars[_charsUsed] = '\0';
			return num3;
		}

		private bool EnsureChars(int relativePosition, bool append)
		{
			if (_charPos + relativePosition >= _charsUsed)
			{
				return ReadChars(relativePosition, append);
			}
			return true;
		}

		private bool ReadChars(int relativePosition, bool append)
		{
			if (_isEndOfFile)
			{
				return false;
			}
			int num = _charPos + relativePosition - _charsUsed + 1;
			int num2 = 0;
			do
			{
				int num3 = ReadData(append, num - num2);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
			}
			while (num2 < num);
			if (num2 < num)
			{
				return false;
			}
			return true;
		}

		[DebuggerStepThrough]
		public override bool Read()
		{
			_readType = ReadType.Read;
			if (!ReadInternal())
			{
				SetToken(JsonToken.None);
				return false;
			}
			return true;
		}

		public override byte[] ReadAsBytes()
		{
			return ReadAsBytesInternal();
		}

		public override decimal? ReadAsDecimal()
		{
			return ReadAsDecimalInternal();
		}

		public override int? ReadAsInt32()
		{
			return ReadAsInt32Internal();
		}

		public override string ReadAsString()
		{
			return ReadAsStringInternal();
		}

		public override DateTime? ReadAsDateTime()
		{
			return ReadAsDateTimeInternal();
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return ReadAsDateTimeOffsetInternal();
		}

		internal override bool ReadInternal()
		{
			do
			{
				switch (_currentState)
				{
				case State.Start:
				case State.Property:
				case State.ArrayStart:
				case State.Array:
				case State.ConstructorStart:
				case State.Constructor:
					return ParseValue();
				case State.ObjectStart:
				case State.Object:
					return ParseObject();
				case State.PostValue:
					break;
				case State.Finished:
					if (EnsureChars(0, append: false))
					{
						EatWhitespace(oneOrMore: false);
						if (_isEndOfFile)
						{
							return false;
						}
						if (_chars[_charPos] == '/')
						{
							ParseComment();
							return true;
						}
						throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
					}
					return false;
				default:
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
			while (!ParsePostValue());
			return true;
		}

		private void ReadStringIntoBuffer(char quote)
		{
			int num = _charPos;
			int charPos = _charPos;
			int num2 = _charPos;
			StringBuffer stringBuffer = null;
			while (true)
			{
				switch (_chars[num++])
				{
				case '\0':
					if (_charsUsed == num - 1)
					{
						num--;
						if (ReadData(append: true) == 0)
						{
							_charPos = num;
							throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
						}
					}
					break;
				case '\\':
				{
					_charPos = num;
					if (!EnsureChars(0, append: true))
					{
						_charPos = num;
						throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
					}
					int writeToPosition = num - 1;
					char c = _chars[num];
					char c2;
					switch (c)
					{
					case 'b':
						num++;
						c2 = '\b';
						break;
					case 't':
						num++;
						c2 = '\t';
						break;
					case 'n':
						num++;
						c2 = '\n';
						break;
					case 'f':
						num++;
						c2 = '\f';
						break;
					case 'r':
						num++;
						c2 = '\r';
						break;
					case '\\':
						num++;
						c2 = '\\';
						break;
					case '"':
					case '\'':
					case '/':
						c2 = c;
						num++;
						break;
					case 'u':
						num = (_charPos = num + 1);
						c2 = ParseUnicode();
						if (StringUtils.IsLowSurrogate(c2))
						{
							c2 = '\ufffd';
						}
						else if (StringUtils.IsHighSurrogate(c2))
						{
							bool flag;
							do
							{
								flag = false;
								if (EnsureChars(2, append: true) && _chars[_charPos] == '\\' && _chars[_charPos + 1] == 'u')
								{
									char writeChar = c2;
									_charPos += 2;
									c2 = ParseUnicode();
									if (!StringUtils.IsLowSurrogate(c2))
									{
										if (StringUtils.IsHighSurrogate(c2))
										{
											writeChar = '\ufffd';
											flag = true;
										}
										else
										{
											writeChar = '\ufffd';
										}
									}
									if (stringBuffer == null)
									{
										stringBuffer = GetBuffer();
									}
									WriteCharToBuffer(stringBuffer, writeChar, num2, writeToPosition);
									num2 = _charPos;
								}
								else
								{
									c2 = '\ufffd';
								}
							}
							while (flag);
						}
						num = _charPos;
						break;
					default:
						num = (_charPos = num + 1);
						throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, "\\" + c));
					}
					if (stringBuffer == null)
					{
						stringBuffer = GetBuffer();
					}
					WriteCharToBuffer(stringBuffer, c2, num2, writeToPosition);
					num2 = num;
					break;
				}
				case '\r':
					_charPos = num - 1;
					ProcessCarriageReturn(append: true);
					num = _charPos;
					break;
				case '\n':
					_charPos = num - 1;
					ProcessLineFeed();
					num = _charPos;
					break;
				case '"':
				case '\'':
					if (_chars[num - 1] != quote)
					{
						break;
					}
					num--;
					if (charPos == num2)
					{
						_stringReference = new StringReference(_chars, charPos, num - charPos);
					}
					else
					{
						if (stringBuffer == null)
						{
							stringBuffer = GetBuffer();
						}
						if (num > num2)
						{
							stringBuffer.Append(_chars, num2, num - num2);
						}
						_stringReference = new StringReference(stringBuffer.GetInternalBuffer(), 0, stringBuffer.Position);
					}
					num = (_charPos = num + 1);
					return;
				}
			}
		}

		private void WriteCharToBuffer(StringBuffer buffer, char writeChar, int lastWritePosition, int writeToPosition)
		{
			if (writeToPosition > lastWritePosition)
			{
				buffer.Append(_chars, lastWritePosition, writeToPosition - lastWritePosition);
			}
			buffer.Append(writeChar);
		}

		private char ParseUnicode()
		{
			if (EnsureChars(4, append: true))
			{
				string s = new string(_chars, _charPos, 4);
				char c = Convert.ToChar(int.Parse(s, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
				char result = c;
				_charPos += 4;
				return result;
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing unicode character.");
		}

		private void ReadNumberIntoBuffer()
		{
			int num = _charPos;
			while (true)
			{
				switch (_chars[num])
				{
				case '\0':
					_charPos = num;
					if (_charsUsed != num || ReadData(append: true) == 0)
					{
						return;
					}
					continue;
				case '+':
				case '-':
				case '.':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'X':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'x':
					num++;
					continue;
				}
				_charPos = num;
				char c = _chars[_charPos];
				if (char.IsWhiteSpace(c) || c == ',' || c == '}' || c == ']' || c == ')' || c == '/')
				{
					return;
				}
				throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
			}
		}

		private void ClearRecentString()
		{
			if (_buffer != null)
			{
				_buffer.Position = 0;
			}
			_stringReference = default(StringReference);
		}

		private bool ParsePostValue()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							_currentState = State.Finished;
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '}':
					_charPos++;
					SetToken(JsonToken.EndObject);
					return true;
				case ']':
					_charPos++;
					SetToken(JsonToken.EndArray);
					return true;
				case ')':
					_charPos++;
					SetToken(JsonToken.EndConstructor);
					return true;
				case '/':
					ParseComment();
					return true;
				case ',':
					_charPos++;
					SetStateBasedOnCurrent();
					return false;
				case '\t':
				case ' ':
					_charPos++;
					break;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
			}
		}

		private bool ParseObject()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '}':
					SetToken(JsonToken.EndObject);
					_charPos++;
					return true;
				case '/':
					ParseComment();
					return true;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				case '\t':
				case ' ':
					_charPos++;
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					return ParseProperty();
				}
			}
		}

		private bool ParseProperty()
		{
			char c = _chars[_charPos];
			char c2;
			if (c == '"' || c == '\'')
			{
				_charPos++;
				c2 = c;
				ShiftBufferIfNeeded();
				ReadStringIntoBuffer(c2);
			}
			else
			{
				if (!ValidIdentifierChar(c))
				{
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				c2 = '\0';
				ShiftBufferIfNeeded();
				ParseUnquotedProperty();
			}
			string text;
			if (NameTable != null)
			{
				text = NameTable.Get(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length);
				if (text == null)
				{
					text = _stringReference.ToString();
				}
			}
			else
			{
				text = _stringReference.ToString();
			}
			EatWhitespace(oneOrMore: false);
			if (_chars[_charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
			}
			_charPos++;
			SetToken(JsonToken.PropertyName, text);
			_quoteChar = c2;
			ClearRecentString();
			return true;
		}

		private bool ValidIdentifierChar(char value)
		{
			if (!char.IsLetterOrDigit(value) && value != '_')
			{
				return value == '$';
			}
			return true;
		}

		private void ParseUnquotedProperty()
		{
			int charPos = _charPos;
			char c;
			while (true)
			{
				if (_chars[_charPos] == '\0')
				{
					if (_charsUsed != _charPos)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						return;
					}
					if (ReadData(append: true) == 0)
					{
						throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
					}
				}
				else
				{
					c = _chars[_charPos];
					if (!ValidIdentifierChar(c))
					{
						break;
					}
					_charPos++;
				}
			}
			if (char.IsWhiteSpace(c) || c == ':')
			{
				_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
				return;
			}
			throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		private bool ParseValue()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '"':
				case '\'':
					ParseString(c);
					return true;
				case 't':
					ParseTrue();
					return true;
				case 'f':
					ParseFalse();
					return true;
				case 'n':
					if (EnsureChars(1, append: true))
					{
						switch (_chars[_charPos + 1])
						{
						case 'u':
							ParseNull();
							break;
						case 'e':
							ParseConstructor();
							break;
						default:
							throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
						}
						return true;
					}
					throw JsonReaderException.Create(this, "Unexpected end.");
				case 'N':
					ParseNumberNaN();
					return true;
				case 'I':
					ParseNumberPositiveInfinity();
					return true;
				case '-':
					if (EnsureChars(1, append: true) && _chars[_charPos + 1] == 'I')
					{
						ParseNumberNegativeInfinity();
					}
					else
					{
						ParseNumber();
					}
					return true;
				case '/':
					ParseComment();
					return true;
				case 'u':
					ParseUndefined();
					return true;
				case '{':
					_charPos++;
					SetToken(JsonToken.StartObject);
					return true;
				case '[':
					_charPos++;
					SetToken(JsonToken.StartArray);
					return true;
				case ']':
					_charPos++;
					SetToken(JsonToken.EndArray);
					return true;
				case ',':
					SetToken(JsonToken.Undefined);
					return true;
				case ')':
					_charPos++;
					SetToken(JsonToken.EndConstructor);
					return true;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				case '\t':
				case ' ':
					_charPos++;
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					if (char.IsNumber(c) || c == '-' || c == '.')
					{
						ParseNumber();
						return true;
					}
					throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
			}
		}

		private void ProcessLineFeed()
		{
			_charPos++;
			OnNewLine(_charPos);
		}

		private void ProcessCarriageReturn(bool append)
		{
			_charPos++;
			if (EnsureChars(1, append) && _chars[_charPos] == '\n')
			{
				_charPos++;
			}
			OnNewLine(_charPos);
		}

		private bool EatWhitespace(bool oneOrMore)
		{
			bool flag = false;
			bool result = false;
			while (!flag)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							flag = true;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				default:
					if (c == ' ' || char.IsWhiteSpace(c))
					{
						result = true;
						_charPos++;
					}
					else
					{
						flag = true;
					}
					break;
				}
			}
			if (oneOrMore)
			{
				return result;
			}
			return true;
		}

		private void ParseConstructor()
		{
			if (MatchValueWithTrailingSeparator("new"))
			{
				EatWhitespace(oneOrMore: false);
				int charPos = _charPos;
				int charPos2;
				while (true)
				{
					char c = _chars[_charPos];
					if (c == '\0')
					{
						if (_charsUsed == _charPos)
						{
							if (ReadData(append: true) == 0)
							{
								throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
							}
							continue;
						}
						charPos2 = _charPos;
						_charPos++;
						break;
					}
					if (char.IsLetterOrDigit(c))
					{
						_charPos++;
						continue;
					}
					switch (c)
					{
					case '\r':
						charPos2 = _charPos;
						ProcessCarriageReturn(append: true);
						break;
					case '\n':
						charPos2 = _charPos;
						ProcessLineFeed();
						break;
					default:
						if (char.IsWhiteSpace(c))
						{
							charPos2 = _charPos;
							_charPos++;
							break;
						}
						if (c == '(')
						{
							charPos2 = _charPos;
							break;
						}
						throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
					}
					break;
				}
				_stringReference = new StringReference(_chars, charPos, charPos2 - charPos);
				string value = _stringReference.ToString();
				EatWhitespace(oneOrMore: false);
				if (_chars[_charPos] != '(')
				{
					throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				_charPos++;
				ClearRecentString();
				SetToken(JsonToken.StartConstructor, value);
				return;
			}
			throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
		}

		private void ParseNumber()
		{
			ShiftBufferIfNeeded();
			char c = _chars[_charPos];
			int charPos = _charPos;
			ReadNumberIntoBuffer();
			SetPostValueState(updateIndex: true);
			_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
			bool flag = char.IsDigit(c) && _stringReference.Length == 1;
			bool flag2 = c == '0' && _stringReference.Length > 1 && _stringReference.Chars[_stringReference.StartIndex + 1] != '.' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'e' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'E';
			object value;
			JsonToken newToken;
			if (_readType == ReadType.ReadAsInt32)
			{
				if (flag)
				{
					value = c - 48;
				}
				else if (flag2)
				{
					string text = _stringReference.ToString();
					try
					{
						int num = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(text, 16) : Convert.ToInt32(text, 8);
						value = num;
					}
					catch (Exception ex)
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, text), ex);
					}
				}
				else
				{
					int value2;
					switch (ConvertUtils.Int32TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out value2))
					{
					case ParseResult.Success:
						break;
					case ParseResult.Overflow:
						throw JsonReaderException.Create(this, "JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					default:
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = value2;
				}
				newToken = JsonToken.Integer;
			}
			else if (_readType == ReadType.ReadAsDecimal)
			{
				if (flag)
				{
					value = (decimal)c - 48m;
				}
				else if (flag2)
				{
					string text2 = _stringReference.ToString();
					try
					{
						long value3 = text2.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text2, 16) : Convert.ToInt64(text2, 8);
						value = Convert.ToDecimal(value3);
					}
					catch (Exception ex2)
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text2), ex2);
					}
				}
				else
				{
					string s = _stringReference.ToString();
					if (!decimal.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out decimal result))
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = result;
				}
				newToken = JsonToken.Float;
			}
			else if (flag)
			{
				value = (long)c - 48L;
				newToken = JsonToken.Integer;
			}
			else if (flag2)
			{
				string text3 = _stringReference.ToString();
				try
				{
					value = (text3.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text3, 16) : Convert.ToInt64(text3, 8));
				}
				catch (Exception ex3)
				{
					throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text3), ex3);
				}
				newToken = JsonToken.Integer;
			}
			else
			{
				long value4;
				switch (ConvertUtils.Int64TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out value4))
				{
				case ParseResult.Success:
					value = value4;
					newToken = JsonToken.Integer;
					break;
				case ParseResult.Overflow:
				{
					string text5 = _stringReference.ToString();
					if (text5.Length > 380)
					{
						throw JsonReaderException.Create(this, "JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = BigIntegerParse(text5, CultureInfo.InvariantCulture);
					newToken = JsonToken.Integer;
					break;
				}
				default:
				{
					string text4 = _stringReference.ToString();
					if (_floatParseHandling == FloatParseHandling.Decimal)
					{
						if (!decimal.TryParse(text4, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out decimal result2))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text4));
						}
						value = result2;
					}
					else
					{
						if (!double.TryParse(text4, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out double result3))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text4));
						}
						value = result3;
					}
					newToken = JsonToken.Float;
					break;
				}
				}
			}
			ClearRecentString();
			SetToken(newToken, value, updateIndex: false);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static object BigIntegerParse(string number, CultureInfo culture)
		{
			return BigInteger.Parse(number, culture);
		}

		private void ParseComment()
		{
			_charPos++;
			if (!EnsureChars(1, append: false))
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			bool flag;
			if (_chars[_charPos] == '*')
			{
				flag = false;
			}
			else
			{
				if (_chars[_charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				flag = true;
			}
			_charPos++;
			int charPos = _charPos;
			bool flag2 = false;
			while (!flag2)
			{
				switch (_chars[_charPos])
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: true) == 0)
						{
							if (!flag)
							{
								throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
							}
							_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
							flag2 = true;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '*':
					_charPos++;
					if (!flag && EnsureChars(0, append: true) && _chars[_charPos] == '/')
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos - 1);
						_charPos++;
						flag2 = true;
					}
					break;
				case '\r':
					if (flag)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						flag2 = true;
					}
					ProcessCarriageReturn(append: true);
					break;
				case '\n':
					if (flag)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						flag2 = true;
					}
					ProcessLineFeed();
					break;
				default:
					_charPos++;
					break;
				}
			}
			SetToken(JsonToken.Comment, _stringReference.ToString());
			ClearRecentString();
		}

		private bool MatchValue(string value)
		{
			if (!EnsureChars(value.Length - 1, append: true))
			{
				return false;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (_chars[_charPos + i] != value[i])
				{
					return false;
				}
			}
			_charPos += value.Length;
			return true;
		}

		private bool MatchValueWithTrailingSeparator(string value)
		{
			if (!MatchValue(value))
			{
				return false;
			}
			if (!EnsureChars(0, append: false))
			{
				return true;
			}
			if (!IsSeparator(_chars[_charPos]))
			{
				return _chars[_charPos] == '\0';
			}
			return true;
		}

		private bool IsSeparator(char c)
		{
			switch (c)
			{
			case ',':
			case ']':
			case '}':
				return true;
			case '/':
			{
				if (!EnsureChars(1, append: false))
				{
					return false;
				}
				char c2 = _chars[_charPos + 1];
				if (c2 != '*')
				{
					return c2 == '/';
				}
				return true;
			}
			case ')':
				if (base.CurrentState == State.Constructor || base.CurrentState == State.ConstructorStart)
				{
					return true;
				}
				break;
			case '\t':
			case '\n':
			case '\r':
			case ' ':
				return true;
			default:
				if (char.IsWhiteSpace(c))
				{
					return true;
				}
				break;
			}
			return false;
		}

		private void ParseTrue()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.True))
			{
				SetToken(JsonToken.Boolean, true);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		private void ParseNull()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.Null))
			{
				SetToken(JsonToken.Null);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing null value.");
		}

		private void ParseUndefined()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.Undefined))
			{
				SetToken(JsonToken.Undefined);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing undefined value.");
		}

		private void ParseFalse()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.False))
			{
				SetToken(JsonToken.Boolean, false);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		private void ParseNumberNegativeInfinity()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read -Infinity as a decimal.");
				}
				SetToken(JsonToken.Float, double.NegativeInfinity);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing negative infinity value.");
		}

		private void ParseNumberPositiveInfinity()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read Infinity as a decimal.");
				}
				SetToken(JsonToken.Float, double.PositiveInfinity);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing positive infinity value.");
		}

		private void ParseNumberNaN()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.NaN))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read NaN as a decimal.");
				}
				SetToken(JsonToken.Float, double.NaN);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing NaN value.");
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput && _reader != null)
			{
				_reader.Close();
			}
			if (_buffer != null)
			{
				_buffer.Clear();
			}
		}

		public bool HasLineInfo()
		{
			return true;
		}
	}
}
