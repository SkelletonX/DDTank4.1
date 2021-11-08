// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.AdoptPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Pet.Handle
{
  [global::Pet(6)]
  public class AdoptPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      if (Player.PetBag.FindFirstEmptySlot() == -1)
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg15"));
      else if (num < 0)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg16"));
        return false;
      }
      return false;
    }
  }
}
