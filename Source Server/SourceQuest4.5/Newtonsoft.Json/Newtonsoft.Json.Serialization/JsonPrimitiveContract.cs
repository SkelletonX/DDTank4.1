using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Serialization
{
	public class JsonPrimitiveContract : JsonContract
	{
		internal PrimitiveTypeCode TypeCode
		{
			get;
			set;
		}

		public JsonPrimitiveContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Primitive;
			TypeCode = ConvertUtils.GetTypeCode(underlyingType);
			IsReadOnlyOrFixedSize = true;
		}
	}
}
