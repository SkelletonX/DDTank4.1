// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PlayerActives
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
  public class PlayerActives
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected object m_lock;
    protected Timer _labyrinthTimer;
    protected GamePlayer m_player;
    private Random random_0;
    private bool bool_0;
    private int int_0;

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }

    public PlayerActives(GamePlayer player, bool saveTodb)
    {
      this.m_lock = new object();
      this.random_0 = new Random();
      this.int_0 = GameProperties.WarriorFamRaidTimeRemain;
      this.m_player = player;
      this.bool_0 = saveTodb;
    }

    private void method_0()
    {
      int num = 1000;
      if (this._labyrinthTimer == null)
        this._labyrinthTimer = new Timer(new TimerCallback(this.LabyrinthCheck), (object) null, num, num);
      else
        this._labyrinthTimer.Change(num, num);
    }

    protected void LabyrinthCheck(object sender)
    {
      try
      {
        int tickCount1 = Environment.TickCount;
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        this.UpdateLabyrinthTime();
        Thread.CurrentThread.Priority = priority;
        int tickCount2 = Environment.TickCount;
      }
      catch (Exception ex)
      {
        Console.WriteLine("LabyrinthCheck: " + (object) ex);
      }
    }

    public void StopLabyrinthTimer()
    {
      if (this._labyrinthTimer == null)
        return;
      this._labyrinthTimer.Change(-1, -1);
      this._labyrinthTimer.Dispose();
      this._labyrinthTimer = (Timer) null;
    }

    public void UpdateLabyrinthTime()
    {
      UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
      labyrinth.isCleanOut = true;
      labyrinth.isInGame = true;
      if (labyrinth.remainTime > 0 && labyrinth.currentRemainTime > 0)
      {
        --labyrinth.remainTime;
        --labyrinth.currentRemainTime;
        --this.int_0;
      }
      if (this.int_0 == 0)
      {
        this.method_1();
        this.int_0 = 120;
        ++labyrinth.currentFloor;
        if (labyrinth.currentFloor > labyrinth.myProgress)
        {
          labyrinth.currentFloor = labyrinth.myProgress;
          labyrinth.isCleanOut = false;
          labyrinth.isInGame = false;
          labyrinth.completeChallenge = false;
          labyrinth.remainTime = 0;
          labyrinth.currentRemainTime = 0;
          labyrinth.cleanOutAllTime = 0;
          this.StopLabyrinthTimer();
        }
      }
      this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
    }

    public void CleantOutLabyrinth()
    {
      this.method_0();
    }

    private void method_1()
    {
      int index = this.m_player.Labyrinth.currentFloor - 1;
      int exp = this.m_player.CreateExps()[index];
      string labyrinthGold = this.m_player.labyrinthGolds[index];
      int count = int.Parse(labyrinthGold.Split('|')[0]);
      int int_3 = int.Parse(labyrinthGold.Split('|')[1]);
      if (this.m_player.PropBag.GetItemByTemplateID(0, 11916) == null || !this.m_player.RemoveTemplate(11916, 1))
        this.m_player.Labyrinth.isDoubleAward = false;
      if (this.m_player.Labyrinth.isDoubleAward)
      {
        exp *= 2;
        count *= 2;
        int_3 *= 2;
      }
      this.m_player.Labyrinth.accumulateExp += exp;
      List<SqlDataProvider.Data.ItemInfo> itemInfoList = new List<SqlDataProvider.Data.ItemInfo>();
      if (this.method_2())
      {
        itemInfoList = this.m_player.CopyDrop(2, 40002);
        this.m_player.AddTemplate(itemInfoList, count, eGameView.dungeonTypeGet);
        this.m_player.AddHardCurrency(int_3);
      }
      this.m_player.AddGP(exp);
      this.method_3(this.m_player.Labyrinth.currentFloor, exp, int_3, itemInfoList);
    }

    private bool method_2()
    {
      bool flag = false;
      for (int index = 0; index <= this.m_player.Labyrinth.myProgress; index += 2)
      {
        if (index == this.m_player.Labyrinth.currentFloor)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private void method_3(int int_1, int int_2, int int_3, List<SqlDataProvider.Data.ItemInfo> ht0VOBWhZvOfbkW24XT)
    {
      if (ht0VOBWhZvOfbkW24XT == null)
        ht0VOBWhZvOfbkW24XT = new List<SqlDataProvider.Data.ItemInfo>();
      GSPacketIn pkg = new GSPacketIn((short) 131, this.m_player.PlayerId);
      pkg.WriteByte((byte) 7);
      pkg.WriteInt(int_1);
      pkg.WriteInt(int_2);
      pkg.WriteInt(ht0VOBWhZvOfbkW24XT.Count);
      foreach (SqlDataProvider.Data.ItemInfo itemInfo in ht0VOBWhZvOfbkW24XT)
      {
        pkg.WriteInt(itemInfo.TemplateID);
        pkg.WriteInt(itemInfo.Count);
      }
      pkg.WriteInt(int_3);
      this.m_player.SendTCP(pkg);
    }

    public void StopCleantOutLabyrinth()
    {
      UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
      labyrinth.isCleanOut = false;
      this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
      this.StopLabyrinthTimer();
    }

    public void SpeededUpCleantOutLabyrinth()
    {
      UserLabyrinthInfo labyrinth = this.Player.Labyrinth;
      labyrinth.isCleanOut = false;
      labyrinth.isInGame = false;
      labyrinth.completeChallenge = false;
      labyrinth.remainTime = 0;
      labyrinth.currentRemainTime = 0;
      labyrinth.cleanOutAllTime = 0;
      for (int currentFloor = labyrinth.currentFloor; currentFloor <= labyrinth.myProgress; ++currentFloor)
      {
        this.method_1();
        ++labyrinth.currentFloor;
      }
      labyrinth.currentFloor = labyrinth.myProgress;
      this.Player.Out.SendLabyrinthUpdataInfo(this.Player.PlayerId, labyrinth);
      this.StopLabyrinthTimer();
    }
  }
}
