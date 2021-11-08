// Decompiled with JetBrains decompiler
// Type: Game.Server.RingStation.RingStationGamePlayer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.RingStation.Action;
using Game.Server.RingStation.RoomGamePkg;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Reflection;

namespace Game.Server.RingStation
{
  public class RingStationGamePlayer
  {
    public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private bool _canUserProp = true;
    private ArrayList m_actions = new ArrayList();
    private RoomGame seflRoom = new RoomGame();
    private int _ID;
    public BaseRoomRingStation CurRoom;
    private long m_passTick;

    public void AddAction(IAction action)
    {
      lock (this.m_actions)
        this.m_actions.Add((object) action);
    }

    public void AddAction(ArrayList action)
    {
      lock (this.m_actions)
        this.m_actions.AddRange((ICollection) action);
    }

    public void AddTurn(GSPacketIn pkg)
    {
      if (pkg.Parameter1 != this.GamePlayerId)
        return;
      this.m_actions.Add((object) new PlayerShotAction(this.LastX, this.LastY - 25, this.LastForce, this.LastAngle, 1000));
    }

    public static double ComputeVx(double dx, float m, float af, float f, float t)
    {
      return (dx - (double) f / (double) m * (double) t * (double) t / 2.0) / (double) t + (double) af / (double) m * dx * 0.8;
    }

    public static double ComputeVy(double dx, float m, float af, float f, float t)
    {
      return (dx - (double) f / (double) m * (double) t * (double) t / 2.0) / (double) t + (double) af / (double) m * dx * 1.3;
    }

    public void FindTarget()
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 143);
      this.SendTCP(pkg);
    }

    public void NextTurn(GSPacketIn pkg)
    {
      this.SendSelfTurn(pkg.Parameter1 == this.GamePlayerId, true);
    }

    public void Pause(int time)
    {
      this.m_passTick = Math.Max(this.m_passTick, TickHelper.GetTickCount() + (long) time);
    }

    internal void ProcessPacket(GSPacketIn pkg)
    {
      if (this.seflRoom == null)
        return;
      this.seflRoom.ProcessData(this, pkg);
    }

    public void Resume()
    {
      this.m_passTick = 0L;
    }

    public void SendCreateGame(GSPacketIn pkg)
    {
      this.ShootCount = 100;
      this.FirtDirection = true;
      this.Direction = -1;
      pkg.ReadInt();
      pkg.ReadInt();
      pkg.ReadInt();
      int num1 = pkg.ReadInt();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadBoolean();
        int num2 = (int) pkg.ReadByte();
        pkg.ReadInt();
        pkg.ReadBoolean();
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadString();
        pkg.ReadString();
        pkg.ReadInt();
        pkg.ReadInt();
        if ((uint) pkg.ReadInt() > 0U)
        {
          pkg.ReadInt();
          pkg.ReadString();
          pkg.ReadDateTime();
        }
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadBoolean();
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadInt();
        pkg.ReadString();
        pkg.ReadInt();
        pkg.ReadBoolean();
        pkg.ReadInt();
        if (pkg.ReadBoolean())
        {
          pkg.ReadInt();
          pkg.ReadString();
        }
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        pkg.ReadInt();
        int num3 = pkg.ReadInt();
        int num4 = pkg.ReadInt();
        pkg.ReadInt();
        if (this.GamePlayerId == num4)
          this.Team = num3;
        int num5 = pkg.ReadInt();
        for (int index2 = 0; index2 < num5; ++index2)
        {
          pkg.ReadInt();
          pkg.ReadInt();
          pkg.ReadInt();
          pkg.ReadString();
          pkg.ReadInt();
          pkg.ReadInt();
          int num6 = pkg.ReadInt();
          for (int index3 = 0; index3 < num6; ++index3)
          {
            pkg.ReadInt();
            pkg.ReadInt();
          }
        }
        int num7 = pkg.ReadInt();
        for (int index2 = 0; index2 < num7; ++index2)
          pkg.ReadInt();
      }
    }

    private void SendDirection(int Direction)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 7);
      pkg.WriteInt(Direction);
      this.SendTCP(pkg);
    }

    public void SendGameCMDShoot(int x, int y, int force, int angle)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 2);
      pkg.WriteInt(x);
      pkg.WriteInt(y);
      pkg.WriteInt(force);
      pkg.WriteInt(angle);
      this.SendTCP(pkg);
    }

    public void sendGameCMDStunt(int type)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 15);
      pkg.WriteInt(type);
      this.SendTCP(pkg);
    }

    public void SendLoadingComplete(int state)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 16);
      pkg.WriteInt(state);
      pkg.WriteInt(104);
      pkg.WriteInt(this.GamePlayerId);
      this.SendTCP(pkg);
    }

    private void SendSelfTurn(bool fire)
    {
      this.SendSelfTurn(fire, false);
    }

    private void SendSelfTurn(bool fire, bool useBuff)
    {
      if (!fire)
        return;
      this.FindTarget();
    }

    public void SendShootTag(bool b, int time)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 96);
      pkg.WriteBoolean(b);
      pkg.WriteByte((byte) time);
      this.SendTCP(pkg);
    }

    private void SendSkipNext()
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 12);
      pkg.WriteByte((byte) 100);
      this.SendTCP(pkg);
    }

    internal void SendTCP(GSPacketIn pkg)
    {
      this.CurRoom.SendTCP(pkg);
    }

    public void SendUseProp(int templateId)
    {
      this.SendUseProp(templateId, 0, 0, 0, 0);
    }

    public void SendUseProp(int templateId, int x, int y, int force, int angle)
    {
      GSPacketIn pkg = new GSPacketIn((short) 91)
      {
        Parameter1 = this.GamePlayerId
      };
      pkg.WriteByte((byte) 32);
      pkg.WriteByte((byte) 3);
      pkg.WriteInt(-1);
      pkg.WriteInt(templateId);
      this.SendTCP(pkg);
      if (templateId != 10001 && templateId != 10002)
        return;
      ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId);
      for (int index = 0; index < itemTemplate.Property2; ++index)
        this.SendGameCMDShoot(x, y, force, angle);
    }

    public void Update(long tick)
    {
      if (this.m_passTick >= tick)
        return;
      ArrayList arrayList;
      lock (this.m_actions)
      {
        arrayList = (ArrayList) this.m_actions.Clone();
        this.m_actions.Clear();
      }
      if (arrayList == null || this.seflRoom == null || arrayList.Count <= 0)
        return;
      ArrayList action1 = new ArrayList();
      foreach (IAction action2 in arrayList)
      {
        try
        {
          action2.Execute(this, tick);
          if (!action2.IsFinished(tick))
            action1.Add((object) action2);
        }
        catch (Exception ex)
        {
          RingStationGamePlayer.log.Error((object) "Bot action error:", ex);
        }
      }
      this.AddAction(action1);
    }

    public int Agility { get; set; }

    public double AntiAddictionRate { get; set; }

    public int Attack { get; set; }

    public float AuncherExperienceRate { get; set; }

    public float AuncherOfferRate { get; set; }

    public float AuncherRichesRate { get; set; }

    public int badgeID { get; set; }

    public double BaseAgility { get; set; }

    public double BaseAttack { get; set; }

    public double BaseBlood { get; set; }

    public double BaseDefence { get; set; }

    public bool CanUserProp
    {
      get
      {
        return this._canUserProp;
      }
      set
      {
        this._canUserProp = value;
      }
    }

    public string Colors { get; set; }

    public int ConsortiaID { get; set; }

    public int ConsortiaLevel { get; set; }

    public string ConsortiaName { get; set; }

    public int ConsortiaRepute { get; set; }

    public int Dander { get; set; }

    public int Defence { get; set; }

    public int Direction { get; set; }

    public int FightPower { get; set; }

    public bool FirtDirection { get; set; }

    public int GamePlayerId
    {
      get
      {
        return this._ID;
      }
      set
      {
        this._ID = value;
      }
    }

    public float GMExperienceRate { get; set; }

    public float GMOfferRate { get; set; }

    public float GMRichesRate { get; set; }

    public int GP { get; set; }

    public double GPAddPlus { get; set; }

    public int Grade { get; set; }

    public int Healstone { get; set; }

    public int HealstoneCount { get; set; }

    public int Hide { get; set; }

    public string Honor { get; set; }

    public int hp { get; set; }

    public int ID
    {
      get
      {
        return this._ID;
      }
      set
      {
        this._ID = value;
      }
    }

    public int LastAngle { get; set; }

    public int LastForce { get; set; }

    public int LastX { get; set; }

    public int LastY { get; set; }

    public int Luck { get; set; }

    public int LX { get; set; }

    public int LY { get; set; }

    public string NickName { get; set; }

    public int Nimbus { get; set; }

    public int Offer { get; set; }

    public double OfferAddPlus { get; set; }

    public int Repute { get; set; }

    public int SecondWeapon { get; set; }

    public bool Sex { get; set; }

    public int ShootCount { get; set; }

    public string Skin { get; set; }

    public string Style { get; set; }

    public int StrengthLevel { get; set; }

    public int Team { get; set; }

    public int TemplateID { get; set; }

    public int Total { get; set; }

    public byte typeVIP { get; set; }

    public int VIPLevel { get; set; }

    public string WeaklessGuildProgressStr { get; set; }

    public int Win { get; set; }

    public int X { get; set; }

    public int Y { get; set; }
  }
}
