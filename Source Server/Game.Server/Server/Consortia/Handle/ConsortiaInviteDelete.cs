// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaInviteDelete
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(13)]
  public class ConsortiaInviteDelete : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      bool val = false;
      string translateId = "ConsortiaInviteDeleteHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.DeleteConsortiaInviteUsers(num, Player.PlayerCharacter.ID))
        {
          translateId = "ConsortiaInviteDeleteHandler.Success";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 13);
      packet1.WriteInt(num);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(translateId));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
