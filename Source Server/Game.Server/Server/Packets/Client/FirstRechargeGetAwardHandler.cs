// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.FirstRechargeGetAwardHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(259, "FIRSTRECHARGE")]
  public class FirstRechargeGetAwardHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      packet.ReadInt();
      if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(0.5), DateTime.Now) > 0)
        return 0;
      string translateId = "FirstRechargeGetAward.Successfull";
      ProduceBussiness produceBussiness = new ProduceBussiness();
      EventRewardInfo[] rewardInfoByType = produceBussiness.GetEventRewardInfoByType(5, 1);
      EventRewardGoodsInfo[] rewardGoodsByType = produceBussiness.GetEventRewardGoodsByType(5, 1);
      List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
      foreach (EventRewardGoodsInfo eventRewardGoodsInfo in rewardGoodsByType)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventRewardGoodsInfo.TemplateId), 1, 104);
        if(fromTemplate != null)
                { 
        fromTemplate.StrengthenLevel = eventRewardGoodsInfo.StrengthLevel;
        fromTemplate.AttackCompose = eventRewardGoodsInfo.AttackCompose;
        fromTemplate.DefendCompose = eventRewardGoodsInfo.DefendCompose;
        fromTemplate.AgilityCompose = eventRewardGoodsInfo.AgilityCompose;
        fromTemplate.LuckCompose = eventRewardGoodsInfo.LuckCompose;
        fromTemplate.IsBinds = eventRewardGoodsInfo.IsBind;
        fromTemplate.Count = eventRewardGoodsInfo.Count;
        fromTemplate.ValidDate = eventRewardGoodsInfo.ValidDate;
                    items.Add(fromTemplate);
                }
        else
                {
                    //Fix Item Inexistente
                    client.Player.SendMessage("Erro ao enviar o item contate o GM");
                    Console.WriteLine("Item inexistente no evento :{0} - {1}", eventRewardGoodsInfo.ActivityType, eventRewardGoodsInfo.SubActivityType);
                }
                
      }
      string str;
      if (!client.Player.PlayerCharacter.IsRecharged)
      {
        str = "FirstRechargeGetAward.NotCharge";
        return 0;
      }
      if (client.Player.PlayerCharacter.IsGetAward)
      {
        str = "FirstRechargeGetAward.AlreadyGetAward";
        return 0;
      }
      foreach (EventRewardInfo eventRewardInfo in rewardInfoByType)
      {
        if (client.Player.PlayerCharacter.IsRecharged && !client.Player.PlayerCharacter.IsGetAward)
        {
          if (client.Player.SendItemsToMail(items, LanguageMgr.GetTranslation("FirstRechargeGetAward.Content"), LanguageMgr.GetTranslation("FirstRechargeGetAward.Title"), eMailType.Manage))
          {
            client.Player.PlayerCharacter.IsGetAward = true;
          }
          else
          {
            str = "FirstRechargeGetAward.Error";
            return 0;
          }
        }
      }
      client.Player.Out.SendUpdateFirstRecharge(client.Player.PlayerCharacter.IsRecharged, client.Player.PlayerCharacter.IsGetAward);
      client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      client.Player.LastOpenCard = DateTime.Now;
      return 1;
    }
  }
}
