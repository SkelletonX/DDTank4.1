// Decompiled with JetBrains decompiler
// Type: Game.Logic.LuaMgr
// Assembly: Game.Logic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E8B04D54-7E5B-47C4-9280-AF82495F6281
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Logic.dll

using LuaInterface;
using System;
using System.Collections.Generic;

namespace Game.Logic
{
  public class LuaMgr
  {
    private static Queue<Lua> m_queue = new Queue<Lua>();

    public static void Setup(int init)
    {
      for (int index = 0; index < init; ++index)
        LuaMgr.m_queue.Enqueue(new Lua());
    }

    public static Lua AllocateLua()
    {
      Lua lua = (Lua) null;
      lock (LuaMgr.m_queue)
      {
        if (LuaMgr.m_queue.Count > 0)
          lua = LuaMgr.m_queue.Dequeue();
      }
      if (lua == null)
        lua = new Lua();
      return lua;
    }

    public static void ReleaseLua(Lua lua)
    {
      lock (LuaMgr.m_queue)
      {
        LuaMgr.m_queue.Enqueue(lua);
        Console.WriteLine("lua queue count:{0}", (object) LuaMgr.m_queue.Count);
      }
    }
  }
}
