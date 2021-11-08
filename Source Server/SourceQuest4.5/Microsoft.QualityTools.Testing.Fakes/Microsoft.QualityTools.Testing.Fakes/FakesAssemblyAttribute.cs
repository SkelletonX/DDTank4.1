using System;

namespace Microsoft.QualityTools.Testing.Fakes
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public sealed class FakesAssemblyAttribute : Attribute
	{
		public string FakedAssemblyName
		{
			get;
			private set;
		}

		public bool DisableShims
		{
			get;
			private set;
		}

		public FakesAssemblyAttribute(string fakedAssemblyName, bool disabledShims)
		{
			if (string.IsNullOrEmpty(fakedAssemblyName))
			{
				throw new ArgumentNullException("fakedAssemblyName");
			}
			FakedAssemblyName = fakedAssemblyName;
			DisableShims = disabledShims;
		}
	}
}
