// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaTryin
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
  [global::Consortia(0)]
  public class ConsortiaTryin : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if ((uint) Player.PlayerCharacter.ConsortiaID > 0U)
        return 0;
      int val1 = packet.ReadInt();
      bool val2 = false;
      string translateId = "ConsortiaApplyLoginHandler.ADD_Failed";
      using (ConsortiaBussiness consortiaBussiness1 = new ConsortiaBussiness())
      {
        ConsortiaBussiness consortiaBussiness2 = consortiaBussiness1;
        ConsortiaApplyUserInfo info = new ConsortiaApplyUserInfo();
        info.ApplyDate = DateTime.Now;
        info.ConsortiaID = val1;
        info.ConsortiaName = "";
        info.IsExist = true;
        info.Remark = "";
        info.UserID = Player.PlayerCharacter.ID;
        info.UserName = Player.PlayerCharacter.NickName;
        ref string local = ref translateId;
        if (consortiaBussiness2.AddConsortiaApplyUsers(info, ref local))
        {
          translateId = val1 != 0 ? "ConsortiaApplyLoginHandler.ADD_Success" : "ConsortiaApplyLoginHandler.DELETE_Success";
          val2 = true;
        }
      }
      GSPacketIn packet1 = new GSPacketIn((short) 129);
      packet1.WriteByte((byte) 0);
      packet1.WriteInt(val1);
      packet1.WriteBoolean(val2);
      packet1.WriteString(LanguageMgr.GetTranslation(translateId));
      Player.Out.SendTCP(packet1);
      return 0;
    }
  }
}
