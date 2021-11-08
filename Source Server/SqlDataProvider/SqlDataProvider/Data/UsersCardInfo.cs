// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.UsersCardInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class UsersCardInfo : DataObject
  {
    private int int_0;
    private int int_1;
    private int int_2;
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
    private bool bool_0;

    public int CardID
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

    public int UserID
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

    public int Place
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

    public int Count
    {
      get
      {
        return this.int_4;
      }
      set
      {
        this.int_4 = value;
        this._isDirty = true;
      }
    }

    public int Attack
    {
      get
      {
        return this.int_5;
      }
      set
      {
        this.int_5 = value;
        this._isDirty = true;
      }
    }

    public int Defence
    {
      get
      {
        return this.int_6;
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
        return this.int_7;
      }
      set
      {
        this.int_7 = value;
        this._isDirty = true;
      }
    }

    public int Luck
    {
      get
      {
        return this.int_8;
      }
      set
      {
        this.int_8 = value;
        this._isDirty = true;
      }
    }

    public int AttackReset
    {
      get
      {
        return this.int_9;
      }
      set
      {
        this.int_9 = value;
        this._isDirty = true;
      }
    }

    public int DefenceReset
    {
      get
      {
        return this.int_10;
      }
      set
      {
        this.int_10 = value;
        this._isDirty = true;
      }
    }

    public int AgilityReset
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

    public int LuckReset
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

    public int TotalAttack
    {
      get
      {
        return this.int_5 + this.int_9;
      }
    }

    public int TotalDefence
    {
      get
      {
        return this.int_6 + this.int_10;
      }
    }

    public int TotalAgility
    {
      get
      {
        return this.int_7 + this.int_11;
      }
    }

    public int TotalLuck
    {
      get
      {
        return this.int_8 + this.int_12;
      }
    }

    public int Guard
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

    public int Damage
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

    public int Level
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

    public int CardGP
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

    public bool isFirstGet
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

    public UsersCardInfo()
    {
    }

    public UsersCardInfo(int userId, int templateId, int count)
    {
      this.int_0 = 0;
      this.int_1 = userId;
      this.int_2 = templateId;
      this.int_4 = count;
      this.bool_0 = true;
    }

    public void Copy(UsersCardInfo copy)
    {
      this.UserID = copy.UserID;
      this.TemplateID = copy.TemplateID;
      this.Place = copy.Place;
      this.Count = copy.Count;
      this.Attack = copy.Attack;
      this.Defence = copy.Defence;
      this.Agility = copy.Agility;
      this.Luck = copy.Luck;
      this.AttackReset = copy.AttackReset;
      this.DefenceReset = copy.DefenceReset;
      this.AgilityReset = copy.AgilityReset;
      this.LuckReset = copy.LuckReset;
      this.Guard = copy.Guard;
      this.Damage = copy.Damage;
      this.Level = copy.Level;
      this.CardGP = copy.CardGP;
      this.isFirstGet = copy.isFirstGet;
    }

    public void CopyProp(UsersCardInfo copy)
    {
      this.Attack = copy.Attack;
      this.Defence = copy.Defence;
      this.Agility = copy.Agility;
      this.Luck = copy.Luck;
      this.AttackReset = copy.AttackReset;
      this.DefenceReset = copy.DefenceReset;
      this.AgilityReset = copy.AgilityReset;
      this.LuckReset = copy.LuckReset;
      this.Guard = copy.Guard;
      this.Damage = copy.Damage;
      this.Level = copy.Level;
      this.CardGP = copy.CardGP;
    }

    public UsersCardInfo Clone()
    {
      return new UsersCardInfo()
      {
        UserID = this.int_1,
        TemplateID = this.int_2,
        Place = this.int_3,
        Count = this.int_4,
        Attack = this.int_5,
        Defence = this.int_6,
        Agility = this.int_7,
        Luck = this.int_8,
        AttackReset = this.int_9,
        DefenceReset = this.int_10,
        AgilityReset = this.int_11,
        LuckReset = this.int_12,
        Guard = this.int_13,
        Damage = this.int_14,
        Level = this.int_15,
        CardGP = this.int_16,
        isFirstGet = this.bool_0
      };
    }
  }
}
