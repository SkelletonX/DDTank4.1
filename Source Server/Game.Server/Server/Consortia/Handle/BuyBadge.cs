// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.BuyBadge
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(28)]
  public class BuyBadge : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      bool result = false;
      int num = packet.ReadInt();
      ConsortiaBadgeConfigInfo consortiaBadgeConfig = ConsortiaExtraMgr.FindConsortiaBadgeConfig(num);
      if (consortiaBadgeConfig == null)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("BuyBadgeHandler.Fail"));
        return 0;
      }
      string msg = "BuyBadgeHandler.Fail";
      int ValidDate = 30;
      string BadgeBuyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(Player.PlayerCharacter.ConsortiaID);
      if (consortiaInfo == null)
        return 0;
      if (consortiaInfo.Riches < consortiaBadgeConfig.Cost)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("BuyBadgeHandler.Fail"));
        return 0;
      }
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        consortiaInfo.BadgeID = num;
        consortiaInfo.ValidDate = ValidDate;
        consortiaInfo.BadgeBuyTime = BadgeBuyTime;
        if (consortiaBussiness.BuyBadge(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, consortiaInfo, ref msg))
        {
          msg = "BuyBadgeHandler.Success";
          result = true;
        }
      }
      if (result)
      {
        using (PlayerBussiness playerBussiness = new PlayerBussiness())
        {
          foreach (ConsortiaUserInfo memberByConsortium in playerBussiness.GetAllMemberByConsortia(Player.PlayerCharacter.ConsortiaID))
          {
            GamePlayer playerById = WorldMgr.GetPlayerById(memberByConsortium.UserID);
            if (playerById != null && playerById.PlayerId != Player.PlayerCharacter.ID)
            {
              playerById.UpdateBadgeId(num);
              playerById.SendMessage(LanguageMgr.GetTranslation("A sua sociedade mudou de emblema!"));
              playerById.UpdateProperties();
            }
          }
        }
      }
      Player.Out.sendBuyBadge(Player.PlayerCharacter.ConsortiaID, num, ValidDate, result, BadgeBuyTime, Player.PlayerCharacter.ID);
      Player.SendMessage(LanguageMgr.GetTranslation(msg));
      Player.UpdateBadgeId(num);
      Player.UpdateProperties();
      if (result)
      {
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          consortiaBussiness.UpdateConsortiaRiches(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, consortiaBadgeConfig.Cost, ref msg);
      }
      return 0;
    }
  }
}
