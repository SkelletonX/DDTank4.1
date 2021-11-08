// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.FeedPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Pet.Handle
{
  [global::Pet(4)]
  public class FeedPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int place = packet.ReadInt();
      eBageType bagType = (eBageType) packet.ReadInt();
      int slot = packet.ReadInt();
      SqlDataProvider.Data.ItemInfo itemAt = Player.GetItemAt(bagType, place);
      if (itemAt == null)
      {
        Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("PetHandler.Msg9"));
        return false;
      }
      int int32 = Convert.ToInt32(PetMgr.FindConfig("MaxHunger").Value);
      UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
      int count = itemAt.Count;
      int property2 = itemAt.Template.Property2;
      int property1 = itemAt.Template.Property1;
      int num1 = count * property1;
      int num2 = num1 + petAt.Hunger;
      int num3 = count * property2;
      string msg = "";
      if (itemAt.TemplateID == 334100)
      {
        num3 = itemAt.DefendCompose;
        if (petAt.breakGrade < itemAt.Hole1)
          petAt.breakGrade = itemAt.Hole1;
        if (petAt.breakBlood < itemAt.Hole6)
          petAt.breakBlood = itemAt.Hole6;
        if (petAt.breakAttack < itemAt.Hole2)
          petAt.breakAttack = itemAt.Hole2;
        if (petAt.breakDefence < itemAt.Hole3)
          petAt.breakDefence = itemAt.Hole3;
        if (petAt.breakAgility < itemAt.Hole4)
          petAt.breakAgility = itemAt.Hole4;
        if (petAt.breakLuck < itemAt.Hole5)
          petAt.breakLuck = itemAt.Hole5;
      }
      int num4 = Player.PetBag.MaxLevelByGrade > petAt.MaxLevel() ? petAt.MaxLevel() : Player.PetBag.MaxLevelByGrade;
      if (petAt.Level < num4)
      {
        int GP = num3 + petAt.GP;
        int level1 = petAt.Level;
        int level2 = PetMgr.GetLevel(GP, num4);
        int gp1 = PetMgr.GetGP(level2 + 1, num4);
        int gp2 = PetMgr.GetGP(num4, num4);
        int num5 = GP;
        if (GP > gp2)
        {
          int num6 = GP - gp2;
          if (num6 >= property2 && (uint) property2 > 0U)
            count -= (int) Math.Ceiling((double) num6 / (double) property2);
        }
        petAt.GP = num5 >= gp2 ? gp2 : num5;
        petAt.Level = level2;
        petAt.MaxGP = gp1 == 0 ? gp2 : gp1;
        petAt.Hunger = num2 > int32 ? int32 : num2;
        int num7 = level2;
        if (level1 < num7)
        {
          Player.PetBag.UpdateEvolutionPet(petAt, level2, num4);
          msg = LanguageMgr.GetTranslation("FeedPet.Success", (object) petAt.Name, (object) level2);
          Player.EquipBag.UpdatePlayerProperties();
        }
        if (itemAt.TemplateID == 334100)
        {
          Player.StoreBag.RemoveItem(itemAt);
        }
        else
        {
          Player.StoreBag.RemoveCountFromStack(itemAt, count);
          Player.OnUsingItem(itemAt.TemplateID, 1);
        }
        Player.PetBag.UpdatePet(petAt);
        Player.PetBag.SaveToDatabase(false);
      }
      else if (petAt.Hunger < int32)
      {
        petAt.Hunger = num2;
        Player.StoreBag.RemoveCountFromStack(itemAt, count);
        msg = LanguageMgr.GetTranslation("PetHandler.Msg10", (object) num1);
        Player.PetBag.UpdatePet(petAt);
        Player.PetBag.SaveToDatabase(false);
      }
      else
        msg = LanguageMgr.GetTranslation("PetHandler.Msg11");
      if (!string.IsNullOrEmpty(msg))
        Player.SendMessage(msg);
      return false;
    }
  }
}
