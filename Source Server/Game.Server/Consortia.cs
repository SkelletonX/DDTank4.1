// Decompiled with JetBrains decompiler
// Type: Consortia
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

internal class Consortia : Attribute
{
  private byte byte_0;

  public Consortia(byte byte_1)
  {
    this.method_1(byte_1);
  }

  private void method_1(byte byte_1)
  {
    this.byte_0 = byte_1;
  }

  public byte method_0()
  {
    return this.byte_0;
  }
}
