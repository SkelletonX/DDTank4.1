using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	internal class MapDecorator<TDictionary, TKey, TValue> : ProtoDecoratorBase where TDictionary : class, IDictionary<TKey, TValue>
	{
		private readonly Type concreteType;

		private readonly IProtoSerializer keyTail;

		private readonly int fieldNumber;

		private readonly WireType wireType;

		private static readonly MethodInfo indexerSet = GetIndexerSetter();

		private static readonly TKey DefaultKey = (typeof(TKey) == typeof(string)) ? ((TKey)(object)"") : default(TKey);

		private static readonly TValue DefaultValue = (typeof(TValue) == typeof(string)) ? ((TValue)(object)"") : default(TValue);

		public override Type ExpectedType => typeof(TDictionary);

		public override bool ReturnsValue => true;

		public override bool RequiresOldValue => AppendToCollection;

		private bool AppendToCollection
		{
			get;
		}

		internal MapDecorator(TypeModel model, Type concreteType, IProtoSerializer keyTail, IProtoSerializer valueTail, int fieldNumber, WireType wireType, WireType keyWireType, WireType valueWireType, bool overwriteList)
			: base((DefaultValue == null) ? ((ProtoDecoratorBase)new TagDecorator(2, valueWireType, strict: false, valueTail)) : ((ProtoDecoratorBase)new DefaultValueDecorator(model, DefaultValue, new TagDecorator(2, valueWireType, strict: false, valueTail))))
		{
			this.wireType = wireType;
			this.keyTail = new DefaultValueDecorator(model, DefaultKey, new TagDecorator(1, keyWireType, strict: false, keyTail));
			this.fieldNumber = fieldNumber;
			this.concreteType = (concreteType ?? typeof(TDictionary));
			if (keyTail.RequiresOldValue)
			{
				throw new InvalidOperationException("Key tail should not require the old value");
			}
			if (!keyTail.ReturnsValue)
			{
				throw new InvalidOperationException("Key tail should return a value");
			}
			if (!valueTail.ReturnsValue)
			{
				throw new InvalidOperationException("Value tail should return a value");
			}
			AppendToCollection = !overwriteList;
		}

		private static MethodInfo GetIndexerSetter()
		{
			PropertyInfo[] properties = typeof(TDictionary).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (PropertyInfo prop in properties)
			{
				if (prop.Name != "Item" || prop.PropertyType != typeof(TValue))
				{
					continue;
				}
				ParameterInfo[] args = prop.GetIndexParameters();
				if (args != null && args.Length == 1 && !(args[0].ParameterType != typeof(TKey)))
				{
					MethodInfo method = prop.GetSetMethod(nonPublic: true);
					if (method != null)
					{
						return method;
					}
				}
			}
			throw new InvalidOperationException("Unable to resolve indexer for map");
		}

		public override object Read(object untyped, ProtoReader source)
		{
			TDictionary typed = AppendToCollection ? ((TDictionary)untyped) : null;
			if (typed == null)
			{
				typed = (TDictionary)Activator.CreateInstance(concreteType);
			}
			do
			{
				TKey key = DefaultKey;
				TValue value = DefaultValue;
				SubItemToken token = ProtoReader.StartSubItem(source);
				int field;
				while ((field = source.ReadFieldHeader()) > 0)
				{
					switch (field)
					{
					case 1:
						key = (TKey)keyTail.Read(null, source);
						break;
					case 2:
						value = (TValue)Tail.Read(Tail.RequiresOldValue ? ((object)value) : null, source);
						break;
					default:
						source.SkipField();
						break;
					}
				}
				ProtoReader.EndSubItem(token, source);
				typed[key] = value;
			}
			while (source.TryReadFieldHeader(fieldNumber));
			return typed;
		}

		public override void Write(object untyped, ProtoWriter dest)
		{
			foreach (KeyValuePair<TKey, TValue> pair in (TDictionary)untyped)
			{
				ProtoWriter.WriteFieldHeader(fieldNumber, wireType, dest);
				SubItemToken token = ProtoWriter.StartSubItem(null, dest);
				if (pair.Key != null)
				{
					keyTail.Write(pair.Key, dest);
				}
				if (pair.Value != null)
				{
					Tail.Write(pair.Value, dest);
				}
				ProtoWriter.EndSubItem(token, dest);
			}
		}

		protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			Type itemType = typeof(KeyValuePair<TKey, TValue>);
			MethodInfo moveNext;
			MethodInfo current;
			MethodInfo getEnumerator = ListDecorator.GetEnumeratorInfo(ctx.Model, ExpectedType, itemType, out moveNext, out current);
			Type enumeratorType = getEnumerator.ReturnType;
			MethodInfo key = itemType.GetProperty("Key").GetGetMethod();
			MethodInfo value = itemType.GetProperty("Value").GetGetMethod();
			using (Local list = ctx.GetLocalWithValue(ExpectedType, valueFrom))
			{
				using (Local iter = new Local(ctx, enumeratorType))
				{
					using (Local token = new Local(ctx, typeof(SubItemToken)))
					{
						using (Local kvp = new Local(ctx, itemType))
						{
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
								if (itemType != ctx.MapType(typeof(object)) && current.ReturnType == ctx.MapType(typeof(object)))
								{
									ctx.CastFromObject(itemType);
								}
								ctx.StoreValue(kvp);
								ctx.LoadValue(fieldNumber);
								ctx.LoadValue((int)wireType);
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("WriteFieldHeader"));
								ctx.LoadNullRef();
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("StartSubItem"));
								ctx.StoreValue(token);
								ctx.LoadAddress(kvp, itemType);
								ctx.EmitCall(key, itemType);
								ctx.WriteNullCheckedTail(typeof(TKey), keyTail, null);
								ctx.LoadAddress(kvp, itemType);
								ctx.EmitCall(value, itemType);
								ctx.WriteNullCheckedTail(typeof(TValue), Tail, null);
								ctx.LoadValue(token);
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoWriter)).GetMethod("EndSubItem"));
								ctx.MarkLabel(next);
								ctx.LoadAddress(iter, enumeratorType);
								ctx.EmitCall(moveNext, enumeratorType);
								ctx.BranchIfTrue(body, @short: false);
							}
						}
					}
				}
			}
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local list = AppendToCollection ? ctx.GetLocalWithValue(ExpectedType, valueFrom) : new Local(ctx, typeof(TDictionary)))
			{
				using (Local token = new Local(ctx, typeof(SubItemToken)))
				{
					using (Local key = new Local(ctx, typeof(TKey)))
					{
						using (Local value = new Local(ctx, typeof(TValue)))
						{
							using (Local fieldNumber = new Local(ctx, ctx.MapType(typeof(int))))
							{
								if (!AppendToCollection)
								{
									ctx.LoadNullRef();
									ctx.StoreValue(list);
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
								CodeLabel redoFromStart = ctx.DefineLabel();
								ctx.MarkLabel(redoFromStart);
								if (typeof(TKey) == typeof(string))
								{
									ctx.LoadValue("");
									ctx.StoreValue(key);
								}
								else
								{
									ctx.InitLocal(typeof(TKey), key);
								}
								if (typeof(TValue) == typeof(string))
								{
									ctx.LoadValue("");
									ctx.StoreValue(value);
								}
								else
								{
									ctx.InitLocal(typeof(TValue), value);
								}
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("StartSubItem"));
								ctx.StoreValue(token);
								CodeLabel @continue = ctx.DefineLabel();
								CodeLabel processField = ctx.DefineLabel();
								ctx.Branch(@continue, @short: false);
								ctx.MarkLabel(processField);
								ctx.LoadValue(fieldNumber);
								CodeLabel @default = ctx.DefineLabel();
								CodeLabel one = ctx.DefineLabel();
								CodeLabel two = ctx.DefineLabel();
								ctx.Switch(new CodeLabel[3]
								{
									@default,
									one,
									two
								});
								ctx.MarkLabel(@default);
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("SkipField"));
								ctx.Branch(@continue, @short: false);
								ctx.MarkLabel(one);
								keyTail.EmitRead(ctx, null);
								ctx.StoreValue(key);
								ctx.Branch(@continue, @short: false);
								ctx.MarkLabel(two);
								Tail.EmitRead(ctx, Tail.RequiresOldValue ? value : null);
								ctx.StoreValue(value);
								ctx.MarkLabel(@continue);
								ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof(int)));
								ctx.CopyValue();
								ctx.StoreValue(fieldNumber);
								ctx.LoadValue(0);
								ctx.BranchIfGreater(processField, @short: false);
								ctx.LoadValue(token);
								ctx.LoadReaderWriter();
								ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("EndSubItem"));
								ctx.LoadAddress(list, ExpectedType);
								ctx.LoadValue(key);
								ctx.LoadValue(value);
								ctx.EmitCall(indexerSet);
								ctx.LoadReaderWriter();
								ctx.LoadValue(this.fieldNumber);
								ctx.EmitCall(ctx.MapType(typeof(ProtoReader)).GetMethod("TryReadFieldHeader"));
								ctx.BranchIfTrue(redoFromStart, @short: false);
								if (ReturnsValue)
								{
									ctx.LoadValue(list);
								}
							}
						}
					}
				}
			}
		}
	}
}
