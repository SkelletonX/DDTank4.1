// Decompiled with JetBrains decompiler
// Type: Game.Logic.LivingConfig
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic
{
  public class LivingConfig
  {
    public bool HaveShield;
    private bool m_DamageForzen;
    private byte m_isBotom;
    private bool m_isConsortiaBoss;
    private bool m_isChristmasBoss;
    private bool m_isFly;
    private bool m_isHelper;
    private bool m_isShowBlood;
    private bool m_isShowSmallMapPoint;
    private bool m_isTurn;
    private bool m_CancelGuard;
    private bool m_CanFrost;
    private bool m_CanCountKill;
    private bool m_CanHeal;
    private bool m_CanCollied;
    private bool m_isWorldBoss;
    private int m_reduceBloodStart;
    private bool m_canTakeDamage;
    private int m_FirstStepMove;
    private int m_MaxStepMove;
    private bool m_CompleteStep;

    public void Clone(LivingConfig _clone)
    {
      this.m_isHelper = _clone.IsHelper;
      this.CanTakeDamage = _clone.CanTakeDamage;
      this.HaveShield = _clone.HaveShield;
      this.isBotom = _clone.isBotom;
      this.isConsortiaBoss = _clone.isConsortiaBoss;
      this.IsFly = _clone.IsFly;
      this.isShowBlood = _clone.isShowBlood;
      this.isShowSmallMapPoint = _clone.isShowSmallMapPoint;
      this.IsTurn = _clone.IsTurn;
      this.ReduceBloodStart = _clone.ReduceBloodStart;
      this.DamageForzen = _clone.DamageForzen;
    }

    public bool CanTakeDamage
    {
      get
      {
        return this.m_canTakeDamage;
      }
      set
      {
        this.m_canTakeDamage = value;
      }
    }

    public bool DamageForzen
    {
      get
      {
        return this.m_DamageForzen;
      }
      set
      {
        this.m_DamageForzen = value;
      }
    }

    public byte isBotom
    {
      get
      {
        return this.m_isBotom;
      }
      set
      {
        this.m_isBotom = value;
      }
    }

    public bool isConsortiaBoss
    {
      get
      {
        return this.m_isConsortiaBoss;
      }
      set
      {
        this.m_isConsortiaBoss = value;
      }
    }

    public bool IsFly
    {
      get
      {
        return this.m_isFly;
      }
      set
      {
        this.m_isFly = value;
      }
    }

    public bool IsHelper
    {
      get
      {
        return this.m_isHelper;
      }
      set
      {
        this.m_isHelper = value;
      }
    }

    public bool isShowBlood
    {
      get
      {
        return this.m_isShowBlood;
      }
      set
      {
        this.m_isShowBlood = value;
      }
    }

    public bool isShowSmallMapPoint
    {
      get
      {
        return this.m_isShowSmallMapPoint;
      }
      set
      {
        this.m_isShowSmallMapPoint = value;
      }
    }

    public bool IsTurn
    {
      get
      {
        return this.m_isTurn;
      }
      set
      {
        this.m_isTurn = value;
      }
    }

    public bool CancelGuard
    {
      get
      {
        return this.m_CancelGuard;
      }
      set
      {
        this.m_CancelGuard = value;
      }
    }

    public bool CanFrost
    {
      get
      {
        return this.m_CanFrost;
      }
      set
      {
        this.m_CanFrost = value;
      }
    }

    public bool CanHeal
    {
      get
      {
        return this.m_CanHeal;
      }
      set
      {
        this.m_CanHeal = value;
      }
    }

    public bool CanCollied
    {
      get
      {
        return this.m_CanCollied;
      }
      set
      {
        this.m_CanCollied = value;
      }
    }

    public bool CanCountKill
    {
      get
      {
        return this.m_CanCountKill;
      }
      set
      {
        this.m_CanCountKill = value;
      }
    }

    public int ReduceBloodStart
    {
      get
      {
        return this.m_reduceBloodStart;
      }
      set
      {
        this.m_reduceBloodStart = value;
      }
    }

    public int FirstStepMove
    {
      get
      {
        return this.m_FirstStepMove;
      }
      set
      {
        this.m_FirstStepMove = value;
      }
    }

    public int MaxStepMove
    {
      get
      {
        return this.m_MaxStepMove;
      }
      set
      {
        this.m_MaxStepMove = value;
      }
    }

    public bool CompleteStep
    {
      get
      {
        return this.m_CompleteStep;
      }
      set
      {
        this.m_CompleteStep = value;
      }
    }
  }
}
