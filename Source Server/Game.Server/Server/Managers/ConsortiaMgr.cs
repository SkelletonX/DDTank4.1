// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.ConsortiaMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Managers
{
  public class ConsortiaMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<string, int> _ally;
    private static Dictionary<int, ConsortiaInfo> _consortia;
    private static Dictionary<int, ConsortiaBossConfigInfo> _consortiaBossConfigInfos;
    private static ReaderWriterLock m_lock;

    public static bool AddConsortia(int consortiaID)
    {
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaMgr._consortia.ContainsKey(consortiaID))
        {
          ConsortiaInfo consortiaInfo = new ConsortiaInfo()
          {
            BuildDate = DateTime.Now,
            Level = 1,
            IsExist = true,
            ConsortiaName = "",
            ConsortiaID = consortiaID
          };
          ConsortiaMgr._consortia.Add(consortiaID, consortiaInfo);
        }
      }
      catch (Exception ex)
      {
        ConsortiaMgr.log.Error((object) "ConsortiaUpGrade", ex);
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return false;
    }

    public static int CanConsortiaFight(int consortiaID1, int consortiaID2)
    {
      if (consortiaID1 == 0 || consortiaID2 == 0 || consortiaID1 == consortiaID2)
        return -1;
      ConsortiaInfo consortiaInfo1 = ConsortiaMgr.FindConsortiaInfo(consortiaID1);
      ConsortiaInfo consortiaInfo2 = ConsortiaMgr.FindConsortiaInfo(consortiaID2);
      if (consortiaInfo1 == null || consortiaInfo2 == null || consortiaInfo1.Level < 3 || consortiaInfo2.Level < 3)
        return -1;
      return ConsortiaMgr.FindConsortiaAlly(consortiaID1, consortiaID2);
    }

    public static int ConsortiaFight(
      int consortiaWin,
      int consortiaLose,
      Dictionary<int, Player> players,
      eRoomType roomType,
      eGameType gameClass,
      int totalKillHealth,
      int playercount)
    {
      if ((uint) roomType > 0U)
        return 0;
      int playerCount = playercount / 2;
      int riches = 0;
      int state = 2;
      int num1 = 1;
      int num2 = 3;
      if (gameClass == eGameType.Guild)
      {
        num2 = 10;
        num1 = (int) RateMgr.GetRate(eRateType.Offer_Rate);
      }
      float rate = RateMgr.GetRate(eRateType.Riches_Rate);
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (gameClass == eGameType.Free)
          playerCount = 0;
        else
          consortiaBussiness.ConsortiaFight(consortiaWin, consortiaLose, playerCount, out riches, state, totalKillHealth, rate);
        foreach (KeyValuePair<int, Player> player in players)
        {
          if (player.Value != null)
          {
            if (player.Value.PlayerDetail.PlayerCharacter.ConsortiaID == consortiaWin)
            {
              player.Value.PlayerDetail.AddOffer((playerCount + num2) * num1);
              player.Value.PlayerDetail.PlayerCharacter.RichesRob += riches;
            }
            else if (player.Value.PlayerDetail.PlayerCharacter.ConsortiaID == consortiaLose)
            {
              player.Value.PlayerDetail.AddOffer((int) Math.Round((double) playerCount * 0.5) * num1);
              player.Value.PlayerDetail.RemoveOffer(num2);
            }
          }
        }
      }
      return riches;
    }

    public static bool ConsortiaShopUpGrade(int consortiaID, int shopLevel)
    {
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaMgr._consortia.ContainsKey(consortiaID) && ConsortiaMgr._consortia[consortiaID].IsExist)
          ConsortiaMgr._consortia[consortiaID].ShopLevel = shopLevel;
      }
      catch (Exception ex)
      {
        ConsortiaMgr.log.Error((object) "ConsortiaUpGrade", ex);
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return false;
    }

    public static bool ConsortiaSmithUpGrade(int consortiaID, int smithLevel)
    {
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaMgr._consortia.ContainsKey(consortiaID) && ConsortiaMgr._consortia[consortiaID].IsExist)
          ConsortiaMgr._consortia[consortiaID].SmithLevel = smithLevel;
      }
      catch (Exception ex)
      {
        ConsortiaMgr.log.Error((object) "ConsortiaUpGrade", ex);
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return false;
    }

    public static bool ConsortiaStoreUpGrade(int consortiaID, int storeLevel)
    {
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaMgr._consortia.ContainsKey(consortiaID) && ConsortiaMgr._consortia[consortiaID].IsExist)
          ConsortiaMgr._consortia[consortiaID].StoreLevel = storeLevel;
      }
      catch (Exception ex)
      {
        ConsortiaMgr.log.Error((object) "ConsortiaUpGrade", ex);
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return false;
    }

    public static bool ConsortiaUpGrade(int consortiaID, int consortiaLevel)
    {
      bool flag = false;
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (ConsortiaMgr._consortia.ContainsKey(consortiaID) && ConsortiaMgr._consortia[consortiaID].IsExist)
        {
          ConsortiaMgr._consortia[consortiaID].Level = consortiaLevel;
          return flag;
        }
        ConsortiaInfo consortiaInfo = new ConsortiaInfo()
        {
          BuildDate = DateTime.Now,
          Level = consortiaLevel,
          IsExist = true
        };
        ConsortiaMgr._consortia.Add(consortiaID, consortiaInfo);
        return flag;
      }
      catch (Exception ex)
      {
        ConsortiaMgr.log.Error((object) nameof (ConsortiaUpGrade), ex);
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return flag;
    }

    public static int FindConsortiaAlly(int cosortiaID1, int consortiaID2)
    {
      if (cosortiaID1 == 0 || consortiaID2 == 0 || cosortiaID1 == consortiaID2)
        return -1;
      string key = cosortiaID1 >= consortiaID2 ? consortiaID2.ToString() + "&" + (object) cosortiaID1 : cosortiaID1.ToString() + "&" + (object) consortiaID2;
      ConsortiaMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ConsortiaMgr._ally.ContainsKey(key))
          return ConsortiaMgr._ally[key];
      }
      catch
      {
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseReaderLock();
      }
      return 0;
    }

    public static int FindConsortiaBossBossMaxLevel(int param1, ConsortiaInfo info)
    {
      int num = param1 != 0 ? param1 : info.Level + info.SmithLevel + info.ShopLevel + info.StoreLevel + info.SkillLevel;
      for (int count = ConsortiaMgr._consortiaBossConfigInfos.Count; count >= 0; --count)
      {
        if (num >= ConsortiaMgr._consortiaBossConfigInfos[count].Level)
          return count;
      }
      return 1;
    }

    public static ConsortiaBossConfigInfo FindConsortiaBossConfig(int level)
    {
      ConsortiaMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ConsortiaMgr._consortiaBossConfigInfos.ContainsKey(level))
          return ConsortiaMgr._consortiaBossConfigInfos[level];
      }
      catch
      {
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseReaderLock();
      }
      return (ConsortiaBossConfigInfo) null;
    }

    public static ConsortiaInfo FindConsortiaInfo(int consortiaID)
    {
      ConsortiaMgr.m_lock.AcquireReaderLock(10000);
      try
      {
        if (ConsortiaMgr._consortia.ContainsKey(consortiaID))
          return ConsortiaMgr._consortia[consortiaID];
      }
      catch
      {
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseReaderLock();
      }
      return (ConsortiaInfo) null;
    }

    private static int GetOffer(int state, eGameType gameType)
    {
      if ((uint) gameType > 0U)
      {
        if (gameType == eGameType.Guild)
        {
          switch (state)
          {
            case 0:
              return 5;
            case 1:
              return 0;
            case 2:
              return 10;
          }
        }
      }
      else
      {
        switch (state)
        {
          case 0:
            return 1;
          case 1:
            return 0;
          case 2:
            return 3;
        }
      }
      return 0;
    }

    public static int GetOffer(int cosortiaID1, int consortiaID2, eGameType gameType)
    {
      return ConsortiaMgr.GetOffer(ConsortiaMgr.FindConsortiaAlly(cosortiaID1, consortiaID2), gameType);
    }

    public static bool Init()
    {
      bool flag;
      try
      {
        ConsortiaMgr.m_lock = new ReaderWriterLock();
        ConsortiaMgr._ally = new Dictionary<string, int>();
        if (!ConsortiaMgr.Load(ConsortiaMgr._ally))
        {
          flag = false;
        }
        else
        {
          ConsortiaMgr._consortia = new Dictionary<int, ConsortiaInfo>();
          ConsortiaMgr._consortiaBossConfigInfos = new Dictionary<int, ConsortiaBossConfigInfo>();
          flag = ConsortiaMgr.LoadConsortia(ConsortiaMgr._consortia, ConsortiaMgr._consortiaBossConfigInfos);
        }
      }
      catch (Exception ex)
      {
        if (ConsortiaMgr.log.IsErrorEnabled)
          ConsortiaMgr.log.Error((object) nameof (ConsortiaMgr), ex);
        flag = false;
      }
      return flag;
    }

    public static int KillPlayer(
      GamePlayer win,
      GamePlayer lose,
      Dictionary<GamePlayer, Player> players,
      eRoomType roomType,
      eGameType gameClass)
    {
      if ((uint) roomType > 0U)
        return -1;
      int consortiaAlly = ConsortiaMgr.FindConsortiaAlly(win.PlayerCharacter.ConsortiaID, lose.PlayerCharacter.ConsortiaID);
      if (consortiaAlly != -1)
      {
        int offer = ConsortiaMgr.GetOffer(consortiaAlly, gameClass);
        if (lose.PlayerCharacter.Offer < offer)
          offer = lose.PlayerCharacter.Offer;
        if ((uint) offer > 0U)
        {
          players[win].GainOffer = offer;
          players[lose].GainOffer = -offer;
        }
      }
      return consortiaAlly;
    }

    private static bool Load(Dictionary<string, int> ally)
    {
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        foreach (ConsortiaAllyInfo consortiaAllyInfo in consortiaBussiness.GetConsortiaAllyAll())
        {
          if (consortiaAllyInfo.IsExist)
          {
            int num;
            string str;
            if (consortiaAllyInfo.Consortia1ID < consortiaAllyInfo.Consortia2ID)
            {
              num = consortiaAllyInfo.Consortia1ID;
              str = num.ToString() + "&" + (object) consortiaAllyInfo.Consortia2ID;
            }
            else
            {
              num = consortiaAllyInfo.Consortia2ID;
              str = num.ToString() + "&" + (object) consortiaAllyInfo.Consortia1ID;
            }
            string key = str;
            if (!ally.ContainsKey(key))
              ally.Add(key, consortiaAllyInfo.State);
          }
        }
      }
      return true;
    }

    private static bool LoadConsortia(
      Dictionary<int, ConsortiaInfo> consortia,
      Dictionary<int, ConsortiaBossConfigInfo> consortiaBossConfig)
    {
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        foreach (ConsortiaInfo consortiaInfo in consortiaBussiness.GetConsortiaAll())
        {
          if (consortiaInfo.IsExist && !consortia.ContainsKey(consortiaInfo.ConsortiaID))
            consortia.Add(consortiaInfo.ConsortiaID, consortiaInfo);
        }
        foreach (ConsortiaBossConfigInfo consortiaBossConfigInfo in consortiaBussiness.GetConsortiaBossConfigAll())
        {
          if (!consortiaBossConfig.ContainsKey(consortiaBossConfigInfo.BossLevel))
            consortiaBossConfig.Add(consortiaBossConfigInfo.BossLevel, consortiaBossConfigInfo);
        }
      }
      return true;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<string, int> ally = new Dictionary<string, int>();
        Dictionary<int, ConsortiaInfo> consortia = new Dictionary<int, ConsortiaInfo>();
        Dictionary<int, ConsortiaBossConfigInfo> consortiaBossConfig = new Dictionary<int, ConsortiaBossConfigInfo>();
        if (ConsortiaMgr.Load(ally) && ConsortiaMgr.LoadConsortia(consortia, consortiaBossConfig))
        {
          ConsortiaMgr.m_lock.AcquireWriterLock(-1);
          try
          {
            ConsortiaMgr._ally = ally;
            ConsortiaMgr._consortia = consortia;
            ConsortiaMgr._consortiaBossConfigInfos = consortiaBossConfig;
            return true;
          }
          catch
          {
          }
          finally
          {
            ConsortiaMgr.m_lock.ReleaseWriterLock();
          }
        }
      }
      catch (Exception ex)
      {
        if (ConsortiaMgr.log.IsErrorEnabled)
          ConsortiaMgr.log.Error((object) nameof (ConsortiaMgr), ex);
      }
      return false;
    }

    public static int UpdateConsortiaAlly(int cosortiaID1, int consortiaID2, int state)
    {
      string key = cosortiaID1 >= consortiaID2 ? consortiaID2.ToString() + "&" + (object) cosortiaID1 : cosortiaID1.ToString() + "&" + (object) consortiaID2;
      ConsortiaMgr.m_lock.AcquireWriterLock(-1);
      try
      {
        if (!ConsortiaMgr._ally.ContainsKey(key))
          ConsortiaMgr._ally.Add(key, state);
        else
          ConsortiaMgr._ally[key] = state;
      }
      catch
      {
      }
      finally
      {
        ConsortiaMgr.m_lock.ReleaseWriterLock();
      }
      return 0;
    }

    public static bool AddBuffConsortia(
      GamePlayer Player,
      ConsortiaBuffTempInfo ConsortiaBuffInfo,
      int consortiaId,
      int id,
      int validate)
    {
      switch (ConsortiaBuffInfo.group)
      {
        case 1:
          AbstractBuffer payBuffer1 = BufferList.CreatePayBuffer(101, ConsortiaBuffInfo.value, validate, id);
          if (payBuffer1 != null)
          {
            payBuffer1.Start(Player);
            break;
          }
          break;
        case 3:
          AbstractBuffer payBuffer2 = BufferList.CreatePayBuffer(103, ConsortiaBuffInfo.value, validate, id);
          if (payBuffer2 != null)
          {
            payBuffer2.Start(Player);
            break;
          }
          break;
        case 6:
          AbstractBuffer payBuffer3 = BufferList.CreatePayBuffer(106, ConsortiaBuffInfo.value, validate, id);
          if (payBuffer3 == null)
            return true;
          payBuffer3.Start(Player);
          return true;
        case 8:
          Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg2"));
          return false;
        case 11:
          AbstractBuffer payBuffer4 = BufferList.CreatePayBuffer(111, ConsortiaBuffInfo.value, validate, id);
          if (payBuffer4 == null)
            return true;
          payBuffer4.Start(Player);
          return true;
        case 12:
          AbstractBuffer payBuffer5 = BufferList.CreatePayBuffer(112, ConsortiaBuffInfo.value, validate, id);
          if (payBuffer5 == null)
            return true;
          payBuffer5.Start(Player);
          return true;
        default:
          using (PlayerBussiness playerBussiness = new PlayerBussiness())
          {
            ConsortiaUserInfo[] memberByConsortia = playerBussiness.GetAllMemberByConsortia(consortiaId);
            AbstractBuffer abstractBuffer = (AbstractBuffer) null;
            switch (ConsortiaBuffInfo.group)
            {
              case 2:
                abstractBuffer = BufferList.CreatePayBuffer(102, ConsortiaBuffInfo.value, validate, id);
                break;
              case 4:
                abstractBuffer = BufferList.CreatePayBuffer(104, ConsortiaBuffInfo.value, validate, id);
                break;
              case 5:
                abstractBuffer = BufferList.CreatePayBuffer(105, ConsortiaBuffInfo.value, validate, id);
                break;
              case 7:
                abstractBuffer = BufferList.CreatePayBuffer(107, ConsortiaBuffInfo.value, validate, id);
                break;
              case 9:
                abstractBuffer = BufferList.CreatePayBuffer(109, ConsortiaBuffInfo.value, validate, id);
                break;
              case 10:
                abstractBuffer = BufferList.CreatePayBuffer(110, ConsortiaBuffInfo.value, validate, id);
                break;
            }
            foreach (ConsortiaUserInfo consortiaUserInfo in memberByConsortia)
            {
              GamePlayer playerById = WorldMgr.GetPlayerById(consortiaUserInfo.UserID);
              if (playerById != null)
              {
                if (abstractBuffer != null)
                  abstractBuffer.Start(playerById);
                if (playerById != Player)
                  playerById.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Consortia.Msg3"));
              }
            }
            if (abstractBuffer != null)
            {
              ConsortiaBufferInfo info = playerBussiness.GetUserConsortiaBufferSingle(ConsortiaBuffInfo.id);
              if (info == null)
              {
                info = new ConsortiaBufferInfo();
                info.ConsortiaID = consortiaId;
                info.IsOpen = true;
                info.BufferID = ConsortiaBuffInfo.id;
                info.Type = abstractBuffer.Info.Type;
                info.Value = abstractBuffer.Info.Value;
                info.ValidDate = abstractBuffer.Info.ValidDate;
                info.BeginDate = abstractBuffer.Info.BeginDate;
              }
              else
              {
                info.BufferID = ConsortiaBuffInfo.id;
                info.Value = abstractBuffer.Info.Value;
                info.ValidDate += abstractBuffer.Info.ValidDate;
              }
              playerBussiness.SaveConsortiaBuffer(info);
              break;
            }
            break;
          }
      }
      return true;
    }
  }
}
