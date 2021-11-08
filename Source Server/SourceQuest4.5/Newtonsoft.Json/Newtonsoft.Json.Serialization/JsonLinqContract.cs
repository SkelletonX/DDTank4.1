using System;

namespace Newtonsoft.Json.Serialization
{
	public class JsonLinqContract : JsonContract
	{
		public JsonLinqContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Linq;
		}
	}
}
