// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Maths.EulerVector
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

namespace Game.Logic.Phy.Maths
{
  public class EulerVector
  {
    public float x0;
    public float x1;
    public float x2;

    public EulerVector(int x0, int x1, float x2)
    {
      this.x0 = (float) x0;
      this.x1 = (float) x1;
      this.x2 = x2;
    }

    public void clear()
    {
      this.x0 = 0.0f;
      this.x1 = 0.0f;
      this.x2 = 0.0f;
    }

    public void clearMotion()
    {
      this.x1 = 0.0f;
      this.x2 = 0.0f;
    }

    public void ComputeOneEulerStep(float m, float af, float f, float dt)
    {
      this.x2 = (f - af * this.x1) / m;
      this.x1 += this.x2 * dt;
      this.x0 += this.x1 * dt;
    }

    public string toString()
    {
      return "x:" + (object) this.x0 + ",v:" + (object) this.x1 + ",a" + (object) this.x2;
    }
  }
}
