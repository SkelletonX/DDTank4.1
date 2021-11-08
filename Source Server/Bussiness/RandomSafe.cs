// Decompiled with JetBrains decompiler
// Type: Bussiness.RandomSafe
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.Security.Cryptography;

namespace Bussiness
{
  public class RandomSafe : Random
  {
    public override int Next(int max)
    {
      return this.Next(0, max);
    }

    public override int Next(int min, int max)
    {
      int num1 = base.Next(1, 50);
      int num2 = max - 1;
      for (int index = 0; index < num1; ++index)
        num2 = base.Next(min, max);
      return num2;
    }

    public int NextSmallValue(int min, int max)
    {
      int num = Math.Abs(this.Next(min, max) - max);
      if (num > max)
        num = max;
      else if (num < min)
        num = min;
      return num;
    }

    private static int smethod_0()
    {
      byte[] data = new byte[4];
      new RNGCryptoServiceProvider().GetBytes(data);
      return BitConverter.ToInt32(data, 0);
    }
  }
}
