// Decompiled with JetBrains decompiler
// Type: Game.Server.Pet.PetProcessorAtribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.Pet
{
  public class PetProcessorAtribute : Attribute
  {
    private byte byte_0;
    private string string_0;

    public PetProcessorAtribute(byte code, string description)
    {
      this.byte_0 = code;
      this.string_0 = description;
    }

    public byte Code
    {
      get
      {
        return this.byte_0;
      }
    }

    public string Description
    {
      get
      {
        return this.string_0;
      }
    }
  }
}
