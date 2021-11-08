// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.Handle.RevertPet
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Pet.Handle
{
  [global::Pet(18)]
  public class RevertPet : IPetCommandHadler
  {
    public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
    {
      int slot = packet.ReadInt();
      int int32 = Convert.ToInt32(PetMgr.FindConfig("RecycleCost").Value);
      UsersPetInfo petAt = Player.PetBag.GetPetAt(slot);
      if (petAt == null)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg26"));
        return false;
      }
      UsersPetInfo usersPetInfo = JsonConvert.DeserializeObject<UsersPetInfo>(petAt.BaseProp);
      if (usersPetInfo == null)
      {
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg7"));
        return false;
      }
      if (Player.RemoveMoney(int32) > 0)
      {
        SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(334100), 1, 102);
        fromTemplate.IsBinds = true;
        fromTemplate.DefendCompose = petAt.GP;
        fromTemplate.AgilityCompose = petAt.MaxGP;
        fromTemplate.Hole1 = petAt.breakGrade;
        fromTemplate.Hole2 = petAt.breakAttack;
        fromTemplate.Hole3 = petAt.breakDefence;
        fromTemplate.Hole4 = petAt.breakAgility;
        fromTemplate.Hole5 = petAt.breakLuck;
        fromTemplate.Hole6 = petAt.breakBlood;
        if (!Player.PropBag.AddTemplate(fromTemplate, 1))
        {
          Player.SendItemToMail(fromTemplate, LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"), LanguageMgr.GetTranslation("UserChangeItemPlaceHandler.full"), eMailType.ItemOverdue);
          Player.Out.SendMailResponse(Player.PlayerCharacter.ID, eMailRespose.Receiver);
        }
        petAt.breakGrade = usersPetInfo.breakGrade;
        petAt.breakAttack = usersPetInfo.breakAttack;
        petAt.breakDefence = usersPetInfo.breakDefence;
        petAt.breakAgility = usersPetInfo.breakAgility;
        petAt.breakLuck = usersPetInfo.breakLuck;
        petAt.breakBlood = usersPetInfo.breakBlood;
        petAt.Attack = usersPetInfo.Attack;
        petAt.Defence = usersPetInfo.Defence;
        petAt.Agility = usersPetInfo.Agility;
        petAt.Luck = usersPetInfo.Luck;
        petAt.Blood = usersPetInfo.Blood;
        petAt.AttackGrow = usersPetInfo.AttackGrow;
        petAt.DefenceGrow = usersPetInfo.DefenceGrow;
        petAt.AgilityGrow = usersPetInfo.AgilityGrow;
        petAt.LuckGrow = usersPetInfo.LuckGrow;
        petAt.BloodGrow = usersPetInfo.BloodGrow;
        petAt.TemplateID = usersPetInfo.TemplateID;
        petAt.Skill = usersPetInfo.Skill;
        petAt.SkillEquip = usersPetInfo.SkillEquip;
        petAt.GP = 0;
        petAt.Level = 1;
        petAt.MaxGP = 55;
        Player.SendMessage(LanguageMgr.GetTranslation("PetHandler.Msg8"));
        Player.PetBag.UpdatePet(petAt);
        Player.PetBag.SaveToDatabase(false);
      }
      return false;
    }
  }
}
