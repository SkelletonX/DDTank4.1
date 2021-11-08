using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Logic.Cmd
{
  [GameCommand(32, "使用道具")]
  public class PropUseCommand : ICommandHandler
  {
    public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
    {
      if (game.GameState != eGameState.Playing || player.GetSealState())
        return;
      int bag = (int) packet.ReadByte();
      int place = packet.ReadInt();
      int templateId = packet.ReadInt();
      ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateId);
      int[] propBag = PropItemMgr.PropBag;
      if (bag == 2)
      {
        if (!new List<string>()
        {
          "10001",
          "10002",
          "10003",
          "10004",
          "10005",
          "10006",
          "10007",
          "10008",
          "10009",
          "10010",
          "10011",
          "10012",
          "10013",
          "10014",
          "10015",
          "10016",
          "10017",
          "10018",
          "10019",
          "10020",
          "10021",
          "10022"
        }.Contains(itemTemplate.TemplateID.ToString()))
          return;
      }
      if (itemTemplate == null || !player.CheckCanUseItem(itemTemplate) || !player.CanUseItem(itemTemplate))
        return;
      if (templateId == 10001 || templateId == 10002 || templateId == 10003)
        player.CanFly = false;
      if (player.PlayerDetail.UsePropItem((AbstractGame) game, bag, place, templateId, player.IsLiving))
      {
        if (player.UseItem(itemTemplate))
          return;
        BaseGame.log.Error((object) "Using prop error");
      }
      else
      {
        if (bag == 2 && !player.PlayerDetail.UsePropItem((AbstractGame) game, bag, place, templateId, player.IsLiving))
          return;
        if (((IEnumerable<int>) propBag).Contains<int>(itemTemplate.TemplateID))
        {
          player.UseItem(itemTemplate, place);
          switch (templateId)
          {
            case 10001:
              if (player.Prop < templateId * 2)
              {
                player.Prop += templateId;
                return;
              }
              break;
            case 10004:
              if (player.Prop < templateId * 2)
              {
                player.Prop += templateId;
                return;
              }
              break;
            default:
              return;
          }
        }
        else
          player.PlayerDetail.SendMessage("O item não pode ser usado");
        if (templateId != 10015 && itemTemplate.CategoryID != 17)
          return;
        player.ShootCount = 1;
      }
    }
  }
}
