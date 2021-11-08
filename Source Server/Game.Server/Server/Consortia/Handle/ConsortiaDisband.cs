// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaDisband
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(2)]
  public class ConsortiaDisband : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int consortiaId = Player.PlayerCharacter.ConsortiaID;
      string consortiaName = Player.PlayerCharacter.ConsortiaName;
      bool val = false;
      string msg = "ConsortiaDisbandHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.DeleteConsortia(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, ref msg))
        {
          val = true;
          msg = "ConsortiaDisbandHandler.Success1";
          Player.ClearConsortia(true);
          GameServer.Instance.LoginServer.SendConsortiaDelete(consortiaId);
        }
      }
      string str = LanguageMgr.GetTranslation(msg);
      if (msg == "ConsortiaDisbandHandler.Success1")
        str = str + consortiaName + LanguageMgr.GetTranslation("ConsortiaDisbandHandler.Success2");
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 2);
      if (val)
      {
        packet1.WriteBoolean(val);
        packet1.WriteInt(Player.PlayerCharacter.ID);
        packet1.WriteString(str);
      }
      else
      {
        packet1.WriteInt(Player.PlayerCharacter.ID);
        packet1.WriteString(str);
      }
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
