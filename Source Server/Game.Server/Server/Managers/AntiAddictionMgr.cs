// Decompiled with JetBrains decompiler
// Type: Game.Server.Managers.AntiAddictionMgr
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Server.GameObjects;
using System;

namespace Game.Server.Managers
{
  internal class AntiAddictionMgr
  {
    private static bool _isASSon;
    public static int count;

    public static int AASStateGet(GamePlayer player)
    {
      int id = player.PlayerCharacter.ID;
      bool result = true;
      player.IsAASInfo = false;
      player.IsMinor = true;
      using (PlayerBussiness playerBussiness = new PlayerBussiness())
      {
        string assInfoSingle = playerBussiness.GetASSInfoSingle(id);
        if (assInfoSingle != "")
        {
          player.IsAASInfo = true;
          result = false;
          int int32_1 = Convert.ToInt32(assInfoSingle.Substring(6, 4));
          int int32_2 = Convert.ToInt32(assInfoSingle.Substring(10, 2));
          DateTime now = DateTime.Now;
          if (now.Year.CompareTo(int32_1 + 18) <= 0)
          {
            now = DateTime.Now;
            int num = now.Year;
            if (num.CompareTo(int32_1 + 18) == 0)
            {
              now = DateTime.Now;
              num = now.Month;
              if (num.CompareTo(int32_2) < 0)
                goto label_9;
            }
            else
              goto label_9;
          }
          player.IsMinor = false;
        }
      }
label_9:
      if (result && player.PlayerCharacter.IsFirst != 0 && (player.PlayerCharacter.DayLoginCount < 1 && AntiAddictionMgr.ISASSon))
        player.Out.SendAASState(result);
      if (player.IsMinor || !player.IsAASInfo && AntiAddictionMgr.ISASSon)
        player.Out.SendAASControl(AntiAddictionMgr.ISASSon, player.IsAASInfo, player.IsMinor);
      return 0;
    }

    public static double GetAntiAddictionCoefficient(int onlineTime)
    {
      if (!AntiAddictionMgr._isASSon || 0 <= onlineTime && onlineTime <= 240)
        return 1.0;
      return 240 >= onlineTime || onlineTime > 300 ? 0.0 : 0.5;
    }

    public static void SetASSState(bool ASSState)
    {
      AntiAddictionMgr._isASSon = ASSState;
    }

    public static bool ISASSon
    {
      get
      {
        return AntiAddictionMgr._isASSon;
      }
    }
  }
}
