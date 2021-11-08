// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.RenamePet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using System;

namespace Game.Server.Pet.Handle
{
  [global::Pet(9)]
  public class RenamePet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      string name = packet.ReadString();
      int int32 = Convert.ToInt32(PetMgr.FindConfig("ChangeNameCost").Value);
      if (Player.RemoveMoney(int32) > 0 && Player.PetBag.RenamePet(place, name))
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg20"));
      return false;
    }
  }
}
