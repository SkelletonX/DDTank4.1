// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.CanShootInfo
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Phy.Object
{
  public class CanShootInfo
  {
    private int m_angle;
    private bool m_canShoot;
    private int m_force;

    public CanShootInfo(bool canShoot, int force, int angle)
    {
      this.m_canShoot = canShoot;
      this.m_force = force;
      this.m_angle = angle;
    }

    public int Angle
    {
      get
      {
        return this.m_angle;
      }
    }

    public bool CanShoot
    {
      get
      {
        return this.m_canShoot;
      }
    }

    public int Force
    {
      get
      {
        return this.m_force;
      }
    }
  }
}
