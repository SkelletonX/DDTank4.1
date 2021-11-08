// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.CardDataHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(216, "防沉迷系统开关")]
  public class CardDataHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      CardInventory cardBag = client.Player.CardBag;
      cardBag.BeginChanges();
      switch (num1)
      {
        case 0:
          int num2 = packet.ReadInt();
          int num3 = packet.ReadInt();
          if (num2 == num3 && num2 >= 5)
            return 0;
          if (num2 < 5 && num3 >= 5 || num2 == num3 && num2 < 5)
          {
            cardBag.RemoveCardAt(num2);
            client.Player.EquipBag.UpdatePlayerProperties();
            break;
          }
          if (num2 >= 5 && num3 < 5)
          {
            UsersCardInfo itemAt = cardBag.GetItemAt(num2);
            if (itemAt != null)
            {
              if (!cardBag.IsCardEquip(itemAt.TemplateID))
              {
                cardBag.RemoveCardAt(num3);
                UsersCardInfo card = itemAt.Clone();
                card.Count = 0;
                cardBag.AddCardTo(card, num3);
                client.Player.EquipBag.UpdatePlayerProperties();
                break;
              }
              client.Player.SendMessage("Este cartão já está equipado");
              break;
            }
            break;
          }
          cardBag.MoveCard(num2, num3);
          break;
        case 1:
          packet.ReadInt();
          break;
        case 2:
          int slot1 = packet.ReadInt();
          int count1 = packet.ReadInt();
          SqlDataProvider.Data.ItemInfo itemAt1 = client.Player.EquipBag.GetItemAt(slot1);
          if (itemAt1 != null)
          {
            if (count1 > 0 && count1 <= itemAt1.Count)
            {
              int property5 = itemAt1.Template.Property5;
              ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(property5);
              if (itemTemplate != null && itemTemplate.CategoryID == 26)
              {
                client.Player.EquipBag.RemoveCountFromStack(itemAt1, itemAt1.Count);
                int count2 = 0;
                Random random = new Random();
                for (int index = 0; index < itemAt1.Count; ++index)
                  count2 += random.Next(1, 3);
                cardBag.AddCard(property5, count2);
                break;
              }
              client.Player.SendMessage("Erro ao adicionar a carta");
              return 0;
            }
            client.Player.SendMessage("Quantidade máxima detectada");
            return 0;
          }
          client.Player.SendMessage("Esta caixa de cartão não existe");
          break;
        case 3:
          int slot2 = packet.ReadInt();
          if (slot2 < 5)
            return 0;
          UsersCardInfo itemAt2 = cardBag.GetItemAt(slot2);
          if (itemAt2 != null)
          {
            if (itemAt2.Level < CardMgr.MaxLevel())
            {
              CardUpdateConditionInfo cardUpdateCondition = CardMgr.GetCardUpdateCondition(itemAt2.Level + 1);
              if (cardUpdateCondition != null && itemAt2.Count >= cardUpdateCondition.UpdateCardCount)
              {
                Random random = new Random();
                itemAt2.Count -= cardUpdateCondition.UpdateCardCount;
                itemAt2.CardGP += random.Next(cardUpdateCondition.MinExp, cardUpdateCondition.MaxExp);
                if (itemAt2.CardGP >= cardUpdateCondition.Exp)
                {
                  CardUpdateInfo cardUpdateInfo = CardMgr.GetCardUpdateInfo(itemAt2.TemplateID, cardUpdateCondition.Level);
                  if (cardUpdateInfo != null)
                  {
                    ++itemAt2.Level;
                    itemAt2.Attack += cardUpdateInfo.Attack;
                    itemAt2.Defence += cardUpdateInfo.Defend;
                    itemAt2.Agility += cardUpdateInfo.Agility;
                    itemAt2.Luck += cardUpdateInfo.Lucky;
                    itemAt2.Damage += cardUpdateInfo.Damage;
                    itemAt2.Guard += cardUpdateInfo.Guard;
                    UsersCardInfo cardEquip = cardBag.GetCardEquip(itemAt2.TemplateID);
                    if (cardEquip != null)
                    {
                      cardEquip.CopyProp(itemAt2);
                      cardBag.UpdateCard(cardEquip);
                      client.Player.EquipBag.UpdatePlayerProperties();
                    }
                  }
                  else
                    Console.WriteLine("cardUpInfo is null - " + (object) itemAt2.TemplateID + " - " + (object) cardUpdateCondition.Level + " - exp: " + (object) cardUpdateCondition.Exp);
                }
                cardBag.UpdateCard(itemAt2);
                break;
              }
              client.Player.SendMessage("O número de cartões não é suficiente para atualizar.");
              break;
            }
            client.Player.SendMessage("A tag alcançou seu nível máximo");
            break;
          }
          break;
        case 4:
          packet.ReadInt();
          packet.ReadInt();
          packet.ReadInt();
          break;
      }
      cardBag.CommitChanges();
      return 0;
    }
  }
}
