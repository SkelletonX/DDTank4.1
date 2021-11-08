// Decompiled with JetBrains decompiler
// Type: Game.Base.ConsoleClient
// Assembly: Game.Base, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2C15C00-C3DB-415D-8006-692895AE7555
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Base.dll

using System;

namespace Game.Base
{
  public class ConsoleClient : BaseClient
  {
    public ConsoleClient()
      : base((byte[]) null, (byte[]) null)
    {
    }

    public override void DisplayMessage(string msg)
    {
      Console.WriteLine(msg);
    }
  }
}
