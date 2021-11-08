using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal class ListDecorator : ProtoDecoratorBase
	{
		private readonly byte options;

		private const byte OPTIONS_IsList = 1;

		private const byte OPTIONS_SuppressIList = 2;

		private const byte OPTIONS_WritePacked = 4;

		private const byte OPTIONS_ReturnList = 8;

		private const byte OPTIONS_OverwriteList = 16;

		private const byte OPTIONS_SupportNull = 32;

		private readonly Type declaredType;

		private readonly Type concreteType;

		private readonly MethodInfo add;

		private readonly int fieldNumber;

		protected readonly WireType packedWireType;

		private static readonly Type ienumeratorType = typeof(IEnumerator);

		private static readonly Type ienumerableType = typeof(IEnumerable);

		private bool IsList => (options & 1) != 0;

		private bool SuppressIList => (options & 2) != 0;

		private bool WritePacked => (options & 4) != 0;

		private bool SupportNull => (options & 0x20) != 0;

		private bool ReturnList => (options & 8) != 0;

		protected virtual bool RequireAdd => true;

		public override Type ExpectedType => declaredType;

		public override bool RequiresOldValue => AppendToCollection;

		public override bool ReturnsValue => ReturnList;

		protected bool AppendToCollection => (options & 0x10) == 0;

		internal static bool CanPack(WireType wireType)
		{
			if ((uint)wireType <= 1u || wireType == WireType.Fixed32 || wireType == WireType.SignedVariant)
			{
				return true;
			}
			return false;
		}

		internal static ListDecorator Create(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull)
		{
			if (returnList && ImmutableCollectionDecorator.IdentifyImmutable(model, declaredType, out MethodInfo builderFactory, out MethodInfo add, out MethodInfo addRange, out MethodInfo finish))
			{
				return new ImmutableCollectionDecorator(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull, builderFactory, add, addRange, finish);
			}
			return new ListDecorator(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull);
		}

		protected ListDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull)
			: base(tail)
		{
			if (returnList)
			{
				options |= 8;
			}
			if (overwriteList)
			{
				options |= 16;
			}
			if (supportNull)
			{
				options |= 32;
			}
			if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (!CanPack(packedWireType))
			{
				if (writePacked)
				{
					throw new InvalidOperationException("Only simple data-types can use packed encoding");
				}
				packedWireType = WireType.None;
			}
			this.fieldNumber = fieldNumber;
			if (writePacked)
			{
				options |= 4;
			}
			this.packedWireType = packedWireType;
			if (declaredType == null)
			{
				throw new ArgumentNullException("declaredType");
			}
			if (declaredType.IsArray)
			{
				throw new ArgumentException("Cannot treat arrays as lists", "declaredType");
			}
			this.declaredType = declaredType;
			this.concreteType = concreteType;
			if (!RequireAdd)
			{
				return;
			}
			add = TypeModel.ResolveListAdd(model, declaredType, tail.ExpectedType, out bool isList);
			if (isList)
			{
				options |= 1;
				string fullName = declaredType.FullName;
				if (fullName != null && fullName.StartsWith("System.Data.Linq.EntitySet`1[["))
				{
					options |= 2;
				}
			}
			if (add == null)
			{
				throw new InvalidOperationException("Unable to resolve a suitable Add method for " + declaredType.FullName);
			}
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			bool returnList = ReturnList;
			using (Local list = AppendToCollection ? ctx.GetLocalWithValue(ExpectedType, valueFrom) : new Local(ctx, declaredType))
			{
				using (Local origlist = (returnList && AppendToCollection && !Helpers.IsValueType(ExpectedType)) ? new Local(ctx, ExpectedType) : null)
				{
					if (!AppendToCollection)
					{
						ctx.LoadNullRef();
						ctx.StoreValue(list);
					}
					else if (returnList && origlist != null)
					{
						ctx.LoadValue(list);
						ctx.StoreValue(origlist);
					}
					if (concreteType != null)
					{
						ctx.LoadValue(list);
						CodeLabel notNull = ctx.DefineLabel();
						ctx.BranchIfTrue(notNull, @short: true);
						ctx.EmitCtor(concreteType);
						ctx.StoreValue(list);
						ctx.MarkLabel(notNull);
					}
					bool castListForAdd = !add.DeclaringType.IsAssignableFrom(declaredType);
					EmitReadList(ctx, list, Tail, add, packedWireType, castListForAdd);
					if (returnList)
					{
						if (AppendToCollection && origlist != null)
						{
							ctx.LoadValue(origlist);
							ctx.LoadValue(list);
							CodeLabel sameList = ctx.DefineLabel();
							CodeLabel allDone = ctx.DefineLabel();
							ctx.BranchIfEqual(sameList, @short: true);
							ctx.LoadValue(list);
							ctx.Branch(allDone, @short: true);
							ctx.MarkLabel(sameList);
							ctx.LoadNullRef();
							ctx.MarkLabel(allDone);
						}
						else
						{
							ctx.LoadValue(list);
						}
					}
				}
			}
		}

		internal static void EmitReadList(CompilerContext ctx, Local list, IProtoSerializer tail, MethodInfo add, WireType packedWireType, bool castListForAdd)
		{
			using (Local fieldNumber = new Local(ctx, ctx.MapType(typeof(int))))
			{
				CodeLabel readPacked = (packedWireType == WireType.None) ? default(CodeLabel) : ctx.DefineLabel();
				if (packedWireType != WireType.None)
				{
					ctx.LoadReaderWriter();
					ctx.LoadValue(typeof(ProtoReader).GetProperty("WireType"));
					ctx.LoadValue(2);
					ctx.BranchIfEqual(readPacked, @short: false);
				}
				ctx.LoadReaderWriter();
				ctx.LoadValue(typeof(ProtoReader).GetProperty("FieldNumber"));
				ctx.StoreValue(fieldNumber);
				CodeLabel @continue = ctx.DefineLabel();
				ctx.MarkLabel(@continue);
				EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);
				ctx.LoadReaderWriter();
				ctx.LoadValue(fieldNumber);
				ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("TryReadFieldHeader"));
				ctx.BranchIfTrue(@continue, @short: false);
				if (packedWireType != WireType.None)
				{
					CodeLabel allDone = ctx.DefineLabel();
					ctx.Branch(allDone, @short: false);
					ctx.MarkLabel(readPacked);
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("StartSubItem"));
					CodeLabel testForData = ctx.DefineLabel();
					CodeLabel noMoreData = ctx.DefineLabel();
					ctx.MarkLabel(testForData);
					ctx.LoadValue((int)packedWireType);
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("HasSubValue"));
					ctx.BranchIfFalse(noMoreData, @short: false);
					EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);
					ctx.Branch(testForData, @short: false);
					ctx.MarkLabel(noMoreData);
					ctx.LoadReaderWriter();
					ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("EndSubItem"));
					ctx.MarkLabel(allDone);
				}
			}
		}

		private static void EmitReadAndAddItem(CompilerContext ctx, Local list, IProtoSerializer tail, MethodInfo add, bool castListForAdd)
		{
			ctx.LoadAddress(list, list.Type);
			if (castListForAdd)
			{
				ctx.Cast(add.DeclaringType);
			}
			Type itemType = tail.ExpectedType;
			bool tailReturnsValue = tail.ReturnsValue;
			if (tail.RequiresOldValue)
			{
				if (Helpers.IsValueType(itemType) || !tailReturnsValue)
				{
					using (Local item = new Local(ctx, itemType))
					{
						if (Helpers.IsValueType(itemType))
						{
							ctx.LoadAddress(item, itemType);
							ctx.EmitCtor(itemType);
						}
						else
						{
							ctx.LoadNullRef();
							ctx.StoreValue(item);
						}
						tail.EmitRead(ctx, item);
						if (!tailReturnsValue)
						{
							ctx.LoadValue(item);
						}
					}
				}
				else
				{
					ctx.LoadNullRef();
					tail.EmitRead(ctx, null);
				}
			}
			else
			{
				if (!tailReturnsValue)
				{
					throw new InvalidOperationException();
				}
				tail.EmitRead(ctx, null);
			}
			Type addParamType = add.GetParameters()[0].ParameterType;
			if (addParamType != itemType)
			{
				if (addParamType == ctx.MapType(typeof(object)))
				{
					ctx.CastToObject(itemType);
				}
				else
				{
					if (!(Helpers.GetUnderlyingType(addParamType) == itemType))
					{
						throw new InvalidOperationException("Conflicting item/add type");
					}
					ConstructorInfo ctor = Helpers.GetConstructor(addParamType, new Type[1]
					{
						itemType
					}, nonPublic: false);
					ctx.EmitCtor(ctor);
				}
			}
			ctx.EmitCall(add, list.Type);
			if (add.ReturnType != ctx.MapType(typeof(void)))
			{
				ctx.DiscardValue();
			}
		}

		protected MethodInfo GetEnumeratorInfo(TypeModel model, out MethodInfo moveNext, out MethodInfo current)
		{
			return GetEnumeratorInfo(model, ExpectedType, Tail.ExpectedType, out moveNext, out current);
		}

		internal static MethodInfo GetEnumeratorInfo(TypeModel model, Type expectedType, Type itemType, out MethodInfo moveNext, out MethodInfo current)
		{
			Type enumeratorType2 = null;
			MethodInfo getEnumerator4 = Helpers.GetInstanceMethod(expectedType, "GetEnumerator", null);
			Type getReturnType4 = null;
			Type iteratorType3;
			if (getEnumerator4 != null)
			{
				getReturnType4 = getEnumerator4.ReturnType;
				iteratorType3 = getReturnType4;
				moveNext = Helpers.GetInstanceMethod(iteratorType3, "MoveNext", null);
				PropertyInfo prop = Helpers.GetProperty(iteratorType3, "Current", nonPublic: false);
				current = ((prop == null) ? null : Helpers.GetGetMethod(prop, nonPublic: false, allowInternal: false));
				if (moveNext == null && model.MapType(ienumeratorType).IsAssignableFrom(iteratorType3))
				{
					moveNext = Helpers.GetInstanceMethod(model.MapType(ienumeratorType), "MoveNext", null);
				}
				if (moveNext != null && moveNext.ReturnType == model.MapType(typeof(bool)) && current != null && current.ReturnType == itemType)
				{
					return getEnumerator4;
				}
				moveNext = (current = (getEnumerator4 = null));
			}
			Type tmp2 = model.MapType(typeof(IEnumerable<>), demand: false);
			if (tmp2 != null)
			{
				tmp2 = tmp2.MakeGenericType(itemType);
				enumeratorType2 = tmp2;
			}
			if (enumeratorType2 != null && enumeratorType2.IsAssignableFrom(expectedType))
			{
				getEnumerator4 = Helpers.GetInstanceMethod(enumeratorType2, "GetEnumerator");
				getReturnType4 = getEnumerator4.ReturnType;
				iteratorType3 = getReturnType4;
				moveNext = Helpers.GetInstanceMethod(model.MapType(ienumeratorType), "MoveNext");
				current = Helpers.GetGetMethod(Helpers.GetProperty(iteratorType3, "Current", nonPublic: false), nonPublic: false, allowInternal: false);
				return getEnumerator4;
			}
			enumeratorType2 = model.MapType(ienumerableType);
			getEnumerator4 = Helpers.GetInstanceMethod(enumeratorType2, "GetEnumerator");
			getReturnType4 = getEnumerator4.ReturnType;
			iteratorType3 = getReturnType4;
			moveNext = Helpers.GetInstanceMethod(iteratorType3, "MoveNext");
			current = Helpers.GetGetMethod(Helpers.GetProperty(iteratorType3, "Current", nonPublic: false), nonPublic: false, allowInternal: false);
			return getEnumerator4;
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			using (Local list = ctx.GetLocalWithValue(ExpectedType, valueFrom))
			{
				MethodInfo moveNext;
				MethodInfo current;
				MethodInfo getEnumerator = GetEnumeratorInfo(ctx.Model, out moveNext, out current);
				Type enumeratorType = getEnumerator.ReturnType;
				bool writePacked = WritePacked;
				using (Local iter = new Local(ctx, enumeratorType))
				{
					using (Local token = writePacked ? new Local(ctx, ctx.MapType(typeof(SubItemToken))) : null)
					{
						if (writePacked)
						{
							ctx.LoadValue(fieldNumber);
							ctx.LoadValue(2);
							ctx.LoadReaderWriter();
							ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("WriteFieldHeader"));
							ctx.LoadValue(list);
							ctx.LoadReaderWriter();
							ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("StartSubItem"));
							ctx.StoreValue(token);
							ctx.LoadValue(fieldNumber);
							ctx.LoadReaderWriter();
							ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("SetPackedField"));
						}
						ctx.LoadAddress(list, ExpectedType);
						ctx.EmitCall(getEnumerator, ExpectedType);
						ctx.StoreValue(iter);
						using (ctx.Using(iter))
						{
							CodeLabel body = ctx.DefineLabel();
							CodeLabel next = ctx.DefineLabel();
							ctx.Branch(next, @short: false);
							ctx.MarkLabel(body);
							ctx.LoadAddress(iter, enumeratorType);
							ctx.EmitCall(current, enumeratorType);
							Type itemType = Tail.ExpectedType;
							if (itemType != ctx.MapType(typeof(object)) && current.ReturnType == ctx.MapType(typeof(object)))
							{
								ctx.CastFromObject(itemType);
							}
							Tail.EmitWrite(ctx, null);
							ctx.MarkLabel(next);
							ctx.LoadAddress(iter, enumeratorType);
							ctx.EmitCall(moveNext, enumeratorType);
							ctx.BranchIfTrue(body, @short: false);
						}
						if (writePacked)
						{
							ctx.LoadValue(token);
							ctx.LoadReaderWriter();
							ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("EndSubItem"));
						}
					}
				}
			}
		}

		public override void Write(object value, ProtoWriter dest)
		{
			bool writePacked = WritePacked;
			bool fixedSizePacked = (writePacked & CanUsePackedPrefix(value)) && value is ICollection;
			SubItemToken token;
			if (writePacked)
			{
				ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, dest);
				if (fixedSizePacked)
				{
					ProtoWriter.WritePackedPrefix(((ICollection)value).Count, packedWireType, dest);
					token = default(SubItemToken);
				}
				else
				{
					token = ProtoWriter.StartSubItem(value, dest);
				}
				ProtoWriter.SetPackedField(fieldNumber, dest);
			}
			else
			{
				token = default(SubItemToken);
			}
			bool checkForNull = !SupportNull;
			foreach (object subItem in (IEnumerable)value)
			{
				if (checkForNull && subItem == null)
				{
					throw new NullReferenceException();
				}
				Tail.Write(subItem, dest);
			}
			if (writePacked)
			{
				if (fixedSizePacked)
				{
					ProtoWriter.ClearPackedField(fieldNumber, dest);
				}
				else
				{
					ProtoWriter.EndSubItem(token, dest);
				}
			}
		}

		private bool CanUsePackedPrefix(object obj)
		{
			return ArrayDecorator.CanUsePackedPrefix(packedWireType, Tail.ExpectedType);
		}

		public override object Read(object value, ProtoReader source)
		{
			try
			{
				int field = source.FieldNumber;
				object origValue = value;
				if (value == null)
				{
					value = Activator.CreateInstance(concreteType);
				}
				bool isList = IsList && !SuppressIList;
				if (packedWireType != WireType.None && source.WireType == WireType.String)
				{
					SubItemToken token = ProtoReader.StartSubItem(source);
					if (isList)
					{
						IList list2 = (IList)value;
						while (ProtoReader.HasSubValue(packedWireType, source))
						{
							list2.Add(Tail.Read(null, source));
						}
					}
					else
					{
						object[] args2 = new object[1];
						while (ProtoReader.HasSubValue(packedWireType, source))
						{
							args2[0] = Tail.Read(null, source);
							add.Invoke(value, args2);
						}
					}
					ProtoReader.EndSubItem(token, source);
				}
				else if (isList)
				{
					IList list = (IList)value;
					do
					{
						list.Add(Tail.Read(null, source));
					}
					while (source.TryReadFieldHeader(field));
				}
				else
				{
					object[] args = new object[1];
					do
					{
						args[0] = Tail.Read(null, source);
						add.Invoke(value, args);
					}
					while (source.TryReadFieldHeader(field));
				}
				return (origValue == value) ? null : value;
			}
			catch (TargetInvocationException tie)
			{
				if (tie.InnerException != null)
				{
					throw tie.InnerException;
				}
				throw;
			}
		}
	}
}
