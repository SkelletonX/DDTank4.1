using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

namespace Newtonsoft.Json.Serialization
{
	public class JsonObjectContract : JsonContainerContract
	{
		private bool? _hasRequiredOrDefaultValueProperties;

		private ConstructorInfo _parametrizedConstructor;

		private ConstructorInfo _overrideConstructor;

		private ObjectConstructor<object> _overrideCreator;

		private ObjectConstructor<object> _parametrizedCreator;

		public MemberSerialization MemberSerialization
		{
			get;
			set;
		}

		public Required? ItemRequired
		{
			get;
			set;
		}

		public JsonPropertyCollection Properties
		{
			get;
			private set;
		}

		[Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
		public JsonPropertyCollection ConstructorParameters => CreatorParameters;

		public JsonPropertyCollection CreatorParameters
		{
			get;
			private set;
		}

		[Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo OverrideConstructor
		{
			get
			{
				return _overrideConstructor;
			}
			set
			{
				_overrideConstructor = value;
				_overrideCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		[Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo ParametrizedConstructor
		{
			get
			{
				return _parametrizedConstructor;
			}
			set
			{
				_parametrizedConstructor = value;
				_parametrizedCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return _overrideCreator;
			}
			set
			{
				_overrideCreator = value;
				_overrideConstructor = null;
			}
		}

		internal ObjectConstructor<object> ParametrizedCreator => _parametrizedCreator;

		public ExtensionDataSetter ExtensionDataSetter
		{
			get;
			set;
		}

		public ExtensionDataGetter ExtensionDataGetter
		{
			get;
			set;
		}

		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (!_hasRequiredOrDefaultValueProperties.HasValue)
				{
					_hasRequiredOrDefaultValueProperties = false;
					if (ItemRequired.GetValueOrDefault(Required.Default) != 0)
					{
						_hasRequiredOrDefaultValueProperties = true;
					}
					else
					{
						foreach (JsonProperty property in Properties)
						{
							if (property.Required != 0 || (((int?)property.DefaultValueHandling & 2) == 2 && property.Writable))
							{
								_hasRequiredOrDefaultValueProperties = true;
								break;
							}
						}
					}
				}
				return _hasRequiredOrDefaultValueProperties.Value;
			}
		}

		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Object;
			Properties = new JsonPropertyCollection(base.UnderlyingType);
			CreatorParameters = new JsonPropertyCollection(base.UnderlyingType);
		}

		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(NonNullableUnderlyingType);
		}
	}
}
