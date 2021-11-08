using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes.UnitTestIsolation
{
	public sealed class ObjectReferenceIdentity : IEqualityComparer<object>
	{
		private static readonly IEqualityComparer<object> _default = new ObjectReferenceIdentity();

		public static IEqualityComparer<object> Default => _default;

		public new bool Equals(object x, object y)
		{
			return object.ReferenceEquals(x, y);
		}

		public int GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}
	}
}
