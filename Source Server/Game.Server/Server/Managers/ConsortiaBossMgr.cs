// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ConsortiaBossMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class ConsortiaBossMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
    private static Dictionary<int, ConsortiaInfo> m_consortias = new Dictionary<int, ConsortiaInfo>();

    public static bool AddConsortia(ConsortiaInfo consortia)
    {
      GSPacketIn packet = new GSPacketIn((short) 180);
      packet.WriteInt(consortia.ConsortiaID);
      packet.WriteInt(consortia.ChairmanID);
      packet.WriteByte((byte) consortia.bossState);
      packet.WriteDateTime(consortia.endTime);
      packet.WriteInt(consortia.extendAvailableNum);
      packet.WriteInt(consortia.callBossLevel);
      packet.WriteInt(consortia.Level);
      packet.WriteInt(consortia.SmithLevel);
      packet.WriteInt(consortia.StoreLevel);
      packet.WriteInt(consortia.SkillLevel);
      packet.WriteInt(consortia.Riches);
      packet.WriteDateTime(consortia.LastOpenBoss);
      GameServer.Instance.LoginServer.SendPacket(packet);
      return true;
    }

    public static bool AddConsortia(int consortiaId, ConsortiaInfo consortia)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return false;
        ConsortiaBossMgr.m_consortias.Add(consortiaId, consortia);
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static void CreateBoss(ConsortiaInfo consortia, int npcId)
    {
      GSPacketIn packet = new GSPacketIn((short) 183);
      packet.WriteInt(consortia.ConsortiaID);
      packet.WriteByte((byte) consortia.bossState);
      packet.WriteDateTime(consortia.endTime);
      packet.WriteDateTime(consortia.LastOpenBoss);
      int val = 15000000;
      NpcInfo npcInfoById = NPCInfoMgr.GetNpcInfoById(npcId);
      if (npcInfoById != null)
        val = npcInfoById.Blood;
      packet.WriteInt(val);
      GameServer.Instance.LoginServer.SendPacket(packet);
    }

    public static void ExtendAvailable(int consortiaId, int riches)
    {
      GSPacketIn packet = new GSPacketIn((short) 182);
      packet.WriteInt(consortiaId);
      packet.WriteInt(riches);
      GameServer.Instance.LoginServer.SendPacket(packet);
    }

    public static long GetConsortiaBossTotalDame(int consortiaId)
    {
      if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
        return 0;
      long num = ConsortiaBossMgr.m_consortias[consortiaId].TotalAllMemberDame;
      long maxBlood = ConsortiaBossMgr.m_consortias[consortiaId].MaxBlood;
      if (num > maxBlood)
        num = maxBlood - 1000L;
      return num;
    }

    public static ConsortiaInfo GetConsortiaById(int consortiaId)
    {
      ConsortiaInfo consortiaInfo = (ConsortiaInfo) null;
      ConsortiaBossMgr.m_clientLocker.AcquireReaderLock(10000);
      try
      {
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          consortiaInfo = ConsortiaBossMgr.m_consortias[consortiaId];
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseReaderLock();
      }
      return consortiaInfo;
    }

    public static bool GetConsortiaExit(int consortiaId)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireReaderLock(10000);
      try
      {
        return ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId);
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseReaderLock();
      }
    }

    public static void reload(int consortiaId)
    {
      GSPacketIn packet = new GSPacketIn((short) 184);
      packet.WriteInt(consortiaId);
      GameServer.Instance.LoginServer.SendPacket(packet);
    }

    public static void SendConsortiaAward(int consortiaId)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          return;
        ConsortiaInfo consortia = ConsortiaBossMgr.m_consortias[consortiaId];
        int copyId = 50000 + consortia.callBossLevel;
        List<SqlDataProvider.Data.ItemInfo> info = (List<SqlDataProvider.Data.ItemInfo>) null;
        DropInventory.CopyAllDrop(copyId, ref info);
        int riches = 0;
        if (consortia.RankList == null)
          return;
        foreach (RankingPersonInfo rankingPersonInfo in consortia.RankList.Values)
        {
          if (consortia.IsBossDie)
          {
            string title = "Recompensas chefe da guilda";
            if (info != null)
              WorldEventMgr.SendItemsToMail(info, rankingPersonInfo.UserID, rankingPersonInfo.Name, title, (string) null);
            else
              Console.WriteLine("Boss Guild award error dropID {0} ", (object) copyId);
          }
          riches += rankingPersonInfo.Damage;
        }
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          consortiaBussiness.ConsortiaRichAdd(consortiaId, ref riches, 5, "Boss Guild");
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
    }

    public static void UpdateBlood(int consortiaId, int damage)
    {
      GSPacketIn packet = new GSPacketIn((short) 186);
      packet.WriteInt(consortiaId);
      packet.WriteInt(damage);
      GameServer.Instance.LoginServer.SendPacket(packet);
    }

    public static bool UpdateConsortia(ConsortiaInfo info)
    {
      ConsortiaBossMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        int consortiaId = info.ConsortiaID;
        if (ConsortiaBossMgr.m_consortias.ContainsKey(consortiaId))
          ConsortiaBossMgr.m_consortias[consortiaId] = info;
      }
      finally
      {
        ConsortiaBossMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static void UpdateRank(
      int consortiaId,
      int damage,
      int richer,
      int honor,
      string Nickname,
      int userID)
    {
      GSPacketIn packet = new GSPacketIn((short) 181);
      packet.WriteInt(consortiaId);
      packet.WriteInt(damage);
      packet.WriteInt(richer);
      packet.WriteInt(honor);
      packet.WriteString(Nickname);
      packet.WriteInt(userID);
      GameServer.Instance.LoginServer.SendPacket(packet);
    }
  }
}
