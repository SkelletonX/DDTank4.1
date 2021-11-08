using System;
using System.Diagnostics.Contracts;

namespace Microsoft.QualityTools.Testing.Fakes
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public sealed class FakesDelegatesTypeAttribute : Attribute
	{
		private readonly Type holderType;

		public Type HolderType => holderType;

		public FakesDelegatesTypeAttribute(Type holderType)
		{
			if ((object)holderType == null)
			{
				throw new ArgumentNullException("holderType");
			}
			if (!ReflectionContract.IsClassOrValueType(holderType))
			{
				throw new ArgumentException("must be a class or a valuetype", "holderType");
			}
			if (!ReflectionContract.IsTypeDefinition(holderType))
			{
				throw new ArgumentException("must be a type definition", "holderType");
			}
			this.holderType = holderType;
		}
	}
}
