// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaUserGradeUpdate
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(18)]
  public class ConsortiaUserGradeUpdate : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int num = packet.ReadInt();
      bool flag = packet.ReadBoolean();
      bool val = false;
      string msg = "ConsortiaUserGradeUpdateHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        string tempUserName = "";
        ConsortiaDutyInfo info = new ConsortiaDutyInfo();
        if (consortiaBussiness.UpdateConsortiaUserGrade(num, Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, flag, ref msg, ref info, ref tempUserName))
        {
          msg = "ConsortiaUserGradeUpdateHandler.Success";
          val = true;
          GameServer.Instance.LoginServer.SendConsortiaDuty(info, flag ? 6 : 7, Player.PlayerCharacter.ConsortiaID, num, tempUserName, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName);
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 18);
      packet1.WriteInt(num);
      packet1.WriteBoolean(flag);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
