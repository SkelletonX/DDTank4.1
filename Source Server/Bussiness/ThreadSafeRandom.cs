// Decompiled with JetBrains decompiler
// Type: Bussiness.ThreadSafeRandom
// Assembly: Bussiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C2537CFF-7BDB-4A06-BE9C-A8074B2C97E3
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Bussiness.dll

using System;
using System.Collections.Generic;

namespace Bussiness
{
  public class ThreadSafeRandom
  {
    private static Random randomStatic = new Random();
    private Random random = new Random();

    public int Next()
    {
      lock (this.random)
        return this.random.Next();
    }

    public int Next(int maxValue)
    {
      lock (this.random)
        return this.random.Next(maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
      lock (this.random)
        return this.random.Next(minValue, maxValue);
    }

    public static int NextStatic()
    {
      lock (ThreadSafeRandom.randomStatic)
        return ThreadSafeRandom.randomStatic.Next();
    }

    public static int NextStatic(int maxValue)
    {
      lock (ThreadSafeRandom.randomStatic)
        return ThreadSafeRandom.randomStatic.Next(maxValue);
    }

    public static void NextStatic(byte[] keys)
    {
      lock (ThreadSafeRandom.randomStatic)
        ThreadSafeRandom.randomStatic.NextBytes(keys);
    }

    public static int NextStatic(int minValue, int maxValue)
    {
      lock (ThreadSafeRandom.randomStatic)
        return ThreadSafeRandom.randomStatic.Next(minValue, maxValue);
    }

    public void Shuffer<T>(T[] array)
    {
      for (int length = array.Length; length > 1; --length)
      {
        int index = this.random.Next(length);
        T obj = array[index];
        array[index] = array[length - 1];
        array[length - 1] = obj;
      }
    }

    public void ShufferList<T>(List<T> array)
    {
      for (int count = array.Count; count > 1; --count)
      {
        int index = this.random.Next(count);
        T obj = array[index];
        array[index] = array[count - 1];
        array[count - 1] = obj;
      }
    }

    public static void ShufferStatic<T>(T[] array)
    {
      for (int length = array.Length; length > 1; --length)
      {
        int index = ThreadSafeRandom.randomStatic.Next(length);
        T obj = array[index];
        array[index] = array[length - 1];
        array[length - 1] = obj;
      }
    }
  }
}
