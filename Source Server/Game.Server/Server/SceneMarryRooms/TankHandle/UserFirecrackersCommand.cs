// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.TankHandle.UserFirecrackersCommand
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System.Linq;

namespace Game.Server.SceneMarryRooms.TankHandle
{
  [MarryCommandAttbute(6)]
  public class UserFirecrackersCommand : IMarryCommandHandler
  {
    public bool HandleCommand(
      TankMarryLogicProcessor process,
      GamePlayer player,
      GSPacketIn packet)
    {
      if (player.CurrentMarryRoom != null)
      {
        packet.ReadInt();
        ShopItemInfo shopItemInfo = ShopMgr.FindShopbyTemplatID(packet.ReadInt()).FirstOrDefault<ShopItemInfo>();
        if (shopItemInfo != null)
        {
          if (shopItemInfo.APrice1 == -2)
          {
            if (player.PlayerCharacter.Gold >= shopItemInfo.AValue1)
            {
              player.RemoveGold(shopItemInfo.AValue1);
              player.CurrentMarryRoom.ReturnPacketForScene(player, packet);
              player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("UserFirecrackersCommand.Successed1", (object) shopItemInfo.AValue1));
              player.OnUsingItem(shopItemInfo.TemplateID, 1);
              return true;
            }
            player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserFirecrackersCommand.GoldNotEnough"));
          }
          if (shopItemInfo.APrice1 == -1)
          {
            if (player.PlayerCharacter.Money + player.PlayerCharacter.MoneyLock >= shopItemInfo.AValue1)
            {
              player.RemoveMoney(shopItemInfo.AValue1);
              player.CurrentMarryRoom.ReturnPacketForScene(player, packet);
              player.Out.SendMessage(eMessageType.ChatNormal, LanguageMgr.GetTranslation("UserFirecrackersCommand.Successed2", (object) shopItemInfo.AValue1));
              player.OnUsingItem(shopItemInfo.TemplateID, 1);
              return true;
            }
            player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("UserFirecrackersCommand.MoneyNotEnough"));
          }
        }
      }
      return false;
    }
  }
}
