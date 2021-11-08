using Fighting.Server.Games;
using Fighting.Server.Guild;
using Game.Logic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Fighting.Server.Rooms
{
	public class ProxyRoomMgr
	{
		private static readonly ILog ilog_0;

		public static readonly int THREAD_INTERVAL;

		public static readonly int PICK_UP_INTERVAL;

		public static readonly int CLEAR_ROOM_INTERVAL;

		private static bool startWithNpc;

		private static int serverId;

		private static Queue<IAction> queue_0;

		private static Thread thread_0;

		private static Dictionary<int, ProxyRoom> dictionary_0;

		private static int int_1;

		private static long long_0;

		private static long long_1;

		public static bool Setup()
		{
			thread_0 = new Thread(smethod_0);
			return true;
		}

		public static void Start()
		{
			if (!startWithNpc)
			{
				startWithNpc = true;
				thread_0.Start();
			}
		}

		public static void Stop()
		{
			if (startWithNpc)
			{
				startWithNpc = false;
				thread_0.Join();
			}
		}

		public static void AddAction(IAction action)
		{
			lock (queue_0)
			{
				queue_0.Enqueue(action);
			}
		}

		private static void smethod_0()
		{
			long num = 0L;
			long_1 = TickHelper.GetTickCount();
			long_0 = TickHelper.GetTickCount();
			while (startWithNpc)
			{
				long tickCount = TickHelper.GetTickCount();
				try
				{
					smethod_1();
					if (long_0 <= tickCount)
					{
						long_0 += PICK_UP_INTERVAL;
						smethod_2(tickCount);
					}
					if (long_1 <= tickCount)
					{
						long_1 += CLEAR_ROOM_INTERVAL;
						smethod_4(tickCount);
						smethod_5();
					}
				}
				catch (Exception exception)
				{
					ilog_0.Error("Room Mgr Thread Error:", exception);
				}
				long tickCount2 = TickHelper.GetTickCount();
				num += THREAD_INTERVAL - (tickCount2 - tickCount);
				if (num > 0)
				{
					Thread.Sleep((int)num);
					num = 0L;
				}
				else if (num < -1000)
				{
					ilog_0.WarnFormat("Room Mgr is delay {0} ms!", num);
					num += 1000;
				}
			}
		}

		private static void smethod_1()
		{
			IAction[] array = null;
			lock (queue_0)
			{
				if (queue_0.Count > 0)
				{
					array = new IAction[queue_0.Count];
					queue_0.CopyTo(array, 0);
					queue_0.Clear();
				}
			}
			if (array != null)
			{
				IAction[] array2 = array;
				foreach (IAction action in array2)
				{
					try
					{
						action.Execute();
					}
					catch (Exception exception)
					{
						ilog_0.Error("RoomMgr execute action error:", exception);
					}
				}
			}
		}

		private static void smethod_2(long long_2)
		{
			List<ProxyRoom> waitMatchRoomUnsafe = GetWaitMatchRoomUnsafe();
			foreach (ProxyRoom item in waitMatchRoomUnsafe)
			{
				int num = int.MinValue;
				ProxyRoom proxyRoom = null;
				if (!item.IsPlaying)
				{
					if (item.RoomType == eRoomType.Match)
					{
						switch (item.GameType)
						{
							case eGameType.Guild:
								foreach (ProxyRoom item2 in waitMatchRoomUnsafe)
								{
									if ((item2.GuildId == 0 || item2.GuildId != item.GuildId) && item2 != item && item2.GameType == eGameType.Guild && item.GameType == eGameType.Guild && !item2.IsPlaying && item2.PlayerCount == item.PlayerCount && !item.isAutoBot && !item2.isAutoBot && item.ZoneId == item2.ZoneId)
									{
										int num7 = GuildMgr.FindGuildRelationShip(item.GuildId, item2.GuildId) + 1;
										int gameType2 = (int)item2.GameType;
										int num8 = Math.Abs(item.FightPower - item2.FightPower);
										int num9 = Math.Abs(item.AvgLevel - item2.AvgLevel);
										int num10 = 10000;
										if (num7 * num10 + gameType2 * 1000 + num8 + num9 > num)
										{
											proxyRoom = item2;
										}
									}
								}
								break;
							case eGameType.ALL:
								foreach (ProxyRoom item3 in waitMatchRoomUnsafe)
								{
									if ((item3.GuildId == 0 || item3.GuildId != item.GuildId) && item3 != item && !item3.IsPlaying && item3.PlayerCount == item.PlayerCount)
									{
										int num3 = GuildMgr.FindGuildRelationShip(item.GuildId, item3.GuildId) + 1;
										int gameType = (int)item3.GameType;
										int num4 = Math.Abs(item.AvgLevel - item3.AvgLevel);
										int num5 = Math.Abs(item.FightPower - item3.FightPower);
										int num6 = 10000;
										if (num3 * num6 + gameType * 1000 + num5 + num4 > num)
										{
											proxyRoom = item3;
										}
									}
								}
								break;
							default:
								if (!item.isAutoBot && !item.startWithNpc)
								{
									ProxyRoom mathRoomUnsafeWithResult = GetMathRoomUnsafeWithResult(item);
									if (mathRoomUnsafeWithResult != null)
									{
										proxyRoom = mathRoomUnsafeWithResult;
										Console.WriteLine("StartMatch in rate: {0}% FP and ratelevel: {1}", item.PickUpRate, item.PickUpRateLevel);
									}
									else
									{
										int num2 = ++item.PickUpRateLevel;
										item.PickUpRate += 10;
									}
								}
								break;
						}
					}
					if (proxyRoom != null)
					{
						smethod_6(item, proxyRoom);
					}
					else
					{
						if (!item.IsCrossZone)
						{
							if (item.PickUpCount >= 4 && !item.startWithNpc && !item.isAutoBot && item.RoomType == eRoomType.Match && item.PlayerCount == 1)
							{
								item.startWithNpc = true;
								item.Client.SendBeginFightNpc(item.selfId, 0, (int)item.GameType, item.NpcId);
								Console.WriteLine("Call AutoBot No.{0}", item.NpcId);
							}
							else if (item.startWithNpc && !item.isAutoBot)
							{
								bool flag = false;
								foreach (ProxyRoom item4 in waitMatchRoomUnsafe)
								{
									if (item4 != item && item4.PlayerCount == item.PlayerCount && !item4.IsPlaying && item4.isAutoBot && item.NpcId == item4.NpcId)
									{
										flag = true;
										Console.WriteLine("Start fight with AutoBot No.{0}. RoomType: {1}, GameType: {2}", item.NpcId, item.RoomType, item.GameType);
										smethod_6(item, item4);
										break;
									}
								}
								if (!flag)
								{
									item.PickUpNPCTotal++;
									Console.WriteLine("Fight with AutoBot No.{0} - Step: {1} is error no room", item.NpcId, item.PickUpNPCTotal);
									if (item.PickUpNPCTotal > 3)
									{
										item.startWithNpc = false;
										item.PickUpNPCTotal = 0;
									}
								}
							}
						}
						if (item.isAutoBot && !item.IsPlaying)
						{
							item.PickUpCount--;
						}
						else
						{
							item.PickUpCount++;
						}
					}
				}
			}
		}

		private static void smethod_3(long long_2)
		{
			List<ProxyRoom> waitMatchRoomUnsafe = GetWaitMatchRoomUnsafe();
			foreach (ProxyRoom item in waitMatchRoomUnsafe)
			{
				int num = int.MinValue;
				int num2 = 2;
				ProxyRoom proxyRoom = null;
				if (item.IsPlaying)
				{
					break;
				}
				if (item.GameType == eGameType.ALL)
				{
					foreach (ProxyRoom item2 in waitMatchRoomUnsafe)
					{
						if ((item2.GuildId == 0 || item2.GuildId != item.GuildId) && item2 != item && !item2.IsPlaying && item2.PlayerCount == item.PlayerCount)
						{
							int num3 = GuildMgr.FindGuildRelationShip(item.GuildId, item2.GuildId) + 1;
							int gameType = (int)item2.GameType;
							int num4 = Math.Abs(item.AvgLevel - item2.AvgLevel);
							int num5 = Math.Abs(item.FightPower - item2.FightPower);
							int num6 = 10000;
							if (num3 * num6 + gameType * 1000 + num5 + num4 > num)
							{
								proxyRoom = item2;
							}
						}
					}
				}
				else if (item.GameType == eGameType.Guild)
				{
					foreach (ProxyRoom item3 in waitMatchRoomUnsafe)
					{
						if ((item3.GuildId == 0 || item3.GuildId != item.GuildId) && item3 != item && item3.GameType == eGameType.Guild && !item3.IsPlaying && item3.PlayerCount == item.PlayerCount && !item.isAutoBot && !item3.isAutoBot)
						{
							int num7 = GuildMgr.FindGuildRelationShip(item.GuildId, item3.GuildId) + 1;
							int gameType2 = (int)item3.GameType;
							int num8 = Math.Abs(item.FightPower - item3.FightPower);
							int num9 = Math.Abs(item.AvgLevel - item3.AvgLevel);
							int num10 = 10000;
							if (num7 * num10 + gameType2 * 1000 + num8 + num9 > num)
							{
								proxyRoom = item3;
							}
						}
					}
				}
				else
				{
					foreach (ProxyRoom item4 in waitMatchRoomUnsafe)
					{
						if (item4 != item && !item.isAutoBot && !item4.isAutoBot && item4.GameType != eGameType.Guild && !item4.IsPlaying && item4.PlayerCount == item.PlayerCount && item4.IsCrossZone == item.IsCrossZone && (!item4.IsCrossZone || item4.ZoneId != item.ZoneId) && ((item.AvgLevel <= 40 && item4.AvgLevel <= 40) || (item.AvgLevel > 40 && item4.AvgLevel > 40)))
						{
							proxyRoom = item4;
						}
					}
				}
				if (proxyRoom != null)
				{
					smethod_6(item, proxyRoom);
				}
				else if (!item.IsCrossZone)
				{
					if (item.PickUpCount == num2 && !item.startWithNpc && !item.isAutoBot && item.RoomType == eRoomType.Match)
					{
						item.startWithNpc = true;
						item.Client.SendBeginFightNpc(item.selfId, 0, (int)item.GameType, item.NpcId);
						Console.WriteLine("Call AutoBot No.{0}", item.NpcId);
					}
					else if (item.PickUpCount > num2 && item.startWithNpc && !item.isAutoBot)
					{
						foreach (ProxyRoom item5 in waitMatchRoomUnsafe)
						{
							if (item5 != item && item5.PlayerCount == item.PlayerCount && !item5.IsPlaying && item5.isAutoBot && item.NpcId == item5.NpcId)
							{
								Console.WriteLine("Start fight with AutoBot No.{0}", item.NpcId);
								smethod_6(item, item5);
							}
						}
					}
					if (item.isAutoBot && !item.IsPlaying)
					{
						item.PickUpCount--;
					}
					else
					{
						item.PickUpCount++;
					}
				}
			}
		}

		private static int WCSHKRMMIU(object object_0, object object_1)
		{
			if (((ProxyRoom)object_0).PickUpCount < 3)
			{
				int num = Math.Abs(((ProxyRoom)object_0).FightPower - ((ProxyRoom)object_1).FightPower);
				if ((((ProxyRoom)object_0).FightPower.ToString().Length < 5 && ((ProxyRoom)object_1).FightPower.ToString().Length < 5 && num < 10000) || (((ProxyRoom)object_0).FightPower.ToString().Length > 5 && ((ProxyRoom)object_1).FightPower.ToString().Length > 5 && num < 100000) || (((ProxyRoom)object_0).FightPower.ToString().Length > 6 && ((ProxyRoom)object_1).FightPower.ToString().Length > 6 && num < 600000))
				{
					return 1;
				}
			}
			return Math.Abs(((ProxyRoom)object_0).AvgLevel - ((ProxyRoom)object_1).AvgLevel);
		}

		private static void smethod_4(long long_2)
		{
			List<ProxyRoom> list = new List<ProxyRoom>();
			foreach (ProxyRoom value in dictionary_0.Values)
			{
				if (!value.IsPlaying && value.Game != null)
				{
					list.Add(value);
				}
			}
			foreach (ProxyRoom item in list)
			{
				dictionary_0.Remove(item.RoomId);
				try
				{
					item.Dispose();
				}
				catch (Exception exception)
				{
					ilog_0.Error("Room dispose error:", exception);
				}
			}
		}

		private static void smethod_5()
		{
			List<ProxyRoom> list = new List<ProxyRoom>();
			foreach (ProxyRoom value in dictionary_0.Values)
			{
				if (!value.IsPlaying && value.PickUpCount < -1)
				{
					list.Add(value);
				}
			}
			foreach (ProxyRoom item in list)
			{
				dictionary_0.Remove(item.RoomId);
				try
				{
					item.Dispose();
				}
				catch (Exception exception)
				{
					ilog_0.Error("Room dispose error:", exception);
				}
			}
		}

		private static void smethod_6(ProxyRoom proxyRoom_0, ProxyRoom proxyRoom_1)
		{
			int mapIndex = MapMgr.GetMapIndex(0, 0, serverId);
			eGameType gameType = eGameType.Free;
			eRoomType roomType = eRoomType.Match;
			if (proxyRoom_0.GameType == proxyRoom_1.GameType)
			{
				gameType = proxyRoom_0.GameType;
			}
			BaseGame baseGame = GameMgr.StartBattleGame(proxyRoom_0.GetPlayers(), proxyRoom_0, proxyRoom_1.GetPlayers(), proxyRoom_1, mapIndex, roomType, gameType, 2);
			if (baseGame != null)
			{
				proxyRoom_1.StartGame(baseGame);
				proxyRoom_0.StartGame(baseGame);
				if (baseGame.GameType == eGameType.Guild)
				{
					proxyRoom_0.Client.SendConsortiaAlly(proxyRoom_0.GetPlayers()[0].PlayerCharacter.ConsortiaID, proxyRoom_1.GetPlayers()[0].PlayerCharacter.ConsortiaID, baseGame.Id);
				}
			}
		}

		public static void StartWithNpcUnsafe(ProxyRoom room)
		{
			int npcId = room.NpcId;
			ProxyRoom roomUnsafe = GetRoomUnsafe(room.RoomId);
			foreach (ProxyRoom item in GetWaitMatchRoomUnsafe())
			{
				if (item.isAutoBot && !item.IsPlaying && item.Game == null && item.NpcId == npcId)
				{
					Console.WriteLine("Start fight with AutoBot or VPlayer No.{0} ", npcId);
					smethod_6(roomUnsafe, item);
				}
			}
		}

		public static bool AddRoomUnsafe(ProxyRoom room)
		{
			if (dictionary_0.ContainsKey(room.RoomId))
			{
				return false;
			}
			dictionary_0.Add(room.RoomId, room);
			return true;
		}

		public static bool RemoveRoomUnsafe(int roomId)
		{
			if (!dictionary_0.ContainsKey(roomId))
			{
				return false;
			}
			dictionary_0.Remove(roomId);
			return true;
		}

		public static ProxyRoom GetRoomUnsafe(int roomId)
		{
			if (dictionary_0.ContainsKey(roomId))
			{
				return dictionary_0[roomId];
			}
			return null;
		}

		public static ProxyRoom[] GetAllRoom()
		{
			lock (dictionary_0)
			{
				return GetAllRoomUnsafe();
			}
		}

		public static ProxyRoom[] GetAllRoomUnsafe()
		{
			ProxyRoom[] array = new ProxyRoom[dictionary_0.Values.Count];
			dictionary_0.Values.CopyTo(array, 0);
			return array;
		}

		public static List<ProxyRoom> GetWaitMatchRoomUnsafe()
		{
			List<ProxyRoom> list = new List<ProxyRoom>();
			foreach (ProxyRoom value in dictionary_0.Values)
			{
				if (!value.IsPlaying && value.Game == null)
				{
					list.Add(value);
				}
			}
			return list;
		}

		public static List<ProxyRoom> GetWaitMatchRoomWithoutBotUnsafe(ProxyRoom roomCompare)
		{
			List<ProxyRoom> list = new List<ProxyRoom>();
			foreach (ProxyRoom value in dictionary_0.Values)
			{
				if (!value.IsPlaying && value.Game == null && !value.isAutoBot && !value.startWithNpc && value.IsCrossZone == roomCompare.IsCrossZone && (value.IsCrossZone || value.ZoneId == roomCompare.ZoneId))
				{
					list.Add(value);
				}
			}
			return list;
		}

		public static ProxyRoom GetMathRoomUnsafeWithResult(ProxyRoom roomCompare)
		{
			List<ProxyRoom> list = new List<ProxyRoom>();
			bool flag = false;
			List<ProxyRoom> waitMatchRoomWithoutBotUnsafe = GetWaitMatchRoomWithoutBotUnsafe(roomCompare);
			foreach (ProxyRoom item in waitMatchRoomWithoutBotUnsafe)
			{
				if (!item.IsPlaying && item.Game == null && item != roomCompare && (item.PickUpRateLevel >= roomCompare.PickUpRateLevel || roomCompare.PickUpRateLevel == 1) && item.PlayerCount == roomCompare.PlayerCount)
				{
					int num = roomCompare.AvgLevel - roomCompare.PickUpRateLevel;
					int num2 = roomCompare.AvgLevel + roomCompare.PickUpRateLevel;
					if (item.AvgLevel >= num && item.AvgLevel <= num2)
					{
						list.Add(item);
						flag = true;
					}
				}
			}
			if (list.Count == 0)
			{
				foreach (ProxyRoom item2 in waitMatchRoomWithoutBotUnsafe)
				{
					if (!item2.IsPlaying && item2.Game == null && item2 != roomCompare && (item2.PickUpRate >= roomCompare.PickUpRate || roomCompare.PickUpRate == 1) && item2.PlayerCount == roomCompare.PlayerCount)
					{
						int num3 = roomCompare.FightPower - roomCompare.FightPower / 100 * roomCompare.PickUpRate;
						int num4 = roomCompare.FightPower + roomCompare.FightPower / 100 * roomCompare.PickUpRate;
						if (item2.FightPower >= num3 && item2.FightPower <= num4)
						{
							list.Add(item2);
						}
					}
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			List<ProxyRoom> obj = flag ? (from a in list
										  orderby a.AvgLevel
										  select a).ToList() : (from a in list
																orderby a.FightPower
																select a).ToList();
			return obj[obj.Count / 2];
		}

		public static int NextRoomId()
		{
			return Interlocked.Increment(ref int_1);
		}

		public static void AddRoom(ProxyRoom room)
		{
			AddAction(new AddRoomAction(room));
		}

		public static void RemoveRoom(ProxyRoom room)
		{
			AddAction(new RemoveRoomAction(room));
		}

		static ProxyRoomMgr()
		{
			ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			THREAD_INTERVAL = 40;
			PICK_UP_INTERVAL = 5000;
			CLEAR_ROOM_INTERVAL = 60;
			startWithNpc = false;
			serverId = 1;
			queue_0 = new Queue<IAction>();
			dictionary_0 = new Dictionary<int, ProxyRoom>();
			int_1 = 0;
			long_0 = 0L;
			long_1 = 0L;
		}
	}
}
