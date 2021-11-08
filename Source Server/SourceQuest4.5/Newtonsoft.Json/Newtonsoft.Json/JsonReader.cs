using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Newtonsoft.Json
{
	public abstract class JsonReader : IDisposable
	{
		protected internal enum State
		{
			Start,
			Complete,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			Closed,
			PostValue,
			ConstructorStart,
			Constructor,
			Error,
			Finished
		}

		private JsonToken _tokenType;

		private object _value;

		internal char _quoteChar;

		internal State _currentState;

		internal ReadType _readType;

		private JsonPosition _currentPosition;

		private CultureInfo _culture;

		private DateTimeZoneHandling _dateTimeZoneHandling;

		private int? _maxDepth;

		private bool _hasExceededMaxDepth;

		internal DateParseHandling _dateParseHandling;

		internal FloatParseHandling _floatParseHandling;

		private string _dateFormatString;

		private readonly List<JsonPosition> _stack;

		protected State CurrentState => _currentState;

		public bool CloseInput
		{
			get;
			set;
		}

		public bool SupportMultipleContent
		{
			get;
			set;
		}

		public virtual char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			protected internal set
			{
				_quoteChar = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling;
			}
			set
			{
				_dateTimeZoneHandling = value;
			}
		}

		public DateParseHandling DateParseHandling
		{
			get
			{
				return _dateParseHandling;
			}
			set
			{
				_dateParseHandling = value;
			}
		}

		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return _floatParseHandling;
			}
			set
			{
				_floatParseHandling = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString;
			}
			set
			{
				_dateFormatString = value;
			}
		}

		public int? MaxDepth
		{
			get
			{
				return _maxDepth;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				_maxDepth = value;
			}
		}

		public virtual JsonToken TokenType => _tokenType;

		public virtual object Value => _value;

		public virtual Type ValueType
		{
			get
			{
				if (_value == null)
				{
					return null;
				}
				return _value.GetType();
			}
		}

		public virtual int Depth
		{
			get
			{
				int count = _stack.Count;
				if (JsonTokenUtils.IsStartToken(TokenType) || _currentPosition.Type == JsonContainerType.None)
				{
					return count;
				}
				return count + 1;
			}
		}

		public virtual string Path
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				IEnumerable<JsonPosition> positions = (_currentState == State.ArrayStart || _currentState == State.ConstructorStart || _currentState == State.ObjectStart) ? _stack : _stack.Concat(new JsonPosition[1]
				{
					_currentPosition
				});
				return JsonPosition.BuildPath(positions);
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				_culture = value;
			}
		}

		internal JsonPosition GetPosition(int depth)
		{
			if (depth < _stack.Count)
			{
				return _stack[depth];
			}
			return _currentPosition;
		}

		protected JsonReader()
		{
			_currentState = State.Start;
			_stack = new List<JsonPosition>(4);
			_dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			_dateParseHandling = DateParseHandling.DateTime;
			_floatParseHandling = FloatParseHandling.Double;
			CloseInput = true;
		}

		private void Push(JsonContainerType value)
		{
			UpdateScopeWithFinishedValue();
			if (_currentPosition.Type == JsonContainerType.None)
			{
				_currentPosition = new JsonPosition(value);
				return;
			}
			_stack.Add(_currentPosition);
			_currentPosition = new JsonPosition(value);
			if (!_maxDepth.HasValue || !(Depth + 1 > _maxDepth) || _hasExceededMaxDepth)
			{
				return;
			}
			_hasExceededMaxDepth = true;
			throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, _maxDepth));
		}

		private JsonContainerType Pop()
		{
			JsonPosition currentPosition;
			if (_stack.Count > 0)
			{
				currentPosition = _currentPosition;
				_currentPosition = _stack[_stack.Count - 1];
				_stack.RemoveAt(_stack.Count - 1);
			}
			else
			{
				currentPosition = _currentPosition;
				_currentPosition = default(JsonPosition);
			}
			if (_maxDepth.HasValue && Depth <= _maxDepth)
			{
				_hasExceededMaxDepth = false;
			}
			return currentPosition.Type;
		}

		private JsonContainerType Peek()
		{
			return _currentPosition.Type;
		}

		public abstract bool Read();

		public abstract int? ReadAsInt32();

		public abstract string ReadAsString();

		public abstract byte[] ReadAsBytes();

		public abstract decimal? ReadAsDecimal();

		public abstract DateTime? ReadAsDateTime();

		public abstract DateTimeOffset? ReadAsDateTimeOffset();

		internal virtual bool ReadInternal()
		{
			throw new NotImplementedException();
		}

		internal DateTimeOffset? ReadAsDateTimeOffsetInternal()
		{
			_readType = ReadType.ReadAsDateTimeOffset;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Date:
					if (Value is DateTime)
					{
						SetToken(JsonToken.Date, new DateTimeOffset((DateTime)Value), updateIndex: false);
					}
					return (DateTimeOffset)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					DateTimeOffset dateTimeOffset;
					if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTimeOffset, DateTimeZoneHandling, _dateFormatString, Culture, out object dt))
					{
						dateTimeOffset = (DateTimeOffset)dt;
						SetToken(JsonToken.Date, dateTimeOffset, updateIndex: false);
						return dateTimeOffset;
					}
					if (DateTimeOffset.TryParse(text, Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
					{
						SetToken(JsonToken.Date, dateTimeOffset, updateIndex: false);
						return dateTimeOffset;
					}
					throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal byte[] ReadAsBytesInternal()
		{
			_readType = ReadType.ReadAsBytes;
			JsonToken tokenType;
			do
			{
				if (!ReadInternal())
				{
					SetToken(JsonToken.None);
					return null;
				}
				tokenType = TokenType;
			}
			while (tokenType == JsonToken.Comment);
			if (IsWrappedInTypeObject())
			{
				byte[] array = ReadAsBytes();
				ReadInternal();
				SetToken(JsonToken.Bytes, array, updateIndex: false);
				return array;
			}
			switch (tokenType)
			{
			case JsonToken.String:
			{
				string text = (string)Value;
				Guid g;
				byte[] array3 = (text.Length == 0) ? new byte[0] : ((!ConvertUtils.TryConvertGuid(text, out g)) ? Convert.FromBase64String(text) : g.ToByteArray());
				SetToken(JsonToken.Bytes, array3, updateIndex: false);
				return array3;
			}
			case JsonToken.Null:
				return null;
			case JsonToken.Bytes:
				if (ValueType == typeof(Guid))
				{
					byte[] array4 = ((Guid)Value).ToByteArray();
					SetToken(JsonToken.Bytes, array4, updateIndex: false);
					return array4;
				}
				return (byte[])Value;
			case JsonToken.StartArray:
			{
				List<byte> list = new List<byte>();
				while (ReadInternal())
				{
					tokenType = TokenType;
					switch (tokenType)
					{
					case JsonToken.Integer:
						list.Add(Convert.ToByte(Value, CultureInfo.InvariantCulture));
						break;
					case JsonToken.EndArray:
					{
						byte[] array2 = list.ToArray();
						SetToken(JsonToken.Bytes, array2, updateIndex: false);
						return array2;
					}
					default:
						throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
					case JsonToken.Comment:
						break;
					}
				}
				throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
			}
			case JsonToken.EndArray:
				return null;
			default:
				throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
		}

		internal decimal? ReadAsDecimalInternal()
		{
			_readType = ReadType.ReadAsDecimal;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
					if (!(Value is decimal))
					{
						SetToken(JsonToken.Float, Convert.ToDecimal(Value, CultureInfo.InvariantCulture), updateIndex: false);
					}
					return (decimal)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					if (decimal.TryParse(text, NumberStyles.Number, Culture, out decimal result))
					{
						SetToken(JsonToken.Float, result, updateIndex: false);
						return result;
					}
					throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal int? ReadAsInt32Internal()
		{
			_readType = ReadType.ReadAsInt32;
			while (ReadInternal())
			{
				switch (TokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
					if (!(Value is int))
					{
						SetToken(JsonToken.Integer, Convert.ToInt32(Value, CultureInfo.InvariantCulture), updateIndex: false);
					}
					return (int)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					if (int.TryParse(text, NumberStyles.Integer, Culture, out int result))
					{
						SetToken(JsonToken.Integer, result, updateIndex: false);
						return result;
					}
					throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal string ReadAsStringInternal()
		{
			_readType = ReadType.ReadAsString;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					continue;
				case JsonToken.String:
					return (string)Value;
				case JsonToken.Null:
					return null;
				}
				if (JsonTokenUtils.IsPrimitiveToken(tokenType) && Value != null)
				{
					string text = (!(Value is IFormattable)) ? Value.ToString() : ((IFormattable)Value).ToString(null, Culture);
					SetToken(JsonToken.String, text, updateIndex: false);
					return text;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return null;
				}
				throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal DateTime? ReadAsDateTimeInternal()
		{
			_readType = ReadType.ReadAsDateTime;
			do
			{
				if (!ReadInternal())
				{
					SetToken(JsonToken.None);
					return null;
				}
			}
			while (TokenType == JsonToken.Comment);
			if (TokenType == JsonToken.Date)
			{
				return (DateTime)Value;
			}
			if (TokenType == JsonToken.Null)
			{
				return null;
			}
			if (TokenType == JsonToken.String)
			{
				string text = (string)Value;
				if (string.IsNullOrEmpty(text))
				{
					SetToken(JsonToken.Null);
					return null;
				}
				DateTime value;
				if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTime, DateTimeZoneHandling, _dateFormatString, Culture, out object dt))
				{
					value = (DateTime)dt;
					value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);
					SetToken(JsonToken.Date, value, updateIndex: false);
					return value;
				}
				if (DateTime.TryParse(text, Culture, DateTimeStyles.RoundtripKind, out value))
				{
					value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);
					SetToken(JsonToken.Date, value, updateIndex: false);
					return value;
				}
				throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
			}
			if (TokenType == JsonToken.EndArray)
			{
				return null;
			}
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
		}

		private bool IsWrappedInTypeObject()
		{
			_readType = ReadType.Read;
			if (TokenType == JsonToken.StartObject)
			{
				if (!ReadInternal())
				{
					throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
				}
				if (Value.ToString() == "$type")
				{
					ReadInternal();
					if (Value != null && Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
					{
						ReadInternal();
						if (Value.ToString() == "$value")
						{
							return true;
						}
					}
				}
				throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
			}
			return false;
		}

		public void Skip()
		{
			if (TokenType == JsonToken.PropertyName)
			{
				Read();
			}
			if (JsonTokenUtils.IsStartToken(TokenType))
			{
				int depth = Depth;
				while (Read() && depth < Depth)
				{
				}
			}
		}

		protected void SetToken(JsonToken newToken)
		{
			SetToken(newToken, null, updateIndex: true);
		}

		protected void SetToken(JsonToken newToken, object value)
		{
			SetToken(newToken, value, updateIndex: true);
		}

		internal void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			_tokenType = newToken;
			_value = value;
			switch (newToken)
			{
			case JsonToken.Comment:
				break;
			case JsonToken.StartObject:
				_currentState = State.ObjectStart;
				Push(JsonContainerType.Object);
				break;
			case JsonToken.StartArray:
				_currentState = State.ArrayStart;
				Push(JsonContainerType.Array);
				break;
			case JsonToken.StartConstructor:
				_currentState = State.ConstructorStart;
				Push(JsonContainerType.Constructor);
				break;
			case JsonToken.EndObject:
				ValidateEnd(JsonToken.EndObject);
				break;
			case JsonToken.EndArray:
				ValidateEnd(JsonToken.EndArray);
				break;
			case JsonToken.EndConstructor:
				ValidateEnd(JsonToken.EndConstructor);
				break;
			case JsonToken.PropertyName:
				_currentState = State.Property;
				_currentPosition.PropertyName = (string)value;
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				SetPostValueState(updateIndex);
				break;
			}
		}

		internal void SetPostValueState(bool updateIndex)
		{
			if (Peek() != 0)
			{
				_currentState = State.PostValue;
			}
			else
			{
				SetFinished();
			}
			if (updateIndex)
			{
				UpdateScopeWithFinishedValue();
			}
		}

		private void UpdateScopeWithFinishedValue()
		{
			if (_currentPosition.HasIndex)
			{
				_currentPosition.Position++;
			}
		}

		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = Pop();
			if (GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (Peek() != 0)
			{
				_currentState = State.PostValue;
			}
			else
			{
				SetFinished();
			}
		}

		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.Object:
				_currentState = State.Object;
				break;
			case JsonContainerType.Array:
				_currentState = State.Array;
				break;
			case JsonContainerType.Constructor:
				_currentState = State.Constructor;
				break;
			case JsonContainerType.None:
				SetFinished();
				break;
			default:
				throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
			}
		}

		private void SetFinished()
		{
			if (SupportMultipleContent)
			{
				_currentState = State.Start;
			}
			else
			{
				_currentState = State.Finished;
			}
		}

		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				return JsonContainerType.Object;
			case JsonToken.EndArray:
				return JsonContainerType.Array;
			case JsonToken.EndConstructor:
				return JsonContainerType.Constructor;
			default:
				throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token));
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_currentState != State.Closed && disposing)
			{
				Close();
			}
		}

		public virtual void Close()
		{
			_currentState = State.Closed;
			_tokenType = JsonToken.None;
			_value = null;
		}
	}
}
