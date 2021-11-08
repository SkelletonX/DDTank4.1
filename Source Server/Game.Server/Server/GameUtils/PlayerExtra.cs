
using Bussiness;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Buffer;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
  public class PlayerExtra
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ThreadSafeRandom rand = new ThreadSafeRandom();
    private int[] buffData = new int[7]
    {
      1,
      2,
      3,
      4,
      5,
      6,
      7
    };
    protected object m_lock = new object();
    public int MapId = 1;
    private int[] positions = new int[34];
    protected Timer _hotSpringTimer;
    private Dictionary<int, EventRewardProcessInfo> dictionary_0;
    public Dictionary<int, int> m_kingBlessInfo;
    private UsersExtraInfo m_Info;
    protected GamePlayer m_player;
    private bool m_saveToDb;
    private List<EventAwardInfo> m_searchGoodItems;
    public readonly DateTime reChangeEnd;
    public readonly DateTime reChangeStart;
    public const int STRENGTH_ENCHANCE = 1;
    public readonly DateTime strengthenEnd;
    public readonly DateTime strengthenStart;
    public readonly DateTime upGradeEnd;
    public readonly DateTime upGradeStart;
    public readonly DateTime useMoneyEnd;
    public readonly DateTime useMoneyStart;

    public PlayerExtra(GamePlayer player, bool saveTodb)
    {
      this.m_player = player;
      this.m_kingBlessInfo = new Dictionary<int, int>();
      this.m_searchGoodItems = new List<EventAwardInfo>();
      this.m_saveToDb = saveTodb;
    }

    public void BeginHotSpringTimer()
    {
      int num = 60000;
      if (this._hotSpringTimer == null)
        this._hotSpringTimer = new Timer(new TimerCallback(this.HotSpringCheck), (object) null, num, num);
      else
        this._hotSpringTimer.Change(num, num);
    }

    public UsersExtraInfo CreateUserExtra(int UserID)
    {
      UsersExtraInfo usersExtraInfo = new UsersExtraInfo();
      usersExtraInfo.UserID = UserID;
      usersExtraInfo.LastTimeHotSpring = DateTime.Now;
      usersExtraInfo.LastFreeTimeHotSpring = DateTime.Now;
      usersExtraInfo.MinHotSpring = 60;
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddDays(-1.0);
      usersExtraInfo.LeftRoutteCount = GameProperties.LeftRouterMaxDay;
      usersExtraInfo.LeftRoutteRate = 0.0f;
      return usersExtraInfo;
    }

    public bool CheckNoviceActiveOpen(NoviceActiveType activeType)
    {
      bool flag = false;
      switch (activeType)
      {
        case NoviceActiveType.GRADE_UP_ACTIVE:
          return true;
        case NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE:
          return true;
        case NoviceActiveType.USE_MONEY_ACTIVE:
          return true;
        case NoviceActiveType.RECHANGE_MONEY_ACTIVE:
          return true;
        default:
          return flag;
      }
    }

    public EventRewardProcessInfo GetEventProcess(int activeType)
    {
      lock (this.m_lock)
      {
        if (!this.dictionary_0.ContainsKey(activeType))
          this.dictionary_0.Add(activeType, this.method_0(activeType));
        return this.dictionary_0[activeType];
      }
    }

    public EventAwardInfo GetSpecialTem(int type, int pos)
    {
      return new EventAwardInfo()
      {
        TemplateID = type,
        Position = pos,
        Count = 1
      };
    }

    public void ResetNoviceEvent(NoviceActiveType activeType)
    {
      EventRewardProcessInfo eventProcess = this.GetEventProcess((int) activeType);
      eventProcess.AwardGot = 0;
      eventProcess.Conditions = 0;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
        playerBussiness.UpdateUsersEventProcess(eventProcess);
    }
        public void CreateSaveLifeBuff()
        {
            if ((this.m_player.PlayerCharacter.VIPLevel >= 4) && !this.m_player.PlayerCharacter.IsVIPExpire())
            {
                AbstractBuffer buffer = BufferList.CreateSaveLifeBuffer(3);
                if (buffer != null)
                {
                    buffer.Start(this.Player);
                }
            }
            else
            {
                AbstractBuffer buffer2 = BufferList.CreateSaveLifeBuffer(0);
                if (buffer2 != null)
                {
                    buffer2.Start(this.Player);
                }
            }
        }

        protected void HotSpringCheck(object sender)
    {
      try
      {
        int tickCount = Environment.TickCount;
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        if (this.m_player.CurrentHotSpringRoom == null)
          this.StopHotSpringTimer();
        else if (this.Info.MinHotSpring <= 0)
        {
          this.m_player.SendMessage("Đã Hết Thời Gian Ở Suối Nước Nóng.");
          this.m_player.CurrentHotSpringRoom.RemovePlayer(this.m_player);
        }
        else
        {
          int num1 = HotSpringMgr.GetExpWithLevel(this.m_player.PlayerCharacter.Grade) / 10;
          if (num1 > 0)
          {
            --this.Info.MinHotSpring;
            this.m_player.OnPlayerSpa(1);
            if (this.Info.MinHotSpring <= 5)
              this.m_player.SendMessage("Bạn Còn ở Trong Suối " + (object) this.Info.MinHotSpring + " Phút.");
            this.m_player.AddGP(num1 * 2);
            this.m_player.Out.SendHotSpringUpdateTime(this.m_player, num1);
            this.m_player.OnHotSpingExpAdd(this.Info.MinHotSpring, num1);
          }
          Thread.CurrentThread.Priority = priority;
          int num2 = Environment.TickCount - tickCount;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("HotSpringCheck: " + (object) ex);
      }
    }

    public virtual void LoadFromDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        this.m_Info = playerBussiness.GetSingleUsersExtra(this.m_player.PlayerCharacter.ID);
        if (this.m_Info == null)
          this.m_Info = this.CreateUserExtra(this.Player.PlayerCharacter.ID);
        this.dictionary_0 = new Dictionary<int, EventRewardProcessInfo>();
        foreach (EventRewardProcessInfo rewardProcessInfo in playerBussiness.GetUserEventProcess(this.m_player.PlayerCharacter.ID))
        {
          if (!this.dictionary_0.ContainsKey(rewardProcessInfo.ActiveType))
            this.dictionary_0.Add(rewardProcessInfo.ActiveType, rewardProcessInfo);
        }
      }
    }

    private EventRewardProcessInfo method_0(int int_0)
    {
      return new EventRewardProcessInfo()
      {
        UserID = this.m_player.PlayerCharacter.ID,
        ActiveType = int_0,
        Conditions = 0,
        AwardGot = 0
      };
    }

    public virtual void SaveToDatabase()
    {
      if (!this.m_saveToDb)
        return;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        lock (this.m_lock)
        {
          if (this.m_Info == null || !this.m_Info.IsDirty)
            return;
          playerBussiness.UpdateUserExtra(this.m_Info);
        }
      }
    }

    public void StopHotSpringTimer()
    {
      if (this._hotSpringTimer == null)
        return;
      this._hotSpringTimer.Dispose();
      this._hotSpringTimer = (Timer) null;
    }

    public void UpdateEventCondition(int activeType, int value)
    {
      this.UpdateEventCondition(activeType, value, false, 0);
    }

    public void UpdateEventCondition(int activeType, int value, bool isPlus, int awardGot)
    {
      PlayerBussiness playerBussiness = new PlayerBussiness();
      EventRewardProcessInfo rewardProcessInfo = this.GetEventProcess(activeType) ?? this.method_0(activeType);
      if (isPlus)
        rewardProcessInfo.Conditions += value;
      else if (rewardProcessInfo.Conditions < value)
        rewardProcessInfo.Conditions = value;
      if ((uint) awardGot > 0U)
        rewardProcessInfo.AwardGot = awardGot;
      DateTime now = DateTime.Now;
      DateTime endTime = DateTime.Now.AddYears(2);
      EventRewardProcessInfo info = rewardProcessInfo;
      playerBussiness.UpdateUsersEventProcess(info);
      this.m_player.Out.SendOpenNoviceActive(0, activeType, rewardProcessInfo.Conditions, rewardProcessInfo.AwardGot, now, endTime);
    }

    public UsersExtraInfo Info
    {
      get
      {
        return this.m_Info;
      }
      set
      {
        this.m_Info = value;
      }
    }

    public Dictionary<int, int> KingBlessInfo
    {
      get
      {
        return this.m_kingBlessInfo;
      }
      set
      {
        this.m_kingBlessInfo = value;
      }
    }

    public GamePlayer Player
    {
      get
      {
        return this.m_player;
      }
    }

    public List<EventAwardInfo> SearchGoodItems
    {
      get
      {
        return this.m_searchGoodItems;
      }
      set
      {
        this.m_searchGoodItems = value;
      }
    }
  }
}
