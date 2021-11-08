// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaLevelUp
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(21)]
  public class ConsortiaLevelUp : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      byte val1 = packet.ReadByte();
      string msg = "";
      string str = "";
      byte val2 = 0;
      bool val3 = false;
      ConsortiaInfo consortiaInfo = ConsortiaMgr.FindConsortiaInfo(Player.PlayerCharacter.ConsortiaID);
      switch (val1)
      {
        case 1:
          msg = "ConsortiaUpGradeHandler.Failed";
          using (ConsortiaBussiness consortiaBussiness1 = new ConsortiaBussiness())
          {
            ConsortiaInfo consortiaSingle = consortiaBussiness1.GetConsortiaSingle(Player.PlayerCharacter.ConsortiaID);
            if (consortiaSingle == null)
            {
              msg = "ConsortiaUpGradeHandler.NoConsortia";
            }
            else
            {
              ConsortiaLevelInfo consortiaLevelInfo = ConsortiaExtraMgr.FindConsortiaLevelInfo(consortiaSingle.Level + 1);
              if (consortiaLevelInfo == null)
                msg = "ConsortiaUpGradeHandler.NoUpGrade";
              else if (consortiaLevelInfo.NeedGold > Player.PlayerCharacter.Gold)
              {
                msg = "ConsortiaUpGradeHandler.NoGold";
              }
              else
              {
                using (ConsortiaBussiness consortiaBussiness2 = new ConsortiaBussiness())
                {
                  if (consortiaBussiness2.UpGradeConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
                  {
                    ++consortiaSingle.Level;
                    Player.RemoveGold(consortiaLevelInfo.NeedGold);
                    GameServer.Instance.LoginServer.SendConsortiaUpGrade(consortiaSingle);
                    msg = "ConsortiaUpGradeHandler.Success";
                    val3 = true;
                    val2 = (byte) consortiaSingle.Level;
                  }
                }
              }
            }
            if (consortiaSingle.Level >= 5)
            {
              str = LanguageMgr.GetTranslation("ConsortiaUpGradeHandler.Notice", (object) consortiaSingle.ConsortiaName, (object) consortiaSingle.Level);
              break;
            }
            break;
          }
        case 2:
          msg = "ConsortiaStoreUpGradeHandler.Failed";
          if (consortiaInfo == null)
          {
            msg = "ConsortiaStoreUpGradeHandler.NoConsortia";
            break;
          }
          using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
          {
            if (consortiaBussiness.UpGradeStoreConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
            {
              ++consortiaInfo.StoreLevel;
              GameServer.Instance.LoginServer.SendConsortiaStoreUpGrade(consortiaInfo);
              msg = "ConsortiaStoreUpGradeHandler.Success";
              val3 = true;
              val2 = (byte) consortiaInfo.StoreLevel;
              break;
            }
            break;
          }
        case 3:
          msg = "ConsortiaShopUpGradeHandler.Failed";
          if (consortiaInfo == null)
          {
            msg = "ConsortiaShopUpGradeHandler.NoConsortia";
          }
          else
          {
            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
              if (consortiaBussiness.UpGradeShopConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
              {
                ++consortiaInfo.ShopLevel;
                GameServer.Instance.LoginServer.SendConsortiaShopUpGrade(consortiaInfo);
                msg = "ConsortiaShopUpGradeHandler.Success";
                val3 = true;
                val2 = (byte) consortiaInfo.ShopLevel;
              }
            }
          }
          if (consortiaInfo.ShopLevel >= 2)
          {
            str = LanguageMgr.GetTranslation("ConsortiaShopUpGradeHandler.Notice", (object) Player.PlayerCharacter.ConsortiaName, (object) consortiaInfo.ShopLevel);
            break;
          }
          break;
        case 4:
          msg = "ConsortiaSmithUpGradeHandler.Failed";
          if (consortiaInfo == null)
          {
            msg = "ConsortiaSmithUpGradeHandler.NoConsortia";
          }
          else
          {
            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
              if (consortiaBussiness.UpGradeSmithConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
              {
                ++consortiaInfo.SmithLevel;
                GameServer.Instance.LoginServer.SendConsortiaSmithUpGrade(consortiaInfo);
                msg = "ConsortiaSmithUpGradeHandler.Success";
                val3 = true;
                val2 = (byte) consortiaInfo.SmithLevel;
              }
            }
          }
          if (consortiaInfo.SmithLevel >= 3)
          {
            str = LanguageMgr.GetTranslation("ConsortiaSmithUpGradeHandler.Notice", (object) Player.PlayerCharacter.ConsortiaName, (object) consortiaInfo.SmithLevel);
            break;
          }
          break;
        case 5:
          msg = "ConsortiaBufferUpGradeHandler.Failed";
          if (consortiaInfo == null)
          {
            msg = "ConsortiaUpGradeHandler.NoConsortia";
          }
          else
          {
            using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
            {
              if (consortiaBussiness.UpGradeSkillConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
              {
                ++consortiaInfo.SkillLevel;
                GameServer.Instance.LoginServer.SendConsortiaKillUpGrade(consortiaInfo);
                msg = "ConsortiaBufferUpGradeHandler.Success";
                val3 = true;
                val2 = (byte) consortiaInfo.SkillLevel;
              }
            }
          }
          if (consortiaInfo.SkillLevel >= 3)
          {
            str = LanguageMgr.GetTranslation("ConsortiaBufferUpGradeHandler.Notice", (object) Player.PlayerCharacter.ConsortiaName, (object) consortiaInfo.SmithLevel);
            break;
          }
          break;
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 21);
      packet1.WriteByte(val1);
      packet1.WriteByte(val2);
      packet1.WriteBoolean(val3);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      if (str != "")
      {
        GSPacketIn packet2 = new GSPacketIn((short) 10);
        packet2.WriteInt(2);
        packet2.WriteString(str);
        GameServer.Instance.LoginServer.SendPacket(packet2);
        foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
        {
          if (allPlayer != Player)
            allPlayer.Out.SendTCP(packet2);
        }
      }
      return 0;
    }
  }
}
