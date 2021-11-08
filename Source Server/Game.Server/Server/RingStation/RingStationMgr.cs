
using Bussiness;
using Bussiness.Managers;
using Game.Server.Battle;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.RingStation.Battle;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Server.RingStation
{
  public sealed class RingStationMgr
  {
    private static VirtualPlayerInfo _virtualPlayerInfo = new VirtualPlayerInfo();
    private static Dictionary<int, UserRingStationInfo> dictionary_1 = new Dictionary<int, UserRingStationInfo>();
    private static Dictionary<int, List<RingstationBattleFieldInfo>> dictionary_2 = new Dictionary<int, List<RingstationBattleFieldInfo>>();
    private static int int_0 = 10;
    private static List<VirtualPlayerInfo> list_0 = new List<VirtualPlayerInfo>();
    private static List<UserRingStationInfo> list_1 = new List<UserRingStationInfo>();
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
    protected static object m_lock = new object();
    private static Dictionary<int, RingStationGamePlayer> m_players = new Dictionary<int, RingStationGamePlayer>();
    private static RingStationBattleServer m_server = (RingStationBattleServer) null;
    private static ThreadSafeRandom rand = new ThreadSafeRandom();
    private static readonly string weaklessGuildProgressStr = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
    private static readonly string[] weapons = new string[6]
    {
      "7006|bombpack",
      "7007|bomb",
      "7008|dart",
      "7009|electricbar",
      "7011|fruit",
      "7001|axe"
    };
    private static RingstationConfigInfo _ringstationConfigInfo;
    private static Func<RingstationBattleFieldInfo, DateTime> func_0;
    private static Func<UserRingStationInfo, bool> func_1;
    private static Func<UserRingStationInfo, int> func_2;
    private static Func<UserRingStationInfo, int> func_3;
    private static Func<UserRingStationInfo, bool> func_4;
    private static Func<UserRingStationInfo, int> func_5;
    protected static Timer m_statusScanTimer;

    public static bool AddPlayer(int playerId, RingStationGamePlayer player)
    {
      RingStationMgr.m_clientLocker.AcquireWriterLock(-1);
      try
      {
        if (RingStationMgr.m_players.ContainsKey(playerId))
          return false;
        RingStationMgr.m_players.Add(playerId, player);
      }
      finally
      {
        RingStationMgr.m_clientLocker.ReleaseWriterLock();
      }
      return true;
    }

    public static void BeginTimer()
    {
      int num = 60000;
      if (RingStationMgr.m_statusScanTimer == null)
        RingStationMgr.m_statusScanTimer = new Timer(new TimerCallback(RingStationMgr.StatusScan), (object) null, num, num);
      else
        RingStationMgr.m_statusScanTimer.Change(num, num);
    }

    public static void CreateAutoBot(int roomtype, int gametype, int npcId)
    {
            Console.WriteLine("CreateBot");
      BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
      {
        RoomType = roomtype,
        GameType = gametype,
        PickUpNpcId = npcId,
        IsAutoBot = true,
        IsFreedom = true
      };
      RingStationGamePlayer player = new RingStationGamePlayer();
      string str = RingStationConfiguration.RandomName[RingStationMgr.rand.Next(0, RingStationConfiguration.RandomName.Length - 1)];
      player.NickName = str + "1";
      player.GP = 1283;
      player.Grade = 5;
      player.Attack = 100;
      player.Defence = 100;
      player.Luck = 100;
      player.Agility = 100;
      player.hp = 1000;
      player.FightPower = 1200;
      player.BaseAttack = 100.0;
      player.BaseDefence = 50.0;
      player.BaseAgility = 1.0 - (double) player.Agility * 0.001;
      player.BaseBlood = 1000.0;
      string weapon = RingStationMgr.weapons[RingStationMgr.rand.Next(RingStationMgr.weapons.Length)];
      player.Style = RingStationMgr.GetAutoBotStyleRandom(weapon);
      player.Colors = ",,,,,,,,,,,,,,,";
      player.Hide = 1111111111;
      player.TemplateID = int.Parse(weapon.Split('|')[0]);
      player.StrengthLevel = 12;
      player.WeaklessGuildProgressStr = "R/O/DeABAtgWdWsIAAAAAAAAgCAECwAAAAAAABgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
      player.ID = npcId;
      if (RingStationMgr.m_server == null)
        return;
      RingStationMgr.AddPlayer(player.ID, player);
      room.AddPlayer(player);
      RingStationMgr.m_server.AddRoom(room);
    }

    public static int GetAutoBot(GamePlayer player, int roomtype, int gametype)
    {
            Console.WriteLine("GetBot");
            int playerId = RingStationConfiguration.NextPlayerID();
      BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId());
      room.RoomType = roomtype;
      room.GameType = gametype;
      room.PickUpNpcId = playerId;
      room.IsAutoBot = true;
      room.IsFreedom = false;
      RingStationGamePlayer player1 = new RingStationGamePlayer();
      player1.NickName = RingStationConfiguration.RandomName[RingStationMgr.rand.Next(0, RingStationConfiguration.RandomName.Length)];
      player1.GP = player.PlayerCharacter.GP;
      player1.Grade = player.PlayerCharacter.Grade;
      player1.Attack = player.PlayerCharacter.Attack / 2;
      player1.Defence = player.PlayerCharacter.Defence / 2;
      player1.Luck = player.PlayerCharacter.Luck / 2;
      player1.Agility = player.PlayerCharacter.Agility / 2;
      player1.hp = player.PlayerCharacter.hp / 2;
      player1.FightPower = player.PlayerCharacter.FightPower / 2;
      player1.BaseAttack = player.GetBaseAttack() / 2.0;
      player1.BaseDefence = player.GetBaseDefence() / 2.0;
      player1.BaseAgility = player.GetBaseAgility() / 2.0;
      player1.BaseBlood = player.GetBaseBlood() / 2.0;
      string weapon = RingStationMgr.weapons[RingStationMgr.rand.Next(RingStationMgr.weapons.Length)];
      player1.Style = string.Format("1214|head13,,3244|hair44,,5276|cloth76,6204|face3,{0},,,,,,,,,", (object) weapon);
      player1.Colors = ",,,,,,,,,,,,,,,";
      player1.Hide = 1111111111;
      player1.TemplateID = int.Parse(weapon.Split('|')[0]);
      player1.StrengthLevel = 0;
      player1.WeaklessGuildProgressStr = "R/O/DeABAtgWdWsIAAAAAAAAgCAECwAAAAAAABgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
      player1.ID = playerId;
      if (RingStationMgr.m_server != null)
      {
        RingStationMgr.AddPlayer(playerId, player1);
        room.AddPlayer(player1);
        RingStationMgr.m_server.AddRoom(room);
      }
      return playerId;
    }

    public static void CreateAutoBot(GamePlayer player, int roomtype, int gametype, int npcId)
    {
            Console.WriteLine("CreateAutoBot");
            BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
      {
        RoomType = roomtype,
        GameType = gametype,
        PickUpNpcId = npcId,
        IsAutoBot = true,
        IsFreedom = true
      };
      RingStationGamePlayer player1 = new RingStationGamePlayer();
      player1.NickName = RingStationConfiguration.RandomName[RingStationMgr.rand.Next(0, RingStationConfiguration.RandomName.Length - 1)];
      player1.GP = player.PlayerCharacter.GP;
      player1.Grade = player.PlayerCharacter.Grade;
      RingStationGamePlayer stationGamePlayer1 = player1;
      int attack1 = player.PlayerCharacter.Attack;
      int attack2 = player.PlayerCharacter.Attack;
      int num1 = attack1;
      stationGamePlayer1.Attack = num1;
      RingStationGamePlayer stationGamePlayer2 = player1;
      int defence1 = player.PlayerCharacter.Defence;
      int defence2 = player.PlayerCharacter.Defence;
      int num2 = defence1;
      stationGamePlayer2.Defence = num2;
      RingStationGamePlayer stationGamePlayer3 = player1;
      int luck1 = player.PlayerCharacter.Luck;
      int luck2 = player.PlayerCharacter.Luck;
      int num3 = luck1;
      stationGamePlayer3.Luck = num3;
      RingStationGamePlayer stationGamePlayer4 = player1;
      int agility1 = player.PlayerCharacter.Agility;
      int agility2 = player.PlayerCharacter.Agility;
      int num4 = agility1;
      stationGamePlayer4.Agility = num4;
      RingStationGamePlayer stationGamePlayer5 = player1;
      int hp1 = player.PlayerCharacter.hp;
      int hp2 = player.PlayerCharacter.hp;
      int num5 = hp1;
      stationGamePlayer5.hp = num5;
      RingStationGamePlayer stationGamePlayer6 = player1;
      int fightPower1 = player.PlayerCharacter.FightPower;
      int fightPower2 = player.PlayerCharacter.FightPower;
      int num6 = fightPower1;
      stationGamePlayer6.FightPower = num6;
      player1.BaseAttack = player.GetBaseAttack() - 0.0 * player.GetBaseAttack();
      player1.BaseDefence = player.GetBaseDefence() - 0.0 * player.GetBaseDefence();
      player1.BaseAgility = player.GetBaseAgility() - 0.0 * player.GetBaseAgility();
      player1.BaseBlood = player.GetBaseBlood() - 0.0 * player.GetBaseBlood();
      string weapon = RingStationMgr.weapons[RingStationMgr.rand.Next(RingStationMgr.weapons.Length)];
      player1.Style = RingStationMgr.GetAutoBotStyleRandom(weapon);
      player1.Colors = ",,,,,,,,,,,,,,,";
      player1.Hide = 1111111111;
      player1.TemplateID = int.Parse(weapon.Split('|')[0]);
      player1.StrengthLevel = 0;
      player1.WeaklessGuildProgressStr = RingStationMgr.weaklessGuildProgressStr;
      player1.ID = npcId;
      if (RingStationMgr.m_server == null)
        return;
      RingStationMgr.AddPlayer(player1.ID, player1);
      room.AddPlayer(player1);
      RingStationMgr.m_server.AddRoom(room);
    }

    public static List<RingStationGamePlayer> GetAllPlayer()
    {
      List<RingStationGamePlayer> stationGamePlayerList = new List<RingStationGamePlayer>();
      RingStationMgr.m_clientLocker.AcquireReaderLock(-1);
      try
      {
        foreach (RingStationGamePlayer stationGamePlayer in RingStationMgr.m_players.Values)
          stationGamePlayerList.Add(stationGamePlayer);
      }
      finally
      {
        RingStationMgr.m_clientLocker.ReleaseReaderLock();
      }
      return stationGamePlayerList;
    }

    public static RingStationGamePlayer GetPlayerById(int playerId)
    {
      RingStationGamePlayer stationGamePlayer = (RingStationGamePlayer) null;
      RingStationMgr.m_clientLocker.AcquireReaderLock(-1);
      try
      {
        if (RingStationMgr.m_players.ContainsKey(playerId))
          stationGamePlayer = RingStationMgr.m_players[playerId];
      }
      finally
      {
        RingStationMgr.m_clientLocker.ReleaseReaderLock();
      }
      return stationGamePlayer;
    }

    public static UserRingStationInfo[] GetRingStationRanks()
    {
      List<UserRingStationInfo> userRingStationInfoList = new List<UserRingStationInfo>();
      foreach (UserRingStationInfo userRingStationInfo in RingStationMgr.list_1)
      {
        userRingStationInfoList.Add(userRingStationInfo);
        if (userRingStationInfoList.Count >= 50)
          break;
      }
      return userRingStationInfoList.ToArray();
    }

    public static UserRingStationInfo GetSingleRingStationInfos(int playerId)
    {
      if (RingStationMgr.dictionary_1.ContainsKey(playerId))
        return RingStationMgr.dictionary_1[playerId];
      return (UserRingStationInfo) null;
    }

    public static VirtualPlayerInfo GetVirtualPlayerInfo()
    {
      int index = RingStationMgr.rand.Next(RingStationMgr.list_0.Count);
      return RingStationMgr.list_0[index];
    }

    public static int GetWeaponID(string style)
    {
      if (!string.IsNullOrEmpty(style))
      {
        string str = style.Split(',')[6];
        if (str.IndexOf("|") != -1)
          return int.Parse(str.Split('|')[0]);
      }
      return 7008;
    }

    public static bool Init()
    {
      bool flag = false;
      try
      {
        RingStationMgr.m_players.Clear();
        BattleServer server = BattleMgr.GetServer(4);
        if (server == null)
          return false;
        RingStationMgr.m_server = new RingStationBattleServer(RingStationConfiguration.ServerID, server.Ip, server.Port, "1,7road");
        try
        {
          //RingStationMgr.NickName = GameProperties.VirtualName.Split(',');
          flag = RingStationMgr.m_server.Start();
          if (!RingStationMgr.SetupVirtualPlayer())
            return false;
          using (new PlayerBussiness())
          {
            RingStationMgr._ringstationConfigInfo = (RingstationConfigInfo) null;
            if (RingStationMgr._ringstationConfigInfo == null)
            {
              RingStationMgr._ringstationConfigInfo = new RingstationConfigInfo();
              RingStationMgr._ringstationConfigInfo.buyCount = 10;
              RingStationMgr._ringstationConfigInfo.buyPrice = 8000;
              RingStationMgr._ringstationConfigInfo.cdPrice = 10000;
              RingStationMgr._ringstationConfigInfo.AwardTime = DateTime.Now.AddDays(3.0);
              RingStationMgr._ringstationConfigInfo.AwardNum = 450;
              RingStationMgr._ringstationConfigInfo.AwardFightWin = "1-50,25|51-100,20|101-1000000,15";
              RingStationMgr._ringstationConfigInfo.AwardFightLost = "1-50,15|51-100,10|101-1000000,5";
              RingStationMgr._ringstationConfigInfo.ChampionText = "";
              RingStationMgr._ringstationConfigInfo.ChallengeNum = 10;
              RingStationMgr._ringstationConfigInfo.IsFirstUpdateRank = true;
            }
          }
          RingStationMgr.BeginTimer();
          RingStationMgr.ReLoadUserRingStation();
          RingStationMgr.ReLoadBattleField();
        }
        catch (Exception ex)
        {
          Console.WriteLine((object) ex);
        }
      }
      catch (Exception ex)
      {
        RingStationMgr.log.Error((object) "RingStationMgr Init", ex);
      }
      return flag;
    }

    public static RingstationBattleFieldInfo[] LoadRingstationBattleFieldDb()
    {
      using (new PlayerBussiness())
        return new List<RingstationBattleFieldInfo>().ToArray();
    }

    public static Dictionary<int, List<RingstationBattleFieldInfo>> LoadRingstationBattleFields(
      RingstationBattleFieldInfo[] RingstationBattleField)
    {
      Dictionary<int, List<RingstationBattleFieldInfo>> dictionary = new Dictionary<int, List<RingstationBattleFieldInfo>>();
      for (int index = 0; index < RingstationBattleField.Length; ++index)
      {
        RingstationBattleFieldInfo info = RingstationBattleField[index];
        if (!dictionary.Keys.Contains<int>(info.UserID))
        {
          IEnumerable<RingstationBattleFieldInfo> source = ((IEnumerable<RingstationBattleFieldInfo>) RingstationBattleField).Where<RingstationBattleFieldInfo>((Func<RingstationBattleFieldInfo, bool>) (s => s.UserID == info.UserID));
          dictionary.Add(info.UserID, source.ToList<RingstationBattleFieldInfo>());
        }
      }
      return dictionary;
    }

    public static void LoadRingStationInfo(PlayerInfo player, int dame, int guard)
    {
      if (player == null)
        return;
      using (new PlayerBussiness())
      {
        if (RingStationMgr.dictionary_1.ContainsKey(player.ID))
        {
          bool flag = false;
          UserRingStationInfo userRingStationInfo = RingStationMgr.dictionary_1[player.ID];
          if (dame != userRingStationInfo.BaseDamage && userRingStationInfo.BaseGuard != guard)
          {
            userRingStationInfo.BaseDamage = dame;
            userRingStationInfo.BaseGuard = guard;
            userRingStationInfo.BaseEnergy = (int) (1.0 - (double) player.Agility * 0.001);
            flag = true;
          }
          int weaponId = RingStationMgr.GetWeaponID(player.Style);
          if (userRingStationInfo.WeaponID == weaponId)
            return;
          userRingStationInfo.WeaponID = weaponId;
          flag = true;
        }
        else
        {
          UserRingStationInfo userRingStationInfo = new UserRingStationInfo()
          {
            UserID = player.ID,
            WeaponID = RingStationMgr.GetWeaponID(player.Style),
            BaseDamage = dame,
            BaseGuard = guard,
            BaseEnergy = (int) (1.0 - (double) player.Agility * 0.001),
            signMsg = LanguageMgr.GetTranslation("RingStation.signMsg"),
            ChallengeNum = RingStationMgr._ringstationConfigInfo.ChallengeNum,
            buyCount = RingStationMgr._ringstationConfigInfo.buyCount,
            ChallengeTime = DateTime.Now,
            LastDate = DateTime.Now,
            Info = player
          };
          RingStationMgr.dictionary_1.Add(player.ID, userRingStationInfo);
        }
      }
    }

    public static UserRingStationInfo[] LoadUserRingStationDb()
    {
      using (new PlayerBussiness())
        return new List<UserRingStationInfo>().ToArray();
    }

    public static Dictionary<int, UserRingStationInfo> LoadUserRingStations(
      UserRingStationInfo[] UserRingStation)
    {
      Dictionary<int, UserRingStationInfo> dictionary = new Dictionary<int, UserRingStationInfo>();
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        for (int index = 0; index < UserRingStation.Length; ++index)
        {
          UserRingStationInfo userRingStationInfo = UserRingStation[index];
          if (!dictionary.Keys.Contains<int>(userRingStationInfo.UserID))
          {
            userRingStationInfo.Info = playerBussiness.GetUserSingleByUserID(userRingStationInfo.UserID);
            if (userRingStationInfo.Info != null)
            {
              userRingStationInfo.WeaponID = RingStationMgr.GetWeaponID(userRingStationInfo.Info.Style);
              dictionary.Add(userRingStationInfo.UserID, userRingStationInfo);
            }
          }
        }
      }
      return dictionary;
    }

    public static bool ReLoadBattleField()
    {
      try
      {
        RingstationBattleFieldInfo[] RingstationBattleField = RingStationMgr.LoadRingstationBattleFieldDb();
        Dictionary<int, List<RingstationBattleFieldInfo>> dictionary = RingStationMgr.LoadRingstationBattleFields(RingstationBattleField);
        if ((uint) RingstationBattleField.Length > 0U)
          Interlocked.Exchange<Dictionary<int, List<RingstationBattleFieldInfo>>>(ref RingStationMgr.dictionary_2, dictionary);
        return true;
      }
      catch (Exception ex)
      {
        if (RingStationMgr.log.IsErrorEnabled)
          RingStationMgr.log.Error((object) "ReLoad RingstationBattleField", ex);
        return false;
      }
    }

    public static bool ReLoadUserRingStation()
    {
      try
      {
        UserRingStationInfo[] UserRingStation = RingStationMgr.LoadUserRingStationDb();
        Dictionary<int, UserRingStationInfo> dictionary = RingStationMgr.LoadUserRingStations(UserRingStation);
        if ((uint) UserRingStation.Length > 0U)
        {
          Interlocked.Exchange<Dictionary<int, UserRingStationInfo>>(ref RingStationMgr.dictionary_1, dictionary);
          UserRingStationInfo[] userRingStationInfoArray = UserRingStation;
          if (RingStationMgr.func_1 == null)
            RingStationMgr.func_1 = new Func<UserRingStationInfo, bool>(RingStationMgr.smethod_1);
          Func<UserRingStationInfo, bool> func1 = RingStationMgr.func_1;
          IEnumerable<UserRingStationInfo> source = ((IEnumerable<UserRingStationInfo>) userRingStationInfoArray).Where<UserRingStationInfo>(func1);
          if (RingStationMgr.func_2 == null)
            RingStationMgr.func_2 = new Func<UserRingStationInfo, int>(RingStationMgr.smethod_2);
          Func<UserRingStationInfo, int> func2 = RingStationMgr.func_2;
          RingStationMgr.list_1 = source.OrderBy<UserRingStationInfo, int>(func2).ToList<UserRingStationInfo>();
        }
        return true;
      }
      catch (Exception ex)
      {
        if (RingStationMgr.log.IsErrorEnabled)
          RingStationMgr.log.Error((object) "ReLoad All UserRingStation", ex);
        return false;
      }
    }

    public static bool SetupVirtualPlayer()
    {
      int[] numArray1 = new int[16]
      {
        7001,
        7006,
        7007,
        7008,
        7009,
        7010,
        7011,
        7012,
        7013,
        7014,
        7026,
        7025,
        7046,
        7024,
        7035,
        7037
      };
      int[] numArray2 = new int[18]
      {
        1111,
        1112,
        1113,
        1114,
        1115,
        1117,
        1116,
        1119,
        1120,
        1121,
        1122,
        1123,
        1124,
        1125,
        1127,
        1128,
        1129,
        1130
      };
      int[] numArray3 = new int[18]
      {
        2131,
        2132,
        2133,
        2134,
        2135,
        2136,
        2137,
        2138,
        2139,
        2140,
        2141,
        2142,
        2143,
        2144,
        2146,
        2145,
        2147,
        2148
      };
      int[] numArray4 = new int[19]
      {
        3131,
        3132,
        3133,
        3134,
        3135,
        3136,
        3137,
        3138,
        3140,
        3139,
        3141,
        3142,
        3143,
        3144,
        3145,
        3147,
        3148,
        3146,
        3149
      };
      int[] numArray5 = new int[18]
      {
        4120,
        4121,
        4122,
        4123,
        4124,
        4125,
        4126,
        4129,
        4130,
        4131,
        4132,
        4133,
        4134,
        4135,
        4136,
        4137,
        4138,
        4139
      };
      int[] numArray6 = new int[18]
      {
        5110,
        5111,
        5112,
        5113,
        5114,
        5115,
        5116,
        5117,
        5118,
        5119,
        5120,
        5121,
        5122,
        5123,
        5124,
        5125,
        5126,
        5127
      };
      int[] numArray7 = new int[20]
      {
        6110,
        6111,
        6112,
        6113,
        6114,
        6115,
        6116,
        6117,
        6118,
        6119,
        6120,
        6121,
        6122,
        6123,
        6124,
        6125,
        6126,
        6128,
        6127,
        6129
      };
      int[] numArray8 = new int[26]
      {
        15009,
        15018,
        15019,
        15020,
        15021,
        15022,
        15023,
        15024,
        15025,
        15026,
        15027,
        15028,
        15029,
        15049,
        15031,
        15032,
        15033,
        15034,
        15035,
        15036,
        15037,
        15038,
        15039,
        15040,
        15041,
        15042
      };
      int length = numArray1.Length;
      for (int index = 0; index < length; ++index)
      {
        ItemTemplateInfo itemTemplate1 = ItemMgr.FindItemTemplate(numArray1[index]);
        ItemTemplateInfo itemTemplate2 = ItemMgr.FindItemTemplate(numArray2[index]);
        ItemTemplateInfo itemTemplate3 = ItemMgr.FindItemTemplate(numArray3[index]);
        ItemTemplateInfo itemTemplate4 = ItemMgr.FindItemTemplate(numArray4[index]);
        ItemTemplateInfo itemTemplate5 = ItemMgr.FindItemTemplate(numArray5[index]);
        ItemTemplateInfo itemTemplate6 = ItemMgr.FindItemTemplate(numArray6[index]);
        ItemTemplateInfo itemTemplate7 = ItemMgr.FindItemTemplate(numArray7[index]);
        ItemTemplateInfo itemTemplate8 = ItemMgr.FindItemTemplate(numArray8[index]);
        if (itemTemplate1 != null && itemTemplate2 != null && (itemTemplate3 != null && itemTemplate4 != null) && (itemTemplate5 != null && itemTemplate6 != null && (itemTemplate7 != null && itemTemplate8 != null)))
        {
          string str1 = string.Format("{0}|{1}", (object) numArray1[index], (object) itemTemplate1.Pic);
          string str2 = string.Format("{0}|{1}", (object) numArray2[index], (object) itemTemplate2.Pic);
          string str3 = string.Format("{0}|{1}", (object) numArray3[index], (object) itemTemplate3.Pic);
          string str4 = string.Format("{0}|{1}", (object) numArray4[index], (object) itemTemplate4.Pic);
          string str5 = string.Format("{0}|{1}", (object) numArray5[index], (object) itemTemplate5.Pic);
          string str6 = string.Format("{0}|{1}", (object) numArray6[index], (object) itemTemplate6.Pic);
          string str7 = string.Format("{0}|{1}", (object) numArray7[index], (object) itemTemplate7.Pic);
          string str8 = string.Format("{0}|{1}", (object) numArray8[index], (object) itemTemplate8.Pic);
          string str9 = string.Format("{0},{1},{2},{3},{4},{5},{6},,{7},,,,,,,,,", (object) str2, (object) str3, (object) str4, (object) str5, (object) str6, (object) str7, (object) str1, (object) str8);
          VirtualPlayerInfo virtualPlayerInfo = new VirtualPlayerInfo()
          {
            Style = str9,
            Weapon = numArray1[index]
          };
          RingStationMgr.list_0.Add(virtualPlayerInfo);
        }
      }
      return RingStationMgr.list_0.Count > Math.Abs(length / 2);
    }

    private static DateTime smethod_0(
      RingstationBattleFieldInfo ringstationBattleFieldInfo_0)
    {
      return ringstationBattleFieldInfo_0.BattleTime;
    }

    private static bool smethod_1(UserRingStationInfo userRingStationInfo_0)
    {
      return (uint) userRingStationInfo_0.Rank > 0U;
    }

    private static int smethod_2(UserRingStationInfo userRingStationInfo_0)
    {
      return userRingStationInfo_0.Rank;
    }

    private static int smethod_3(UserRingStationInfo userRingStationInfo_0)
    {
      return userRingStationInfo_0.Total;
    }

    private static bool smethod_4(UserRingStationInfo userRingStationInfo_0)
    {
      return (uint) userRingStationInfo_0.Rank > 0U;
    }

    private static int smethod_5(UserRingStationInfo userRingStationInfo_0)
    {
      return userRingStationInfo_0.Rank;
    }

    protected static void StatusScan(object sender)
    {
      try
      {
        RingStationMgr.log.Info((object) "Begin Scan RingStation Info....");
        int tickCount = Environment.TickCount;
        ThreadPriority priority = Thread.CurrentThread.Priority;
        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
        bool flag = false;
        if (RingStationMgr.ReLoadUserRingStation())
        {
          List<UserRingStationInfo> userRingStationInfoList = new List<UserRingStationInfo>();
          foreach (UserRingStationInfo userRingStationInfo in RingStationMgr.dictionary_1.Values)
            userRingStationInfoList.Add(userRingStationInfo);
          if (RingStationMgr._ringstationConfigInfo.IsFirstUpdateRank && userRingStationInfoList.Count > RingStationMgr.int_0)
          {
            List<UserRingStationInfo> source = userRingStationInfoList;
            if (RingStationMgr.func_3 == null)
              RingStationMgr.func_3 = new Func<UserRingStationInfo, int>(RingStationMgr.smethod_3);
            Func<UserRingStationInfo, int> func3 = RingStationMgr.func_3;
            List<UserRingStationInfo> list = source.OrderByDescending<UserRingStationInfo, int>(func3).ToList<UserRingStationInfo>();
            lock (RingStationMgr.m_lock)
            {
              for (int index = 0; index < list.Count; ++index)
              {
                UserRingStationInfo ring = list[index];
                ring.Rank = index + 1;
                RingStationMgr.UpdateRingStationInfo(ring);
              }
            }
            RingStationMgr._ringstationConfigInfo.IsFirstUpdateRank = false;
            flag = true;
          }
          List<UserRingStationInfo> source1 = userRingStationInfoList;
          if (RingStationMgr.func_4 == null)
            RingStationMgr.func_4 = new Func<UserRingStationInfo, bool>(RingStationMgr.smethod_4);
          Func<UserRingStationInfo, bool> func4 = RingStationMgr.func_4;
          IEnumerable<UserRingStationInfo> source2 = source1.Where<UserRingStationInfo>(func4);
          if (RingStationMgr.func_5 == null)
            RingStationMgr.func_5 = new Func<UserRingStationInfo, int>(RingStationMgr.smethod_5);
          Func<UserRingStationInfo, int> func5 = RingStationMgr.func_5;
          RingStationMgr.list_1 = source2.OrderBy<UserRingStationInfo, int>(func5).ToList<UserRingStationInfo>();
          if (RingStationMgr.list_1.Count > 0)
          {
            UserRingStationInfo userRingStationInfo = RingStationMgr.list_1[0];
            if (userRingStationInfo.Info != null)
            {
              RingStationMgr._ringstationConfigInfo.ChampionText = userRingStationInfo.Info.NickName;
              flag = true;
            }
          }
          if (RingStationMgr._ringstationConfigInfo.IsEndTime())
          {
            lock (RingStationMgr.m_lock)
            {
              RingStationMgr._ringstationConfigInfo.AwardTime = DateTime.Now;
              RingStationMgr._ringstationConfigInfo.AwardTime = DateTime.Now.AddDays(3.0);
              flag = true;
            }
            if (userRingStationInfoList.Count > 0)
            {
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(-1000);
              string translation = LanguageMgr.GetTranslation("RingStation.RankAward");
              if (itemTemplate != null)
              {
                foreach (UserRingStationInfo userRingStationInfo in userRingStationInfoList)
                {
                  int num = RingStationMgr._ringstationConfigInfo.AwardNumByRank(userRingStationInfo.Rank);
                  List<SqlDataProvider.Data.ItemInfo> infos = new List<SqlDataProvider.Data.ItemInfo>();
                  if (num > 0)
                  {
                    SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(itemTemplate, 1, 102);
                    fromTemplate.Count = num;
                    fromTemplate.ValidDate = 0;
                    fromTemplate.IsBinds = true;
                    infos.Add(fromTemplate);
                    if (WorldEventMgr.SendItemsToMail(infos, userRingStationInfo.UserID, userRingStationInfo.Info.NickName, translation, (string) null))
                      WorldMgr.GetPlayerById(userRingStationInfo.UserID)?.Out.SendMailResponse(userRingStationInfo.UserID, eMailRespose.Receiver);
                  }
                }
              }
            }
          }
          int num1 = flag ? 1 : 0;
        }
        Thread.CurrentThread.Priority = priority;
        int num2 = Environment.TickCount - tickCount;
        RingStationMgr.log.Info((object) "End Scan RingStation Info....");
      }
      catch (Exception ex)
      {
        RingStationMgr.log.Error((object) "StatusScan ", ex);
      }
    }

    public static bool UpdateRingStationFight(UserRingStationInfo ring)
    {
      bool flag;
      if (ring == null)
      {
        flag = false;
      }
      else
      {
        lock (RingStationMgr.m_lock)
        {
          if (RingStationMgr.dictionary_1.ContainsKey(ring.UserID))
          {
            RingStationMgr.dictionary_1[ring.UserID] = ring;
            return true;
          }
        }
        flag = false;
      }
      return flag;
    }

    public static bool UpdateRingStationInfo(UserRingStationInfo ring)
    {
      if (ring != null)
      {
        using (new PlayerBussiness())
        {
          lock (RingStationMgr.m_lock)
          {
            if (RingStationMgr.dictionary_1.ContainsKey(ring.UserID))
            {
              RingStationMgr.dictionary_1[ring.UserID] = ring;
              return true;
            }
          }
        }
      }
      return false;
    }

    public static string GetAutoBotStyleRandom(string weapon)
    {
      string str = string.Format("1142|head51,,3107|hair6,,5160|cloth60,6103|face2,{0},,,,,,,,,", (object) weapon);
      switch (new Random().Next(1, 20))
      {
        case 1:
          str = string.Format("1142|head51,,3158|hair58,,5106|cloth5,6103|face2,{0},,,,,,,,,", (object) weapon);
          break;
        case 2:
          str = string.Format("1142|head51,,3158|hair58,,5180|cloth80,6103|face2,{0},,,,,,,,,", (object) weapon);
          break;
        case 3:
          str = string.Format(",,3150|hair50,,5106|cloth5,,{0},,,,,,,,,", (object) weapon);
          break;
        case 4:
          str = string.Format("1144|head53,,3150|hair50,,5180|cloth80,6103|face2,{0},,,,,,,,,", (object) weapon);
          break;
        case 5:
          str = string.Format(",,3150|hair50,,5106|cloth5,,{0},,,,17002|offhand2,,,,,", (object) weapon);
          break;
        case 6:
          str = string.Format("1137|head42,2106|glass5,3150|hair50,,5130|cloth30,,{0},13144|suits44,,", (object) weapon);
          break;
        case 7:
          str = string.Format(",,3170|hair70,4104|eff4,5303|cloth103,6134|face33,{0},,,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 8:
          str = string.Format(",,3308|hair108,4104|eff4,5145|cloth45,6131|face30,{0},13136|suits36,15048|wing48,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 9:
          str = string.Format(",,3164|hair64,4104|eff4,5112|cloth11,6128|face27,{0},13136|suits36,15048|wing48,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 10:
          str = string.Format(",,3160|hair60,4104|eff4,5140|cloth40,6135|face34,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 11:
          str = string.Format(",,,,,,{0},13145|suits45,,,,,,,,", (object) weapon);
          break;
        case 12:
          str = string.Format(",,3163|hair63,4104|eff4,5107|cloth6,6120|face19,{0},13136|suits36,15004|wing004,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 13:
          str = string.Format(",,3150|hair50,4148|eff48,5136|cloth36,6115|face14,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 14:
          str = string.Format(",,3138|hair38,4104|eff4,5107|cloth6,6120|face19,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 15:
          str = string.Format(",,3143|hair43,4117|eff17,5137|cloth37,6105|face4,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 16:
          str = string.Format(",,3125|hair25,4118|eff18,5128|cloth28,6116|face15,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 17:
          str = string.Format(",,3110|hair9,4117|eff17,5137|cloth37,6120|face19,{0},13136|suits36,15026|wing026,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 18:
          str = string.Format(",,3135|hair35,4117|eff17,5131|cloth31,6105|face4,{0},13136|suits36,15004|wing004,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 19:
          str = string.Format(",,3151|hair51,4117|eff17,5141|cloth41,6110|face9,{0},13136|suits36,15003|wing003,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
        case 20:
          str = string.Format(",,3107|hair6,4117|eff17,5135|cloth35,6120|face19,{0},13136|suits36,15003|wing003,,17002|offhand2,9522|ring55,,,,", (object) weapon);
          break;
      }
      return str;
    }

    public static RingstationConfigInfo ConfigInfo
    {
      get
      {
        return RingStationMgr._ringstationConfigInfo;
      }
    }

    public static VirtualPlayerInfo NormalPlayer
    {
      get
      {
        return RingStationMgr._virtualPlayerInfo;
      }
      set
      {
        RingStationMgr._virtualPlayerInfo = value;
      }
    }
  }
}
