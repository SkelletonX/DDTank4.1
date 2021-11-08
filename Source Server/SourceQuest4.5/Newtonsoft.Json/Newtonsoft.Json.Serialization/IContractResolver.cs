using System;

namespace Newtonsoft.Json.Serialization
{
	public interface IContractResolver
	{
		JsonContract ResolveContract(Type type);
	}
}
