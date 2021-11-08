// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PetInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness;
using Bussiness.Managers;
using Game.Server.GameObjects;
using log4net;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.GameUtils
{
    public class PetInventory : PetAbstractInventory
    {
        private static readonly ILog ilog_1 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool bool_0;
        private List<UsersPetInfo> list_0;
        protected GamePlayer m_player;
        private PlayerInventory playerInventory_0;
        private EatPetsInfo eatPetsInfo_0;

        public GamePlayer Player
        {
            get
            {
                return this.m_player;
            }
        }

        public EatPetsInfo EatPets
        {
            get
            {
                return this.eatPetsInfo_0;
            }
        }

        public int MaxLevel
        {
            get
            {
                return Convert.ToInt32(PetMgr.FindConfig(nameof(MaxLevel)).Value);
            }
        }

        public int MaxLevelByGrade
        {
            get
            {
                if (this.m_player == null || this.m_player.Level > this.MaxLevel)
                    return this.MaxLevel;
                return this.m_player.Level;
            }
        }

        public PetInventory(
          GamePlayer player,
          bool saveTodb,
          int capibility,
          int aCapability,
          int beginSlot)
          : base(capibility, aCapability, beginSlot)
        {
            this.m_player = player;
            this.bool_0 = saveTodb;
            this.list_0 = new List<UsersPetInfo>();
            this.playerInventory_0 = new PlayerInventory(player, true, 49, 5012, 0, false);
        }

        public PlayerInventory Equips
        {
            get
            {
                return this.playerInventory_0;
            }
        }

        public virtual void LoadFromDatabase()
        {
            if (!this.bool_0)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                int id = this.m_player.PlayerCharacter.ID;
                UsersPetInfo[] userPetSingles = playerBussiness.GetUserPetSingles(id);
                EatPetsInfo allEatPetsByID = playerBussiness.GetAllEatPetsByID(id);
                this.Equips.LoadFromDatabase();
                this.BeginChanges();
                try
                {
                    foreach (UsersPetInfo pet1 in userPetSingles)
                    {
                        if (string.IsNullOrEmpty(pet1.BaseProp))
                        {
                            string str = pet1.TemplateID.ToString();
                            PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(!(str.Substring(str.Length - 1, 1) == "1") ? (!(str.Substring(str.Length - 1, 1) == "2") ? pet1.TemplateID - 2 : pet1.TemplateID - 1) : (pet1.Level >= 30 ? (pet1.Level >= 50 ? pet1.TemplateID - 2 : pet1.TemplateID - 1) : pet1.TemplateID));
                            if (petTemplate != null)
                            {
                                UsersPetInfo pet2 = PetMgr.CreatePet(petTemplate, pet1.UserID, pet1.Place, pet1.Level);
                                pet1.BaseProp = JsonConvert.SerializeObject((object)pet2);
                                this.UpdateEvolutionPet(pet2, pet1.Level, this.MaxLevelByGrade);
                                pet1.AttackGrow = pet2.AttackGrow;
                                pet1.DefenceGrow = pet2.DefenceGrow;
                                pet1.AgilityGrow = pet2.AgilityGrow;
                                pet1.LuckGrow = pet2.LuckGrow;
                                pet1.BloodGrow = pet2.BloodGrow;
                                pet1.DamageGrow = pet2.DamageGrow;
                                pet1.GuardGrow = pet2.GuardGrow;
                            }
                        }
                        this.AddPetTo(pet1, pet1.Place);
                    }
                    if (allEatPetsByID != null)
                    {
                        lock (this.m_lock)
                        {
                            this.eatPetsInfo_0 = allEatPetsByID;
                        }
                    }
                    else
                    {
                        lock (this.m_lock)
                        {
                            this.eatPetsInfo_0 = new EatPetsInfo()
                            {
                                UserID = id
                            };
                        }
                    }
                }
                finally
                {
                    try
                    {
                        if (this.FindFirstEmptySlot(this.BeginSlot) != -1)
                        {
                            if ((uint)userPetSingles.Length > 0U)
                            {
                                for (int index = 1; this.FindFirstEmptySlot(this.BeginSlot) < userPetSingles[userPetSingles.Length - index].Place; ++index)
                                    this.Player.PetBag.MovePet(userPetSingles[userPetSingles.Length - index].Place, this.FindFirstEmptySlot(this.BeginSlot));
                            }
                        }
                    }
                    catch
                    {
                    }
                    this.CommitChanges();
                    for (int index = 0; index < this.Equips.Capalility; ++index)
                    {
                        SqlDataProvider.Data.ItemInfo itemAt = this.Equips.GetItemAt(index);
                        if (itemAt != null)
                        {
                            this.m_player.AddTemplate(SqlDataProvider.Data.ItemInfo.CloneFromTemplate(itemAt.Template, itemAt));
                            this.Equips.RemoveItemAt(index);
                        }
                    }
                }
            }
        }

        public virtual void SaveToDatabase(bool saveAdopt)
        {
            if (this.bool_0)
            {
                using (PlayerBussiness playerBussiness = new PlayerBussiness())
                {
                    lock (this.m_lock)
                    {
                        for (int index = 0; index < this.m_pets.Length; ++index)
                        {
                            UsersPetInfo pet = this.m_pets[index];
                            if (pet != null && pet.IsDirty)
                            {
                                pet.eQPets = this.SerializePetEquip(pet.PetEquips);
                                if (pet.ID > 0)
                                    playerBussiness.UpdateUserPet(pet);
                                else
                                    playerBussiness.AddUserPet(pet);
                            }
                        }
                    }
                    lock (this.m_lock)
                    {
                        if (this.eatPetsInfo_0 != null && this.eatPetsInfo_0.IsDirty)
                        {
                            if (this.eatPetsInfo_0.ID != 0)
                            {
                                playerBussiness.UpdateEatPets(this.eatPetsInfo_0);
                            }
                            else
                            {
                                playerBussiness.AddEatPets(this.eatPetsInfo_0);

                            }
                        }
                    }
                    lock (this.m_lock)
                    {
                        foreach (UsersPetInfo usersPetInfo in this.list_0)
                            playerBussiness.UpdateUserPet(usersPetInfo);
                        this.list_0.Clear();
                    }
                }
            }
            this.Equips.SaveToDatabase();
        }

        public override bool AddPetTo(UsersPetInfo pet, int place)
        {
            if (!base.AddPetTo(pet, place))
                return false;
            pet.UserID = this.m_player.PlayerCharacter.ID;
            pet.PetEquips = this.DeserializePetEquip(pet.eQPets);
            return true;
        }

        public virtual void ReduceHunger()
        {
            UsersPetInfo petIsEquip = this.GetPetIsEquip();
            if (petIsEquip == null)
                return;
            int num1 = 40;
            int num2 = 100;
            if (petIsEquip.Hunger < 100)
                return;
            if (petIsEquip.Level >= 60)
                petIsEquip.Hunger -= num2;
            else
                petIsEquip.Hunger -= num1;
            this.UpdatePet(petIsEquip, petIsEquip.Place);
        }

        public bool CanAdd(SqlDataProvider.Data.ItemInfo item, List<PetEquipInfo> infos)
        {
            if (infos.Count == 3 || item.Template == null)
                return false;
            foreach (PetEquipInfo info in infos)
            {
                if (item.eqType() == info.eqType)
                    return false;
            }
            return true;
        }

        public bool AddEqPet(int place, SqlDataProvider.Data.ItemInfo item)
        {
            UsersPetInfo petAt = this.GetPetAt(place);
            if (petAt == null || !this.CanAdd(item, this.m_pets[place].PetEquips))
                return false;
            petAt.PetEquips.Add(new PetEquipInfo(item.Template)
            {
                eqTemplateID = item.TemplateID,
                eqType = item.eqType(),
                ValidDate = item.ValidDate,
                startTime = item.BeginDate
            });
            return true;
        }

        public PetEquipInfo GetEqPet(List<PetEquipInfo> infos, int place)
        {
            foreach (PetEquipInfo info in infos)
            {
                if (info.eqType == place)
                    return info;
            }
            return (PetEquipInfo)null;
        }

        public bool RemoveEqPet(int petPlace, int eqPlace)
        {
            UsersPetInfo petAt = this.GetPetAt(petPlace);
            if (petAt == null)
                return false;
            PetEquipInfo eqPet = this.GetEqPet(petAt.PetEquips, eqPlace);
            if (eqPet == null)
                return false;
            this.ChangeEqPetToItem(eqPet);
            return petAt.PetEquips.Remove(eqPet);
        }

        public void ChangeEqPetToItem(PetEquipInfo info)
        {
            if (info.Template == null)
                return;
            SqlDataProvider.Data.ItemInfo fromTemplate = SqlDataProvider.Data.ItemInfo.CreateFromTemplate(info.Template, 1, 105);
            fromTemplate.IsBinds = true;
            fromTemplate.IsUsed = true;
            fromTemplate.ValidDate = info.ValidDate;
            fromTemplate.BeginDate = info.startTime;
            this.m_player.AddTemplate(fromTemplate);
        }

        public void RemoveAllEqPet(List<PetEquipInfo> infos)
        {
            foreach (PetEquipInfo info in infos)
                this.ChangeEqPetToItem(info);
        }

        public List<PetEquipInfo> DeserializePetEquip(string eqString)
        {
            if (string.IsNullOrEmpty(eqString))
                return new List<PetEquipInfo>();
            List<PetEquipInfo> petEquipInfoList1 = JsonConvert.DeserializeObject<List<PetEquipInfo>>(eqString);
            List<PetEquipInfo> petEquipInfoList2 = new List<PetEquipInfo>();
            foreach (PetEquipInfo petEquipInfo in petEquipInfoList1)
            {
                if (petEquipInfo.Template == null)
                {
                    ItemTemplateInfo itemTemplate = ItemMgr.FindItemTemplate(petEquipInfo.eqTemplateID);
                    if (itemTemplate != null)
                        petEquipInfoList2.Add(new PetEquipInfo(itemTemplate)
                        {
                            eqTemplateID = petEquipInfo.eqTemplateID,
                            eqType = petEquipInfo.eqType,
                            ValidDate = petEquipInfo.ValidDate,
                            startTime = petEquipInfo.startTime
                        });
                }
                else
                    petEquipInfoList2.Add(petEquipInfo);
            }
            return petEquipInfoList2;
        }

        public string SerializePetEquip(List<PetEquipInfo> eqs)
        {
            return JsonConvert.SerializeObject((object)eqs);
        }

        public virtual bool OnChangedPetEquip(int place)
        {
            lock (this.m_lock)
            {
                if (this.m_pets[place] != null)
                {
                    if (this.m_pets[place].IsEquip)
                        this.m_player.EquipBag.UpdatePlayerProperties();
                }
            }
            this.OnPlaceChanged(place);
            return true;
        }

        public override bool RemovePet(UsersPetInfo pet)
        {
            if (!base.RemovePet(pet))
                return false;
            if (pet.PetEquips != null && pet.PetEquips.Count > 0)
                this.RemoveAllEqPet(pet.PetEquips);
            lock (this.list_0)
            {
                pet.IsExit = false;
                this.list_0.Add(pet);
            }
            return true;
        }

        public override void UpdateChangedPlaces()
        {
            this.m_player.Out.SendUpdateUserPet(this, this.m_changedPlaces.ToArray());
            base.UpdateChangedPlaces();
        }

        public void UpdateEatPets()
        {
            this.m_player.Out.SendEatPetsInfo(this.EatPets);
        }
    }
}
