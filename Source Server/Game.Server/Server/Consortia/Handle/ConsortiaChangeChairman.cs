// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaChangeChairman
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(19)]
  public class ConsortiaChangeChairman : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      string str1 = packet.ReadString();
      bool val = false;
      string msg = "ConsortiaChangeChairmanHandler.Failed";
      if (string.IsNullOrEmpty(str1))
        msg = "ConsortiaChangeChairmanHandler.NoName";
      else if (str1 == Player.PlayerCharacter.NickName)
      {
        msg = "ConsortiaChangeChairmanHandler.Self";
      }
      else
      {
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          string tempUserName = "";
          int tempUserID = 0;
          ConsortiaDutyInfo info1 = new ConsortiaDutyInfo();
          if (consortiaBussiness.UpdateConsortiaChairman(str1, Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg, ref info1, ref tempUserID, ref tempUserName))
          {
            ConsortiaDutyInfo info2 = new ConsortiaDutyInfo();
            info2.Level = Player.PlayerCharacter.DutyLevel;
            info2.DutyName = Player.PlayerCharacter.DutyName;
            info2.Right = Player.PlayerCharacter.Right;
            msg = "ConsortiaChangeChairmanHandler.Success1";
            val = true;
            GameServer.Instance.LoginServer.SendConsortiaDuty(info2, 9, Player.PlayerCharacter.ConsortiaID, tempUserID, tempUserName, 0, "");
            GameServer.Instance.LoginServer.SendConsortiaDuty(info1, 8, Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, 0, "");
          }
        }
      }
      string translation = LanguageMgr.GetTranslation(msg);
      if (msg == "ConsortiaChangeChairmanHandler.Success1")
      {
        string str2 = translation + str1 + LanguageMgr.GetTranslation("ConsortiaChangeChairmanHandler.Success2");
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 19);
      packet1.WriteString(str1);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
