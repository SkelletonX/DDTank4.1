// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.DelPetEquip
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Pet.Handle
{
  [global::Pet(21)]
  public class DelPetEquip : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      int eqPlace = packet.ReadInt();
      if (Player.PetBag.RemoveEqPet(num, eqPlace))
        Player.PetBag.OnChangedPetEquip(num);
      return false;
    }
  }
}
