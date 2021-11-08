// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaRenegade
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(3)]
  public class ConsortiaRenegade : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int num = packet.ReadInt();
      bool val = false;
      string nickName = "";
      string msg = num == Player.PlayerCharacter.ID ? "ConsortiaUserDeleteHandler.ExitFailed" : "ConsortiaUserDeleteHandler.KickFailed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.DeleteConsortiaUser(Player.PlayerCharacter.ID, num, Player.PlayerCharacter.ConsortiaID, ref msg, ref nickName))
        {
          msg = num == Player.PlayerCharacter.ID ? "ConsortiaUserDeleteHandler.ExitSuccess" : "ConsortiaUserDeleteHandler.KickSuccess";
          int consortiaId = Player.PlayerCharacter.ConsortiaID;
          if (num == Player.PlayerCharacter.ID)
          {
            Player.ClearConsortia(true);
            Player.Out.SendMailResponse(Player.PlayerCharacter.ID, eMailRespose.Receiver);
          }
          GameServer.Instance.LoginServer.SendConsortiaUserDelete(num, consortiaId, num != Player.PlayerCharacter.ID, nickName, Player.PlayerCharacter.NickName);
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 3);
      packet1.WriteInt(num);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
