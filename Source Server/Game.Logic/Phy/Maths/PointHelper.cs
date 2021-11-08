// Decompiled with JetBrains decompiler
// Type: Game.Logic.Phy.Maths.PointHelper
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using System;
using System.Drawing;

namespace Game.Logic.Phy.Maths
{
  public static class PointHelper
  {
    public static double Distance(this Point point, Point target)
    {
      int num1 = point.X - target.X;
      int num2 = point.Y - target.Y;
      return Math.Sqrt((double) (num1 * num1 + num2 * num2));
    }

    public static double Distance(this Point point, int tx, int ty)
    {
      int num1 = point.X - tx;
      int num2 = point.Y - ty;
      return Math.Sqrt((double) (num1 * num1 + num2 * num2));
    }

    public static double Length(this Point point)
    {
      return Math.Sqrt((double) (point.X * point.X + point.Y * point.Y));
    }

    public static double Length(this PointF point)
    {
      return Math.Sqrt((double) point.X * (double) point.X + (double) point.Y * (double) point.Y);
    }

    public static Point Normalize(this Point point, int len)
    {
      double num = point.Length();
      return new Point((int) ((double) (point.X * len) / num), (int) ((double) (point.Y * len) / num));
    }

    public static PointF Normalize(this PointF point, float len)
    {
      double num = Math.Sqrt((double) point.X * (double) point.X + (double) point.Y * (double) point.Y);
      return new PointF((float) ((double) point.X * (double) len / num), (float) ((double) point.Y * (double) len / num));
    }
  }
}
