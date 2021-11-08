using System;

namespace Microsoft.QualityTools.Testing.Fakes.Shims
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class ShimMethodAttribute : Attribute
	{
		public string ShortName
		{
			get;
			private set;
		}

		public ShimBinding Binding
		{
			get;
			private set;
		}

		public ShimMethodAttribute(string shortName, int binding)
		{
			if (string.IsNullOrEmpty(shortName))
			{
				throw new ArgumentNullException("shortName");
			}
			if (!Enum.IsDefined(typeof(ShimBinding), binding))
			{
				throw new ArgumentOutOfRangeException("binding");
			}
			ShortName = shortName;
			Binding = (ShimBinding)binding;
		}
	}
}
