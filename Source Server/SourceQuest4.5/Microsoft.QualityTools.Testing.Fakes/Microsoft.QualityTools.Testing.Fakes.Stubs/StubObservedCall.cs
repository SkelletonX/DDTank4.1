using System;
using System.Reflection;

namespace Microsoft.QualityTools.Testing.Fakes.Stubs
{
	public sealed class StubObservedCall
	{
		private readonly Type stubbedType;

		private readonly Delegate stub;

		private readonly object[] _arguments;

		private static readonly object[] emptyArguments = new object[0];

		private MethodInfo _stubbedMethod;

		public Type StubbedType => stubbedType;

		public Delegate Stub => stub;

		public MethodInfo StubbedMethod
		{
			get
			{
				MethodInfo methodInfo = _stubbedMethod;
				if ((object)methodInfo == null)
				{
					methodInfo = (_stubbedMethod = StubRuntime.ResolveStubbedMethod(StubbedType, Stub));
				}
				return methodInfo;
			}
		}

		public StubObservedCall(Type stubbedType, Delegate stub, object[] arguments)
		{
			this.stubbedType = stubbedType;
			this.stub = stub;
			_arguments = arguments;
		}

		public object[] GetArguments()
		{
			if (_arguments != null)
			{
				return (object[])_arguments.Clone();
			}
			return emptyArguments;
		}
	}
}
