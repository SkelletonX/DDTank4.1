// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Maps.Map
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using Game.Logic.Phy.Object;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Game.Logic.Phy.Maps
{
  public class Map
  {
    private MapInfo mapInfo_0;
    private float float_0;
    private HashSet<Physics> hashSet_0;
    protected Tile _layer1;
    protected Tile _layer2;
    protected Rectangle _bound;
    private Random random_0;

    public float wind
    {
      get
      {
        return this.float_0;
      }
      set
      {
        this.float_0 = value;
      }
    }

    public float gravity
    {
      get
      {
        return (float) this.mapInfo_0.Weight;
      }
    }

    public float airResistance
    {
      get
      {
        return (float) this.mapInfo_0.DragIndex;
      }
    }

    public Tile Ground
    {
      get
      {
        return this._layer1;
      }
    }

    public Tile DeadTile
    {
      get
      {
        return this._layer2;
      }
    }

    public MapInfo Info
    {
      get
      {
        return this.mapInfo_0;
      }
    }

    public Rectangle Bound
    {
      get
      {
        return this._bound;
      }
    }

    public Map(MapInfo info, Tile layer1, Tile layer2)
    {
      this.mapInfo_0 = info;
      this.hashSet_0 = new HashSet<Physics>();
      this.random_0 = new Random();
      this._layer1 = layer1;
      this._layer2 = layer2;
      if (this._layer1 != null)
        this._bound = new Rectangle(0, 0, this._layer1.Width, this._layer1.Height);
      else
        this._bound = new Rectangle(0, 0, this._layer2.Width, this._layer2.Height);
    }

    public void Dig(int cx, int cy, Tile surface, Tile border)
    {
      if (this._layer1 != null)
        this._layer1.Dig(cx, cy, surface, border);
      if (this._layer2 == null)
        return;
      this._layer2.Dig(cx, cy, surface, border);
    }

    public bool IsEmpty(int x, int y)
    {
      if (this._layer1 != null && !this._layer1.IsEmpty(x, y))
        return false;
      if (this._layer2 != null)
        return this._layer2.IsEmpty(x, y);
      return true;
    }

    public bool IsRectangleEmpty(Rectangle rect)
    {
      if (this._layer1 != null && !this._layer1.IsRectangleEmptyQuick(rect))
        return false;
      if (this._layer2 != null)
        return this._layer2.IsRectangleEmptyQuick(rect);
      return true;
    }

    public Point FindYLineNotEmptyPointDown(int x, int y, int h)
    {
      x = x < 0 ? 0 : (x >= this._bound.Width ? this._bound.Width - 1 : x);
      y = y < 0 ? 0 : y;
      h = y + h >= this._bound.Height ? this._bound.Height - y - 1 : h;
      for (int index = 0; index < h; ++index)
      {
        if (!this.IsEmpty(x - 1, y) || !this.IsEmpty(x + 1, y))
          return new Point(x, y);
        ++y;
      }
      return Point.Empty;
    }

    public Point FindYLineNotEmptyPointDown(int x, int y)
    {
      return this.FindYLineNotEmptyPointDown(x, y, this._bound.Height);
    }

    public Point FindYLineNotEmptyPointUp(int x, int y, int h)
    {
      x = x < 0 ? 0 : (x >= this._bound.Width ? this._bound.Width : x);
      y = y < 0 ? 0 : y;
      h = y + h >= this._bound.Height ? this._bound.Height - y : h;
      for (int index = 0; index < h; ++index)
      {
        if (!this.IsEmpty(x - 1, y) || !this.IsEmpty(x + 1, y))
          return new Point(x, y);
        --y;
      }
      return Point.Empty;
    }

    public Point FindNextWalkPoint(int x, int y, int direction, int stepX, int stepY)
    {
      if (direction != 1 && direction != -1)
        return Point.Empty;
      int x1 = x + direction * stepX;
      if (x1 < 0 || x1 > this._bound.Width)
        return Point.Empty;
      Point point = this.FindYLineNotEmptyPointDown(x1, y - stepY - 1, this._bound.Width);
      if (point != Point.Empty && Math.Abs(point.Y - y) > stepY)
        point = Point.Empty;
      return point;
    }

    public List<Living> FindLiving(int fx, int tx, List<Living> exceptLivings)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          bool flag = true;
          if (physics is Living && physics.IsLiving && (physics.X > fx && physics.X < tx))
          {
            if (exceptLivings != null && (uint) exceptLivings.Count > 0U)
            {
              foreach (Living exceptLiving in exceptLivings)
              {
                if (physics.Id == exceptLiving.Id)
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                livingList.Add(physics as Living);
            }
            else
              livingList.Add(physics as Living);
          }
        }
      }
      return livingList;
    }

    public Point FindNextWalkPointDown(int x, int y, int direction, int stepX, int stepY)
    {
      if (direction != 1 && direction != -1)
        return Point.Empty;
      int x1 = x + direction * stepX;
      if (x1 < 0 || x1 > this._bound.Width)
        return Point.Empty;
      Point point = this.FindYLineNotEmptyPointDown(x1, y - stepY - 1);
      if (point != Point.Empty && Math.Abs(point.Y - y) > stepY)
        point = Point.Empty;
      return point;
    }

    public List<Living> FindRandomPlayer(int fx, int tx, List<Player> exceptPlayers)
    {
      List<Living> livingList1 = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Player && physics.IsLiving && (physics.X > fx && physics.X < tx))
          {
            foreach (Player exceptPlayer in exceptPlayers)
            {
              if (((Player) physics).PlayerDetail == exceptPlayer.PlayerDetail)
                livingList1.Add(physics as Living);
            }
          }
        }
      }
      List<Living> livingList2 = new List<Living>();
      if (livingList1.Count > 0)
        livingList2.Add(livingList1[this.random_0.Next(livingList1.Count)]);
      return livingList2;
    }

    public List<Living> FindRandomLiving(int fx, int tx)
    {
      List<Living> livingList1 = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if ((physics is SimpleNpc || physics is SimpleBoss) && (physics.IsLiving && physics.X > fx) && physics.X < tx)
            livingList1.Add(physics as Living);
        }
      }
      List<Living> livingList2 = new List<Living>();
      if (livingList1.Count > 0)
        livingList2.Add(livingList1[this.random_0.Next(livingList1.Count)]);
      return livingList2;
    }

    public bool canMove(int x, int y)
    {
      if (this.IsEmpty(x, y))
        return !this.IsOutMap(x, y);
      return false;
    }

    public bool IsOutMap(int x, int y)
    {
      if (x >= this._bound.X && x <= this._bound.Width)
        return y > this._bound.Height;
      return true;
    }

    public void AddPhysical(Physics phy)
    {
      phy.SetMap(this);
      lock (this.hashSet_0)
        this.hashSet_0.Add(phy);
    }

    public void RemovePhysical(Physics phy)
    {
      phy.SetMap((Map) null);
      lock (this.hashSet_0)
        this.hashSet_0.Remove(phy);
    }

    public List<Physics> GetAllPhysicalSafe()
    {
      List<Physics> physicsList = new List<Physics>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
          physicsList.Add(physics);
      }
      return physicsList;
    }

    public List<PhysicalObj> GetAllPhysicalObjSafe()
    {
      List<PhysicalObj> physicalObjList = new List<PhysicalObj>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is PhysicalObj)
            physicalObjList.Add(physics as PhysicalObj);
        }
      }
      return physicalObjList;
    }

    public Physics[] FindPhysicalObjects(Rectangle rect, Physics except)
    {
      List<Physics> physicsList = new List<Physics>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics.IsLiving && physics != except)
          {
            Rectangle bound = physics.Bound;
            Rectangle bound1 = physics.Bound1;
            bound.Offset(physics.X, physics.Y);
            bound1.Offset(physics.X, physics.Y);
            if (bound.IntersectsWith(rect) || bound1.IntersectsWith(rect))
              physicsList.Add(physics);
          }
        }
      }
      return physicsList.ToArray();
    }

    public bool FindPlayers(Point p, int radius)
    {
      int num = 0;
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Player && physics.IsLiving && (physics as Player).BoundDistance(p) < (double) radius)
            ++num;
          if (num >= 2)
            return true;
        }
      }
      return false;
    }

    public List<Player> FindPlayers(int x, int y, int radius)
    {
      List<Player> playerList = new List<Player>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Player && physics.IsLiving && physics.Distance(x, y) < (double) radius)
            playerList.Add(physics as Player);
        }
      }
      return playerList;
    }

    public List<Living> FindLivings(int x, int y, int radius)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Living && physics.IsLiving && physics.Distance(x, y) < (double) radius)
            livingList.Add(physics as Living);
        }
      }
      return livingList;
    }

    public List<Living> FindPlayers(int fx, int tx, List<Player> exceptPlayers)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if ((physics is Player || physics is Living && (physics as Living).Config.IsHelper) && (physics.IsLiving && physics.X > fx && physics.X < tx) && (!(physics is Player) || (physics as Player).IsActive))
          {
            if (exceptPlayers != null)
            {
              foreach (Player exceptPlayer in exceptPlayers)
              {
                if (physics is Player && ((TurnedLiving) physics).DefaultDelay != exceptPlayer.DefaultDelay)
                  livingList.Add(physics as Living);
              }
            }
            else
              livingList.Add(physics as Living);
          }
        }
      }
      return livingList;
    }

    public List<Living> FindHitByHitPiont(Point p, int radius)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Living && physics.IsLiving && (physics as Living).BoundDistance(p) < (double) radius)
            livingList.Add(physics as Living);
        }
      }
      return livingList;
    }

    public Living FindNearestEnemy(int x, int y, double maxdistance, Living except)
    {
      Living living = (Living) null;
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Living && physics != except && (physics.IsLiving && ((Living) physics).Team != except.Team))
          {
            double num = physics.Distance(x, y);
            if (num < maxdistance)
            {
              living = physics as Living;
              maxdistance = num;
            }
          }
        }
      }
      return living;
    }

    public List<Living> FindAllNearestEnemy(
      int x,
      int y,
      double maxdistance,
      Living except)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Living && physics != except && (physics.IsLiving && ((Living) physics).Team != except.Team))
          {
            double num = physics.Distance(x, y);
            if (num < maxdistance)
            {
              livingList.Add(physics as Living);
              maxdistance = num;
            }
          }
        }
      }
      return livingList;
    }

    public List<Living> FindAllNearestSameTeam(
      int x,
      int y,
      double maxdistance,
      Living except)
    {
      List<Living> livingList = new List<Living>();
      lock (this.hashSet_0)
      {
        foreach (Physics physics in this.hashSet_0)
        {
          if (physics is Living && physics != except && (physics.IsLiving && ((Living) physics).Team == except.Team))
          {
            double num = physics.Distance(x, y);
            if (num < maxdistance)
            {
              livingList.Add(physics as Living);
              maxdistance = num;
            }
          }
        }
      }
      return livingList;
    }

    public void Dispose()
    {
      foreach (Physics physics in this.hashSet_0)
        physics.Dispose();
    }

    public Map Clone()
    {
      return new Map(this.mapInfo_0, this._layer1 != null ? this._layer1.Clone() : (Tile) null, this._layer2 != null ? this._layer2.Clone() : (Tile) null);
    }
  }
}
