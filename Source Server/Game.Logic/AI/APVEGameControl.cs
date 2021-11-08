// Decompiled with JetBrains decompiler
// Type: Game.Logic.AI.APVEGameControl
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.AI
{
  public abstract class APVEGameControl
  {
    protected PVEGame m_game;

    public virtual int CalculateScoreGrade(int score)
    {
      return 0;
    }

    public virtual void Dispose()
    {
    }

    public virtual void OnCreated()
    {
    }

    public virtual void OnGameOverAllSession()
    {
    }

    public virtual void OnPrepated()
    {
    }

    public PVEGame Game
    {
      get
      {
        return this.m_game;
      }
      set
      {
        this.m_game = value;
      }
    }
  }
}
