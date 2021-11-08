// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaChat
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(20)]
  public class ConsortiaChat : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      if (Player.PlayerCharacter.IsBanChat)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ConsortiaChatHandler.IsBanChat"));
        return 1;
      }
      packet.ClientID = Player.PlayerCharacter.ID;
      int num = (int) packet.ReadByte();
      packet.ReadString();
      packet.ReadString();
      packet.WriteInt(Player.PlayerCharacter.ConsortiaID);
      foreach (GamePlayer allPlayer in WorldMgr.GetAllPlayers())
      {
        if (allPlayer.PlayerCharacter.ConsortiaID == Player.PlayerCharacter.ConsortiaID)
          allPlayer.Out.SendTCP(packet);
      }
      GameServer.Instance.LoginServer.SendPacket(packet);
      return 0;
    }
  }
}
