// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsotiaApplyState
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(7)]
  public class ConsotiaApplyState : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      bool flag = packet.ReadBoolean();
      bool val = false;
      string msg = "CONSORTIA_APPLY_STATE.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        if (consortiaBussiness.UpdateConsotiaApplyState(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, flag, ref msg))
        {
          msg = "CONSORTIA_APPLY_STATE.Success";
          val = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 7);
      packet1.WriteBoolean(flag);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
