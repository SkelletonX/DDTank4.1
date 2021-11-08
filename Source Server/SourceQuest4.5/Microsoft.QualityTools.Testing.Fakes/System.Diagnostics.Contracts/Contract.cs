namespace System.Diagnostics.Contracts
{
	internal static class Contract
	{
		[Serializable]
		private class ContractException : Exception
		{
			public ContractException()
			{
			}

			public ContractException(string message)
				: base(message)
			{
			}
		}

		[Conditional("DEBUG")]
		public static void EndContractBlock()
		{
		}

		[Conditional("DEBUG")]
		public static void Requires(bool condition)
		{
			if (!condition)
			{
				throw new ContractException();
			}
		}

		[Conditional("DEBUG")]
		private static void Break()
		{
			if (!Debugger.IsAttached)
			{
				Debugger.Launch();
			}
			Debugger.Break();
		}

		[Conditional("DEBUG")]
		public static void Requires(bool condition, string message)
		{
			if (!condition)
			{
				throw new ContractException(message);
			}
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition)
		{
			if (!condition)
			{
				throw new ContractException();
			}
		}

		[Conditional("DEBUG")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				throw new ContractException(message);
			}
		}
	}
}
