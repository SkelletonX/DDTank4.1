// Decompiled with JetBrains decompiler
// Type: Game.Logic.BallConfigMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Bussiness;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Logic
{
  public class BallConfigMgr
  {
    private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static Dictionary<int, BallConfigInfo> m_infos;

    public static BallConfigInfo FindBall(int id)
    {
      if (BallConfigMgr.m_infos.ContainsKey(id))
        return BallConfigMgr.m_infos[id];
      return (BallConfigInfo) null;
    }

    public static bool Init()
    {
      return BallConfigMgr.ReLoad();
    }

    private static Dictionary<int, BallConfigInfo> LoadFromDatabase()
    {
      Dictionary<int, BallConfigInfo> dictionary = new Dictionary<int, BallConfigInfo>();
      using (ProduceBussiness produceBussiness = new ProduceBussiness())
      {
        foreach (BallConfigInfo ballConfigInfo in produceBussiness.GetAllBallConfig())
        {
          if (!dictionary.ContainsKey(ballConfigInfo.TemplateID))
            dictionary.Add(ballConfigInfo.TemplateID, ballConfigInfo);
        }
      }
      return dictionary;
    }

    public static bool ReLoad()
    {
      try
      {
        Dictionary<int, BallConfigInfo> dictionary = BallConfigMgr.LoadFromDatabase();
        if (dictionary.Values.Count > 0)
        {
          Interlocked.Exchange<Dictionary<int, BallConfigInfo>>(ref BallConfigMgr.m_infos, dictionary);
          return true;
        }
      }
      catch (Exception ex)
      {
        BallConfigMgr.log.Error((object) "Ball Mgr init error:", ex);
      }
      return false;
    }
  }
}
