using System;

namespace Newtonsoft.Json.Serialization
{
	internal struct ResolverContractKey : IEquatable<ResolverContractKey>
	{
		private readonly Type _resolverType;

		private readonly Type _contractType;

		public ResolverContractKey(Type resolverType, Type contractType)
		{
			_resolverType = resolverType;
			_contractType = contractType;
		}

		public override int GetHashCode()
		{
			return _resolverType.GetHashCode() ^ _contractType.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ResolverContractKey))
			{
				return false;
			}
			return Equals((ResolverContractKey)obj);
		}

		public bool Equals(ResolverContractKey other)
		{
			if (_resolverType == other._resolverType)
			{
				return _contractType == other._contractType;
			}
			return false;
		}
	}
}
