// Decompiled with JetBrains decompiler
// Type: Game.Logic.BallMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using Game.Logic.Phy.Maps;
using Game.Logic.Phy.Object;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class BallMgr
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, BallInfo> dictionary_0;
    private static Dictionary<int, Tile> dictionary_1;

    public static BallInfo FindBall(int id)
    {
      return !BallMgr.dictionary_0.ContainsKey(id) ? (BallInfo) null : BallMgr.dictionary_0[id];
    }

    public static Tile FindTile(int id)
    {
      return !BallMgr.dictionary_1.ContainsKey(id) ? (Tile) null : BallMgr.dictionary_1[id];
    }

    public static BombType GetBallType(int ballId)
    {
      switch (ballId)
      {
        case 1:
        case 56:
        case 99:
          return BombType.FORZEN;
        case 2:
        case 4:
          return BombType.Normal;
        case 3:
          return BombType.FLY;
        case 5:
          return BombType.CURE;
        case 59:
          return BombType.CURE;
        case 64:
          return BombType.CURE;
        case 97:
        case 98:
          return BombType.CURE;
        case 120:
        case 10009:
          return BombType.CURE;
        default:
          return BombType.Normal;
      }
    }

    public static bool Init()
    {
      return BallMgr.ReLoad();
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, BallInfo> list = BallMgr.smethod_0();
        Dictionary<int, Tile> dictionary = BallMgr.smethod_1(list);
        if (list.Values.Count > 0)
        {
          if (dictionary.Values.Count > 0)
          {
            Interlocked.Exchange<Dictionary<int, BallInfo>>(ref BallMgr.dictionary_0, list);
            Interlocked.Exchange<Dictionary<int, Tile>>(ref BallMgr.dictionary_1, dictionary);
            return true;
          }
        }
      }
      catch (Exception ex)
      {
        BallMgr.ilog_0.Error((object) "Ball Mgr init error:", ex);
      }
      return false;
    }

    private static Dictionary<int, BallInfo> smethod_0()
    {
      Dictionary<int, BallInfo> dictionary = new Dictionary<int, BallInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (BallInfo ballInfo in produceBussiness.GetAllBall())
        {
          if (!dictionary.ContainsKey(ballInfo.ID))
            dictionary.Add(ballInfo.ID, ballInfo);
        }
      }
      return dictionary;
    }

    private static Dictionary<int, Tile> smethod_1(Dictionary<int, BallInfo> list)
    {
      Dictionary<int, Tile> dictionary = new Dictionary<int, Tile>();
      foreach (BallInfo ballInfo in list.Values)
      {
        if (ballInfo.HasTunnel)
        {
          string str = string.Format("bomb\\{0}.bomb", (object) ballInfo.ID);
          Tile tile = (Tile) null;
          if (File.Exists(str))
            tile = new Tile(str, false);
          dictionary.Add(ballInfo.ID, tile);
          if (tile == null && ballInfo.ID != 1 && (ballInfo.ID != 2 && ballInfo.ID != 3))
            BallMgr.ilog_0.ErrorFormat("can't find bomb file:{0}", (object) str);
        }
      }
      return dictionary;
    }
  }
}
