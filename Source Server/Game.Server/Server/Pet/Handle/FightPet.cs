// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.FightPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
  [global::Pet(17)]
  public class FightPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num = packet.ReadInt();
      bool isEquip = packet.ReadBoolean();
      UsersPetInfo petAt = Player.PetBag.GetPetAt(num);
      if (petAt == null)
        return false;
      if (petAt.Level > Player.PetBag.MaxLevelByGrade && !petAt.IsEquip)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg21"));
        return false;
      }
      if (Player.PetBag.EquipPet(num, isEquip))
        Player.EquipBag.UpdatePlayerProperties();
      else
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg22"));
      return false;
    }
  }
}
