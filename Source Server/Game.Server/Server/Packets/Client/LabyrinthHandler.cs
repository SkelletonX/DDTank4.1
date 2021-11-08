// Decompiled with JetBrains decompiler
// Type: Game.Server.Packets.Client.LabyrinthHandler
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
  [PacketHandler(131, "场景用户离开")]
  public class LabyrinthHandler : IPacketHandler
  {
    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
      LabyrinthPackageType labyrinthPackageType = (LabyrinthPackageType) packet.ReadInt();
      int sType = 0;
      UserLabyrinthInfo laby = client.Player.Labyrinth ?? client.Player.LoadLabyrinth(sType);
      int id = client.Player.PlayerCharacter.ID;
      switch (labyrinthPackageType)
      {
        case LabyrinthPackageType.DOUBLE_REWARD:
          bool flag = packet.ReadBoolean();
          if (client.Player.PropBag.GetItemByTemplateID(0, 11916) == null)
            return 0;
          if (flag && !laby.isDoubleAward && client.Player.RemoveTemplate(11916, 1))
            laby.isDoubleAward = flag;
          client.Player.Out.SendLabyrinthUpdataInfo(id, laby);
          break;
        case LabyrinthPackageType.REQUEST_UPDATE:
          if (laby.isValidDate())
          {
            laby.completeChallenge = true;
            laby.accumulateExp = 0;
            laby.isInGame = false;
            laby.currentFloor = 1;
            laby.tryAgainComplete = true;
            laby.LastDate = DateTime.Now;
            laby.ProcessAward = client.Player.InitProcessAward();
          }
          client.Player.CalculatorClearnOutLabyrinth();
          client.Player.Out.SendLabyrinthUpdataInfo(id, laby);
          break;
        case LabyrinthPackageType.CLEAN_OUT:
          int warriorFamRaidDdtPrice = GameProperties.WarriorFamRaidDDTPrice;
          if (client.Player.PlayerCharacter.GiftToken < warriorFamRaidDdtPrice)
          {
            client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg1"));
            client.Player.Actives.StopCleantOutLabyrinth();
            break;
          }
          laby.isCleanOut = true;
          client.Player.RemoveGiftToken(warriorFamRaidDdtPrice);
          client.Player.Actives.CleantOutLabyrinth();
          break;
        case LabyrinthPackageType.SPEEDED_UP_CLEAN_OUT:
          if (!laby.isCleanOut)
          {
            client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg2"));
            return 0;
          }
          int num1 = Math.Abs(laby.currentRemainTime / 60);
          int num2 = GameProperties.WarriorFamRaidPricePerMin * num1;
          if (client.Player.MoneyDirect(num2, false))
          {
            client.Player.Actives.SpeededUpCleantOutLabyrinth();
            break;
          }
          break;
        case LabyrinthPackageType.STOP_CLEAN_OUT:
          client.Player.Actives.StopCleantOutLabyrinth();
          break;
        case LabyrinthPackageType.RESET_LABYRINTH:
          if (laby.tryAgainComplete)
          {
            laby.currentFloor = 1;
            laby.accumulateExp = 0;
            laby.tryAgainComplete = false;
            laby.ProcessAward = client.Player.InitProcessAward();
            client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg4"));
            client.Player.Out.SendLabyrinthUpdataInfo(id, laby);
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg5"));
          break;
        case LabyrinthPackageType.TRY_AGAIN:
          int num3 = packet.ReadBoolean() ? 1 : 0;
          packet.ReadBoolean();
          if ((uint) num3 > 0U)
          {
            int num4 = client.Player.LabyrinthTryAgainMoney();
            if (client.Player.RemoveMoney(num4) > 0)
            {
              laby.completeChallenge = true;
              laby.isInGame = true;
              client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg6"));
              break;
            }
            break;
          }
          client.Player.SendMessage(LanguageMgr.GetTranslation("Labyrinth.Msg4"));
          break;
        default:
          Console.WriteLine("LabyrinthPackageType: " + (object) labyrinthPackageType);
          break;
      }
      return 0;
    }
  }
}
