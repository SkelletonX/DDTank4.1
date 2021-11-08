using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class ImmutableCollectionDecorator : ListDecorator
	{
		private readonly MethodInfo builderFactory;

		private readonly MethodInfo add;

		private readonly MethodInfo addRange;

		private readonly MethodInfo finish;

		protected override bool RequireAdd => false;

		private static Type ResolveIReadOnlyCollection(Type declaredType, Type t)
		{
			Type[] interfaces = declaredType.GetInterfaces();
			foreach (Type intImpl in interfaces)
			{
				if (!intImpl.IsGenericType || !intImpl.Name.StartsWith("IReadOnlyCollection`"))
				{
					continue;
				}
				if (t != null)
				{
					Type[] typeArgs = intImpl.GetGenericArguments();
					if (typeArgs.Length != 1 && typeArgs[0] != t)
					{
						continue;
					}
				}
				return intImpl;
			}
			return null;
		}

		internal static bool IdentifyImmutable(TypeModel model, Type declaredType, out MethodInfo builderFactory, out MethodInfo add, out MethodInfo addRange, out MethodInfo finish)
		{
			builderFactory = (add = (addRange = (finish = null)));
			if (model == null || declaredType == null)
			{
				return false;
			}
			if (!declaredType.IsGenericType)
			{
				return false;
			}
			Type[] typeArgs = declaredType.GetGenericArguments();
			Type[] effectiveType;
			switch (typeArgs.Length)
			{
			case 1:
				effectiveType = typeArgs;
				break;
			case 2:
			{
				Type kvp2 = model.MapType(typeof(KeyValuePair<, >));
				if (kvp2 == null)
				{
					return false;
				}
				kvp2 = kvp2.MakeGenericType(typeArgs);
				effectiveType = new Type[1]
				{
					kvp2
				};
				break;
			}
			default:
				return false;
			}
			if (ResolveIReadOnlyCollection(declaredType, null) == null)
			{
				return false;
			}
			string name2 = declaredType.Name;
			int i = name2.IndexOf('`');
			if (i <= 0)
			{
				return false;
			}
			name2 = (declaredType.IsInterface ? name2.Substring(1, i - 1) : name2.Substring(0, i));
			Type outerType = model.GetType(declaredType.Namespace + "." + name2, declaredType.Assembly);
			if (outerType == null && name2 == "ImmutableSet")
			{
				outerType = model.GetType(declaredType.Namespace + ".ImmutableHashSet", declaredType.Assembly);
			}
			if (outerType == null)
			{
				return false;
			}
			MethodInfo[] methods = outerType.GetMethods();
			foreach (MethodInfo method in methods)
			{
				if (method.IsStatic && !(method.Name != "CreateBuilder") && method.IsGenericMethodDefinition && method.GetParameters().Length == 0 && method.GetGenericArguments().Length == typeArgs.Length)
				{
					builderFactory = method.MakeGenericMethod(typeArgs);
					break;
				}
			}
			Type voidType = model.MapType(typeof(void));
			if (builderFactory == null || builderFactory.ReturnType == null || builderFactory.ReturnType == voidType)
			{
				return false;
			}
			add = Helpers.GetInstanceMethod(builderFactory.ReturnType, "Add", effectiveType);
			if (add == null)
			{
				return false;
			}
			finish = Helpers.GetInstanceMethod(builderFactory.ReturnType, "ToImmutable", Helpers.EmptyTypes);
			if (finish == null || finish.ReturnType == null || finish.ReturnType == voidType)
			{
				return false;
			}
			if (!(finish.ReturnType == declaredType) && !Helpers.IsAssignableFrom(declaredType, finish.ReturnType))
			{
				return false;
			}
			addRange = Helpers.GetInstanceMethod(builderFactory.ReturnType, "AddRange", new Type[1]
			{
				declaredType
			});
			if (addRange == null)
			{
				Type enumerable = model.MapType(typeof(IEnumerable<>), demand: false);
				if (enumerable != null)
				{
					addRange = Helpers.GetInstanceMethod(builderFactory.ReturnType, "AddRange", new Type[1]
					{
						enumerable.MakeGenericType(effectiveType)
					});
				}
			}
			return true;
		}

		internal ImmutableCollectionDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull, MethodInfo builderFactory, MethodInfo add, MethodInfo addRange, MethodInfo finish)
			: base(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull)
		{
			this.builderFactory = builderFactory;
			this.add = add;
			this.addRange = addRange;
			this.finish = finish;
		}

		public override object Read(object value, ProtoReader source)
		{
			object builderInstance = builderFactory.Invoke(null, null);
			int field = source.FieldNumber;
			object[] args = new object[1];
			if (base.AppendToCollection && value != null && ((ICollection)value).Count != 0)
			{
				if (addRange != null)
				{
					args[0] = value;
					addRange.Invoke(builderInstance, args);
				}
				else
				{
					foreach (object item2 in (ICollection)value)
					{
						object item = args[0] = item2;
						add.Invoke(builderInstance, args);
					}
				}
			}
			if (packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				while (ProtoReader.HasSubValue(packedWireType, source))
				{
					args[0] = Tail.Read(null, source);
					add.Invoke(builderInstance, args);
				}
				ProtoReader.EndSubItem(token, source);
			}
			else
			{
				do
				{
					args[0] = Tail.Read(null, source);
					add.Invoke(builderInstance, args);
				}
				while (source.TryReadFieldHeader(field));
			}
			return finish.Invoke(builderInstance, null);
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local oldList = base.AppendToCollection ? ctx.GetLocalWithValue(ExpectedType, valueFrom) : null)
			{
				using (Local builder = new Local(ctx, builderFactory.ReturnType))
				{
					ctx.EmitCall(builderFactory);
					ctx.StoreValue(builder);
					if (base.AppendToCollection)
					{
						CodeLabel done = ctx.DefineLabel();
						if (!Helpers.IsValueType(ExpectedType))
						{
							ctx.LoadValue(oldList);
							ctx.BranchIfFalse(done, @short: false);
						}
						Type typeInfo = ExpectedType;
						PropertyInfo prop = Helpers.GetProperty(typeInfo, "Length", nonPublic: false);
						if (prop == null)
						{
							prop = Helpers.GetProperty(typeInfo, "Count", nonPublic: false);
						}
						if (prop == null)
						{
							prop = Helpers.GetProperty(ResolveIReadOnlyCollection(ExpectedType, Tail.ExpectedType), "Count", nonPublic: false);
						}
						ctx.LoadAddress(oldList, oldList.Type);
						ctx.EmitCall(Helpers.GetGetMethod(prop, nonPublic: false, allowInternal: false));
						ctx.BranchIfFalse(done, @short: false);
						Type voidType = ctx.MapType(typeof(void));
						if (addRange != null)
						{
							ctx.LoadValue(builder);
							ctx.LoadValue(oldList);
							ctx.EmitCall(addRange);
							if (addRange.ReturnType != null && add.ReturnType != voidType)
							{
								ctx.DiscardValue();
							}
						}
						else
						{
							MethodInfo moveNext;
							MethodInfo current;
							MethodInfo getEnumerator = GetEnumeratorInfo(ctx.Model, out moveNext, out current);
							Type enumeratorType = getEnumerator.ReturnType;
							using (Local iter = new Local(ctx, enumeratorType))
							{
								ctx.LoadAddress(oldList, ExpectedType);
								ctx.EmitCall(getEnumerator);
								ctx.StoreValue(iter);
								using (ctx.Using(iter))
								{
									CodeLabel body = ctx.DefineLabel();
									CodeLabel next = ctx.DefineLabel();
									ctx.Branch(next, @short: false);
									ctx.MarkLabel(body);
									ctx.LoadAddress(builder, builder.Type);
									ctx.LoadAddress(iter, enumeratorType);
									ctx.EmitCall(current);
									ctx.EmitCall(add);
									if (add.ReturnType != null && add.ReturnType != voidType)
									{
										ctx.DiscardValue();
									}
									ctx.MarkLabel(next);
									ctx.LoadAddress(iter, enumeratorType);
									ctx.EmitCall(moveNext);
									ctx.BranchIfTrue(body, @short: false);
								}
							}
						}
						ctx.MarkLabel(done);
					}
					ListDecorator.EmitReadList(ctx, builder, Tail, add, packedWireType, castListForAdd: false);
					ctx.LoadAddress(builder, builder.Type);
					ctx.EmitCall(finish);
					if (ExpectedType != finish.ReturnType)
					{
						ctx.Cast(ExpectedType);
					}
				}
			}
		}
	}
}
