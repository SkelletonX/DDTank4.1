// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaTryinPass
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(4)]
  public class ConsortiaTryinPass : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int num = packet.ReadInt();
      bool val = false;
      string msg = "ConsortiaApplyLoginPassHandler.Failed";
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        int consortiaRepute = 0;
        ConsortiaUserInfo info = new ConsortiaUserInfo();
        if (consortiaBussiness.PassConsortiaApplyUsers(num, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, Player.PlayerCharacter.ConsortiaID, ref msg, info, ref consortiaRepute))
        {
          msg = "ConsortiaApplyLoginPassHandler.Success";
          val = true;
          if ((uint) info.UserID > 0U)
          {
            info.ConsortiaID = Player.PlayerCharacter.ConsortiaID;
            info.ConsortiaName = Player.PlayerCharacter.ConsortiaName;
            GameServer.Instance.LoginServer.SendConsortiaUserPass(Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, info, false, consortiaRepute);
          }
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 4);
      packet1.WriteInt(num);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
