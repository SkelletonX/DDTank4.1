using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
  [PacketHandler(258, "Novice Activity")]
  public class NoviceActivityGetAward : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int SubActivityType = packet.ReadInt();
      if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(1.5), DateTime.Now) > 0)
        return 0;
      bool isPlus = false;
      string translateId = "Parabéns você completou o evento, verifique sua caixa de correio.";
      ProduceBussiness produceBussiness = new ProduceBussiness();
      EventRewardProcessInfo eventProcess = client.Player.Extra.GetEventProcess(num);
      List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
      int awardGot = SubActivityType != 1 ? eventProcess.AwardGot * 2 + 1 : 1;
      switch (awardGot)
      {
        case 1:
          SubActivityType = 1;
          break;
        case 3:
          SubActivityType = 2;
          break;
        case 7:
          SubActivityType = 3;
          break;
        case 15:
          SubActivityType = 4;
          break;
        case 31:
          SubActivityType = 5;
          break;
        case 63:
          SubActivityType = 6;
          break;
        case (int) sbyte.MaxValue:
          SubActivityType = 7;
          break;
        case (int) byte.MaxValue:
          SubActivityType = 8;
          break;
        case 511:
          SubActivityType = 9;
          break;
      }
      EventRewardInfo[] rewardInfoByType = produceBussiness.GetEventRewardInfoByType(num, SubActivityType);
            foreach (EventRewardGoodsInfo eventRewardGoodsInfo in produceBussiness.GetEventRewardGoodsByType(num, SubActivityType))
            {
                SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventRewardGoodsInfo.TemplateId), 1, 104);
                if (fromTemplate != null)
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
                    Console.WriteLine("Erro No Evento de Recarga {0}", SubActivityType);
                    client.Player.SendMessage("Houve Um erro Por favor Contate ao ADM");
                }
            }
      foreach (EventRewardInfo eventRewardInfo in rewardInfoByType)
      {
        if (awardGot != 999)
        {
          client.Player.Extra.UpdateEventCondition(num, eventRewardInfo.Condition, isPlus, awardGot);
          client.Player.SendItemsToMail(items, LanguageMgr.GetTranslation("Parabéns você completou o evento"), LanguageMgr.GetTranslation("Premiação Evento"), eMailType.Manage);
        }
      }
      client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
      client.Player.LastOpenCard = DateTime.Now;
      return 1;
    }
  }
}
