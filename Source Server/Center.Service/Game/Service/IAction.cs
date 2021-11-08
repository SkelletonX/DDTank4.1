// Decompiled with JetBrains decompiler
// Type: Game.Service.IAction
// Assembly: Center.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0BC5694D-A9FA-4488-B8B6-DD6BBEB5CBAF
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Service.exe

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
