using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace ProtoBuf.Serializers
{
	internal sealed class TypeSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		private readonly Type forType;

		private readonly Type constructType;

		private readonly IProtoSerializer[] serializers;

		private readonly int[] fieldNumbers;

		private readonly bool isRootType;

		private readonly bool useConstructor;

		private readonly bool isExtensible;

		private readonly bool hasConstructor;

		private readonly CallbackSet callbacks;

		private readonly MethodInfo[] baseCtorCallbacks;

		private readonly MethodInfo factory;

		private static readonly Type iextensible = typeof(IExtensible);

		public Type ExpectedType => forType;

		private bool CanHaveInheritance
		{
			get
			{
				if (forType.IsClass || forType.IsInterface)
				{
					return !forType.IsSealed;
				}
				return false;
			}
		}

		bool IProtoSerializer.RequiresOldValue => true;

		bool IProtoSerializer.ReturnsValue => false;

		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			if (callbacks != null && callbacks[callbackType] != null)
			{
				return true;
			}
			for (int i = 0; i < serializers.Length; i++)
			{
				if (serializers[i].ExpectedType != forType && ((IProtoTypeSerializer)serializers[i]).HasCallbacks(callbackType))
				{
					return true;
				}
			}
			return false;
		}

		public TypeSerializer(TypeModel model, Type forType, int[] fieldNumbers, IProtoSerializer[] serializers, MethodInfo[] baseCtorCallbacks, bool isRootType, bool useConstructor, CallbackSet callbacks, Type constructType, MethodInfo factory)
		{
			Helpers.Sort(fieldNumbers, serializers);
			bool hasSubTypes = false;
			for (int i = 0; i < fieldNumbers.Length; i++)
			{
				if (i != 0 && fieldNumbers[i] == fieldNumbers[i - 1])
				{
					throw new InvalidOperationException("Duplicate field-number detected; " + fieldNumbers[i] + " on: " + forType.FullName);
				}
				if (!hasSubTypes && serializers[i].ExpectedType != forType)
				{
					hasSubTypes = true;
				}
			}
			this.forType = forType;
			this.factory = factory;
			if (constructType == null)
			{
				constructType = forType;
			}
			else if (!forType.IsAssignableFrom(constructType))
			{
				throw new InvalidOperationException(forType.FullName + " cannot be assigned from " + constructType.FullName);
			}
			this.constructType = constructType;
			this.serializers = serializers;
			this.fieldNumbers = fieldNumbers;
			this.callbacks = callbacks;
			this.isRootType = isRootType;
			this.useConstructor = useConstructor;
			if (baseCtorCallbacks != null && baseCtorCallbacks.Length == 0)
			{
				baseCtorCallbacks = null;
			}
			this.baseCtorCallbacks = baseCtorCallbacks;
			if (Helpers.GetUnderlyingType(forType) != null)
			{
				throw new ArgumentException("Cannot create a TypeSerializer for nullable types", "forType");
			}
			if (model.MapType(iextensible).IsAssignableFrom(forType))
			{
				if (forType.IsValueType || !isRootType || hasSubTypes)
				{
					throw new NotSupportedException("IExtensible is not supported in structs or classes with inheritance");
				}
				isExtensible = true;
			}
			hasConstructor = (!constructType.IsAbstract && Helpers.GetConstructor(constructType, Helpers.EmptyTypes, nonPublic: true) != null);
			if (constructType != forType && useConstructor && !hasConstructor)
			{
				throw new ArgumentException("The supplied default implementation cannot be created: " + constructType.FullName, "constructType");
			}
		}

		bool IProtoTypeSerializer.CanCreateInstance()
		{
			return true;
		}

		object IProtoTypeSerializer.CreateInstance(ProtoReader source)
		{
			return CreateInstance(source, includeLocalCallback: false);
		}

		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			if (callbacks != null)
			{
				InvokeCallback(callbacks[callbackType], value, context);
			}
			((IProtoTypeSerializer)GetMoreSpecificSerializer(value))?.Callback(value, callbackType, context);
		}

		private IProtoSerializer GetMoreSpecificSerializer(object value)
		{
			if (!CanHaveInheritance)
			{
				return null;
			}
			Type actualType = value.GetType();
			if (actualType == forType)
			{
				return null;
			}
			for (int i = 0; i < serializers.Length; i++)
			{
				IProtoSerializer ser = serializers[i];
				if (ser.ExpectedType != forType && Helpers.IsAssignableFrom(ser.ExpectedType, actualType))
				{
					return ser;
				}
			}
			if (actualType == constructType)
			{
				return null;
			}
			TypeModel.ThrowUnexpectedSubtype(forType, actualType);
			return null;
		}

		public void Write(object value, ProtoWriter dest)
		{
			if (isRootType)
			{
				Callback(value, TypeModel.CallbackType.BeforeSerialize, dest.Context);
			}
			GetMoreSpecificSerializer(value)?.Write(value, dest);
			for (int i = 0; i < serializers.Length; i++)
			{
				IProtoSerializer ser = serializers[i];
				if (ser.ExpectedType == forType)
				{
					ser.Write(value, dest);
				}
			}
			if (isExtensible)
			{
				ProtoWriter.AppendExtensionData((IExtensible)value, dest);
			}
			if (isRootType)
			{
				Callback(value, TypeModel.CallbackType.AfterSerialize, dest.Context);
			}
		}

		public object Read(object value, ProtoReader source)
		{
			if (isRootType && value != null)
			{
				Callback(value, TypeModel.CallbackType.BeforeDeserialize, source.Context);
			}
			int lastFieldNumber = 0;
			int lastFieldIndex = 0;
			int fieldNumber;
			while ((fieldNumber = source.ReadFieldHeader()) > 0)
			{
				bool fieldHandled = false;
				if (fieldNumber < lastFieldNumber)
				{
					lastFieldNumber = (lastFieldIndex = 0);
				}
				for (int i = lastFieldIndex; i < fieldNumbers.Length; i++)
				{
					if (fieldNumbers[i] != fieldNumber)
					{
						continue;
					}
					IProtoSerializer ser = serializers[i];
					Type serType = ser.ExpectedType;
					if (value == null)
					{
						if (serType == forType)
						{
							value = CreateInstance(source, includeLocalCallback: true);
						}
					}
					else if (serType != forType && ((IProtoTypeSerializer)ser).CanCreateInstance() && serType.IsSubclassOf(value.GetType()))
					{
						value = ProtoReader.Merge(source, value, ((IProtoTypeSerializer)ser).CreateInstance(source));
					}
					if (ser.ReturnsValue)
					{
						value = ser.Read(value, source);
					}
					else
					{
						ser.Read(value, source);
					}
					lastFieldIndex = i;
					lastFieldNumber = fieldNumber;
					fieldHandled = true;
					break;
				}
				if (!fieldHandled)
				{
					if (value == null)
					{
						value = CreateInstance(source, includeLocalCallback: true);
					}
					if (isExtensible)
					{
						source.AppendExtensionData((IExtensible)value);
					}
					else
					{
						source.SkipField();
					}
				}
			}
			if (value == null)
			{
				value = CreateInstance(source, includeLocalCallback: true);
			}
			if (isRootType)
			{
				Callback(value, TypeModel.CallbackType.AfterDeserialize, source.Context);
			}
			return value;
		}

		private object InvokeCallback(MethodInfo method, object obj, SerializationContext context)
		{
			object result = null;
			if (method != null)
			{
				ParameterInfo[] parameters = method.GetParameters();
				object[] args;
				bool handled;
				if (parameters.Length == 0)
				{
					args = null;
					handled = true;
				}
				else
				{
					args = new object[parameters.Length];
					handled = true;
					for (int i = 0; i < args.Length; i++)
					{
						Type paramType = parameters[i].ParameterType;
						object val;
						if (paramType == typeof(SerializationContext))
						{
							val = context;
						}
						else if (paramType == typeof(Type))
						{
							val = constructType;
						}
						else if (paramType == typeof(StreamingContext))
						{
							val = (StreamingContext)context;
						}
						else
						{
							val = null;
							handled = false;
						}
						args[i] = val;
					}
				}
				if (!handled)
				{
					throw CallbackSet.CreateInvalidCallbackSignature(method);
				}
				result = method.Invoke(obj, args);
			}
			return result;
		}

		private object CreateInstance(ProtoReader source, bool includeLocalCallback)
		{
			object obj;
			if (factory != null)
			{
				obj = InvokeCallback(factory, null, source.Context);
			}
			else if (useConstructor)
			{
				if (!hasConstructor)
				{
					TypeModel.ThrowCannotCreateInstance(constructType);
				}
				obj = Activator.CreateInstance(constructType, nonPublic: true);
			}
			else
			{
				obj = BclHelpers.GetUninitializedObject(constructType);
			}
			ProtoReader.NoteObject(obj, source);
			if (baseCtorCallbacks != null)
			{
				for (int i = 0; i < baseCtorCallbacks.Length; i++)
				{
					InvokeCallback(baseCtorCallbacks[i], obj, source.Context);
				}
			}
			if (includeLocalCallback && callbacks != null)
			{
				InvokeCallback(callbacks.BeforeDeserialize, obj, source.Context);
			}
			return obj;
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			Type expected = ExpectedType;
			using (Local loc = ctx.GetLocalWithValue(expected, valueFrom))
			{
				EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeSerialize);
				CodeLabel startFields = ctx.DefineLabel();
				if (CanHaveInheritance)
				{
					for (int j = 0; j < serializers.Length; j++)
					{
						IProtoSerializer ser2 = serializers[j];
						Type serType = ser2.ExpectedType;
						if (serType != forType)
						{
							CodeLabel ifMatch = ctx.DefineLabel();
							CodeLabel nextTest = ctx.DefineLabel();
							ctx.LoadValue(loc);
							ctx.TryCast(serType);
							ctx.CopyValue();
							ctx.BranchIfTrue(ifMatch, @short: true);
							ctx.DiscardValue();
							ctx.Branch(nextTest, @short: true);
							ctx.MarkLabel(ifMatch);
							ser2.EmitWrite(ctx, null);
							ctx.Branch(startFields, @short: false);
							ctx.MarkLabel(nextTest);
						}
					}
					if (constructType != null && constructType != forType)
					{
						using (Local actualType = new Local(ctx, ctx.MapType(typeof(Type))))
						{
							ctx.LoadValue(loc);
							ctx.EmitCall(ctx.MapType(typeof(object)).GetMethod("GetType"));
							ctx.CopyValue();
							ctx.StoreValue(actualType);
							ctx.LoadValue(forType);
							ctx.BranchIfEqual(startFields, @short: true);
							ctx.LoadValue(actualType);
							ctx.LoadValue(constructType);
							ctx.BranchIfEqual(startFields, @short: true);
						}
					}
					else
					{
						ctx.LoadValue(loc);
						ctx.EmitCall(ctx.MapType(typeof(object)).GetMethod("GetType"));
						ctx.LoadValue(forType);
						ctx.BranchIfEqual(startFields, @short: true);
					}
					ctx.LoadValue(forType);
					ctx.LoadValue(loc);
					ctx.EmitCall(ctx.MapType(typeof(object)).GetMethod("GetType"));
					ctx.EmitCall(ctx.MapType(typeof(TypeModel)).GetMethod("ThrowUnexpectedSubtype", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
				}
				ctx.MarkLabel(startFields);
				for (int i = 0; i < serializers.Length; i++)
				{
					IProtoSerializer ser = serializers[i];
					if (ser.ExpectedType == forType)
					{
						ser.EmitWrite(ctx, loc);
					}
				}
				if (isExtensible)
				{
					ctx.LoadValue(loc);
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("AppendExtensionData"));
				}
				EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.AfterSerialize);
			}
		}

		private static void EmitInvokeCallback(CompilerContext ctx, MethodInfo method, bool copyValue, Type constructType, Type type)
		{
			if (!(method != null))
			{
				return;
			}
			if (copyValue)
			{
				ctx.CopyValue();
			}
			ParameterInfo[] parameters = method.GetParameters();
			bool handled = true;
			for (int i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				if (parameterType == ctx.MapType(typeof(SerializationContext)))
				{
					ctx.LoadSerializationContext();
				}
				else if (parameterType == ctx.MapType(typeof(Type)))
				{
					Type tmp = constructType;
					if (tmp == null)
					{
						tmp = type;
					}
					ctx.LoadValue(tmp);
				}
				else if (parameterType == ctx.MapType(typeof(StreamingContext)))
				{
					ctx.LoadSerializationContext();
					MethodInfo op = ctx.MapType(typeof(SerializationContext)).GetMethod("op_Implicit", new Type[1]
					{
						ctx.MapType(typeof(SerializationContext))
					});
					if (op != null)
					{
						ctx.EmitCall(op);
						handled = true;
					}
				}
				else
				{
					handled = false;
				}
			}
			if (handled)
			{
				ctx.EmitCall(method);
				if (constructType != null && method.ReturnType == ctx.MapType(typeof(object)))
				{
					ctx.CastFromObject(type);
				}
				return;
			}
			throw CallbackSet.CreateInvalidCallbackSignature(method);
		}

		private void EmitCallbackIfNeeded(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
			if (isRootType && ((IProtoTypeSerializer)this).HasCallbacks(callbackType))
			{
				((IProtoTypeSerializer)this).EmitCallback(ctx, valueFrom, callbackType);
			}
		}

		void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
			bool actuallyHasInheritance = false;
			if (CanHaveInheritance)
			{
				for (int j = 0; j < serializers.Length; j++)
				{
					IProtoSerializer ser2 = serializers[j];
					if (ser2.ExpectedType != forType && ((IProtoTypeSerializer)ser2).HasCallbacks(callbackType))
					{
						actuallyHasInheritance = true;
					}
				}
			}
			MethodInfo method = (callbacks == null) ? null : callbacks[callbackType];
			if (method == null && !actuallyHasInheritance)
			{
				return;
			}
			ctx.LoadAddress(valueFrom, ExpectedType);
			EmitInvokeCallback(ctx, method, actuallyHasInheritance, null, forType);
			if (!actuallyHasInheritance)
			{
				return;
			}
			CodeLabel @break = ctx.DefineLabel();
			for (int i = 0; i < serializers.Length; i++)
			{
				IProtoSerializer ser = serializers[i];
				Type serType = ser.ExpectedType;
				IProtoTypeSerializer typeser;
				if (serType != forType && (typeser = (IProtoTypeSerializer)ser).HasCallbacks(callbackType))
				{
					CodeLabel ifMatch = ctx.DefineLabel();
					CodeLabel nextTest = ctx.DefineLabel();
					ctx.CopyValue();
					ctx.TryCast(serType);
					ctx.CopyValue();
					ctx.BranchIfTrue(ifMatch, @short: true);
					ctx.DiscardValue();
					ctx.Branch(nextTest, @short: false);
					ctx.MarkLabel(ifMatch);
					typeser.EmitCallback(ctx, null, callbackType);
					ctx.Branch(@break, @short: false);
					ctx.MarkLabel(nextTest);
				}
			}
			ctx.MarkLabel(@break);
			ctx.DiscardValue();
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			Type expected = ExpectedType;
			using (Local loc = ctx.GetLocalWithValue(expected, valueFrom))
			{
				using (Local fieldNumber = new Local(ctx, ctx.MapType(typeof(int))))
				{
					if (HasCallbacks(TypeModel.CallbackType.BeforeDeserialize))
					{
						if (Helpers.IsValueType(ExpectedType))
						{
							EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeDeserialize);
						}
						else
						{
							CodeLabel callbacksDone = ctx.DefineLabel();
							ctx.LoadValue(loc);
							ctx.BranchIfFalse(callbacksDone, @short: false);
							EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.BeforeDeserialize);
							ctx.MarkLabel(callbacksDone);
						}
					}
					CodeLabel @continue = ctx.DefineLabel();
					CodeLabel processField = ctx.DefineLabel();
					ctx.Branch(@continue, @short: false);
					ctx.MarkLabel(processField);
					BasicList.NodeEnumerator enumerator = BasicList.GetContiguousGroups(fieldNumbers, serializers).GetEnumerator();
					while (enumerator.MoveNext())
					{
						BasicList.Group group = (BasicList.Group)enumerator.Current;
						CodeLabel tryNextField = ctx.DefineLabel();
						int groupItemCount = group.Items.Count;
						if (groupItemCount == 1)
						{
							ctx.LoadValue(fieldNumber);
							ctx.LoadValue(group.First);
							CodeLabel processThisField = ctx.DefineLabel();
							ctx.BranchIfEqual(processThisField, @short: true);
							ctx.Branch(tryNextField, @short: false);
							WriteFieldHandler(ctx, expected, loc, processThisField, @continue, (IProtoSerializer)group.Items[0]);
						}
						else
						{
							ctx.LoadValue(fieldNumber);
							ctx.LoadValue(group.First);
							ctx.Subtract();
							CodeLabel[] jmp = new CodeLabel[groupItemCount];
							for (int j = 0; j < groupItemCount; j++)
							{
								jmp[j] = ctx.DefineLabel();
							}
							ctx.Switch(jmp);
							ctx.Branch(tryNextField, @short: false);
							for (int i = 0; i < groupItemCount; i++)
							{
								WriteFieldHandler(ctx, expected, loc, jmp[i], @continue, (IProtoSerializer)group.Items[i]);
							}
						}
						ctx.MarkLabel(tryNextField);
					}
					EmitCreateIfNull(ctx, loc);
					ctx.LoadReaderWriter();
					if (isExtensible)
					{
						ctx.LoadValue(loc);
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("AppendExtensionData"));
					}
					else
					{
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("SkipField"));
					}
					ctx.MarkLabel(@continue);
					ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof(int)));
					ctx.CopyValue();
					ctx.StoreValue(fieldNumber);
					ctx.LoadValue(0);
					ctx.BranchIfGreater(processField, @short: false);
					EmitCreateIfNull(ctx, loc);
					EmitCallbackIfNeeded(ctx, loc, TypeModel.CallbackType.AfterDeserialize);
					if (valueFrom != null && !loc.IsSame(valueFrom))
					{
						ctx.LoadValue(loc);
						ctx.Cast(valueFrom.Type);
						ctx.StoreValue(valueFrom);
					}
				}
			}
		}

		private void WriteFieldHandler(CompilerContext ctx, Type expected, Local loc, CodeLabel handler, CodeLabel @continue, IProtoSerializer serializer)
		{
			ctx.MarkLabel(handler);
			Type serType = serializer.ExpectedType;
			if (serType == forType)
			{
				EmitCreateIfNull(ctx, loc);
				serializer.EmitRead(ctx, loc);
			}
			else
			{
				if (((IProtoTypeSerializer)serializer).CanCreateInstance())
				{
					CodeLabel allDone = ctx.DefineLabel();
					ctx.LoadValue(loc);
					ctx.BranchIfFalse(allDone, @short: false);
					ctx.LoadValue(loc);
					ctx.TryCast(serType);
					ctx.BranchIfTrue(allDone, @short: false);
					ctx.LoadReaderWriter();
					ctx.LoadValue(loc);
					((IProtoTypeSerializer)serializer).EmitCreateInstance(ctx);
					ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("Merge"));
					ctx.Cast(expected);
					ctx.StoreValue(loc);
					ctx.MarkLabel(allDone);
				}
				ctx.LoadValue(loc);
				ctx.Cast(serType);
				serializer.EmitRead(ctx, null);
			}
			if (serializer.ReturnsValue)
			{
				ctx.StoreValue(loc);
			}
			ctx.Branch(@continue, @short: false);
		}

		void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
		{
			bool callNoteObject = true;
			if (factory != null)
			{
				EmitInvokeCallback(ctx, factory, copyValue: false, constructType, forType);
			}
			else if (!useConstructor)
			{
				ctx.LoadValue(constructType);
				ctx.EmitCall(ctx.MapType(typeof(BclHelpers)).GetMethod("GetUninitializedObject"));
				ctx.Cast(forType);
			}
			else if (Helpers.IsClass(constructType) && hasConstructor)
			{
				ctx.EmitCtor(constructType);
			}
			else
			{
				ctx.LoadValue(ExpectedType);
				ctx.EmitCall(ctx.MapType(typeof(TypeModel)).GetMethod("ThrowCannotCreateInstance", BindingFlags.Static | BindingFlags.Public));
				ctx.LoadNullRef();
				callNoteObject = false;
			}
			if (callNoteObject)
			{
				ctx.CopyValue();
				ctx.LoadReaderWriter();
				ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("NoteObject", BindingFlags.Static | BindingFlags.Public));
			}
			if (baseCtorCallbacks != null)
			{
				for (int i = 0; i < baseCtorCallbacks.Length; i++)
				{
					EmitInvokeCallback(ctx, baseCtorCallbacks[i], copyValue: true, null, forType);
				}
			}
		}

		private void EmitCreateIfNull(CompilerContext ctx, Local storage)
		{
			if (!Helpers.IsValueType(ExpectedType))
			{
				CodeLabel afterNullCheck = ctx.DefineLabel();
				ctx.LoadValue(storage);
				ctx.BranchIfTrue(afterNullCheck, @short: false);
				((IProtoTypeSerializer)this).EmitCreateInstance(ctx);
				if (callbacks != null)
				{
					EmitInvokeCallback(ctx, callbacks.BeforeDeserialize, copyValue: true, null, forType);
				}
				ctx.StoreValue(storage);
				ctx.MarkLabel(afterNullCheck);
			}
		}
	}
}
