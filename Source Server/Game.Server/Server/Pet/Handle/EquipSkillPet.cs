// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.EquipSkillPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;

namespace Game.Server.Pet.Handle
{
  [global::Pet(7)]
  public class EquipSkillPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      int killId = packet.ReadInt();
      int killindex = packet.ReadInt();
      if (!Player.PetBag.EquipSkillPet(place, killId, killindex))
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg18"));
      return false;
    }
  }
}
