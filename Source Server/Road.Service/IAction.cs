// Decompiled with JetBrains decompiler
// Type: Game.Service.IAction
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 39736CC6-5447-4D8B-81FB-29999A4887F8
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Road.Service.exe

using System.Collections;

namespace Game.Service
{
  internal interface IAction
  {
    string Name { get; }

    string Syntax { get; }

    string Description { get; }

    void OnAction(Hashtable parameters);
  }
}
