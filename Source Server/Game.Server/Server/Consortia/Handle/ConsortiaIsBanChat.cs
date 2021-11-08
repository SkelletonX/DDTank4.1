// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaIsBanChat
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(16)]
  public class ConsortiaIsBanChat : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int num = packet.ReadInt();
      bool flag = packet.ReadBoolean();
      int tempID = 0;
      string tempName = "";
      bool val = false;
      string msg = "ConsortiaIsBanChatHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.UpdateConsortiaIsBanChat(num, Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, flag, ref tempID, ref tempName, ref msg))
        {
          msg = "ConsortiaIsBanChatHandler.Success";
          val = true;
          GameServer.Instance.LoginServer.SendConsortiaBanChat(tempID, tempName, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, flag);
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 16);
      packet1.WriteInt(num);
      packet1.WriteBoolean(flag);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
