using System;
using System.Diagnostics.Contracts;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class StubClassAttribute : Attribute
	{
		private readonly Type targetType;

		public Type TargetType => targetType;

		public StubClassAttribute(Type targetType)
		{
			if ((object)targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (!ReflectionContract.IsClassOrInterface(targetType))
			{
				throw new ArgumentException("must be a class or an interface", "targetType");
			}
			if (!ReflectionContract.IsTypeDefinition(targetType))
			{
				throw new ArgumentException("must be a type definition", "targetType");
			}
			this.targetType = targetType;
		}
	}
}
