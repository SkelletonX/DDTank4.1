using Bussiness;
using Bussiness.Managers;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.GameUtils
{
    public class PlayerEquipInventory : PlayerInventory
    {
        private static readonly int[] StyleIndex = new int[15]
        {
      1,
      2,
      3,
      4,
      5,
      6,
      11,
      13,
      14,
      15,
      16,
      17,
      18,
      19,
      20
        };

        public PlayerEquipInventory(GamePlayer player)
          : base(player, true, 80, 0, 31, true)
        {
        }

        public void AddBaseTotemProperty(int totemId, ref int totematk, ref int totemdef, ref int totemagi, ref int totemluc, ref int totemhp)
        {
            totematk += TotemMgr.GetTotemProp(totemId, "att");
            totemdef += TotemMgr.GetTotemProp(totemId, "def");
            totemagi += TotemMgr.GetTotemProp(totemId, "agi");
            totemluc += TotemMgr.GetTotemProp(totemId, "luc");
            totemhp += TotemMgr.GetTotemProp(totemId, "blo");
        }

        public void AddBaseGemstoneProperty(ItemInfo item, ref int gemattack, ref int gemdefence, ref int gemagility, ref int gemlucky, ref int gemhp)
        {
            int num = 0;
            int num1 = 0;
            foreach (UserGemStone gemStone in this.m_player.GemStone)
            {
                try
                {
                    string[] strArrays = gemStone.FigSpiritIdValue.Split(new char[] { '|' });
                    int figSpiritId = gemStone.FigSpiritId;
                    int place = item.Place;
                    int num2 = 0;
                    while (num2 < (int)strArrays.Length)
                    {
                        int num3 = Convert.ToInt32(strArrays[num2].Split(new char[] { ',' })[0]);
                        int place1 = item.Place;
                        switch (place1)
                        {
                            case 2:
                                {
                                    gemattack += FightSpiritTemplateMgr.GetProp(figSpiritId, num3, place, ref num, ref num1);
                                    goto case 4;
                                }
                            case 3:
                                {
                                    gemlucky += FightSpiritTemplateMgr.GetProp(figSpiritId, num3, place, ref num, ref num1);
                                    goto case 4;
                                }
                            case 4:
                                {
                                    num2++;
                                    continue;
                                }
                            case 5:
                                {
                                    gemagility += FightSpiritTemplateMgr.GetProp(figSpiritId, num3, place, ref num, ref num1);
                                    goto case 4;
                                }
                            default:
                                {
                                    if (place1 == 11)
                                    {
                                        gemdefence += FightSpiritTemplateMgr.GetProp(figSpiritId, num3, place, ref num, ref num1);
                                        goto case 4;
                                    }
                                    else if (place1 == 13)
                                    {
                                        gemhp += FightSpiritTemplateMgr.GetProp(figSpiritId, num3, place, ref num, ref num1);
                                        goto case 4;
                                    }
                                    else
                                    {
                                        goto case 4;
                                    }
                                }
                        }
                    }
                }
                catch
                {
                    AbstractInventory.log.ErrorFormat("Add Base Gemstone UserID: {0}, UserName: {1}, FigSpiritId {2}, FigSpiritIdValue: {3}, have error can not get Property", new object[] { this.m_player.PlayerCharacter.ID, this.m_player.PlayerCharacter.UserName, gemStone.FigSpiritId, gemStone.FigSpiritIdValue });
                }
            }
            //this.m_player.PlayerCharacter.GoldenAddAttack = num;
            //this.m_player.PlayerCharacter.GoldenReduceDamage = num1;
        }

        public override void LoadFromDatabase()
        {
            List<SqlDataProvider.Data.ItemInfo> items = new List<SqlDataProvider.Data.ItemInfo>();
            this.BeginChanges();
            try
            {
                base.LoadFromDatabase();
                using (new PlayerBussiness())
                {
                    for (int index = 0; index < 31; ++index)
                    {
                        SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[index];
                        if (this.m_items[index] != null && !this.m_items[index].IsValidItem())
                        {
                            int firstEmptySlot = this.FindFirstEmptySlot(31);
                            if (firstEmptySlot >= 0)
                                this.MoveItem(itemInfo.Place, firstEmptySlot, itemInfo.Count);
                            else
                                items.Add(itemInfo);
                        }
                        if (this.m_items[index] != null && (!this.m_items[index].Template.CanCompose || !this.m_items[index].Template.CanStrengthen) && !this.m_items[index].Template.CanCompose && (this.m_items[index].AttackCompose > 0 || this.m_items[index].AgilityCompose > 0 || (this.m_items[index].LuckCompose > 0 || this.m_items[index].DefendCompose > 0)))
                        {
                            this.m_items[index].AttackCompose = 0;
                            this.m_items[index].DefendCompose = 0;
                            this.m_items[index].AgilityCompose = 0;
                            this.m_items[index].LuckCompose = 0;
                            this.UpdateItem(this.m_items[index]);
                        }
                    }
                }
            }
            finally
            {
                this.CommitChanges();
            }
            if (items.Count <= 0)
                return;
            this.m_player.SendItemsToMail(items, "Itens expirados", "itens expirados", eMailType.BuyItem);
            this.m_player.Out.SendMailResponse(this.m_player.PlayerCharacter.ID, eMailRespose.Receiver);
        }

        public void ClearStrengthenExp()
        {
            for (int index = 0; index < this.Capalility; ++index)
            {
                if (this.m_items[index] != null && this.m_items[index].Template.CanAdvanced() && this.m_items[index].StrengthenExp > 0)
                {
                    this.m_items[index].StrengthenExp = 0;
                    this.UpdateItem(this.m_items[index]);
                }
            }
        }

        public override bool MoveItem(int fromSlot, int toSlot, int count)
        {
            if (this.m_items[fromSlot] == null)
                return false;
            this.Player.OnNewGearEvent(this.m_items[fromSlot]);
            if (this.m_items[fromSlot] != null && this.m_items[toSlot] != null && (Equip.isDress(this.m_items[fromSlot].Template) && Equip.isDress(this.m_items[toSlot].Template)) && this.m_items[toSlot].CanStackedTo(this.m_items[fromSlot]))
            {
                if ((uint)this.m_items[toSlot].ValidDate > 0U)
                {
                    if ((uint)this.m_items[fromSlot].ValidDate > 0U)
                        this.m_items[toSlot].ValidDate += this.m_items[fromSlot].ValidDate;
                    else
                        this.m_items[toSlot].ValidDate = 0;
                }
                this.RemoveItemAt(fromSlot);
                this.UpdateItem(this.m_items[toSlot]);
                return true;
            }
            if (base.IsEquipSlot(fromSlot) && !base.IsEquipSlot(toSlot) && this.m_items[toSlot] != null && this.m_items[toSlot].Template.CategoryID != this.m_items[fromSlot].Template.CategoryID)
            {
                if (!this.CanEquipSlotContains(fromSlot, this.m_items[toSlot].Template))
                    toSlot = !Equip.isDress(this.m_items[fromSlot].Template) ? this.FindFirstEmptySlot(31, 80) : this.FindFirstEmptySlot(81);
            }
            else
            {


                if (base.IsEquipSlot(toSlot))
                {
                    if (!this.CanEquipSlotContains(toSlot, this.m_items[fromSlot].Template))
                    {
                        this.UpdateItem(this.m_items[fromSlot]);
                        return false;
                    }
                    if (!this.m_player.CanEquip(this.m_items[fromSlot].Template) || !this.m_items[fromSlot].IsValidItem())
                    {
                        this.UpdateItem(this.m_items[fromSlot]);
                        return false;
                    }

                }
                if (base.IsEquipSlot(fromSlot) && this.m_items[toSlot] != null && !this.CanEquipSlotContains(fromSlot, this.m_items[toSlot].Template))
                {
                    this.UpdateItem(this.m_items[toSlot]);
                    return false;
                }
            }
            if (!Equip.isDress(this.m_items[fromSlot].Template) && toSlot > 80)
                toSlot = this.FindFirstEmptySlot(31, 80);
            else if (Equip.isDress(this.m_items[fromSlot].Template) && !base.IsEquipSlot(toSlot))
                toSlot = this.FindFirstEmptySlot(81);
            return base.MoveItem(fromSlot, toSlot, count);
        }

        public override void UpdateChangedPlaces()
        {
            int[] array = this.m_changedPlaces.ToArray();
            bool flag = false;
            foreach (int slot in array)
            {
                if (this.IsEquipSlot(slot))
                {
                    SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(slot);
                    if (itemAt != null)
                    {
                        this.m_player.OnUsingItem(this.GetItemAt(slot).TemplateID, 1);
                        itemAt.IsBinds = true;
                        if (!itemAt.IsUsed)
                        {
                            itemAt.IsUsed = true;
                            itemAt.BeginDate = DateTime.Now;
                        }
                    }
                    flag = true;
                    break;
                }
            }
            base.UpdateChangedPlaces();
            if (!flag)
                return;
            this.UpdatePlayerProperties();
        }

        public void UpdatePlayerProperties()
        {
            this.m_player.BeginChanges();
            try
            {
                int attack1 = 0;
                int defence1 = 0;
                int agility1 = 0;
                int lucky1 = 0;
                int num1 = 0;
                int level = 0;
                string style = "";
                string colors = "";
                string skin = "";
                int attack2 = 0;
                int defence2 = 0;
                int agility2 = 0;
                int lucky2 = 0;
                int hp = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                int num9 = 0;
                int num10 = 0;
                int num11 = 0;
                int num12 = 0;
                int num13 = 0;
                int num14 = 0;
                int num15 = 0;
                int totematk = 0;
                int totemdef = 0;
                int totemagi = 0;
                int totemluc = 0;
                int totemhp = 0;
                int gemattack = 0;
                int gemdefence = 0;
                int gemagility = 0;
                int gemlucky = 0;
                int gemhp = 0;
                int totalAttack = 0;
                int totalDefence = 0;
                int totalAgility = 0;
                int totalLucky = 0;
                int totalBlood = 0;
                this.m_player.UpdatePet(this.m_player.PetBag.GetPetIsEquip());
                lock (this.m_lock)
                {
                    int templateId;
                    string str1;
                    if (this.m_items[0] != null)
                    {
                        templateId = this.m_items[0].TemplateID;
                        str1 = templateId.ToString();
                    }
                    else
                        str1 = "";
                    style = str1;
                    colors = this.m_items[0] == null ? "" : this.m_items[0].Color;
                    skin = this.m_items[5] == null ? "" : this.m_items[5].Skin;
                    for (int index = 0; index < 31; ++index)
                    {
                        SqlDataProvider.Data.ItemInfo itemInfo = this.m_items[index];
                        if (itemInfo != null)
                        {
                            attack1 += itemInfo.Attack;
                            defence1 += itemInfo.Defence;
                            agility1 += itemInfo.Agility;
                            lucky1 += itemInfo.Luck;
                            level = level > itemInfo.StrengthenLevel ? level : itemInfo.StrengthenLevel;
                            this.AddBaseGemstoneProperty(itemInfo, ref gemattack, ref gemdefence, ref gemagility, ref gemlucky, ref gemhp);
                            if (itemInfo != null)
                            {
                                attack1 += itemInfo.Hole5Level * 10;
                                defence1 += itemInfo.Hole5Level * 10;
                                agility1 += itemInfo.Hole5Level * 10;
                                lucky1 += itemInfo.Hole5Level * 10;
                            }
                            if (itemInfo.isGold)
                            {
                                GoldEquipTemplateInfo goldEquipByTemplate = GoldEquipMgr.FindGoldEquipByTemplate(itemInfo.Template.TemplateID, itemInfo.Template.CategoryID);
                                if (goldEquipByTemplate != null)
                                {
                                    attack1 += goldEquipByTemplate.Attack > 0 ? goldEquipByTemplate.Attack : 0;
                                    defence1 += goldEquipByTemplate.Defence > 0 ? goldEquipByTemplate.Defence : 0;
                                    agility1 += goldEquipByTemplate.Agility > 0 ? goldEquipByTemplate.Agility : 0;
                                    lucky1 += goldEquipByTemplate.Luck > 0 ? goldEquipByTemplate.Luck : 0;
                                    num1 += goldEquipByTemplate.Boold > 0 ? goldEquipByTemplate.Boold : 0;
                                }
                            }
                            this.AddProperty(itemInfo, ref attack1, ref defence1, ref agility1, ref lucky1);
                        }
                    }
                    this.EquipBuffer();
                    for (int index = 0; index < PlayerEquipInventory.StyleIndex.Length; ++index)
                    {
                        style += ",";
                        colors += ",";
                        if (this.m_items[PlayerEquipInventory.StyleIndex[index]] != null)
                        {
                            string str2 = style;
                            templateId = this.m_items[PlayerEquipInventory.StyleIndex[index]].TemplateID;
                            string str3 = templateId.ToString();
                            style = str2 + str3;
                            colors += this.m_items[PlayerEquipInventory.StyleIndex[index]].Color;
                        }
                    }
                    attack2 += ExerciseMgr.GetExercise(this.m_player.PlayerCharacter.Texp.attTexpExp, "A");
                    defence2 += ExerciseMgr.GetExercise(this.m_player.PlayerCharacter.Texp.defTexpExp, "D");
                    agility2 += ExerciseMgr.GetExercise(this.m_player.PlayerCharacter.Texp.spdTexpExp, "AG");
                    lucky2 += ExerciseMgr.GetExercise(this.m_player.PlayerCharacter.Texp.lukTexpExp, "L");
                    hp += ExerciseMgr.GetExercise(this.m_player.PlayerCharacter.Texp.hpTexpExp, "H");
                    foreach (UsersCardInfo card in this.m_player.CardBag.GetCards(0, 4))
                    {
                        ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(card.TemplateID);
                        if (itemTemplate != null)
                        {
                            num7 += itemTemplate.Attack + card.Attack;
                            num8 += itemTemplate.Defence + card.Defence;
                            num9 += itemTemplate.Agility + card.Agility;
                            num10 += itemTemplate.Luck + card.Luck;
                        }
                    }
                    this.AddBaseTotemProperty(this.m_player.PlayerCharacter.totemId, ref totematk, ref totemdef, ref totemagi, ref totemluc, ref totemhp);
                    if (this.m_player.Pet != null)
                    {
                        num11 += this.m_player.Pet.TotalAttack;
                        num12 += this.m_player.Pet.TotalDefence;
                        num13 += this.m_player.Pet.TotalAgility;
                        num14 += this.m_player.Pet.TotalLuck;
                        num15 += this.m_player.Pet.TotalBlood;
                        PetFightPropertyInfo fightProperty = PetMgr.FindFightProperty(this.m_player.PlayerCharacter.evolutionGrade);
                        if (fightProperty != null)
                        {
                            num11 += fightProperty.Attack;
                            num12 += fightProperty.Defence;
                            num13 += fightProperty.Agility;
                            num14 += fightProperty.Lucky;
                            num15 += fightProperty.Blood;
                        }
                        EatPetsInfo eatPets = this.m_player.PetBag.EatPets;
                        if (eatPets != null)
                        {
                            PetMoePropertyInfo petMoePropertyInfo = PetMoePropertyMgr.FindPetMoeProperty(eatPets.weaponLevel);
                            if (petMoePropertyInfo != null && this.m_player.PetBag.GetEqPet(this.m_player.Pet.PetEquips, 0) != null)
                            {
                                totalAttack += petMoePropertyInfo.Attack;
                                totalLucky += petMoePropertyInfo.Lucky;
                                //Console.WriteLine("num 11 = {0} + Attack = {1} / Weapon Level = {2}", num11, petMoePropertyInfo.Attack, eatPets.weaponLevel);
                                //Console.WriteLine("num 14 = {0} + Lucky = {1} / Weapon Level = {2}", num14, petMoePropertyInfo.Lucky, eatPets.weaponLevel);
                            }
                            petMoePropertyInfo = PetMoePropertyMgr.FindPetMoeProperty(eatPets.clothesLevel);
                            if (petMoePropertyInfo != null && this.m_player.PetBag.GetEqPet(this.m_player.Pet.PetEquips, 2) != null)
                            {
                                totalAgility += petMoePropertyInfo.Agility;
                                totalBlood += petMoePropertyInfo.Blood;
                                //Console.WriteLine("num 13 = {0} + Agility = {1} / Cloth Level = {2}", num13, petMoePropertyInfo.Agility, eatPets.clothesLevel);
                                //Console.WriteLine("num 15 = {0} + Blood = {1} / Cloth Level = {2}", num15, petMoePropertyInfo.Blood, eatPets.clothesLevel);
                            }
                            petMoePropertyInfo = PetMoePropertyMgr.FindPetMoeProperty(eatPets.hatLevel);
                            if (petMoePropertyInfo != null && this.m_player.PetBag.GetEqPet(this.m_player.Pet.PetEquips, 1) != null)
                            {
                                totalDefence += petMoePropertyInfo.Defence;
                                //Console.WriteLine("num 12 = {0} + Defence = {1} / Head Level = {2}", num12, petMoePropertyInfo.Defence, eatPets.hatLevel);
                            }
                        }
                    }
                }
                this.m_player.UpdateBaseProperties(attack1 + totematk + gemattack + totalAttack + (attack2 + num2 + num7 + num11), defence1 + totemdef + gemdefence + totalDefence  + (defence2 + num3 + num8 + num12), agility1 + totemagi + gemagility + totalAgility + (agility2 + num4 + num9 + num13), lucky1 + totemluc + gemlucky + totalLucky + (lucky2 + num5 + num10 + num14), num1 + totemhp + gemhp + totalBlood + (hp + num6 + num15));
                this.m_player.UpdateStyle(style, colors, skin);
                this.GetUserNimbus();
                this.m_player.ApertureEquip(level);
                this.m_player.UpdateWeapon(this.m_items[6]);
                this.m_player.UpdateSecondWeapon(this.m_items[15]);
                this.m_player.UpdateHealstone(this.m_items[18]);
                this.m_player.PlayerProp.CreateProp(true, "Texp", attack2, defence2, agility2, lucky2, hp);
                this.m_player.PlayerProp.CreateProp(true, "Gem", gemattack, gemdefence, gemagility, gemlucky, gemhp);
                this.m_player.PlayerProp.CreateProp(true, "Pet", num11, num12, num13, num14, num15);
                this.m_player.PlayerProp.CreateProp(true, "EatPets", totalAttack, totalDefence, totalAgility, totalLucky, totalBlood);
                this.m_player.UpdateFightPower();
                this.m_player.PlayerProp.ViewCurrent();
                this.m_player.OnPropertiesChange();
            }
            finally
            {
                this.m_player.CommitChanges();
            }
        }

        public int FindItemEpuipSlot(ItemTemplateInfo item)
        {
            switch (item.CategoryID)
            {
                case 8:
                    return this.m_items[7] != null ? 8 : 7;
                case 9:
                    return this.m_items[9] != null ? 10 : 9;
                case 13:
                    return 11;
                case 14:
                    return 12;
                case 15:
                    return 13;
                case 16:
                    return 14;
                case 17:
                    return 15;
                default:
                    return item.CategoryID - 1;
            }
        }

        public bool CanEquipSlotContains(int slot, ItemTemplateInfo temp)
        {
            if (temp.CategoryID == 8)
            {
                if (slot != 7)
                    return slot == 8;
                return true;
            }
            if (temp.CategoryID == 9)
            {
                if (temp.IsRing())
                {
                    if (slot != 9 && slot != 10)
                        return slot == 16;
                    return true;
                }
                if (slot != 9)
                    return slot == 10;
                return true;
            }
            if (temp.CategoryID == 13)
                return slot == 11;
            if (temp.CategoryID == 14)
                return slot == 12;
            if (temp.CategoryID == 15)
                return slot == 13;
            if (temp.CategoryID == 16)
                return slot == 14;
            if (temp.CategoryID == 17)
                return slot == 15;
            return temp.CategoryID - 1 == slot;
        }

        public new bool IsEquipSlot(int slot)
        {
            if (slot >= 0)
                return slot < 31;
            return false;
        }
        //Clircle Ligth e SimpleLigth
        public void GetUserNimbus()
        {
            int num1 = 0;
            int num2 = 0;
            for (int slot = 0; slot < 31; ++slot)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(slot);
                if (itemAt != null)
                {
                    int strengthenLevel = itemAt.StrengthenLevel;
                    if (strengthenLevel >= 5 && strengthenLevel <= 8)
                    {
                        if (itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5)
                            num1 = num1 > 1 ? num1 : 1;
                        if (itemAt.Template.CategoryID == 7)
                            num2 = num2 > 1 ? num2 : 1;
                    }
                    if (strengthenLevel >= 9 && strengthenLevel <= 11)
                    {
                        if (itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5)
                            num1 = num1 > 1 ? num1 : 2;
                        if (itemAt.Template.CategoryID == 7)
                            num2 = num2 > 1 ? num2 : 2;
                    }
                    if (strengthenLevel == 12 )
                    {
                        if (itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5)
                            num1 = num1 > 1 ? num1 : 3;
                        if (itemAt.Template.CategoryID == 7)
                            num2 = num2 > 1 ? num2 : 3;
                    }
                    if (strengthenLevel >= 13 && strengthenLevel <= 15)
                    {
                        if (itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5)
                            num1 = num1 > 1 ? num1 : 4;
                        if (itemAt.Template.CategoryID == 7)
                            num2 = num2 > 1 ? num2 : 4;
                    }
                    if (itemAt.isGold)
                    {
                        if (itemAt.Template.CategoryID == 1 || itemAt.Template.CategoryID == 5)
                            num1 = 5;
                        if (itemAt.Template.CategoryID == 7)
                            num2 = 5;
                    }
                }
            }
            this.m_player.PlayerCharacter.Nimbus = num1 * 100 + num2;
            this.m_player.Out.SendUpdatePublicPlayer(this.m_player.PlayerCharacter, this.m_player.MatchInfo, this.m_player.Extra.Info);
        }

        public void EquipBuffer()
        {
            this.m_player.EquipEffect.Clear();
            for (int slot = 0; slot < 31; ++slot)
            {
                SqlDataProvider.Data.ItemInfo itemAt = this.GetItemAt(slot);
                if (itemAt != null)
                {
                    if (itemAt.Hole1 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole1);
                    if (itemAt.Hole2 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole2);
                    if (itemAt.Hole3 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole3);
                    if (itemAt.Hole4 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole4);
                    if (itemAt.Hole5 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole5);
                    if (itemAt.Hole6 > 0)
                        this.m_player.EquipEffect.Add(itemAt.Hole6);
                }
            }
        }

        public void AddProperty(
          SqlDataProvider.Data.ItemInfo item,
          ref int attack,
          ref int defence,
          ref int agility,
          ref int lucky)
        {
            if (item == null)
                return;
            if (item.Hole1 > 0)
                this.AddBaseProperty(item.Hole1, ref attack, ref defence, ref agility, ref lucky);
            if (item.Hole2 > 0)
                this.AddBaseProperty(item.Hole2, ref attack, ref defence, ref agility, ref lucky);
            if (item.Hole3 > 0)
                this.AddBaseProperty(item.Hole3, ref attack, ref defence, ref agility, ref lucky);
            if (item.Hole4 > 0)
                this.AddBaseProperty(item.Hole4, ref attack, ref defence, ref agility, ref lucky);
            if (item.Hole5 > 0)
                this.AddBaseProperty(item.Hole5, ref attack, ref defence, ref agility, ref lucky);
            if (item.Hole6 <= 0)
                return;
            this.AddBaseProperty(item.Hole6, ref attack, ref defence, ref agility, ref lucky);
        }

        public void AddBaseProperty(
          int templateid,
          ref int attack,
          ref int defence,
          ref int agility,
          ref int lucky)
        {
            ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(templateid);
            if (itemTemplate == null || itemTemplate.CategoryID != 11 || (itemTemplate.Property1 != 31 || itemTemplate.Property2 != 3))
                return;
            attack += itemTemplate.Property3;
            defence += itemTemplate.Property4;
            agility += itemTemplate.Property5;
            lucky += itemTemplate.Property6;
        }
    }
}
