using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
	public class JsonProperty
	{
		internal Required? _required;

		internal bool _hasExplicitDefaultValue;

		private object _defaultValue;

		private bool _hasGeneratedDefaultValue;

		private string _propertyName;

		internal bool _skipPropertyNameEscape;

		private Type _propertyType;

		internal JsonContract PropertyContract
		{
			get;
			set;
		}

		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
			set
			{
				_propertyName = value;
				_skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(_propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		public Type DeclaringType
		{
			get;
			set;
		}

		public int? Order
		{
			get;
			set;
		}

		public string UnderlyingName
		{
			get;
			set;
		}

		public IValueProvider ValueProvider
		{
			get;
			set;
		}

		public IAttributeProvider AttributeProvider
		{
			get;
			set;
		}

		public Type PropertyType
		{
			get
			{
				return _propertyType;
			}
			set
			{
				if (_propertyType != value)
				{
					_propertyType = value;
					_hasGeneratedDefaultValue = false;
				}
			}
		}

		public JsonConverter Converter
		{
			get;
			set;
		}

		public JsonConverter MemberConverter
		{
			get;
			set;
		}

		public bool Ignored
		{
			get;
			set;
		}

		public bool Readable
		{
			get;
			set;
		}

		public bool Writable
		{
			get;
			set;
		}

		public bool HasMemberAttribute
		{
			get;
			set;
		}

		public object DefaultValue
		{
			get
			{
				if (!_hasExplicitDefaultValue)
				{
					return null;
				}
				return _defaultValue;
			}
			set
			{
				_hasExplicitDefaultValue = true;
				_defaultValue = value;
			}
		}

		public Required Required
		{
			get
			{
				return _required ?? Required.Default;
			}
			set
			{
				_required = value;
			}
		}

		public bool? IsReference
		{
			get;
			set;
		}

		public NullValueHandling? NullValueHandling
		{
			get;
			set;
		}

		public DefaultValueHandling? DefaultValueHandling
		{
			get;
			set;
		}

		public ReferenceLoopHandling? ReferenceLoopHandling
		{
			get;
			set;
		}

		public ObjectCreationHandling? ObjectCreationHandling
		{
			get;
			set;
		}

		public TypeNameHandling? TypeNameHandling
		{
			get;
			set;
		}

		public Predicate<object> ShouldSerialize
		{
			get;
			set;
		}

		public Predicate<object> GetIsSpecified
		{
			get;
			set;
		}

		public Action<object, object> SetIsSpecified
		{
			get;
			set;
		}

		public JsonConverter ItemConverter
		{
			get;
			set;
		}

		public bool? ItemIsReference
		{
			get;
			set;
		}

		public TypeNameHandling? ItemTypeNameHandling
		{
			get;
			set;
		}

		public ReferenceLoopHandling? ItemReferenceLoopHandling
		{
			get;
			set;
		}

		internal object GetResolvedDefaultValue()
		{
			if (_propertyType == null)
			{
				return null;
			}
			if (!_hasExplicitDefaultValue && !_hasGeneratedDefaultValue)
			{
				_defaultValue = ReflectionUtils.GetDefaultValue(PropertyType);
				_hasGeneratedDefaultValue = true;
			}
			return _defaultValue;
		}

		public override string ToString()
		{
			return PropertyName;
		}

		internal void WritePropertyName(JsonWriter writer)
		{
			if (_skipPropertyNameEscape)
			{
				writer.WritePropertyName(PropertyName, escape: false);
			}
			else
			{
				writer.WritePropertyName(PropertyName);
			}
		}
	}
}
