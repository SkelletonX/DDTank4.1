// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Object.Physics
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Maps;
using System;
using System.Drawing;

namespace Game.Logic.Phy.Object
{
  public class Physics
  {
    protected int m_id;
    protected bool m_isLiving;
    protected bool m_isMoving;
    protected Map m_map;
    protected Rectangle m_rect;
    protected Rectangle m_rectBomb;
    protected int m_x;
    protected int m_y;
    private object object_1;
    private object object_2;
    private object properties1;

    public Physics(int id)
    {
      this.m_id = id;
      this.m_rect = new Rectangle(-5, -5, 10, 10);
      this.m_rectBomb = new Rectangle(0, 0, 0, 0);
      this.m_isLiving = true;
    }

    public virtual void CollidedByObject(Physics phy)
    {
    }

    public virtual void Die()
    {
      this.StopMoving();
      this.m_isLiving = false;
    }

    public virtual void Dispose()
    {
      if (this.m_map == null)
        return;
      this.m_map.RemovePhysical(this);
    }

    public double Distance(int x, int y)
    {
      return Math.Sqrt((double) ((this.m_x - x) * (this.m_x - x) + (this.m_y - y) * (this.m_y - y)));
    }

    public virtual Point GetCollidePoint()
    {
      return new Point(this.X, this.Y);
    }

    public static int PointToLine(int x1, int y1, int x2, int y2, int px, int py)
    {
      int num1 = y1 - y2;
      int num2 = x2 - x1;
      int num3 = x1 * y2 - x2 * y1;
      return (int) ((double) Math.Abs(num1 * px + num2 * py + num3) / Math.Sqrt((double) (num1 * num1 + num2 * num2)));
    }

    public virtual void PrepareNewTurn()
    {
    }

    public virtual void SetMap(Map map)
    {
      this.m_map = map;
    }

    public void SetRect(int x, int y, int width, int height)
    {
      this.m_rect.X = x;
      this.m_rect.Y = y;
      this.m_rect.Width = width;
      this.m_rect.Height = height;
    }

    public void SetRectBomb(int x, int y, int width, int height)
    {
      this.m_rectBomb.X = x;
      this.m_rectBomb.Y = y;
      this.m_rectBomb.Width = width;
      this.m_rectBomb.Height = height;
    }

    public void SetXY(Point p)
    {
      this.SetXY(p.X, p.Y);
    }

    public virtual void SetXY(int x, int y)
    {
      this.m_x = x;
      this.m_y = y;
    }

    public virtual void StartMoving()
    {
      if (this.m_map == null)
        return;
      this.m_isMoving = true;
    }

    public virtual void StopMoving()
    {
      this.m_isMoving = false;
    }

    public Rectangle Bound
    {
      get
      {
        return this.m_rect;
      }
    }

    public Rectangle Bound1
    {
      get
      {
        return this.m_rectBomb;
      }
    }

    public int Id
    {
      get
      {
        return this.m_id;
      }
    }

    public bool IsLiving
    {
      get
      {
        return this.m_isLiving;
      }
      set
      {
        this.m_isLiving = value;
      }
    }

    public bool IsMoving
    {
      get
      {
        return this.m_isMoving;
      }
    }

    public object Properties1
    {
      get
      {
        return this.properties1;
      }
      set
      {
        this.properties1 = value;
      }
    }

    public object Properties2
    {
      get
      {
        return this.object_1;
      }
      set
      {
        this.object_1 = value;
      }
    }

    public object Properties3
    {
      get
      {
        return this.object_2;
      }
      set
      {
        this.object_2 = value;
      }
    }

    public virtual int X
    {
      get
      {
        return this.m_x;
      }
    }

    public virtual int Y
    {
      get
      {
        return this.m_y;
      }
    }
  }
}
