// Decompiled with JetBrains decompiler
// Type: Game.Logic.AI.AMissionControl
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Base.Packets;

namespace Game.Logic.AI
{
  public abstract class AMissionControl
  {
    private PVEGame m_game;

    public virtual int CalculateScoreGrade(int score)
    {
      return 0;
    }

    public virtual bool CanGameOver()
    {
      return true;
    }

    public virtual void Dispose()
    {
    }

    public virtual void DoOther()
    {
    }

    public virtual void GameOverAllSession()
    {
    }

    public virtual void OnBeginNewTurn()
    {
    }

    public virtual void OnDied()
    {
    }

    public virtual void OnGameOver()
    {
    }

    public virtual void OnGameOverMovie()
    {
    }

    public virtual void OnNewTurnStarted()
    {
    }

    public virtual void OnPrepareNewGame()
    {
    }

    public virtual void OnPrepareNewSession()
    {
    }

    public virtual void OnShooted()
    {
    }

    public virtual void OnStartGame()
    {
    }

    public virtual void OnStartMovie()
    {
    }

    public virtual void OnMissionEvent(GSPacketIn packet)
    {
    }

    public virtual void OnPrepareGameOver()
    {
    }

    public virtual int UpdateUIData()
    {
      return 0;
    }

    public virtual void OnPrepareStartGame()
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
