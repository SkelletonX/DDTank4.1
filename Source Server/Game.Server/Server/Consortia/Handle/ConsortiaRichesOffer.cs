// Decompiled with JetBrains decompiler
// Type: Game.Server.Consortia.Handle.ConsortiaRichesOffer
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;

namespace Game.Server.Consortia.Handle
{
  [global::Consortia(6)]
  public class ConsortiaRichesOffer : IConsortiaCommandHadler
  {
    public int CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      if (Player.PlayerCharacter.ConsortiaID == 0)
        return 0;
      int val1 = packet.ReadInt();
      if (Player.PlayerCharacter.HasBagPassword && Player.PlayerCharacter.IsLocked)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
        return 1;
      }
      if (val1 >= 1 && Player.PlayerCharacter.Money + Player.PlayerCharacter.MoneyLock >= val1)
      {
        bool val2 = false;
        string translateId = "ConsortiaRichesOfferHandler.Failed";
        using (ConsortiaBussiness consortiaBussiness = new ConsortiaBussiness())
        {
          int riches = val1 / 2;
          if (consortiaBussiness.ConsortiaRichAdd(Player.PlayerCharacter.ConsortiaID, ref riches, 5, Player.PlayerCharacter.NickName))
          {
            val2 = true;
            Player.AddRichesOffer(riches);
            Player.RemoveMoney(val1);
            translateId = "ConsortiaRichesOfferHandler.Successed";
            Player.SaveIntoDatabase();
            GameServer.Instance.LoginServer.SendConsortiaRichesOffer(Player.PlayerCharacter.ConsortiaID, Player.PlayerCharacter.ID, Player.PlayerCharacter.NickName, riches);
          }
        }
        GSPacketIn packet1 = new GSPacketIn((short) 129);
        packet1.WriteByte((byte) 6);
        packet1.WriteInt(val1);
        packet1.WriteBoolean(val2);
        packet1.WriteString(LanguageMgr.GetTranslation(translateId));
        Player.Out.SendTCP(packet1);
        return 0;
      }
      Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("ConsortiaRichesOfferHandler.NoMoney"));
      return 1;
    }
  }
}
