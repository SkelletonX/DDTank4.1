using log4net;
using System;
using System.Reflection;
using System.Text;

namespace Game.Base
{
	public class WeakMulticastDelegate
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private WeakReference weakRef;

		private MethodInfo method;

		private WeakMulticastDelegate prev;

		public WeakMulticastDelegate(Delegate realDelegate)
		{
			if (realDelegate.Target != null)
			{
				weakRef = new WeakRef(realDelegate.Target);
			}
			method = realDelegate.Method;
		}

		public static WeakMulticastDelegate Combine(WeakMulticastDelegate weakDelegate, Delegate realDelegate)
		{
			if ((object)realDelegate == null)
			{
				return null;
			}
			if (weakDelegate != null)
			{
				return weakDelegate.Combine(realDelegate);
			}
			return new WeakMulticastDelegate(realDelegate);
		}

		public static WeakMulticastDelegate CombineUnique(WeakMulticastDelegate weakDelegate, Delegate realDelegate)
		{
			if ((object)realDelegate == null)
			{
				return null;
			}
			if (weakDelegate != null)
			{
				return weakDelegate.CombineUnique(realDelegate);
			}
			return new WeakMulticastDelegate(realDelegate);
		}

		private WeakMulticastDelegate Combine(Delegate realDelegate)
		{
			WeakMulticastDelegate weakMulticastDelegate = new WeakMulticastDelegate(realDelegate);
			weakMulticastDelegate.prev = prev;
			prev = weakMulticastDelegate;
			return this;
		}

		protected bool Equals(Delegate realDelegate)
		{
			if (weakRef == null)
			{
				if (realDelegate.Target == null && method == realDelegate.Method)
				{
					return true;
				}
				return false;
			}
			if (weakRef.Target == realDelegate.Target && method == realDelegate.Method)
			{
				return true;
			}
			return false;
		}

		private WeakMulticastDelegate CombineUnique(Delegate realDelegate)
		{
			bool flag = Equals(realDelegate);
			if (!flag && prev != null)
			{
				WeakMulticastDelegate weakMulticastDelegate = prev;
				while (!flag && weakMulticastDelegate != null)
				{
					if (weakMulticastDelegate.Equals(realDelegate))
					{
						flag = true;
					}
					weakMulticastDelegate = weakMulticastDelegate.prev;
				}
			}
			if (!flag)
			{
				return Combine(realDelegate);
			}
			return this;
		}

		public static WeakMulticastDelegate operator +(WeakMulticastDelegate d, Delegate realD)
		{
			return Combine(d, realD);
		}

		public static WeakMulticastDelegate operator -(WeakMulticastDelegate d, Delegate realD)
		{
			return Remove(d, realD);
		}

		public static WeakMulticastDelegate Remove(WeakMulticastDelegate weakDelegate, Delegate realDelegate)
		{
			if ((object)realDelegate == null || weakDelegate == null)
			{
				return null;
			}
			return weakDelegate.Remove(realDelegate);
		}

		private WeakMulticastDelegate Remove(Delegate realDelegate)
		{
			if (Equals(realDelegate))
			{
				return prev;
			}
			WeakMulticastDelegate weakMulticastDelegate = prev;
			WeakMulticastDelegate weakMulticastDelegate2 = this;
			while (weakMulticastDelegate != null)
			{
				if (weakMulticastDelegate.Equals(realDelegate))
				{
					weakMulticastDelegate2.prev = weakMulticastDelegate.prev;
					weakMulticastDelegate.prev = null;
					break;
				}
				weakMulticastDelegate2 = weakMulticastDelegate;
				weakMulticastDelegate = weakMulticastDelegate.prev;
			}
			return this;
		}

		public void Invoke(object[] args)
		{
			for (WeakMulticastDelegate weakMulticastDelegate = this; weakMulticastDelegate != null; weakMulticastDelegate = weakMulticastDelegate.prev)
			{
				int tickCount = Environment.TickCount;
				if (weakMulticastDelegate.weakRef == null)
				{
					weakMulticastDelegate.method.Invoke(null, args);
				}
				else if (weakMulticastDelegate.weakRef.IsAlive)
				{
					weakMulticastDelegate.method.Invoke(weakMulticastDelegate.weakRef.Target, args);
				}
				if (Environment.TickCount - tickCount > 500 && log.IsWarnEnabled)
				{
					log.Warn("Invoke took " + (Environment.TickCount - tickCount) + "ms! " + weakMulticastDelegate.ToString());
				}
			}
		}

		public void InvokeSafe(object[] args)
		{
			for (WeakMulticastDelegate weakMulticastDelegate = this; weakMulticastDelegate != null; weakMulticastDelegate = weakMulticastDelegate.prev)
			{
				int tickCount = Environment.TickCount;
				try
				{
					if (weakMulticastDelegate.weakRef == null)
					{
						weakMulticastDelegate.method.Invoke(null, args);
					}
					else if (weakMulticastDelegate.weakRef.IsAlive)
					{
						weakMulticastDelegate.method.Invoke(weakMulticastDelegate.weakRef.Target, args);
					}
				}
				catch (Exception exception)
				{
					if (log.IsErrorEnabled)
					{
						log.Error("InvokeSafe", exception);
					}
				}
				if (Environment.TickCount - tickCount > 500 && log.IsWarnEnabled)
				{
					log.Warn("InvokeSafe took " + (Environment.TickCount - tickCount) + "ms! " + weakMulticastDelegate.ToString());
				}
			}
		}

		public string Dump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			WeakMulticastDelegate weakMulticastDelegate = this;
			int num = 0;
			while (weakMulticastDelegate != null)
			{
				num++;
				if (weakMulticastDelegate.weakRef == null)
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(num);
					stringBuilder.Append(") ");
					stringBuilder.Append(weakMulticastDelegate.method.Name);
					stringBuilder.Append(Environment.NewLine);
				}
				else if (weakMulticastDelegate.weakRef.IsAlive)
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(num);
					stringBuilder.Append(") ");
					stringBuilder.Append(weakMulticastDelegate.weakRef.Target);
					stringBuilder.Append(".");
					stringBuilder.Append(weakMulticastDelegate.method.Name);
					stringBuilder.Append(Environment.NewLine);
				}
				else
				{
					stringBuilder.Append("\t");
					stringBuilder.Append(num);
					stringBuilder.Append(") INVALID.");
					stringBuilder.Append(weakMulticastDelegate.method.Name);
					stringBuilder.Append(Environment.NewLine);
				}
				weakMulticastDelegate = weakMulticastDelegate.prev;
			}
			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			Type type = null;
			if (method != null)
			{
				type = method.DeclaringType;
			}
			object obj = null;
			if (weakRef != null && weakRef.IsAlive)
			{
				obj = weakRef.Target;
			}
			return new StringBuilder(64).Append("method: ").Append((type == null) ? "(null)" : type.FullName).Append('.')
				.Append((method == null) ? "(null)" : method.Name)
				.Append(" target: ")
				.Append((obj == null) ? "null" : obj.ToString())
				.ToString();
		}
	}
}
