// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.PhysicalObj
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Actions;
using System.Collections.Generic;

namespace Game.Logic.Phy.Object
{
  public class PhysicalObj : Physics
  {
    private Dictionary<string, string> m_actionMapping;
    private string m_model;
    private string m_currentAction;
    private int m_scale;
    private int m_rotation;
    private BaseGame m_game;
    private bool m_canPenetrate;
    private string m_name;
    private int m_phyBringToFront;
    private int m_type;
    private int m_typeEffect;

    public virtual int Type
    {
      get
      {
        return this.m_type;
      }
      set
      {
        this.m_type = value;
      }
    }

    public string Model
    {
      get
      {
        return this.m_model;
      }
    }

    public string CurrentAction
    {
      get
      {
        return this.m_currentAction;
      }
      set
      {
        this.m_currentAction = value;
      }
    }

    public int Scale
    {
      get
      {
        return this.m_scale;
      }
    }

    public int Rotation
    {
      get
      {
        return this.m_rotation;
      }
    }

    public virtual int phyBringToFront
    {
      get
      {
        return this.m_phyBringToFront;
      }
    }

    public int typeEffect
    {
      get
      {
        return this.m_typeEffect;
      }
    }

    public bool CanPenetrate
    {
      get
      {
        return this.m_canPenetrate;
      }
      set
      {
        this.m_canPenetrate = value;
      }
    }

    public string Name
    {
      get
      {
        return this.m_name;
      }
    }

    public void SetGame(BaseGame game)
    {
      this.m_game = game;
    }

    public void PlayMovie(string action, int delay, int movieTime)
    {
      if (this.m_game == null)
        return;
      this.m_game.AddAction((IAction) new PhysicalObjDoAction(this, action, delay, movieTime));
    }

    public override void CollidedByObject(Physics phy)
    {
      if (this.m_canPenetrate || !(phy is SimpleBomb))
        return;
      ((SimpleBomb) phy).Bomb();
    }

    public PhysicalObj(
      int id,
      string name,
      string model,
      string defaultAction,
      int scale,
      int rotation,
      int typeEffect)
      : base(id)
    {
      this.m_name = name;
      this.m_model = model;
      this.m_currentAction = defaultAction;
      this.m_scale = scale;
      this.m_rotation = rotation;
      this.m_canPenetrate = true;
      this.m_typeEffect = typeEffect;
      if (name != null)
      {
        if (name == "hide")
        {
          this.m_phyBringToFront = 6;
          goto label_6;
        }
        else if (name == "top")
        {
          this.m_phyBringToFront = 1;
          goto label_6;
        }
      }
      this.m_phyBringToFront = -1;
label_6:
      this.m_actionMapping = new Dictionary<string, string>();
      if (model == "asset.game.transmitted")
        this.m_type = 3;
      else if (model == "asset.game.six.ball")
      {
        if (this.m_actionMapping.ContainsKey(defaultAction))
          return;
        this.m_actionMapping.Add(defaultAction, this.getActionMap(defaultAction));
      }
      else
        this.m_type = 0;
    }

    public Dictionary<string, string> ActionMapping
    {
      get
      {
        return this.m_actionMapping;
      }
    }

    private string getActionMap(string act)
    {
      string str1 = act;
      if (str1 != null)
      {
        string str2 = str1;
        if (str2 != null)
        {
          switch (str2)
          {
            case "double":
              return "shield-double";
            case "s-1":
              return "shield-1";
            case "s-2":
              return "shield-2";
            case "s-3":
              return "shield-3";
            case "s-4":
              return "shield-4";
            case "s-5":
              return "shield-5";
            case "s-6":
              return "shield-6";
            case "s1":
              return "shield1";
            case "s2":
              return "shield2";
            case "s3":
              return "shield3";
            case "s4":
              return "shield4";
            case "s5":
              return "shield5";
            case "s6":
              return "shield6";
          }
        }
      }
      return act;
    }
  }
}
