namespace Microsoft.QualityTools.Testing.Fakes
{
	public static class FakesDelegates
	{
		public delegate void Action();

		public delegate void Action<T1>(T1 arg1);

		public delegate void Action<T1, T2>(T1 arg1, T2 arg2);

		public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);

		public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		public delegate void Action<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		public delegate void Action<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);

		public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19);

		public delegate void OutAction<TOut>(out TOut @out);

		public delegate void OutOutAction<TOut1, TOut2>(out TOut1 @out, out TOut2 out2);

		public delegate void OutAction<T1, TOut>(T1 arg1, out TOut @out);

		public delegate void OutOutAction<T1, TOut1, TOut2>(T1 arg1, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, TOut>(T1 arg1, T2 arg2, out TOut @out);

		public delegate void OutOutAction<T1, T2, TOut1, TOut2>(T1 arg1, T2 arg2, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, TOut>(T1 arg1, T2 arg2, T3 arg3, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, out TOut1 out1, out TOut2 out2);

		public delegate void OutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TOut>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, out TOut @out);

		public delegate void OutOutAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TOut1, TOut2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, out TOut1 out1, out TOut2 out2);

		public delegate void RefAction<TRef>(ref TRef @ref);

		public delegate void RefRefAction<TRef1, TRef2>(ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, TRef>(T1 arg1, ref TRef @ref);

		public delegate void RefRefAction<T1, TRef1, TRef2>(T1 arg1, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, TRef>(T1 arg1, T2 arg2, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, TRef1, TRef2>(T1 arg1, T2 arg2, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, TRef>(T1 arg1, T2 arg2, T3 arg3, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, ref TRef1 ref1, ref TRef2 ref2);

		public delegate void RefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TRef>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, ref TRef @ref);

		public delegate void RefRefAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TRef1, TRef2>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult Func<TResult>();

		public delegate TResult Func<T1, TResult>(T1 arg1);

		public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);

		public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);

		public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

		public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);

		public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19);

		public delegate TResult OutFunc<TOut, TResult>(out TOut @out);

		public delegate TResult OutOutFunc<TOut1, TOut2, TResult>(out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, TOut, TResult>(T1 arg1, out TOut @out);

		public delegate TResult OutOutFunc<T1, TOut1, TOut2, TResult>(T1 arg1, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, TOut, TResult>(T1 arg1, T2 arg2, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, out TOut1 out1, out TOut2 out2);

		public delegate TResult OutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TOut, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, out TOut @out);

		public delegate TResult OutOutFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TOut1, TOut2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, out TOut1 out1, out TOut2 out2);

		public delegate TResult RefFunc<TRef, TResult>(ref TRef @ref);

		public delegate TResult RefRefFunc<TRef1, TRef2, TResult>(ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, TRef, TResult>(T1 arg1, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, TRef1, TRef2, TResult>(T1 arg1, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, TRef, TResult>(T1 arg1, T2 arg2, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, ref TRef1 ref1, ref TRef2 ref2);

		public delegate TResult RefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TRef, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, ref TRef @ref);

		public delegate TResult RefRefFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TRef1, TRef2, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, ref TRef1 ref1, ref TRef2 ref2);
	}
}
