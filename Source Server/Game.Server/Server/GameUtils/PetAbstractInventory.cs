// Decompiled with JetBrains decompiler
// Type: Game.Server.GameUtils.PetAbstractInventory
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Bussiness.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
  public abstract class PetAbstractInventory
  {
    private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected object m_lock;
    private int int_0;
    private int int_1;
    private int int_2;
    protected UsersPetInfo[] m_pets;
    protected List<int> m_changedPlaces;
    private int int_3;

    public int BeginSlot
    {
      get
      {
        return this.int_2;
      }
    }

    public int Capalility
    {
      get
      {
        return this.int_0;
      }
      set
      {
        this.int_0 = value < 0 ? 0 : (value > this.m_pets.Length ? this.m_pets.Length : value);
      }
    }

    public PetAbstractInventory(int capability, int aCapability, int beginSlot)
    {
      this.m_lock = new object();
      this.m_changedPlaces = new List<int>();
      this.int_0 = capability;
      this.int_1 = aCapability;
      this.int_2 = beginSlot;
      this.m_pets = new UsersPetInfo[capability];
    }

    public virtual UsersPetInfo GetPetIsEquip()
    {
      for (int index = 0; index < this.int_0; ++index)
      {
        if (this.m_pets[index] != null && this.m_pets[index].IsEquip)
          return this.m_pets[index];
      }
      return (UsersPetInfo) null;
    }

    public virtual bool AddPetTo(UsersPetInfo pet, int place)
    {
      if (pet == null || place >= this.int_0 || place < 0)
        return false;
      lock (this.m_lock)
      {
        if (this.m_pets[place] == null)
        {
          this.m_pets[place] = pet;
          pet.Place = place;
        }
        else
          place = -1;
      }
      if (place != -1)
        this.OnPlaceChanged(place);
      return place != -1;
    }

    public virtual bool RemovePet(UsersPetInfo pet)
    {
      if (pet == null)
        return false;
      int place = -1;
      lock (this.m_lock)
      {
        for (int index = 0; index < this.int_0; ++index)
        {
          if (this.m_pets[index] == pet)
          {
            place = index;
            this.m_pets[index] = (UsersPetInfo) null;
            break;
          }
        }
      }
      if (place != -1)
      {
        this.OnPlaceChanged(place);
        pet.Place = -1;
      }
      return place != -1;
    }

    public virtual bool MovePet(int fromSlot, int toSlot)
    {
      if (fromSlot < 0 || toSlot < 0 || (fromSlot >= this.int_0 || toSlot >= this.int_0) || fromSlot == toSlot)
        return false;
      bool flag = false;
      lock (this.m_lock)
        flag = this.ExchangePet(fromSlot, toSlot);
      if (flag)
      {
        this.BeginChanges();
        try
        {
          this.OnPlaceChanged(fromSlot);
          this.OnPlaceChanged(toSlot);
        }
        finally
        {
          this.CommitChanges();
        }
      }
      return flag;
    }

    protected virtual bool ExchangePet(int fromSlot, int toSlot)
    {
      UsersPetInfo pet1 = this.m_pets[toSlot];
      UsersPetInfo pet2 = this.m_pets[fromSlot];
      this.m_pets[fromSlot] = pet1;
      this.m_pets[toSlot] = pet2;
      if (pet1 != null)
        pet1.Place = fromSlot;
      if (pet2 != null)
        pet2.Place = toSlot;
      return true;
    }

    public virtual UsersPetInfo GetPetAt(int slot)
    {
      if (slot >= 0 && slot < this.int_0)
        return this.m_pets[slot];
      return (UsersPetInfo) null;
    }

    public int FindFirstEmptySlot()
    {
      return this.FindFirstEmptySlot(this.int_2);
    }

    public int FindFirstEmptySlot(int minSlot)
    {
      if (minSlot >= this.int_0)
        return -1;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_pets[index] == null)
            return index;
        }
        return -1;
      }
    }

    public int FindLastEmptySlot()
    {
      lock (this.m_lock)
      {
        for (int index = this.int_0 - 1; index >= 0; --index)
        {
          if (this.m_pets[index] == null)
            return index;
        }
        return -1;
      }
    }

    public virtual void Clear()
    {
      lock (this.m_lock)
      {
        for (int index = 0; index < this.int_0; ++index)
          this.m_pets[index] = (UsersPetInfo) null;
      }
    }

    public virtual UsersPetInfo GetPetByTemplateID(int minSlot, int templateId)
    {
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_pets[index] != null && this.m_pets[index].TemplateID == templateId)
            return this.m_pets[index];
        }
        return (UsersPetInfo) null;
      }
    }

    public virtual UsersPetInfo[] GetPets()
    {
      List<UsersPetInfo> usersPetInfoList = new List<UsersPetInfo>();
      for (int index = 0; index < this.int_0; ++index)
      {
        if (this.m_pets[index] != null && this.m_pets[index].IsExit)
          usersPetInfoList.Add(this.m_pets[index]);
      }
      return usersPetInfoList.ToArray();
    }

    public int GetEmptyCount()
    {
      return this.GetEmptyCount(this.int_2);
    }

    public virtual int GetEmptyCount(int minSlot)
    {
      if (minSlot < 0 || minSlot > this.int_0 - 1)
        return 0;
      int num = 0;
      lock (this.m_lock)
      {
        for (int index = minSlot; index < this.int_0; ++index)
        {
          if (this.m_pets[index] == null)
            ++num;
        }
      }
      return num;
    }

    protected void OnPlaceChanged(int place)
    {
      if (!this.m_changedPlaces.Contains(place))
        this.m_changedPlaces.Add(place);
      if (this.int_3 > 0 || this.m_changedPlaces.Count <= 0)
        return;
      this.UpdateChangedPlaces();
    }

    public void BeginChanges()
    {
      Interlocked.Increment(ref this.int_3);
    }

    public void CommitChanges()
    {
      int num = Interlocked.Decrement(ref this.int_3);
      if (num < 0)
      {
        if (PetAbstractInventory.ilog_0.IsErrorEnabled)
          PetAbstractInventory.ilog_0.Error((object) ("Inventory changes counter is bellow zero (forgot to use BeginChanges?)!\n\n" + Environment.StackTrace));
        Thread.VolatileWrite(ref this.int_3, 0);
      }
      if (num > 0 || this.m_changedPlaces.Count <= 0)
        return;
      this.UpdateChangedPlaces();
    }

    public virtual void UpdateChangedPlaces()
    {
      this.m_changedPlaces.Clear();
    }

    public virtual bool RenamePet(int place, string name)
    {
      lock (this.m_lock)
      {
        if (this.m_pets[place] != null)
          this.m_pets[place].Name = name;
      }
      this.OnPlaceChanged(place);
      return true;
    }

    public bool IsEquipSkill(int slot, string kill)
    {
      try
      {
        List<string> skillEquip = this.m_pets[slot].GetSkillEquip();
        for (int index = 0; index < skillEquip.Count; ++index)
        {
          if (skillEquip[index].Split(',')[0] == kill)
            return false;
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    public virtual bool EquipSkillPet(int place, int killId, int killindex)
    {
      string skill = killId.ToString() + "," + (object) killindex;
      UsersPetInfo pet = this.m_pets[place];
      lock (this.m_lock)
      {
        if (killId == 0)
        {
          this.m_pets[place].SkillEquip = this.SetSkillEquip(pet, killindex, skill);
          this.OnPlaceChanged(place);
          return true;
        }
        if (this.IsEquipSkill(place, killId.ToString()))
        {
          this.m_pets[place].SkillEquip = this.SetSkillEquip(pet, killindex, skill);
          this.OnPlaceChanged(place);
          return true;
        }
        Console.WriteLine("place:{0}, killId:{1}, killindex:{2}", (object) place, (object) killId, (object) killindex);
      }
      return false;
    }

    public string SetSkillEquip(UsersPetInfo pet, int place, string skill)
    {
      List<string> skillEquip = pet.GetSkillEquip();
      string str = "";
      try
      {
        skillEquip[place] = skill;
        str = skillEquip[0];
        for (int index = 1; index < skillEquip.Count; ++index)
          str = str + "|" + skillEquip[index];
      }
      catch
      {
        return str;
      }
      return str;
    }

    public virtual bool UpdatePet(UsersPetInfo pet, int place)
    {
      if (pet == null)
        return false;
      int place1 = -1;
      lock (this.m_lock)
      {
        for (int index = 0; index < this.m_pets.Length; ++index)
        {
          if (this.m_pets[index] != null)
          {
            place1 = this.m_pets[index].Place;
            if (place1 == place)
              this.m_pets[index] = pet;
            this.OnPlaceChanged(place1);
          }
        }
      }
      return place1 > -1;
    }

    public virtual bool EquipPet(int place, bool isEquip)
    {
      int place1 = -1;
      lock (this.m_lock)
      {
        for (int index = 0; index < this.m_pets.Length; ++index)
        {
          if (this.m_pets[index] != null)
          {
            place1 = this.m_pets[index].Place;
            if (place1 == place)
            {
              if (this.m_pets[index].Hunger == 0)
                return false;
              this.m_pets[index].IsEquip = isEquip;
            }
            else
              this.m_pets[index].IsEquip = false;
            this.OnPlaceChanged(place1);
          }
        }
      }
      return place1 > -1;
    }

    public double GetRandomDouble(Random random, double min, double max)
    {
      return min + random.NextDouble() * (max - min);
    }

    public virtual void UpdatePet(UsersPetInfo pet)
    {
      if (pet.Place > this.Capalility || pet.Place < 0)
        return;
      lock (this.m_lock)
        this.m_pets[pet.Place] = pet;
      this.OnPlaceChanged(pet.Place);
    }

    public virtual bool UpdateEvolutionPet(UsersPetInfo pet, int level, int maxLevel)
    {
      int TemplateID = PetMgr.UpdateEvolution(pet.TemplateID, level);
      if (TemplateID > pet.TemplateID)
      {
        pet.TemplateID = TemplateID;
        PetTemplateInfo petTemplate = PetMgr.FindPetTemplate(TemplateID);
        if (petTemplate != null)
        {
          double[] propArr = (double[]) null;
          double[] growArr = (double[]) null;
          PetMgr.GetEvolutionPropArr(pet, petTemplate, ref propArr, ref growArr);
          if (propArr != null && growArr != null)
          {
            Random random = new Random();
            double min = (double) petTemplate.RareLevel * 0.1;
            if (min < growArr[0])
              pet.BloodGrow += (int) (this.GetRandomDouble(random, min, growArr[0]) * 10.0);
            if (min < growArr[1])
              pet.AttackGrow += (int) (this.GetRandomDouble(random, min, growArr[1]) * 10.0);
            if (min < growArr[2])
              pet.DefenceGrow += (int) (this.GetRandomDouble(random, min, growArr[2]) * 10.0);
            if (min < growArr[3])
              pet.AgilityGrow += (int) (this.GetRandomDouble(random, min, growArr[3]) * 10.0);
            if (min < growArr[4])
              pet.LuckGrow += (int) (this.GetRandomDouble(random, min, growArr[4]) * 10.0);
          }
        }
      }
      string skill = pet.Skill;
      string str = PetMgr.UpdateSkillPet(level, pet.TemplateID, maxLevel);
      pet.Skill = str == "" ? skill : str;
      pet.SkillEquip = PetMgr.ActiveEquipSkill(level);
      pet.BuildProp(pet);
      return true;
    }
  }
}
