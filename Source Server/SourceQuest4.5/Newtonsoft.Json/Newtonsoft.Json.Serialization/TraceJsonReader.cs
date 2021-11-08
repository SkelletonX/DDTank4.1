using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Serialization
{
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		private readonly JsonReader _innerReader;

		private readonly JsonTextWriter _textWriter;

		private readonly StringWriter _sw;

		public override int Depth => _innerReader.Depth;

		public override string Path => _innerReader.Path;

		public override char QuoteChar
		{
			get
			{
				return _innerReader.QuoteChar;
			}
			protected internal set
			{
				_innerReader.QuoteChar = value;
			}
		}

		public override JsonToken TokenType => _innerReader.TokenType;

		public override object Value => _innerReader.Value;

		public override Type ValueType => _innerReader.ValueType;

		int IJsonLineInfo.LineNumber => (_innerReader as IJsonLineInfo)?.LineNumber ?? 0;

		int IJsonLineInfo.LinePosition => (_innerReader as IJsonLineInfo)?.LinePosition ?? 0;

		public TraceJsonReader(JsonReader innerReader)
		{
			_innerReader = innerReader;
			_sw = new StringWriter(CultureInfo.InvariantCulture);
			_textWriter = new JsonTextWriter(_sw);
			_textWriter.Formatting = Formatting.Indented;
		}

		public string GetJson()
		{
			return _sw.ToString();
		}

		public override bool Read()
		{
			bool result = _innerReader.Read();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override int? ReadAsInt32()
		{
			int? result = _innerReader.ReadAsInt32();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override string ReadAsString()
		{
			string result = _innerReader.ReadAsString();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override byte[] ReadAsBytes()
		{
			byte[] result = _innerReader.ReadAsBytes();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? result = _innerReader.ReadAsDecimal();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = _innerReader.ReadAsDateTime();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = _innerReader.ReadAsDateTimeOffset();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override void Close()
		{
			_innerReader.Close();
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			return (_innerReader as IJsonLineInfo)?.HasLineInfo() ?? false;
		}
	}
}
