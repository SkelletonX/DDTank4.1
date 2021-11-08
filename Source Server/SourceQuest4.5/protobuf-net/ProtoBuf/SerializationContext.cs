using System;
using System.Runtime.Serialization;

namespace ProtoBuf
{
	public sealed class SerializationContext
	{
		private bool frozen;

		private object context;

		private static readonly SerializationContext @default;

		private StreamingContextStates state = StreamingContextStates.Persistence;

		public object Context
		{
			get
			{
				return context;
			}
			set
			{
				if (context != value)
				{
					ThrowIfFrozen();
					context = value;
				}
			}
		}

		internal static SerializationContext Default => @default;

		public StreamingContextStates State
		{
			get
			{
				return state;
			}
			set
			{
				if (state != value)
				{
					ThrowIfFrozen();
					state = value;
				}
			}
		}

		internal void Freeze()
		{
			frozen = true;
		}

		private void ThrowIfFrozen()
		{
			if (frozen)
			{
				throw new InvalidOperationException("The serialization-context cannot be changed once it is in use");
			}
		}

		static SerializationContext()
		{
			@default = new SerializationContext();
			@default.Freeze();
		}

		public static implicit operator StreamingContext(SerializationContext ctx)
		{
			if (ctx == null)
			{
				return new StreamingContext(StreamingContextStates.Persistence);
			}
			return new StreamingContext(ctx.state, ctx.context);
		}

		public static implicit operator SerializationContext(StreamingContext ctx)
		{
			SerializationContext result = new SerializationContext();
			result.Context = ctx.Context;
			result.State = ctx.State;
			return result;
		}
	}
}
