using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bussiness.Managers
{
	public class ConsortiaExtraMgr
	{
		private static readonly ILog ilog_0;

		private static Dictionary<int, ConsortiaLevelInfo> dictionary_0;

		private static Dictionary<int, ConsortiaBuffTempInfo> dictionary_1;

		private static Dictionary<int, ConsortiaBossConfigInfo> dictionary_2;

		private static Dictionary<int, ConsortiaBadgeConfigInfo> dictionary_3;

		private static ReaderWriterLock readerWriterLock_0;

		private static ThreadSafeRandom threadSafeRandom_0;

		private static int int_0;

		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ConsortiaLevelInfo> VDAEVuh8JIiPH5u2QO = new Dictionary<int, ConsortiaLevelInfo>();
				Dictionary<int, ConsortiaBuffTempInfo> uVEcJgIYrOMpJAq3U5 = new Dictionary<int, ConsortiaBuffTempInfo>();
				Dictionary<int, ConsortiaBadgeConfigInfo> dictionary = smethod_0();
				if (OvbRxniGbs(VDAEVuh8JIiPH5u2QO, uVEcJgIYrOMpJAq3U5))
				{
					readerWriterLock_0.AcquireWriterLock(-1);
					try
					{
						dictionary_0 = VDAEVuh8JIiPH5u2QO;
						dictionary_1 = uVEcJgIYrOMpJAq3U5;
						if (dictionary.Values.Count > 0)
						{
							Interlocked.Exchange(ref dictionary_3, dictionary);
						}
						return true;
					}
					catch
					{
					}
					finally
					{
						readerWriterLock_0.ReleaseWriterLock();
					}
				}
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("ConsortiaExtraMgr", ex);
				}
			}
			return false;
		}

		public static bool Init()
		{
			try
			{
				return ReLoad();
			}
			catch (Exception ex)
			{
				if (ilog_0.IsErrorEnabled)
				{
					ilog_0.Error("ConsortiaExtraMgr", ex);
				}
				return false;
			}
		}

		private static Dictionary<int, ConsortiaBadgeConfigInfo> smethod_0()
		{
			Dictionary<int, ConsortiaBadgeConfigInfo> dictionary = new Dictionary<int, ConsortiaBadgeConfigInfo>();
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				ConsortiaBadgeConfigInfo[] allConsortiaBadgeConfig = produceBussiness.GetAllConsortiaBadgeConfig();
				foreach (ConsortiaBadgeConfigInfo consortiaBadgeConfigInfo in allConsortiaBadgeConfig)
				{
					if (!dictionary.ContainsKey(consortiaBadgeConfigInfo.BadgeID))
					{
						dictionary.Add(consortiaBadgeConfigInfo.BadgeID, consortiaBadgeConfigInfo);
					}
				}
				return dictionary;
			}
		}

		private static bool OvbRxniGbs(Dictionary<int, ConsortiaLevelInfo> VDAEVuh8JIiPH5u2QO, Dictionary<int, ConsortiaBuffTempInfo> uVEcJgIYrOMpJAq3U5)
		{
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				ConsortiaLevelInfo[] allConsortiaLevel = produceBussiness.GetAllConsortiaLevel();
				foreach (ConsortiaLevelInfo consortiaLevelInfo in allConsortiaLevel)
				{
					if (!VDAEVuh8JIiPH5u2QO.ContainsKey(consortiaLevelInfo.Level))
					{
						VDAEVuh8JIiPH5u2QO.Add(consortiaLevelInfo.Level, consortiaLevelInfo);
					}
				}
				ConsortiaBuffTempInfo[] allConsortiaBuffTemp = produceBussiness.GetAllConsortiaBuffTemp();
				foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in allConsortiaBuffTemp)
				{
					if (!uVEcJgIYrOMpJAq3U5.ContainsKey(consortiaBuffTempInfo.id))
					{
						uVEcJgIYrOMpJAq3U5.Add(consortiaBuffTempInfo.id, consortiaBuffTempInfo);
					}
				}
			}
			return true;
		}

		public static ConsortiaBadgeConfigInfo FindConsortiaBadgeConfig(int id)
		{
			if (dictionary_3.ContainsKey(id))
			{
				return dictionary_3[id];
			}
			return null;
		}

		public static ConsortiaBossConfigInfo FindConsortiaBossInfo(int level)
		{
			readerWriterLock_0.AcquireReaderLock(-1);
			try
			{
				if (dictionary_2.ContainsKey(level))
				{
					return dictionary_2[level];
				}
			}
			catch
			{
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
			return null;
		}

		public static ConsortiaLevelInfo FindConsortiaLevelInfo(int level)
		{
			readerWriterLock_0.AcquireReaderLock(-1);
			try
			{
				if (dictionary_0.ContainsKey(level))
				{
					return dictionary_0[level];
				}
			}
			catch
			{
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
			return null;
		}

		public static ConsortiaBuffTempInfo FindConsortiaBuffInfo(int id)
		{
			readerWriterLock_0.AcquireReaderLock(-1);
			try
			{
				if (dictionary_1.ContainsKey(id))
				{
					return dictionary_1[id];
				}
			}
			catch
			{
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
			return null;
		}

		public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff()
		{
			readerWriterLock_0.AcquireReaderLock(-1);
			List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
			try
			{
				foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in dictionary_1.Values)
				{
					consortiaBuffTempInfoList.Add(consortiaBuffTempInfo);
				}
				return consortiaBuffTempInfoList;
			}
			catch
			{
				return consortiaBuffTempInfoList;
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
		}

		public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff(int level, int type)
		{
			readerWriterLock_0.AcquireReaderLock(-1);
			List<ConsortiaBuffTempInfo> consortiaBuffTempInfoList = new List<ConsortiaBuffTempInfo>();
			try
			{
				foreach (ConsortiaBuffTempInfo consortiaBuffTempInfo in dictionary_1.Values)
				{
					if (consortiaBuffTempInfo.level == level && consortiaBuffTempInfo.type == type)
					{
						consortiaBuffTempInfoList.Add(consortiaBuffTempInfo);
					}
				}
				return consortiaBuffTempInfoList;
			}
			catch
			{
				return consortiaBuffTempInfoList;
			}
			finally
			{
				readerWriterLock_0.ReleaseReaderLock();
			}
		}

		static ConsortiaExtraMgr()
		{
			ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			dictionary_3 = new Dictionary<int, ConsortiaBadgeConfigInfo>();
			readerWriterLock_0 = new ReaderWriterLock();
			int_0 = 10000;
		}
	}
}
