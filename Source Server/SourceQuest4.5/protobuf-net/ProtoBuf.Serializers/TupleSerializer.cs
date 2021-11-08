using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal sealed class TupleSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		private readonly MemberInfo[] members;

		private readonly ConstructorInfo ctor;

		private IProtoSerializer[] tails;

		public Type ExpectedType => ctor.DeclaringType;

		public bool RequiresOldValue => true;

		public bool ReturnsValue => false;

		public TupleSerializer(RuntimeTypeModel model, ConstructorInfo ctor, MemberInfo[] members)
		{
			if (ctor == null)
			{
				throw new ArgumentNullException("ctor");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			this.ctor = ctor;
			this.members = members;
			tails = new IProtoSerializer[members.Length];
			ParameterInfo[] parameters = ctor.GetParameters();
			int i = 0;
			Type tmp;
			while (true)
			{
				if (i < members.Length)
				{
					Type finalType = parameters[i].ParameterType;
					Type itemType = null;
					Type defaultType = null;
					MetaType.ResolveListTypes(model, finalType, ref itemType, ref defaultType);
					tmp = ((itemType == null) ? finalType : itemType);
					bool asReference = false;
					int typeIndex = model.FindOrAddAuto(tmp, demand: false, addWithContractOnly: true, addEvenIfAutoDisabled: false);
					if (typeIndex >= 0)
					{
						asReference = model[tmp].AsReferenceDefault;
					}
					IProtoSerializer tail2 = ValueMember.TryGetCoreSerializer(model, DataFormat.Default, tmp, out WireType wireType, asReference, dynamicType: false, overwriteList: false, allowComplexTypes: true);
					if (tail2 == null)
					{
						break;
					}
					tail2 = new TagDecorator(i + 1, wireType, strict: false, tail2);
					IProtoSerializer serializer = (!(itemType == null)) ? ((!finalType.IsArray) ? ((ProtoDecoratorBase)ListDecorator.Create(model, finalType, defaultType, tail2, i + 1, writePacked: false, wireType, returnList: true, overwriteList: false, supportNull: false)) : ((ProtoDecoratorBase)new ArrayDecorator(model, tail2, i + 1, writePacked: false, wireType, finalType, overwriteList: false, supportNull: false))) : tail2;
					tails[i] = serializer;
					i++;
					continue;
				}
				return;
			}
			throw new InvalidOperationException("No serializer defined for type: " + tmp.FullName);
		}

		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		public void EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
		}

		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		object IProtoTypeSerializer.CreateInstance(ProtoReader source)
		{
			throw new NotSupportedException();
		}

		private object GetValue(object obj, int index)
		{
			PropertyInfo prop;
			if ((prop = (members[index] as PropertyInfo)) != null)
			{
				if (obj == null)
				{
					if (!Helpers.IsValueType(prop.PropertyType))
					{
						return null;
					}
					return Activator.CreateInstance(prop.PropertyType);
				}
				return prop.GetValue(obj, null);
			}
			FieldInfo field;
			if ((field = (members[index] as FieldInfo)) != null)
			{
				if (obj == null)
				{
					if (!Helpers.IsValueType(field.FieldType))
					{
						return null;
					}
					return Activator.CreateInstance(field.FieldType);
				}
				return field.GetValue(obj);
			}
			throw new InvalidOperationException();
		}

		public object Read(object value, ProtoReader source)
		{
			object[] values = new object[members.Length];
			bool invokeCtor = false;
			if (value == null)
			{
				invokeCtor = true;
			}
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = GetValue(value, i);
			}
			int field;
			while ((field = source.ReadFieldHeader()) > 0)
			{
				invokeCtor = true;
				if (field <= tails.Length)
				{
					IProtoSerializer tail = tails[field - 1];
					values[field - 1] = tails[field - 1].Read(tail.RequiresOldValue ? values[field - 1] : null, source);
				}
				else
				{
					source.SkipField();
				}
			}
			if (!invokeCtor)
			{
				return value;
			}
			return ctor.Invoke(values);
		}

		public void Write(object value, ProtoWriter dest)
		{
			for (int i = 0; i < tails.Length; i++)
			{
				object val = GetValue(value, i);
				if (val != null)
				{
					tails[i].Write(val, dest);
				}
			}
		}

		private Type GetMemberType(int index)
		{
			Type result = Helpers.GetMemberType(members[index]);
			if (result == null)
			{
				throw new InvalidOperationException();
			}
			return result;
		}

		bool IProtoTypeSerializer.CanCreateInstance()
		{
			return false;
		}

		public void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			using (Local loc = ctx.GetLocalWithValue(ctor.DeclaringType, valueFrom))
			{
				for (int i = 0; i < tails.Length; i++)
				{
					Type type = GetMemberType(i);
					ctx.LoadAddress(loc, ExpectedType);
					if (members[i] is FieldInfo)
					{
						ctx.LoadValue((FieldInfo)members[i]);
					}
					else if (members[i] is PropertyInfo)
					{
						ctx.LoadValue((PropertyInfo)members[i]);
					}
					ctx.WriteNullCheckedTail(type, tails[i], null);
				}
			}
		}

		void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
		{
			throw new NotSupportedException();
		}

		public void EmitRead(CompilerContext ctx, Local incoming)
		{
			using (Local objValue = ctx.GetLocalWithValue(ExpectedType, incoming))
			{
				Local[] locals = new Local[members.Length];
				try
				{
					for (int n = 0; n < locals.Length; n++)
					{
						Type type = GetMemberType(n);
						bool store = true;
						locals[n] = new Local(ctx, type);
						if (!Helpers.IsValueType(ExpectedType))
						{
							if (Helpers.IsValueType(type))
							{
								switch (Helpers.GetTypeCode(type))
								{
								case ProtoTypeCode.Boolean:
								case ProtoTypeCode.SByte:
								case ProtoTypeCode.Byte:
								case ProtoTypeCode.Int16:
								case ProtoTypeCode.UInt16:
								case ProtoTypeCode.Int32:
								case ProtoTypeCode.UInt32:
									ctx.LoadValue(0);
									break;
								case ProtoTypeCode.Int64:
								case ProtoTypeCode.UInt64:
									ctx.LoadValue(0L);
									break;
								case ProtoTypeCode.Single:
									ctx.LoadValue(0f);
									break;
								case ProtoTypeCode.Double:
									ctx.LoadValue(0.0);
									break;
								case ProtoTypeCode.Decimal:
									ctx.LoadValue(0m);
									break;
								case ProtoTypeCode.Guid:
									ctx.LoadValue(Guid.Empty);
									break;
								default:
									ctx.LoadAddress(locals[n], type);
									ctx.EmitCtor(type);
									store = false;
									break;
								}
							}
							else
							{
								ctx.LoadNullRef();
							}
							if (store)
							{
								ctx.StoreValue(locals[n]);
							}
						}
					}
					CodeLabel skipOld = Helpers.IsValueType(ExpectedType) ? default(CodeLabel) : ctx.DefineLabel();
					if (!Helpers.IsValueType(ExpectedType))
					{
						ctx.LoadAddress(objValue, ExpectedType);
						ctx.BranchIfFalse(skipOld, @short: false);
					}
					for (int m = 0; m < members.Length; m++)
					{
						ctx.LoadAddress(objValue, ExpectedType);
						if (members[m] is FieldInfo)
						{
							ctx.LoadValue((FieldInfo)members[m]);
						}
						else if (members[m] is PropertyInfo)
						{
							ctx.LoadValue((PropertyInfo)members[m]);
						}
						ctx.StoreValue(locals[m]);
					}
					if (!Helpers.IsValueType(ExpectedType))
					{
						ctx.MarkLabel(skipOld);
					}
					using (Local fieldNumber = new Local(ctx, ctx.MapType(typeof(int))))
					{
						CodeLabel @continue = ctx.DefineLabel();
						CodeLabel processField = ctx.DefineLabel();
						CodeLabel notRecognised = ctx.DefineLabel();
						ctx.Branch(@continue, @short: false);
						CodeLabel[] handlers = new CodeLabel[members.Length];
						for (int l = 0; l < members.Length; l++)
						{
							handlers[l] = ctx.DefineLabel();
						}
						ctx.MarkLabel(processField);
						ctx.LoadValue(fieldNumber);
						ctx.LoadValue(1);
						ctx.Subtract();
						ctx.Switch(handlers);
						ctx.Branch(notRecognised, @short: false);
						for (int k = 0; k < handlers.Length; k++)
						{
							ctx.MarkLabel(handlers[k]);
							IProtoSerializer tail = tails[k];
							Local oldValIfNeeded = tail.RequiresOldValue ? locals[k] : null;
							ctx.ReadNullCheckedTail(locals[k].Type, tail, oldValIfNeeded);
							if (tail.ReturnsValue)
							{
								if (Helpers.IsValueType(locals[k].Type))
								{
									ctx.StoreValue(locals[k]);
								}
								else
								{
									CodeLabel hasValue = ctx.DefineLabel();
									CodeLabel allDone = ctx.DefineLabel();
									ctx.CopyValue();
									ctx.BranchIfTrue(hasValue, @short: true);
									ctx.DiscardValue();
									ctx.Branch(allDone, @short: true);
									ctx.MarkLabel(hasValue);
									ctx.StoreValue(locals[k]);
									ctx.MarkLabel(allDone);
								}
							}
							ctx.Branch(@continue, @short: false);
						}
						ctx.MarkLabel(notRecognised);
						ctx.LoadReaderWriter();
						ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("SkipField"));
						ctx.MarkLabel(@continue);
						ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof(int)));
						ctx.CopyValue();
						ctx.StoreValue(fieldNumber);
						ctx.LoadValue(0);
						ctx.BranchIfGreater(processField, @short: false);
					}
					for (int j = 0; j < locals.Length; j++)
					{
						ctx.LoadValue(locals[j]);
					}
					ctx.EmitCtor(ctor);
					ctx.StoreValue(objValue);
				}
				finally
				{
					for (int i = 0; i < locals.Length; i++)
					{
						if (locals[i] != null)
						{
							locals[i].Dispose();
						}
					}
				}
			}
		}
	}
}
