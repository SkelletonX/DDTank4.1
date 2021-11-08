using System;
using System.Reflection.Emit;

namespace ProtoBuf.Compiler
{
	internal sealed class Local : IDisposable
	{
		private LocalBuilder value;

		private CompilerContext ctx;

		private readonly Type type;

		public Type Type => type;

		internal LocalBuilder Value
		{
			get
			{
				if (value == null)
				{
					throw new ObjectDisposedException(GetType().Name);
				}
				return value;
			}
		}

		public Local AsCopy()
		{
			if (ctx == null)
			{
				return this;
			}
			return new Local(value, type);
		}

		public void Dispose()
		{
			if (ctx != null)
			{
				ctx.ReleaseToPool(value);
				value = null;
				ctx = null;
			}
		}

		private Local(LocalBuilder value, Type type)
		{
			this.value = value;
			this.type = type;
		}

		internal Local(CompilerContext ctx, Type type)
		{
			this.ctx = ctx;
			if (ctx != null)
			{
				value = ctx.GetFromPool(type);
			}
			this.type = type;
		}

		internal bool IsSame(Local other)
		{
			if (this == other)
			{
				return true;
			}
			object ourVal = value;
			if (other != null)
			{
				return ourVal == other.value;
			}
			return false;
		}
	}
}
