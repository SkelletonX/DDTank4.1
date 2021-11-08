// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaPlacardUpdate
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
  [global::Consortia(15)]
  public class ConsortiaPlacardUpdate : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      string str = packet.ReadString();
      if (Encoding.Default.GetByteCount(str) > 300)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ConsortiaPlacardUpdateHandler.Long"));
        return 1;
      }
      bool val = false;
      string msg = "ConsortiaPlacardUpdateHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.UpdateConsortiaPlacard(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, str, ref msg))
        {
          msg = "ConsortiaPlacardUpdateHandler.Success";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 15);
      packet1.WriteString(str);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
