// Decompiled with JetBrains decompiler
// Type: Fighting.Service.IAction
// Assembly: Fighting.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A10CE59F-8EFA-4220-9FA2-1100E5246235
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Fight\Fighting.Service.exe

using System.Collections;

namespace Fighting.Service
{
  internal interface IAction
  {
    string Name { get; }

    string Syntax { get; }

    string Description { get; }

    void OnAction(Hashtable parameters);
  }
}
