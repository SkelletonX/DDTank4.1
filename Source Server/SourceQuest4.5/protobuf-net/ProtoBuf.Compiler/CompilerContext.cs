using ProtoBuf.Meta;
using ProtoBuf.Serializers;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ProtoBuf.Compiler
{
	internal sealed class CompilerContext
	{
		private sealed class UsingBlock : IDisposable
		{
			private Local local;

			private CompilerContext ctx;

			private CodeLabel label;

			public UsingBlock(CompilerContext ctx, Local local)
			{
				if (ctx == null)
				{
					throw new ArgumentNullException("ctx");
				}
				if (local == null)
				{
					throw new ArgumentNullException("local");
				}
				Type type = local.Type;
				if ((!Helpers.IsValueType(type) && !Helpers.IsSealed(type)) || ctx.MapType(typeof(IDisposable)).IsAssignableFrom(type))
				{
					this.local = local;
					this.ctx = ctx;
					label = ctx.BeginTry();
				}
			}

			public void Dispose()
			{
				if (local == null || ctx == null)
				{
					return;
				}
				ctx.EndTry(label, @short: false);
				ctx.BeginFinally();
				Type disposableType = ctx.MapType(typeof(IDisposable));
				MethodInfo dispose = disposableType.GetMethod("Dispose");
				Type type = local.Type;
				if (Helpers.IsValueType(type))
				{
					ctx.LoadAddress(local, type);
					if (ctx.MetadataVersion == ILVersion.Net1)
					{
						ctx.LoadValue(local);
						ctx.CastToObject(type);
					}
					else
					{
						ctx.Constrain(type);
					}
					ctx.EmitCall(dispose);
				}
				else
				{
					CodeLabel @null = ctx.DefineLabel();
					if (disposableType.IsAssignableFrom(type))
					{
						ctx.LoadValue(local);
						ctx.BranchIfFalse(@null, @short: true);
						ctx.LoadAddress(local, type);
					}
					else
					{
						using (Local disp = new Local(ctx, disposableType))
						{
							ctx.LoadValue(local);
							ctx.TryCast(disposableType);
							ctx.CopyValue();
							ctx.StoreValue(disp);
							ctx.BranchIfFalse(@null, @short: true);
							ctx.LoadAddress(disp, disposableType);
						}
					}
					ctx.EmitCall(dispose);
					ctx.MarkLabel(@null);
				}
				ctx.EndFinally();
				local = null;
				ctx = null;
				label = default(CodeLabel);
			}
		}

		public enum ILVersion
		{
			Net1,
			Net2
		}

		private readonly DynamicMethod method;

		private static int next;

		private readonly bool isStatic;

		private readonly RuntimeTypeModel.SerializerPair[] methodPairs;

		private readonly bool isWriter;

		private readonly bool nonPublic;

		private readonly Local inputValue;

		private readonly string assemblyName;

		private readonly ILGenerator il;

		private MutableList locals = new MutableList();

		private int nextLabel;

		private BasicList knownTrustedAssemblies;

		private BasicList knownUntrustedAssemblies;

		private readonly TypeModel model;

		private readonly ILVersion metadataVersion;

		public TypeModel Model => model;

		internal bool NonPublic => nonPublic;

		public Local InputValue => inputValue;

		public ILVersion MetadataVersion => metadataVersion;

		internal CodeLabel DefineLabel()
		{
			CodeLabel result = new CodeLabel(il.DefineLabel(), nextLabel++);
			return result;
		}

		[Conditional("DEBUG_COMPILE")]
		private void TraceCompile(string value)
		{
		}

		internal void MarkLabel(CodeLabel label)
		{
			il.MarkLabel(label.Value);
		}

		public static ProtoSerializer BuildSerializer(IProtoSerializer head, TypeModel model)
		{
			Type type = head.ExpectedType;
			try
			{
				CompilerContext ctx = new CompilerContext(type, isWriter: true, isStatic: true, model, typeof(object));
				ctx.LoadValue(ctx.InputValue);
				ctx.CastFromObject(type);
				ctx.WriteNullCheckedTail(type, head, null);
				ctx.Emit(OpCodes.Ret);
				return (ProtoSerializer)ctx.method.CreateDelegate(typeof(ProtoSerializer));
			}
			catch (Exception ex)
			{
				string name = type.FullName;
				if (string.IsNullOrEmpty(name))
				{
					name = type.Name;
				}
				throw new InvalidOperationException("It was not possible to prepare a serializer for: " + name, ex);
			}
		}

		public static ProtoDeserializer BuildDeserializer(IProtoSerializer head, TypeModel model)
		{
			Type type = head.ExpectedType;
			CompilerContext ctx = new CompilerContext(type, isWriter: false, isStatic: true, model, typeof(object));
			using (Local typedVal = new Local(ctx, type))
			{
				if (!Helpers.IsValueType(type))
				{
					ctx.LoadValue(ctx.InputValue);
					ctx.CastFromObject(type);
					ctx.StoreValue(typedVal);
				}
				else
				{
					ctx.LoadValue(ctx.InputValue);
					CodeLabel notNull = ctx.DefineLabel();
					CodeLabel endNull = ctx.DefineLabel();
					ctx.BranchIfTrue(notNull, @short: true);
					ctx.LoadAddress(typedVal, type);
					ctx.EmitCtor(type);
					ctx.Branch(endNull, @short: true);
					ctx.MarkLabel(notNull);
					ctx.LoadValue(ctx.InputValue);
					ctx.CastFromObject(type);
					ctx.StoreValue(typedVal);
					ctx.MarkLabel(endNull);
				}
				head.EmitRead(ctx, typedVal);
				if (head.ReturnsValue)
				{
					ctx.StoreValue(typedVal);
				}
				ctx.LoadValue(typedVal);
				ctx.CastToObject(type);
			}
			ctx.Emit(OpCodes.Ret);
			return (ProtoDeserializer)ctx.method.CreateDelegate(typeof(ProtoDeserializer));
		}

		internal void Return()
		{
			Emit(OpCodes.Ret);
		}

		private static bool IsObject(Type type)
		{
			return type == typeof(object);
		}

		internal void CastToObject(Type type)
		{
			if (!IsObject(type))
			{
				if (Helpers.IsValueType(type))
				{
					il.Emit(OpCodes.Box, type);
				}
				else
				{
					il.Emit(OpCodes.Castclass, MapType(typeof(object)));
				}
			}
		}

		internal void CastFromObject(Type type)
		{
			if (IsObject(type))
			{
				return;
			}
			if (Helpers.IsValueType(type))
			{
				if (MetadataVersion == ILVersion.Net1)
				{
					il.Emit(OpCodes.Unbox, type);
					il.Emit(OpCodes.Ldobj, type);
				}
				else
				{
					il.Emit(OpCodes.Unbox_Any, type);
				}
			}
			else
			{
				il.Emit(OpCodes.Castclass, type);
			}
		}

		internal MethodBuilder GetDedicatedMethod(int metaKey, bool read)
		{
			if (methodPairs == null)
			{
				return null;
			}
			for (int i = 0; i < methodPairs.Length; i++)
			{
				if (methodPairs[i].MetaKey == metaKey)
				{
					if (!read)
					{
						return methodPairs[i].Serialize;
					}
					return methodPairs[i].Deserialize;
				}
			}
			throw new ArgumentException("Meta-key not found", "metaKey");
		}

		internal int MapMetaKeyToCompiledKey(int metaKey)
		{
			if (metaKey < 0 || methodPairs == null)
			{
				return metaKey;
			}
			for (int i = 0; i < methodPairs.Length; i++)
			{
				if (methodPairs[i].MetaKey == metaKey)
				{
					return i;
				}
			}
			throw new ArgumentException("Key could not be mapped: " + metaKey, "metaKey");
		}

		internal CompilerContext(ILGenerator il, bool isStatic, bool isWriter, RuntimeTypeModel.SerializerPair[] methodPairs, TypeModel model, ILVersion metadataVersion, string assemblyName, Type inputType, string traceName)
		{
			if (il == null)
			{
				throw new ArgumentNullException("il");
			}
			if (methodPairs == null)
			{
				throw new ArgumentNullException("methodPairs");
			}
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (Helpers.IsNullOrEmpty(assemblyName))
			{
				throw new ArgumentNullException("assemblyName");
			}
			this.assemblyName = assemblyName;
			this.isStatic = isStatic;
			this.methodPairs = methodPairs;
			this.il = il;
			this.isWriter = isWriter;
			this.model = model;
			this.metadataVersion = metadataVersion;
			if (inputType != null)
			{
				inputValue = new Local(null, inputType);
			}
		}

		private CompilerContext(Type associatedType, bool isWriter, bool isStatic, TypeModel model, Type inputType)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			metadataVersion = ILVersion.Net2;
			this.isStatic = isStatic;
			this.isWriter = isWriter;
			this.model = model;
			nonPublic = true;
			Type returnType;
			Type[] paramTypes;
			if (isWriter)
			{
				returnType = typeof(void);
				paramTypes = new Type[2]
				{
					typeof(object),
					typeof(ProtoWriter)
				};
			}
			else
			{
				returnType = typeof(object);
				paramTypes = new Type[2]
				{
					typeof(object),
					typeof(ProtoReader)
				};
			}
			method = new DynamicMethod("proto_" + Interlocked.Increment(ref next), returnType, paramTypes, associatedType.IsInterface ? typeof(object) : associatedType, skipVisibility: true);
			il = method.GetILGenerator();
			if (inputType != null)
			{
				inputValue = new Local(null, inputType);
			}
		}

		private void Emit(OpCode opcode)
		{
			il.Emit(opcode);
		}

		public void LoadValue(string value)
		{
			if (value == null)
			{
				LoadNullRef();
			}
			else
			{
				il.Emit(OpCodes.Ldstr, value);
			}
		}

		public void LoadValue(float value)
		{
			il.Emit(OpCodes.Ldc_R4, value);
		}

		public void LoadValue(double value)
		{
			il.Emit(OpCodes.Ldc_R8, value);
		}

		public void LoadValue(long value)
		{
			il.Emit(OpCodes.Ldc_I8, value);
		}

		public void LoadValue(int value)
		{
			switch (value)
			{
			case 0:
				Emit(OpCodes.Ldc_I4_0);
				return;
			case 1:
				Emit(OpCodes.Ldc_I4_1);
				return;
			case 2:
				Emit(OpCodes.Ldc_I4_2);
				return;
			case 3:
				Emit(OpCodes.Ldc_I4_3);
				return;
			case 4:
				Emit(OpCodes.Ldc_I4_4);
				return;
			case 5:
				Emit(OpCodes.Ldc_I4_5);
				return;
			case 6:
				Emit(OpCodes.Ldc_I4_6);
				return;
			case 7:
				Emit(OpCodes.Ldc_I4_7);
				return;
			case 8:
				Emit(OpCodes.Ldc_I4_8);
				return;
			case -1:
				Emit(OpCodes.Ldc_I4_M1);
				return;
			}
			if (value >= -128 && value <= 127)
			{
				il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4, value);
			}
		}

		internal LocalBuilder GetFromPool(Type type)
		{
			int count = locals.Count;
			for (int i = 0; i < count; i++)
			{
				LocalBuilder item = (LocalBuilder)locals[i];
				if (item != null && item.LocalType == type)
				{
					locals[i] = null;
					return item;
				}
			}
			return il.DeclareLocal(type);
		}

		internal void ReleaseToPool(LocalBuilder value)
		{
			int count = locals.Count;
			for (int i = 0; i < count; i++)
			{
				if (locals[i] == null)
				{
					locals[i] = value;
					return;
				}
			}
			locals.Add(value);
		}

		public void LoadReaderWriter()
		{
			Emit(isStatic ? OpCodes.Ldarg_1 : OpCodes.Ldarg_2);
		}

		public void StoreValue(Local local)
		{
			if (local == InputValue)
			{
				byte b = (byte)((!isStatic) ? 1 : 0);
				il.Emit(OpCodes.Starg_S, b);
				return;
			}
			switch (local.Value.LocalIndex)
			{
			case 0:
				Emit(OpCodes.Stloc_0);
				break;
			case 1:
				Emit(OpCodes.Stloc_1);
				break;
			case 2:
				Emit(OpCodes.Stloc_2);
				break;
			case 3:
				Emit(OpCodes.Stloc_3);
				break;
			default:
			{
				OpCode code = UseShortForm(local) ? OpCodes.Stloc_S : OpCodes.Stloc;
				il.Emit(code, local.Value);
				break;
			}
			}
		}

		public void LoadValue(Local local)
		{
			if (local == null)
			{
				return;
			}
			if (local == InputValue)
			{
				Emit(isStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
				return;
			}
			switch (local.Value.LocalIndex)
			{
			case 0:
				Emit(OpCodes.Ldloc_0);
				break;
			case 1:
				Emit(OpCodes.Ldloc_1);
				break;
			case 2:
				Emit(OpCodes.Ldloc_2);
				break;
			case 3:
				Emit(OpCodes.Ldloc_3);
				break;
			default:
			{
				OpCode code = UseShortForm(local) ? OpCodes.Ldloc_S : OpCodes.Ldloc;
				il.Emit(code, local.Value);
				break;
			}
			}
		}

		public Local GetLocalWithValue(Type type, Local fromValue)
		{
			if (fromValue != null)
			{
				if (fromValue.Type == type)
				{
					return fromValue.AsCopy();
				}
				LoadValue(fromValue);
				if (!Helpers.IsValueType(type) && (fromValue.Type == null || !type.IsAssignableFrom(fromValue.Type)))
				{
					Cast(type);
				}
			}
			Local result = new Local(this, type);
			StoreValue(result);
			return result;
		}

		internal void EmitBasicRead(string methodName, Type expectedType)
		{
			MethodInfo method = MapType(typeof(ProtoReader)).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null || method.ReturnType != expectedType || method.GetParameters().Length != 0)
			{
				throw new ArgumentException("methodName");
			}
			LoadReaderWriter();
			EmitCall(method);
		}

		internal void EmitBasicRead(Type helperType, string methodName, Type expectedType)
		{
			MethodInfo method = helperType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null || method.ReturnType != expectedType || method.GetParameters().Length != 1)
			{
				throw new ArgumentException("methodName");
			}
			LoadReaderWriter();
			EmitCall(method);
		}

		internal void EmitBasicWrite(string methodName, Local fromValue)
		{
			if (Helpers.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}
			LoadValue(fromValue);
			LoadReaderWriter();
			EmitCall(GetWriterMethod(methodName));
		}

		private MethodInfo GetWriterMethod(string methodName)
		{
			Type writerType = MapType(typeof(ProtoWriter));
			MethodInfo[] methods = writerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			MethodInfo[] array = methods;
			foreach (MethodInfo method in array)
			{
				if (!(method.Name != methodName))
				{
					ParameterInfo[] pis = method.GetParameters();
					if (pis.Length == 2 && pis[1].ParameterType == writerType)
					{
						return method;
					}
				}
			}
			throw new ArgumentException("No suitable method found for: " + methodName, "methodName");
		}

		internal void EmitWrite(Type helperType, string methodName, Local valueFrom)
		{
			if (Helpers.IsNullOrEmpty(methodName))
			{
				throw new ArgumentNullException("methodName");
			}
			MethodInfo method = helperType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (method == null || method.ReturnType != MapType(typeof(void)))
			{
				throw new ArgumentException("methodName");
			}
			LoadValue(valueFrom);
			LoadReaderWriter();
			EmitCall(method);
		}

		public void EmitCall(MethodInfo method)
		{
			EmitCall(method, null);
		}

		public void EmitCall(MethodInfo method, Type targetType)
		{
			MemberInfo member = method;
			CheckAccessibility(ref member);
			OpCode opcode;
			if (method.IsStatic || Helpers.IsValueType(method.DeclaringType))
			{
				opcode = OpCodes.Call;
			}
			else
			{
				opcode = OpCodes.Callvirt;
				if (targetType != null && Helpers.IsValueType(targetType) && !Helpers.IsValueType(method.DeclaringType))
				{
					Constrain(targetType);
				}
			}
			il.EmitCall(opcode, method, null);
		}

		public void LoadNullRef()
		{
			Emit(OpCodes.Ldnull);
		}

		internal void WriteNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
		{
			if (Helpers.IsValueType(type))
			{
				Type underlyingType2 = null;
				underlyingType2 = Helpers.GetUnderlyingType(type);
				if (underlyingType2 == null)
				{
					tail.EmitWrite(this, valueFrom);
				}
				else
				{
					using (Local valOrNull = GetLocalWithValue(type, valueFrom))
					{
						LoadAddress(valOrNull, type);
						LoadValue(type.GetProperty("HasValue"));
						CodeLabel end2 = DefineLabel();
						BranchIfFalse(end2, @short: false);
						LoadAddress(valOrNull, type);
						EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
						tail.EmitWrite(this, null);
						MarkLabel(end2);
					}
				}
				return;
			}
			LoadValue(valueFrom);
			CopyValue();
			CodeLabel hasVal = DefineLabel();
			CodeLabel end = DefineLabel();
			BranchIfTrue(hasVal, @short: true);
			DiscardValue();
			Branch(end, @short: false);
			MarkLabel(hasVal);
			tail.EmitWrite(this, null);
			MarkLabel(end);
		}

		internal void ReadNullCheckedTail(Type type, IProtoSerializer tail, Local valueFrom)
		{
			Type underlyingType;
			if (Helpers.IsValueType(type) && (underlyingType = Helpers.GetUnderlyingType(type)) != null)
			{
				if (tail.RequiresOldValue)
				{
					using (Local loc = GetLocalWithValue(type, valueFrom))
					{
						LoadAddress(loc, type);
						EmitCall(type.GetMethod("GetValueOrDefault", Helpers.EmptyTypes));
					}
				}
				tail.EmitRead(this, null);
				if (tail.ReturnsValue)
				{
					EmitCtor(type, underlyingType);
				}
			}
			else
			{
				tail.EmitRead(this, valueFrom);
			}
		}

		public void EmitCtor(Type type)
		{
			EmitCtor(type, Helpers.EmptyTypes);
		}

		public void EmitCtor(ConstructorInfo ctor)
		{
			if (ctor == null)
			{
				throw new ArgumentNullException("ctor");
			}
			MemberInfo ctorMember = ctor;
			CheckAccessibility(ref ctorMember);
			il.Emit(OpCodes.Newobj, ctor);
		}

		public void InitLocal(Type type, Local target)
		{
			LoadAddress(target, type, evenIfClass: true);
			il.Emit(OpCodes.Initobj, type);
		}

		public void EmitCtor(Type type, params Type[] parameterTypes)
		{
			if (Helpers.IsValueType(type) && parameterTypes.Length == 0)
			{
				il.Emit(OpCodes.Initobj, type);
				return;
			}
			ConstructorInfo ctor = Helpers.GetConstructor(type, parameterTypes, nonPublic: true);
			if (ctor == null)
			{
				throw new InvalidOperationException("No suitable constructor found for " + type.FullName);
			}
			EmitCtor(ctor);
		}

		private bool InternalsVisible(Assembly assembly)
		{
			if (Helpers.IsNullOrEmpty(assemblyName))
			{
				return false;
			}
			if (knownTrustedAssemblies != null && knownTrustedAssemblies.IndexOfReference(assembly) >= 0)
			{
				return true;
			}
			if (knownUntrustedAssemblies != null && knownUntrustedAssemblies.IndexOfReference(assembly) >= 0)
			{
				return false;
			}
			bool isTrusted = false;
			Type attributeType = MapType(typeof(InternalsVisibleToAttribute));
			if (attributeType == null)
			{
				return false;
			}
			object[] customAttributes = assembly.GetCustomAttributes(attributeType, inherit: false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				InternalsVisibleToAttribute attrib = (InternalsVisibleToAttribute)customAttributes[i];
				if (attrib.AssemblyName == assemblyName || attrib.AssemblyName.StartsWith(assemblyName + ","))
				{
					isTrusted = true;
					break;
				}
			}
			if (isTrusted)
			{
				if (knownTrustedAssemblies == null)
				{
					knownTrustedAssemblies = new BasicList();
				}
				knownTrustedAssemblies.Add(assembly);
			}
			else
			{
				if (knownUntrustedAssemblies == null)
				{
					knownUntrustedAssemblies = new BasicList();
				}
				knownUntrustedAssemblies.Add(assembly);
			}
			return isTrusted;
		}

		internal void CheckAccessibility(ref MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (NonPublic)
			{
				return;
			}
			if (member is FieldInfo && (member.Name.StartsWith("<") & member.Name.EndsWith(">k__BackingField")))
			{
				string propName = member.Name.Substring(1, member.Name.Length - 17);
				PropertyInfo prop = member.DeclaringType.GetProperty(propName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				if (prop != null)
				{
					member = prop;
				}
			}
			MemberTypes memberType = member.MemberType;
			bool isPublic;
			switch (memberType)
			{
			case MemberTypes.TypeInfo:
			{
				Type type = (Type)member;
				isPublic = (type.IsPublic || InternalsVisible(type.Assembly));
				break;
			}
			case MemberTypes.NestedType:
			{
				Type type = (Type)member;
				do
				{
					isPublic = (type.IsNestedPublic || type.IsPublic || ((type.DeclaringType == null || type.IsNestedAssembly || type.IsNestedFamORAssem) && InternalsVisible(type.Assembly)));
				}
				while (isPublic && (type = type.DeclaringType) != null);
				break;
			}
			case MemberTypes.Field:
			{
				FieldInfo field = (FieldInfo)member;
				isPublic = (field.IsPublic || ((field.IsAssembly || field.IsFamilyOrAssembly) && InternalsVisible(field.DeclaringType.Assembly)));
				break;
			}
			case MemberTypes.Constructor:
			{
				ConstructorInfo ctor = (ConstructorInfo)member;
				isPublic = (ctor.IsPublic || ((ctor.IsAssembly || ctor.IsFamilyOrAssembly) && InternalsVisible(ctor.DeclaringType.Assembly)));
				break;
			}
			case MemberTypes.Method:
			{
				MethodInfo method = (MethodInfo)member;
				isPublic = (method.IsPublic || ((method.IsAssembly || method.IsFamilyOrAssembly) && InternalsVisible(method.DeclaringType.Assembly)));
				if (!isPublic && (member is MethodBuilder || member.DeclaringType == MapType(typeof(TypeModel))))
				{
					isPublic = true;
				}
				break;
			}
			case MemberTypes.Property:
				isPublic = true;
				break;
			default:
				throw new NotSupportedException(memberType.ToString());
			}
			if (!isPublic)
			{
				if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
				{
					throw new InvalidOperationException("Non-public type cannot be used with full dll compilation: " + ((Type)member).FullName);
				}
				throw new InvalidOperationException("Non-public member cannot be used with full dll compilation: " + member.DeclaringType.FullName + "." + member.Name);
			}
		}

		public void LoadValue(FieldInfo field)
		{
			MemberInfo member = field;
			CheckAccessibility(ref member);
			if (member is PropertyInfo)
			{
				LoadValue((PropertyInfo)member);
				return;
			}
			OpCode code = field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld;
			il.Emit(code, field);
		}

		public void StoreValue(FieldInfo field)
		{
			MemberInfo member = field;
			CheckAccessibility(ref member);
			if (member is PropertyInfo)
			{
				StoreValue((PropertyInfo)member);
				return;
			}
			OpCode code = field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld;
			il.Emit(code, field);
		}

		public void LoadValue(PropertyInfo property)
		{
			MemberInfo member = property;
			CheckAccessibility(ref member);
			EmitCall(Helpers.GetGetMethod(property, nonPublic: true, allowInternal: true));
		}

		public void StoreValue(PropertyInfo property)
		{
			MemberInfo member = property;
			CheckAccessibility(ref member);
			EmitCall(Helpers.GetSetMethod(property, nonPublic: true, allowInternal: true));
		}

		internal static void LoadValue(ILGenerator il, int value)
		{
			switch (value)
			{
			case 0:
				il.Emit(OpCodes.Ldc_I4_0);
				break;
			case 1:
				il.Emit(OpCodes.Ldc_I4_1);
				break;
			case 2:
				il.Emit(OpCodes.Ldc_I4_2);
				break;
			case 3:
				il.Emit(OpCodes.Ldc_I4_3);
				break;
			case 4:
				il.Emit(OpCodes.Ldc_I4_4);
				break;
			case 5:
				il.Emit(OpCodes.Ldc_I4_5);
				break;
			case 6:
				il.Emit(OpCodes.Ldc_I4_6);
				break;
			case 7:
				il.Emit(OpCodes.Ldc_I4_7);
				break;
			case 8:
				il.Emit(OpCodes.Ldc_I4_8);
				break;
			case -1:
				il.Emit(OpCodes.Ldc_I4_M1);
				break;
			default:
				il.Emit(OpCodes.Ldc_I4, value);
				break;
			}
		}

		private bool UseShortForm(Local local)
		{
			return local.Value.LocalIndex < 256;
		}

		internal void LoadAddress(Local local, Type type, bool evenIfClass = false)
		{
			if (evenIfClass || Helpers.IsValueType(type))
			{
				if (local == null)
				{
					throw new InvalidOperationException("Cannot load the address of the head of the stack");
				}
				if (local == InputValue)
				{
					il.Emit(OpCodes.Ldarga_S, (byte)((!isStatic) ? 1 : 0));
					return;
				}
				OpCode code = UseShortForm(local) ? OpCodes.Ldloca_S : OpCodes.Ldloca;
				il.Emit(code, local.Value);
			}
			else
			{
				LoadValue(local);
			}
		}

		internal void Branch(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Br_S : OpCodes.Br;
			il.Emit(code, label.Value);
		}

		internal void BranchIfFalse(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Brfalse_S : OpCodes.Brfalse;
			il.Emit(code, label.Value);
		}

		internal void BranchIfTrue(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Brtrue_S : OpCodes.Brtrue;
			il.Emit(code, label.Value);
		}

		internal void BranchIfEqual(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Beq_S : OpCodes.Beq;
			il.Emit(code, label.Value);
		}

		internal void CopyValue()
		{
			Emit(OpCodes.Dup);
		}

		internal void BranchIfGreater(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Bgt_S : OpCodes.Bgt;
			il.Emit(code, label.Value);
		}

		internal void BranchIfLess(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Blt_S : OpCodes.Blt;
			il.Emit(code, label.Value);
		}

		internal void DiscardValue()
		{
			Emit(OpCodes.Pop);
		}

		public void Subtract()
		{
			Emit(OpCodes.Sub);
		}

		public void Switch(CodeLabel[] jumpTable)
		{
			if (jumpTable.Length <= 128)
			{
				Label[] labels = new Label[jumpTable.Length];
				for (int j = 0; j < labels.Length; j++)
				{
					labels[j] = jumpTable[j].Value;
				}
				il.Emit(OpCodes.Switch, labels);
			}
			else
			{
				using (Local val = GetLocalWithValue(MapType(typeof(int)), null))
				{
					int count = jumpTable.Length;
					int offset = 0;
					int blockCount = count / 128;
					if (count % 128 != 0)
					{
						blockCount++;
					}
					Label[] blockLabels = new Label[blockCount];
					for (int i = 0; i < blockCount; i++)
					{
						blockLabels[i] = il.DefineLabel();
					}
					CodeLabel endOfSwitch = DefineLabel();
					LoadValue(val);
					LoadValue(128);
					Emit(OpCodes.Div);
					il.Emit(OpCodes.Switch, blockLabels);
					Branch(endOfSwitch, @short: false);
					Label[] innerLabels = new Label[128];
					for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
					{
						il.MarkLabel(blockLabels[blockIndex]);
						int itemsThisBlock = Math.Min(128, count);
						count -= itemsThisBlock;
						if (innerLabels.Length != itemsThisBlock)
						{
							innerLabels = new Label[itemsThisBlock];
						}
						int subtract = offset;
						for (int k = 0; k < itemsThisBlock; k++)
						{
							innerLabels[k] = jumpTable[offset++].Value;
						}
						LoadValue(val);
						if (subtract != 0)
						{
							LoadValue(subtract);
							Emit(OpCodes.Sub);
						}
						il.Emit(OpCodes.Switch, innerLabels);
						if (count != 0)
						{
							Branch(endOfSwitch, @short: false);
						}
					}
					MarkLabel(endOfSwitch);
				}
			}
		}

		internal void EndFinally()
		{
			il.EndExceptionBlock();
		}

		internal void BeginFinally()
		{
			il.BeginFinallyBlock();
		}

		internal void EndTry(CodeLabel label, bool @short)
		{
			OpCode code = @short ? OpCodes.Leave_S : OpCodes.Leave;
			il.Emit(code, label.Value);
		}

		internal CodeLabel BeginTry()
		{
			CodeLabel label = new CodeLabel(il.BeginExceptionBlock(), nextLabel++);
			return label;
		}

		internal void Constrain(Type type)
		{
			il.Emit(OpCodes.Constrained, type);
		}

		internal void TryCast(Type type)
		{
			il.Emit(OpCodes.Isinst, type);
		}

		internal void Cast(Type type)
		{
			il.Emit(OpCodes.Castclass, type);
		}

		public IDisposable Using(Local local)
		{
			return new UsingBlock(this, local);
		}

		internal void Add()
		{
			Emit(OpCodes.Add);
		}

		internal void LoadLength(Local arr, bool zeroIfNull)
		{
			if (zeroIfNull)
			{
				CodeLabel notNull = DefineLabel();
				CodeLabel done = DefineLabel();
				LoadValue(arr);
				CopyValue();
				BranchIfTrue(notNull, @short: true);
				DiscardValue();
				LoadValue(0);
				Branch(done, @short: true);
				MarkLabel(notNull);
				Emit(OpCodes.Ldlen);
				Emit(OpCodes.Conv_I4);
				MarkLabel(done);
			}
			else
			{
				LoadValue(arr);
				Emit(OpCodes.Ldlen);
				Emit(OpCodes.Conv_I4);
			}
		}

		internal void CreateArray(Type elementType, Local length)
		{
			LoadValue(length);
			il.Emit(OpCodes.Newarr, elementType);
		}

		internal void LoadArrayValue(Local arr, Local i)
		{
			Type type2 = arr.Type;
			type2 = type2.GetElementType();
			LoadValue(arr);
			LoadValue(i);
			switch (Helpers.GetTypeCode(type2))
			{
			case ProtoTypeCode.SByte:
				Emit(OpCodes.Ldelem_I1);
				return;
			case ProtoTypeCode.Int16:
				Emit(OpCodes.Ldelem_I2);
				return;
			case ProtoTypeCode.Int32:
				Emit(OpCodes.Ldelem_I4);
				return;
			case ProtoTypeCode.Int64:
				Emit(OpCodes.Ldelem_I8);
				return;
			case ProtoTypeCode.Byte:
				Emit(OpCodes.Ldelem_U1);
				return;
			case ProtoTypeCode.UInt16:
				Emit(OpCodes.Ldelem_U2);
				return;
			case ProtoTypeCode.UInt32:
				Emit(OpCodes.Ldelem_U4);
				return;
			case ProtoTypeCode.UInt64:
				Emit(OpCodes.Ldelem_I8);
				return;
			case ProtoTypeCode.Single:
				Emit(OpCodes.Ldelem_R4);
				return;
			case ProtoTypeCode.Double:
				Emit(OpCodes.Ldelem_R8);
				return;
			}
			if (Helpers.IsValueType(type2))
			{
				il.Emit(OpCodes.Ldelema, type2);
				il.Emit(OpCodes.Ldobj, type2);
			}
			else
			{
				Emit(OpCodes.Ldelem_Ref);
			}
		}

		internal void LoadValue(Type type)
		{
			il.Emit(OpCodes.Ldtoken, type);
			EmitCall(MapType(typeof(Type)).GetMethod("GetTypeFromHandle"));
		}

		internal void ConvertToInt32(ProtoTypeCode typeCode, bool uint32Overflow)
		{
			switch (typeCode)
			{
			case ProtoTypeCode.Int32:
				break;
			case ProtoTypeCode.SByte:
			case ProtoTypeCode.Byte:
			case ProtoTypeCode.Int16:
			case ProtoTypeCode.UInt16:
				Emit(OpCodes.Conv_I4);
				break;
			case ProtoTypeCode.Int64:
				Emit(OpCodes.Conv_Ovf_I4);
				break;
			case ProtoTypeCode.UInt32:
				Emit(uint32Overflow ? OpCodes.Conv_Ovf_I4_Un : OpCodes.Conv_Ovf_I4);
				break;
			case ProtoTypeCode.UInt64:
				Emit(OpCodes.Conv_Ovf_I4_Un);
				break;
			default:
				throw new InvalidOperationException("ConvertToInt32 not implemented for: " + typeCode);
			}
		}

		internal void ConvertFromInt32(ProtoTypeCode typeCode, bool uint32Overflow)
		{
			switch (typeCode)
			{
			case ProtoTypeCode.Int32:
				break;
			case ProtoTypeCode.SByte:
				Emit(OpCodes.Conv_Ovf_I1);
				break;
			case ProtoTypeCode.Byte:
				Emit(OpCodes.Conv_Ovf_U1);
				break;
			case ProtoTypeCode.Int16:
				Emit(OpCodes.Conv_Ovf_I2);
				break;
			case ProtoTypeCode.UInt16:
				Emit(OpCodes.Conv_Ovf_U2);
				break;
			case ProtoTypeCode.UInt32:
				Emit(uint32Overflow ? OpCodes.Conv_Ovf_U4 : OpCodes.Conv_U4);
				break;
			case ProtoTypeCode.Int64:
				Emit(OpCodes.Conv_I8);
				break;
			case ProtoTypeCode.UInt64:
				Emit(OpCodes.Conv_U8);
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		internal void LoadValue(decimal value)
		{
			if (value == 0m)
			{
				LoadValue(typeof(decimal).GetField("Zero"));
				return;
			}
			int[] bits = decimal.GetBits(value);
			LoadValue(bits[0]);
			LoadValue(bits[1]);
			LoadValue(bits[2]);
			LoadValue((int)((uint)bits[3] >> 31));
			LoadValue((bits[3] >> 16) & 0xFF);
			EmitCtor(MapType(typeof(decimal)), MapType(typeof(int)), MapType(typeof(int)), MapType(typeof(int)), MapType(typeof(bool)), MapType(typeof(byte)));
		}

		internal void LoadValue(Guid value)
		{
			if (value == Guid.Empty)
			{
				LoadValue(typeof(Guid).GetField("Empty"));
				return;
			}
			byte[] bytes = value.ToByteArray();
			int j = bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
			LoadValue(j);
			short s2 = (short)(bytes[4] | (bytes[5] << 8));
			LoadValue(s2);
			s2 = (short)(bytes[6] | (bytes[7] << 8));
			LoadValue(s2);
			for (j = 8; j <= 15; j++)
			{
				LoadValue(bytes[j]);
			}
			EmitCtor(MapType(typeof(Guid)), MapType(typeof(int)), MapType(typeof(short)), MapType(typeof(short)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)), MapType(typeof(byte)));
		}

		internal void LoadSerializationContext()
		{
			LoadReaderWriter();
			LoadValue((isWriter ? typeof(ProtoWriter) : typeof(ProtoReader)).GetProperty("Context"));
		}

		internal Type MapType(Type type)
		{
			return model.MapType(type);
		}

		internal bool AllowInternal(PropertyInfo property)
		{
			if (!NonPublic)
			{
				return InternalsVisible(Helpers.GetAssembly(property.DeclaringType));
			}
			return true;
		}
	}
}
