// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.PetRisingStar
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Newtonsoft.Json;
using SqlDataProvider.Data;

namespace Game.Server.Pet.Handle
{
  [global::Pet(22)]
  public class PetRisingStar : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int templateID = packet.ReadInt();
      int count = packet.ReadInt();
      int slot = packet.ReadInt();
      bool val = false;
      UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
      if (petAt == null)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetRisingStar.PetNotFound"));
      }
      else
      {
        int num1 = 11162;
        ItemInfo itemByTemplateId = Player.GetItemByTemplateID(templateID);
        bool flag = false;
        if (itemByTemplateId == null)
        {
          Player.SendMessage(LanguageMgr.GetTranslation("PetRisingStar.ItemNotFound"));
        }
        else
        {
          PetStarExpInfo petStarExp = PetMgr.FindPetStarExp(petAt.TemplateID);
          if (petStarExp == null)
            Player.SendMessage(LanguageMgr.GetTranslation("PetRisingStar.UnSupport"));
          else if (itemByTemplateId.TemplateID == num1)
          {
            if (itemByTemplateId.Count < count)
              count = itemByTemplateId.Count;
            int num2 = itemByTemplateId.Template.Property2 * count;
            int num3 = petAt.currentStarExp + num2;
            if (num3 >= petStarExp.Exp)
            {
              int num4 = petStarExp.Exp - petAt.currentStarExp;
              if (num4 < num2 && !flag)
                count = (num2 - num4) / itemByTemplateId.Template.Property2;
              petAt.currentStarExp = 0;
              PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(petStarExp.NewID);
              if (petTemplate != null)
              {
                UsersPetInfo pet = PetMgr.CreatePet(petTemplate, petAt.UserID, petAt.Place, petAt.Level);
                petAt.BaseProp = JsonConvert.SerializeObject((object) pet);
                pet.Level = petAt.Level;
                Player.PetBag.UpdateEvolutionPet(pet, petAt.Level, Player.PetBag.MaxLevelByGrade);
                petAt.TemplateID = pet.TemplateID;
                petAt.AttackGrow = pet.AttackGrow;
                petAt.DefenceGrow = pet.DefenceGrow;
                petAt.AgilityGrow = pet.AgilityGrow;
                petAt.LuckGrow = pet.LuckGrow;
                petAt.BloodGrow = pet.BloodGrow;
                petAt.DamageGrow = pet.DamageGrow;
                petAt.GuardGrow = pet.GuardGrow;
                petAt.Attack = pet.Attack;
                petAt.Defence = pet.Defence;
                petAt.Agility = pet.Agility;
                petAt.Luck = pet.Luck;
                petAt.Blood = pet.Blood;
                petAt.Damage = pet.Damage;
                petAt.Guard = pet.Guard;
                val = true;
              }
            }
            else
              petAt.currentStarExp = num3;
            Player.PetBag.UpdatePet(petAt);
            if (!flag)
              Player.RemoveCountFromStack(itemByTemplateId, count);
          }
          else
            Player.SendMessage(LanguageMgr.GetTranslation("PetRisingStar.ItemNotFound"));
        }
      }
      GSPacketIn pkg = new GSPacketIn((short) 68);
      pkg.WriteByte((byte) 22);
      pkg.WriteBoolean(val);
      Player.SendTCP(pkg);
      return false;
    }
  }
}
