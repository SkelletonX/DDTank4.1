using System.Diagnostics;

namespace Microsoft.QualityTools.Testing.Fakes
{
	internal static class FakesTraceSources
	{
		private static TraceSource runtime;

		public static TraceSource Runtime => runtime ?? (runtime = CreateTraceSource("Runtime"));

		private static TraceSource CreateTraceSource(string propertyName)
		{
			string name = typeof(FakesTraceSources).Namespace + "." + propertyName;
			return new TraceSource(name);
		}
	}
}
