// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.PetEvolution
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
  [global::Pet(23)]
  public class PetEvolution : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int num1 = packet.ReadInt();
      int count = packet.ReadInt();
      if (num1 != 11163)
        return false;
      int num2 = 0;
      ItemInfo itemByTemplateId = Player.GetItemByTemplateID(num1);
      if (itemByTemplateId != null && count > 0 && num1 == 11163)
      {
        if (itemByTemplateId.Count < count)
          count = itemByTemplateId.Count;
        num2 = itemByTemplateId.Template.Property2 * count;
      }
      if (num2 > 0)
      {
        bool val = false;
        int evolutionGrade = Player.PlayerCharacter.evolutionGrade;
        int evolutionExp = Player.PlayerCharacter.evolutionExp;
        int num3 = Player.PlayerCharacter.evolutionExp + num2;
        int evolutionMax = PetMgr.GetEvolutionMax();
        for (int index = evolutionGrade; index <= evolutionMax; ++index)
        {
          PetFightPropertyInfo fightProperty = PetMgr.FindFightProperty(index + 1);
          if (fightProperty != null && fightProperty.Exp <= num3)
          {
            Player.PlayerCharacter.evolutionGrade = index + 1;
            val = true;
          }
        }
        Player.PlayerCharacter.evolutionExp = num3;
        Player.PropBag.RemoveTemplate(num1, count);
        Player.EquipBag.UpdatePlayerProperties();
        Player.SendUpdatePublicPlayer();
        GSPacketIn pkg = new GSPacketIn((short) 68);
        pkg.WriteByte((byte) 23);
        pkg.WriteBoolean(val);
        Player.SendTCP(pkg);
      }
      return false;
    }
  }
}
