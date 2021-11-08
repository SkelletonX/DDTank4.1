// Decompiled with JetBrains decompiler
// Type: SqlDataProvider.Data.BallInfo
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

namespace SqlDataProvider.Data
{
  public class BallInfo
  {
    public bool IsSpecial()
    {
      int id = this.ID;
      if (id <= 64)
      {
        if (id > 16)
          return id == 59 || id == 64;
        switch (id)
        {
          case 1:
          case 3:
          case 5:
          case 16:
            break;
          case 2:
          case 4:
            return false;
          default:
            return false;
        }
      }
      else
      {
        if (id <= 98)
          return id == 97 || id == 98;
        if (id != 110 && id != 117 && (uint) (id - 10001) > 22U)
          return false;
      }
      return true;
    }

    public int ActionType { get; set; }

    public int Amount { get; set; }

    public int AttackResponse { get; set; }

    public string BombPartical { get; set; }

    public string BombSound { get; set; }

    public string Crater { get; set; }

    public int Delay { get; set; }

    public int DragIndex { get; set; }

    public string FlyingPartical { get; set; }

    public bool HasTunnel { get; set; }

    public int ID { get; set; }

    public bool IsSpin { get; set; }

    public int Mass { get; set; }

    public string Name { get; set; }

    public double Power { get; set; }

    public int Radii { get; set; }

    public bool Shake { get; set; }

    public string ShootSound { get; set; }

    public int SpinV { get; set; }

    public double SpinVA { get; set; }

    public int Weight { get; set; }

    public int Wind { get; set; }
  }
}
