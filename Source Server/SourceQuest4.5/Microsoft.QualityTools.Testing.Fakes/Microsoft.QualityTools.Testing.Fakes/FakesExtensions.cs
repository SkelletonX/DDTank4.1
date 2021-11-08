using Microsoft.QualityTools.Testing.Fakes.Shims;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Microsoft.QualityTools.Testing.Fakes
{
	[__Instrument]
	public static class FakesExtensions
	{
		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1> : Uncurrifier<FakesDelegates.Action<T1>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1)
			{
				InnerDelegate(t1);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, TResult> : Uncurrifier<FakesDelegates.Func<T1, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1)
			{
				return InnerDelegate(t1);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, out TOut @out)
			{
				InnerDelegate(t1, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, out TOut @out)
			{
				return InnerDelegate(t1, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, ref TRef @ref)
			{
				InnerDelegate(t1, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, ref TRef @ref)
			{
				return InnerDelegate(t1, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2> : Uncurrifier<FakesDelegates.Action<T1, T2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2)
			{
				InnerDelegate(t1, t2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2)
			{
				return InnerDelegate(t1, t2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, out TOut @out)
			{
				InnerDelegate(t1, t2, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, out TOut @out)
			{
				return InnerDelegate(t1, t2, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, ref TRef @ref)
			{
				InnerDelegate(t1, t2, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3> : Uncurrifier<FakesDelegates.Action<T1, T2, T3>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3)
			{
				InnerDelegate(t1, t2, t3);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3)
			{
				return InnerDelegate(t1, t2, t3);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
			{
				InnerDelegate(t1, t2, t3, t4);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
			{
				return InnerDelegate(t1, t2, t3, t4);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
			{
				InnerDelegate(t1, t2, t3, t4, t5);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
			{
				return InnerDelegate(t1, t2, t3, t4, t5);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : Uncurrifier<FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyActionClosure(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult> : Uncurrifier<FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyFuncClosure(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut> : Uncurrifier<FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutActionClosure(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut @out)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutActionClosure(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutFuncClosure(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut @out)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out @out);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyOutOutFuncClosure(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out out1, out out2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef> : Uncurrifier<FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefActionClosure(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef @ref)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefActionClosure(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefFuncClosure(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef @ref)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref @ref);
			}
		}

		[CompilerGenerated]
		private sealed class UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UncurrifyRefRefFuncClosure(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			public TResult Get(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref ref1, ref ref2);
			}
		}

		[__Instrument]
		[CompilerGenerated]
		private sealed class UnCurryActionClosure<T0> : Uncurrifier<FakesDelegates.Action>
		{
			[DebuggerNonUserCode]
			internal UnCurryActionClosure(FakesDelegates.Action action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0)
			{
				InnerDelegate();
			}
		}

		[__Instrument]
		[CompilerGenerated]
		private sealed class UnCurryFuncClosure<T0, TResult> : Uncurrifier<FakesDelegates.Func<TResult>>
		{
			[DebuggerNonUserCode]
			internal UnCurryFuncClosure(FakesDelegates.Func<TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public TResult Get(T0 t0)
			{
				return InnerDelegate();
			}
		}

		[__Instrument]
		[CompilerGenerated]
		private sealed class UnCurryOutActionClosure<T0, TOut> : Uncurrifier<FakesDelegates.OutAction<TOut>>
		{
			[DebuggerNonUserCode]
			internal UnCurryOutActionClosure(FakesDelegates.OutAction<TOut> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, out TOut @out)
			{
				InnerDelegate(out @out);
			}
		}

		[__Instrument]
		[CompilerGenerated]
		private sealed class UnCurryOutOutActionClosure<T0, TOut1, TOut2> : Uncurrifier<FakesDelegates.OutOutAction<TOut1, TOut2>>
		{
			[DebuggerNonUserCode]
			internal UnCurryOutOutActionClosure(FakesDelegates.OutOutAction<TOut1, TOut2> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			public void Get(T0 t0, out TOut1 out1, out TOut2 out2)
			{
				InnerDelegate(out out1, out out2);
			}
		}

		[CompilerGenerated]
		[__Instrument]
		private sealed class UnCurryOutFuncClosure<T0, TOut, TResult> : Uncurrifier<FakesDelegates.OutFunc<TOut, TResult>>
		{
			[DebuggerNonUserCode]
			internal UnCurryOutFuncClosure(FakesDelegates.OutFunc<TOut, TResult> func)
				: base(func)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public TResult Get(T0 t0, out TOut @out)
			{
				return InnerDelegate(out @out);
			}
		}

		[CompilerGenerated]
		[__Instrument]
		private sealed class UnCurryOutOutFuncClosure<T0, TOut1, TOut2, TResult> : Uncurrifier<FakesDelegates.OutOutFunc<TOut1, TOut2, TResult>>
		{
			[DebuggerNonUserCode]
			internal UnCurryOutOutFuncClosure(FakesDelegates.OutOutFunc<TOut1, TOut2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public TResult Get(T0 t0, out TOut1 out1, out TOut2 out2)
			{
				return InnerDelegate(out out1, out out2);
			}
		}

		[CompilerGenerated]
		[__Instrument]
		private sealed class UnCurryRefActionClosure<T0, TRef> : Uncurrifier<FakesDelegates.RefAction<TRef>>
		{
			[DebuggerNonUserCode]
			internal UnCurryRefActionClosure(FakesDelegates.RefAction<TRef> action)
				: base(action)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public void Get(T0 t0, ref TRef @ref)
			{
				InnerDelegate(ref @ref);
			}
		}

		[CompilerGenerated]
		[__Instrument]
		private sealed class UnCurryRefRefActionClosure<T0, TRef1, TRef2> : Uncurrifier<FakesDelegates.RefRefAction<TRef1, TRef2>>
		{
			[DebuggerNonUserCode]
			internal UnCurryRefRefActionClosure(FakesDelegates.RefRefAction<TRef1, TRef2> action)
				: base(action)
			{
			}

			[DebuggerHidden]
			[DebuggerNonUserCode]
			public void Get(T0 t0, ref TRef1 ref1, ref TRef2 ref2)
			{
				InnerDelegate(ref ref1, ref ref2);
			}
		}

		[CompilerGenerated]
		[__Instrument]
		private sealed class UnCurryRefFuncClosure<T0, TRef, TResult> : Uncurrifier<FakesDelegates.RefFunc<TRef, TResult>>
		{
			[DebuggerNonUserCode]
			public UnCurryRefFuncClosure(FakesDelegates.RefFunc<TRef, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public TResult Get(T0 t0, ref TRef @ref)
			{
				return InnerDelegate(ref @ref);
			}
		}

		[__Instrument]
		[CompilerGenerated]
		private sealed class UnCurryRefRefFuncClosure<T0, TRef1, TRef2, TResult> : Uncurrifier<FakesDelegates.RefRefFunc<TRef1, TRef2, TResult>>
		{
			[DebuggerNonUserCode]
			public UnCurryRefRefFuncClosure(FakesDelegates.RefRefFunc<TRef1, TRef2, TResult> func)
				: base(func)
			{
			}

			[DebuggerNonUserCode]
			[DebuggerHidden]
			public TResult Get(T0 t0, ref TRef1 ref1, ref TRef2 ref2)
			{
				return InnerDelegate(ref ref1, ref ref2);
			}
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1> Uncurrify<T0, T1>(FakesDelegates.Action<T1> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1)
			{
				new UncurrifyActionClosure<T0, T1>(action).InnerDelegate(t1);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, TResult> Uncurrify<T0, T1, TResult>(FakesDelegates.Func<T1, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1) => new UncurrifyFuncClosure<T0, T1, TResult>(func).InnerDelegate(t1);
		}

		public static FakesDelegates.OutAction<T0, T1, TOut> Uncurrify<T0, T1, TOut>(FakesDelegates.OutAction<T1, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, TOut>(action).InnerDelegate(t1, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, TOut1, TOut2> Uncurrify<T0, T1, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, TOut1, TOut2>(action).InnerDelegate(t1, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, TOut, TResult> Uncurrify<T0, T1, TOut, TResult>(FakesDelegates.OutFunc<T1, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, TOut, TResult>(func).InnerDelegate(t1, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, TOut1, TOut2, TResult> Uncurrify<T0, T1, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, TOut1, TOut2, TResult>(func).InnerDelegate(t1, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, TRef> Uncurrify<T0, T1, TRef>(FakesDelegates.RefAction<T1, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, TRef>(action).InnerDelegate(t1, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, TRef1, TRef2> Uncurrify<T0, T1, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, TRef1, TRef2>(action).InnerDelegate(t1, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, TRef, TResult> Uncurrify<T0, T1, TRef, TResult>(FakesDelegates.RefFunc<T1, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, TRef, TResult>(func).InnerDelegate(t1, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, TRef1, TRef2, TResult> Uncurrify<T0, T1, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, TRef1, TRef2, TResult>(func).InnerDelegate(t1, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2> Uncurrify<T0, T1, T2>(FakesDelegates.Action<T1, T2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2)
			{
				new UncurrifyActionClosure<T0, T1, T2>(action).InnerDelegate(t1, t2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, TResult> Uncurrify<T0, T1, T2, TResult>(FakesDelegates.Func<T1, T2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2) => new UncurrifyFuncClosure<T0, T1, T2, TResult>(func).InnerDelegate(t1, t2);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, TOut> Uncurrify<T0, T1, T2, TOut>(FakesDelegates.OutAction<T1, T2, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, TOut>(action).InnerDelegate(t1, t2, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, TOut1, TOut2> Uncurrify<T0, T1, T2, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, TOut1, TOut2>(action).InnerDelegate(t1, t2, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, TOut, TResult> Uncurrify<T0, T1, T2, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, TOut, TResult>(func).InnerDelegate(t1, t2, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, TRef> Uncurrify<T0, T1, T2, TRef>(FakesDelegates.RefAction<T1, T2, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, TRef>(action).InnerDelegate(t1, t2, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, TRef1, TRef2> Uncurrify<T0, T1, T2, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, TRef1, TRef2>(action).InnerDelegate(t1, t2, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, TRef, TResult> Uncurrify<T0, T1, T2, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, TRef, TResult>(func).InnerDelegate(t1, t2, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3> Uncurrify<T0, T1, T2, T3>(FakesDelegates.Action<T1, T2, T3> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3>(action).InnerDelegate(t1, t2, t3);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, TResult> Uncurrify<T0, T1, T2, T3, TResult>(FakesDelegates.Func<T1, T2, T3, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3) => new UncurrifyFuncClosure<T0, T1, T2, T3, TResult>(func).InnerDelegate(t1, t2, t3);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, TOut> Uncurrify<T0, T1, T2, T3, TOut>(FakesDelegates.OutAction<T1, T2, T3, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, TOut>(action).InnerDelegate(t1, t2, t3, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, TOut, TResult> Uncurrify<T0, T1, T2, T3, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, TOut, TResult>(func).InnerDelegate(t1, t2, t3, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, TRef> Uncurrify<T0, T1, T2, T3, TRef>(FakesDelegates.RefAction<T1, T2, T3, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, TRef>(action).InnerDelegate(t1, t2, t3, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, TRef, TResult> Uncurrify<T0, T1, T2, T3, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, TRef, TResult>(func).InnerDelegate(t1, t2, t3, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4> Uncurrify<T0, T1, T2, T3, T4>(FakesDelegates.Action<T1, T2, T3, T4> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4>(action).InnerDelegate(t1, t2, t3, t4);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, TResult> Uncurrify<T0, T1, T2, T3, T4, TResult>(FakesDelegates.Func<T1, T2, T3, T4, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, TResult>(func).InnerDelegate(t1, t2, t3, t4);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, TOut> Uncurrify<T0, T1, T2, T3, T4, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, TOut>(action).InnerDelegate(t1, t2, t3, t4, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, TRef> Uncurrify<T0, T1, T2, T3, T4, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, TRef>(action).InnerDelegate(t1, t2, t3, t4, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5> Uncurrify<T0, T1, T2, T3, T4, T5>(FakesDelegates.Action<T1, T2, T3, T4, T5> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5>(action).InnerDelegate(t1, t2, t3, t4, t5);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6> Uncurrify<T0, T1, T2, T3, T4, T5, T6>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6>(action).InnerDelegate(t1, t2, t3, t4, t5, t6);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(FakesDelegates.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18)
			{
				new UncurrifyActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult>(FakesDelegates.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18) => new UncurrifyFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18);
		}

		public static FakesDelegates.OutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut>(FakesDelegates.OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut @out)
			{
				new UncurrifyOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2>(FakesDelegates.OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut1 out1, out TOut2 out2)
			{
				new UncurrifyOutOutActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult>(FakesDelegates.OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut @out)
			{
				return new UncurrifyOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, out TOut1 out1, out TOut2 out2)
			{
				return new UncurrifyOutOutFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef>(FakesDelegates.RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef @ref)
			{
				new UncurrifyRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2>(FakesDelegates.RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UncurrifyRefRefActionClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2>(action).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult>(FakesDelegates.RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef @ref)
			{
				return new UncurrifyRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult> Uncurrify<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17, T18 t18, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UncurrifyRefRefFuncClosure<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult>(func).InnerDelegate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16, t17, t18, ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Action<T0> Uncurrify<T0>(FakesDelegates.Action action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate
			{
				new UnCurryActionClosure<T0>(action).InnerDelegate();
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.Func<T0, TResult> Uncurrify<T0, TResult>(FakesDelegates.Func<TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return (T0 t0) => new UnCurryFuncClosure<T0, TResult>(func).InnerDelegate();
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutAction<T0, TOut> Uncurrify<T0, TOut>(FakesDelegates.OutAction<TOut> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, out TOut @out)
			{
				new UnCurryOutActionClosure<T0, TOut>(action).InnerDelegate(out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutAction<T0, TOut1, TOut2> Uncurrify<T0, TOut1, TOut2>(FakesDelegates.OutOutAction<TOut1, TOut2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, out TOut1 out1, out TOut2 out2)
			{
				new UnCurryOutOutActionClosure<T0, TOut1, TOut2>(action).InnerDelegate(out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutFunc<T0, TOut, TResult> Uncurrify<T0, TOut, TResult>(FakesDelegates.OutFunc<TOut, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, out TOut @out)
			{
				return new UnCurryOutFuncClosure<T0, TOut, TResult>(func).InnerDelegate(out @out);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.OutOutFunc<T0, TOut1, TOut2, TResult> Uncurrify<T0, TOut1, TOut2, TResult>(FakesDelegates.OutOutFunc<TOut1, TOut2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, out TOut1 out1, out TOut2 out2)
			{
				return new UnCurryOutOutFuncClosure<T0, TOut1, TOut2, TResult>(func).InnerDelegate(out out1, out out2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefAction<T0, TRef> Uncurrify<T0, TRef>(FakesDelegates.RefAction<TRef> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, ref TRef @ref)
			{
				new UnCurryRefActionClosure<T0, TRef>(action).InnerDelegate(ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefAction<T0, TRef1, TRef2> Uncurrify<T0, TRef1, TRef2>(FakesDelegates.RefRefAction<TRef1, TRef2> action)
		{
			if (action == null)
			{
				return null;
			}
			return delegate(T0 t0, ref TRef1 ref1, ref TRef2 ref2)
			{
				new UnCurryRefRefActionClosure<T0, TRef1, TRef2>(action).InnerDelegate(ref ref1, ref ref2);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefFunc<T0, TRef, TResult> Uncurrify<T0, TRef, TResult>(FakesDelegates.RefFunc<TRef, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, ref TRef @ref)
			{
				return new UnCurryRefFuncClosure<T0, TRef, TResult>(func).InnerDelegate(ref @ref);
			};
		}

		[DebuggerNonUserCode]
		public static FakesDelegates.RefRefFunc<T0, TRef1, TRef2, TResult> Uncurrify<T0, TRef1, TRef2, TResult>(FakesDelegates.RefRefFunc<TRef1, TRef2, TResult> func)
		{
			if (func == null)
			{
				return null;
			}
			return delegate(T0 t0, ref TRef1 ref1, ref TRef2 ref2)
			{
				return new UnCurryRefRefFuncClosure<T0, TRef1, TRef2, TResult>(func).InnerDelegate(ref ref1, ref ref2);
			};
		}
	}
}
