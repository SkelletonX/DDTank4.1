using Newtonsoft.Json.Utilities;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	public class JsonDynamicContract : JsonContainerContract
	{
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(CreateCallSiteGetter);

		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(CreateCallSiteSetter);

		public JsonPropertyCollection Properties
		{
			get;
			private set;
		}

		public Func<string, string> PropertyNameResolver
		{
			get;
			set;
		}

		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			GetMemberBinder innerBinder = (GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember(innerBinder));
		}

		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			SetMemberBinder innerBinder = (SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember(innerBinder));
		}

		public JsonDynamicContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Dynamic;
			Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = _callSiteGetters.Get(name);
			object obj = callSite.Target(callSite, dynamicProvider);
			if (!object.ReferenceEquals(obj, NoThrowExpressionVisitor.ErrorResult))
			{
				value = obj;
				return true;
			}
			value = null;
			return false;
		}

		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = _callSiteSetters.Get(name);
			object objA = callSite.Target(callSite, dynamicProvider, value);
			return !object.ReferenceEquals(objA, NoThrowExpressionVisitor.ErrorResult);
		}
	}
}
