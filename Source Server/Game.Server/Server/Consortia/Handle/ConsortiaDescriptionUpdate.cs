// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaDescriptionUpdate
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using System.Text;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(14)]
  public class ConsortiaDescriptionUpdate : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      string str = packet.ReadString();
      if (Encoding.Default.GetByteCount(str) > 300)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ConsortiaDescriptionUpdateHandler.Long"));
        return 1;
      }
      bool val = false;
      string msg = "ConsortiaDescriptionUpdateHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.UpdateConsortiaDescription(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, str, ref msg))
        {
          msg = "ConsortiaDescriptionUpdateHandler.Success";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 14);
      packet1.WriteString(str);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
