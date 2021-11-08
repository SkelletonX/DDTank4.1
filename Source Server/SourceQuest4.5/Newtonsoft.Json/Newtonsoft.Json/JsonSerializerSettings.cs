using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
	public class JsonSerializerSettings
	{
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;

		internal const Formatting DefaultFormatting = Formatting.None;

		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		internal const FormatterAssemblyStyle DefaultFormatterAssemblyStyle = FormatterAssemblyStyle.Simple;

		internal const bool DefaultCheckAdditionalContent = false;

		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		internal static readonly StreamingContext DefaultContext;

		internal static readonly CultureInfo DefaultCulture;

		internal Formatting? _formatting;

		internal DateFormatHandling? _dateFormatHandling;

		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		internal DateParseHandling? _dateParseHandling;

		internal FloatFormatHandling? _floatFormatHandling;

		internal FloatParseHandling? _floatParseHandling;

		internal StringEscapeHandling? _stringEscapeHandling;

		internal CultureInfo _culture;

		internal bool? _checkAdditionalContent;

		internal int? _maxDepth;

		internal bool _maxDepthSet;

		internal string _dateFormatString;

		internal bool _dateFormatStringSet;

		internal FormatterAssemblyStyle? _typeNameAssemblyFormat;

		internal DefaultValueHandling? _defaultValueHandling;

		internal PreserveReferencesHandling? _preserveReferencesHandling;

		internal NullValueHandling? _nullValueHandling;

		internal ObjectCreationHandling? _objectCreationHandling;

		internal MissingMemberHandling? _missingMemberHandling;

		internal ReferenceLoopHandling? _referenceLoopHandling;

		internal StreamingContext? _context;

		internal ConstructorHandling? _constructorHandling;

		internal TypeNameHandling? _typeNameHandling;

		internal MetadataPropertyHandling? _metadataPropertyHandling;

		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling ?? ReferenceLoopHandling.Error;
			}
			set
			{
				_referenceLoopHandling = value;
			}
		}

		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return _missingMemberHandling ?? MissingMemberHandling.Ignore;
			}
			set
			{
				_missingMemberHandling = value;
			}
		}

		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling ?? ObjectCreationHandling.Auto;
			}
			set
			{
				_objectCreationHandling = value;
			}
		}

		public NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling ?? NullValueHandling.Include;
			}
			set
			{
				_nullValueHandling = value;
			}
		}

		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling ?? DefaultValueHandling.Include;
			}
			set
			{
				_defaultValueHandling = value;
			}
		}

		public IList<JsonConverter> Converters
		{
			get;
			set;
		}

		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return _preserveReferencesHandling ?? PreserveReferencesHandling.None;
			}
			set
			{
				_preserveReferencesHandling = value;
			}
		}

		public TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling ?? TypeNameHandling.None;
			}
			set
			{
				_typeNameHandling = value;
			}
		}

		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return _metadataPropertyHandling ?? MetadataPropertyHandling.Default;
			}
			set
			{
				_metadataPropertyHandling = value;
			}
		}

		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return _typeNameAssemblyFormat ?? FormatterAssemblyStyle.Simple;
			}
			set
			{
				_typeNameAssemblyFormat = value;
			}
		}

		public ConstructorHandling ConstructorHandling
		{
			get
			{
				return _constructorHandling ?? ConstructorHandling.Default;
			}
			set
			{
				_constructorHandling = value;
			}
		}

		public IContractResolver ContractResolver
		{
			get;
			set;
		}

		public IReferenceResolver ReferenceResolver
		{
			get;
			set;
		}

		public ITraceWriter TraceWriter
		{
			get;
			set;
		}

		public SerializationBinder Binder
		{
			get;
			set;
		}

		public EventHandler<ErrorEventArgs> Error
		{
			get;
			set;
		}

		public StreamingContext Context
		{
			get
			{
				return _context ?? DefaultContext;
			}
			set
			{
				_context = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				_dateFormatString = value;
				_dateFormatStringSet = true;
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
				_maxDepthSet = true;
			}
		}

		public Formatting Formatting
		{
			get
			{
				return _formatting ?? Formatting.None;
			}
			set
			{
				_formatting = value;
			}
		}

		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
			}
			set
			{
				_dateFormatHandling = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
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
				return _dateParseHandling ?? DateParseHandling.DateTime;
			}
			set
			{
				_dateParseHandling = value;
			}
		}

		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _floatFormatHandling ?? FloatFormatHandling.String;
			}
			set
			{
				_floatFormatHandling = value;
			}
		}

		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return _floatParseHandling ?? FloatParseHandling.Double;
			}
			set
			{
				_floatParseHandling = value;
			}
		}

		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _stringEscapeHandling ?? StringEscapeHandling.Default;
			}
			set
			{
				_stringEscapeHandling = value;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? DefaultCulture;
			}
			set
			{
				_culture = value;
			}
		}

		public bool CheckAdditionalContent
		{
			get
			{
				return _checkAdditionalContent ?? false;
			}
			set
			{
				_checkAdditionalContent = value;
			}
		}

		static JsonSerializerSettings()
		{
			DefaultContext = default(StreamingContext);
			DefaultCulture = CultureInfo.InvariantCulture;
		}

		public JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>();
		}
	}
}
