// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaInviteAdd
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(11)]
  public class ConsortiaInviteAdd : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      string str = packet.ReadString();
      bool val = false;
      string msg = "ConsortiaInviteAddHandler.Failed";
      if (string.IsNullOrEmpty(str))
        return 0;
      using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
      {
        ConsortiaInviteUserInfo info = new ConsortiaInviteUserInfo();
        info.ConsortiaID = Player.PlayerCharacter.ConsortiaID;
        info.ConsortiaName = Player.PlayerCharacter.ConsortiaName;
        info.InviteDate = DateTime.Now;
        info.InviteID = Player.PlayerCharacter.ID;
        info.InviteName = Player.PlayerCharacter.NickName;
        info.IsExist = true;
        info.Remark = "";
        info.UserID = 0;
        info.UserName = str;
        if (consortiaBussiness.AddConsortiaInviteUsers(info, ref msg))
        {
          msg = "ConsortiaInviteAddHandler.Success";
          val = true;
          GameServer.Instance.LoginServer.SendConsortiaInvite(info.ID, info.UserID, info.UserName, info.InviteID, info.InviteName, info.ConsortiaName, info.ConsortiaID);
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 11);
      packet1.WriteString(str);
      packet1.WriteBoolean(val);
      packet1.WriteString(LanguageMgr.GetTranslation(msg));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
