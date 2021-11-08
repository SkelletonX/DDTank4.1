using System;
using System.Diagnostics.Contracts;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ShimClassAttribute : Attribute
	{
		private readonly Type targetType;

		public Type TargetType => targetType;

		public ShimClassAttribute(Type targetType)
		{
			if ((object)targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (!ReflectionContract.IsClassOrValueType(targetType))
			{
				throw new ArgumentException("must be a class or a valuetype", "targetType");
			}
			if (!ReflectionContract.IsTypeDefinition(targetType))
			{
				throw new ArgumentException("must be a type definition", "targetType");
			}
			this.targetType = targetType;
		}
	}
}
