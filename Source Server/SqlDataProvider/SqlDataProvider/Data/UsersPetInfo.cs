// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UsersPetInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections.Generic;

namespace SqlDataProvider.Data
{
  public class UsersPetInfo : DataObject
  {
    private string string_0;
    private string string_1;
    private int int_0;
    private int int_1;
    private int int_2;
    private string string_2;
    private int int_3;
    private int int_4;
    private int int_5;
    private int int_6;
    private int int_7;
    private int int_8;
    private int int_9;
    private int int_10;
    private int int_11;
    private int int_12;
    private int int_13;
    private int int_14;
    private int int_15;
    private int int_16;
    private int int_17;
    private int int_18;
    private int int_19;
    private int int_20;
    private int int_21;
    private int int_22;
    private bool bool_0;
    private int int_23;
    private int int_24;
    private bool bool_1;
    private int int_25;
    private int int_26;
    private int int_27;
    private int int_28;
    private int int_29;
    private int int_30;
    private List<PetEquipInfo> list_0;
    private string string_3;
    private string string_4;
    private int int_31;

    public List<string> GetSkill()
    {
      List<string> stringList = new List<string>();
      string string1 = this.string_1;
      char[] chArray = new char[1]{ '|' };
      foreach (string str in string1.Split(chArray))
        stringList.Add(str);
      return stringList;
    }

    public List<string> GetSkillEquip()
    {
      List<string> stringList1 = new List<string>();
      string string1 = this.string_1;
      char[] chArray = new char[1]{ '|' };
      foreach (string str in string1.Split(chArray))
        stringList1.Add(str.Split(',')[0]);
      List<string> stringList2 = new List<string>();
      int num = 1;
      if (this.Level >= 20 && this.Level < 30)
        num = 2;
      if (this.Level >= 30 && this.Level < 50)
        num = 3;
      if (this.Level >= 50 && this.Level < 60)
        num = 4;
      if (this.Level >= 60)
        num = 5;
      string[] strArray = this.string_0.Split('|');
      for (int index = 0; index < num; ++index)
      {
        if (index < strArray.Length)
        {
          if (stringList1.Contains(strArray[index].Split(',')[0]))
          {
            stringList2.Add(strArray[index]);
            continue;
          }
        }
        stringList2.Add("0," + (object) index);
      }
      return stringList2;
    }

    public string SkillEquip
    {
      get
      {
        return this.string_0;
      }
      set
      {
        this.string_0 = value;
        this._isDirty = true;
      }
    }

    public string Skill
    {
      get
      {
        return this.string_1;
      }
      set
      {
        this.string_1 = value;
        this._isDirty = true;
      }
    }

    public int ID
    {
      get
      {
        return this.int_0;
      }
      set
      {
        this.int_0 = value;
        this._isDirty = true;
      }
    }

    public int PetID
    {
      get
      {
        return this.int_1;
      }
      set
      {
        this.int_1 = value;
        this._isDirty = true;
      }
    }

    public int TemplateID
    {
      get
      {
        return this.int_2;
      }
      set
      {
        this.int_2 = value;
        this._isDirty = true;
      }
    }

    public string Name
    {
      get
      {
        return this.string_2;
      }
      set
      {
        this.string_2 = value;
        this._isDirty = true;
      }
    }

    public int UserID
    {
      get
      {
        return this.int_3;
      }
      set
      {
        this.int_3 = value;
        this._isDirty = true;
      }
    }

    public int Attack
    {
      get
      {
        return this.int_4 - this.method_1(this.int_4);
      }
      set
      {
        this.int_4 = value;
        this._isDirty = true;
      }
    }

    public int Defence
    {
      get
      {
        return this.int_5 - this.method_1(this.int_5);
      }
      set
      {
        this.int_5 = value;
        this._isDirty = true;
      }
    }

    public int Luck
    {
      get
      {
        return this.int_6 - this.method_1(this.int_6);
      }
      set
      {
        this.int_6 = value;
        this._isDirty = true;
      }
    }

    public int Agility
    {
      get
      {
        return this.int_7 - this.method_1(this.int_7);
      }
      set
      {
        this.int_7 = value;
        this._isDirty = true;
      }
    }

    public int Blood
    {
      get
      {
        return this.int_8 - this.method_1(this.int_8);
      }
      set
      {
        this.int_8 = value;
        this._isDirty = true;
      }
    }

    public int Damage
    {
      get
      {
        return this.int_9 - this.method_1(this.int_9);
      }
      set
      {
        this.int_9 = value;
        this._isDirty = true;
      }
    }

    public int Guard
    {
      get
      {
        return this.int_10 - this.method_1(this.int_10);
      }
      set
      {
        this.int_10 = value;
        this._isDirty = true;
      }
    }

    public int AttackGrow
    {
      get
      {
        return this.int_11;
      }
      set
      {
        this.int_11 = value;
        this._isDirty = true;
      }
    }

    public int DefenceGrow
    {
      get
      {
        return this.int_12;
      }
      set
      {
        this.int_12 = value;
        this._isDirty = true;
      }
    }

    public int LuckGrow
    {
      get
      {
        return this.int_13;
      }
      set
      {
        this.int_13 = value;
        this._isDirty = true;
      }
    }

    public int AgilityGrow
    {
      get
      {
        return this.int_14;
      }
      set
      {
        this.int_14 = value;
        this._isDirty = true;
      }
    }

    public int BloodGrow
    {
      get
      {
        return this.int_15;
      }
      set
      {
        this.int_15 = value;
        this._isDirty = true;
      }
    }

    public int DamageGrow
    {
      get
      {
        return this.int_16;
      }
      set
      {
        this.int_16 = value;
        this._isDirty = true;
      }
    }

    public int GuardGrow
    {
      get
      {
        return this.int_17;
      }
      set
      {
        this.int_17 = value;
        this._isDirty = true;
      }
    }

    public int Level
    {
      get
      {
        return this.int_18;
      }
      set
      {
        this.int_18 = value;
        this._isDirty = true;
      }
    }

    public int MaxLevel()
    {
      switch (this.int_25)
      {
        case 1:
          return 63;
        case 2:
          return 65;
        case 3:
          return 68;
        case 4:
          return 70;
        default:
          return 60;
      }
    }

    public int GP
    {
      get
      {
        return this.int_19;
      }
      set
      {
        this.int_19 = value;
        this._isDirty = true;
      }
    }

    public int MaxGP
    {
      get
      {
        return this.int_20;
      }
      set
      {
        this.int_20 = value;
        this._isDirty = true;
      }
    }

    public int Hunger
    {
      get
      {
        return this.int_21;
      }
      set
      {
        this.int_21 = value;
        this._isDirty = true;
      }
    }

    private int method_0()
    {
      double num1 = (double) this.int_21 / 10000.0 * 100.0;
      int num2 = 0;
      if (num1 >= 80.0)
        num2 = 3;
      if (num1 < 80.0 && num1 >= 60.0)
        num2 = 2;
      if (num1 < 60.0 && num1 > 0.0)
        num2 = 1;
      return num2;
    }

    public int PetHappyStar
    {
      get
      {
        return this.method_0();
      }
    }

    public int MP
    {
      get
      {
        return this.int_22;
      }
      set
      {
        this.int_22 = value;
        this._isDirty = true;
      }
    }

    public bool IsEquip
    {
      get
      {
        return this.bool_0;
      }
      set
      {
        this.bool_0 = value;
        this._isDirty = true;
      }
    }

    public int Place
    {
      get
      {
        return this.int_23;
      }
      set
      {
        this.int_23 = value;
        this._isDirty = true;
      }
    }

    public int currentStarExp
    {
      get
      {
        return this.int_24;
      }
      set
      {
        this.int_24 = value;
        this._isDirty = true;
      }
    }

    public bool IsExit
    {
      get
      {
        return this.bool_1;
      }
      set
      {
        this.bool_1 = value;
        this._isDirty = true;
      }
    }

    public int breakGrade
    {
      get
      {
        return this.int_25;
      }
      set
      {
        this.int_25 = value;
        this._isDirty = true;
      }
    }

    public int breakAttack
    {
      get
      {
        return this.int_26;
      }
      set
      {
        this.int_26 = value;
        this._isDirty = true;
      }
    }

    public int breakDefence
    {
      get
      {
        return this.int_27;
      }
      set
      {
        this.int_27 = value;
        this._isDirty = true;
      }
    }

    public int breakAgility
    {
      get
      {
        return this.int_28;
      }
      set
      {
        this.int_28 = value;
        this._isDirty = true;
      }
    }

    public int breakLuck
    {
      get
      {
        return this.int_29;
      }
      set
      {
        this.int_29 = value;
        this._isDirty = true;
      }
    }

    public int breakBlood
    {
      get
      {
        return this.int_30;
      }
      set
      {
        this.int_30 = value;
        this._isDirty = true;
      }
    }

    private int method_1(int int_32)
    {
      if (this.method_0() == 2)
        return int_32 * 20 / 100;
      if (this.method_0() == 1)
        return int_32 * 40 / 100;
      return 0;
    }

    public List<PetEquipInfo> PetEquips
    {
      get
      {
        return this.list_0;
      }
      set
      {
        this.list_0 = value;
      }
    }

    public string eQPets
    {
      get
      {
        return this.string_3;
      }
      set
      {
        this.string_3 = value;
        this._isDirty = true;
      }
    }

    public string BaseProp
    {
      get
      {
        return this.string_4;
      }
      set
      {
        this.string_4 = value;
        this._isDirty = true;
      }
    }

    public int TotalAttack
    {
      get
      {
        this.int_31 = 0;
        foreach (PetEquipInfo petEquip in this.PetEquips)
        {
          if (petEquip.IsValidItem() && petEquip.Template != null)
            this.int_31 += petEquip.Template.Attack;
        }
        return this.Attack + this.int_31 + this.int_26;
      }
    }

    public int TotalDefence
    {
      get
      {
        this.int_31 = 0;
        foreach (PetEquipInfo petEquip in this.PetEquips)
        {
          if (petEquip.IsValidItem() && petEquip.Template != null)
            this.int_31 += petEquip.Template.Defence;
        }
        return this.Defence + this.int_31 + this.int_27;
      }
    }

    public int TotalLuck
    {
      get
      {
        this.int_31 = 0;
        foreach (PetEquipInfo petEquip in this.PetEquips)
        {
          if (petEquip.IsValidItem() && petEquip.Template != null)
            this.int_31 += petEquip.Template.Luck;
        }
        return this.Luck + this.int_31 + this.breakLuck;
      }
    }

    public int TotalAgility
    {
      get
      {
        this.int_31 = 0;
        foreach (PetEquipInfo petEquip in this.PetEquips)
        {
          if (petEquip.IsValidItem() && petEquip.Template != null)
            this.int_31 += petEquip.Template.Agility;
        }
        return this.Agility + this.int_31 + this.int_28;
      }
    }

    public int TotalBlood
    {
      get
      {
        return this.Blood + this.int_30;
      }
    }

    public int TotalDamage
    {
      get
      {
        return this.Damage;
      }
    }

    public int TotalGuard
    {
      get
      {
        return this.Guard;
      }
    }

    private double[] method_2(int int_32, double[] double_0)
    {
      double[] numArray = new double[double_0.Length];
      numArray[0] = double_0[0] * Math.Pow(2.0, (double) (int_32 - 1));
      for (int index = 1; index < double_0.Length; ++index)
        numArray[index] = double_0[index] * Math.Pow(1.5, (double) (int_32 - 1));
      return numArray;
    }

    public void BuildProp(UsersPetInfo petInfo)
    {
      double[] numArray1 = new double[5]
      {
        (double) (this.BloodGrow * 10),
        (double) this.AttackGrow,
        (double) this.DefenceGrow,
        (double) this.AgilityGrow,
        (double) this.LuckGrow
      };
      double[] double_0 = new double[5]
      {
        (double) this.BloodGrow,
        (double) this.AttackGrow,
        (double) this.DefenceGrow,
        (double) this.AgilityGrow,
        (double) this.LuckGrow
      };
      double[] numArray2 = numArray1;
      double[] numArray3 = numArray1;
      double[] numArray4 = numArray1;
      double[] numArray5 = this.method_2(1, double_0);
      double[] numArray6 = this.method_2(2, double_0);
      double[] numArray7 = this.method_2(3, double_0);
      double[] numArray8 = new double[numArray2.Length];
      double[] numArray9;
      if (this.Level < 30)
      {
        for (int index = 0; index < numArray2.Length; ++index)
        {
          numArray2[index] = numArray2[index] + (double) (this.Level - 1) * numArray5[index];
          numArray2[index] = Math.Ceiling(numArray2[index] / 10.0) / 10.0;
        }
        numArray9 = numArray2;
      }
      else if (this.Level < 50)
      {
        for (int index = 0; index < numArray3.Length; ++index)
        {
          numArray3[index] = numArray3[index] + ((double) (this.Level - 30) * numArray6[index] + 29.0 * numArray5[index]);
          numArray3[index] = Math.Ceiling(numArray3[index] / 10.0) / 10.0;
        }
        numArray9 = numArray3;
      }
      else
      {
        for (int index = 0; index < numArray4.Length; ++index)
        {
          numArray4[index] = numArray4[index] + ((double) (this.Level - 50) * numArray7[index] + 20.0 * numArray6[index] + 29.0 * numArray5[index]);
          numArray4[index] = Math.Ceiling(numArray4[index] / 10.0) / 10.0;
        }
        numArray9 = numArray4;
      }
      this.int_8 = (int) numArray9[0];
      this.int_4 = (int) numArray9[1];
      this.int_5 = (int) numArray9[2];
      this.int_7 = (int) numArray9[3];
      this.int_6 = (int) numArray9[4];
    }

    public UsersPetInfo()
    {
      this.list_0 = new List<PetEquipInfo>();
    }
  }
}
