// Decompiled with JetBrains decompiler
// Type: Game.Server.SceneMarryRooms.MarryProcessorAttribute
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System;

namespace Game.Server.SceneMarryRooms
{
  public class MarryProcessorAttribute : Attribute
  {
    private byte _code;
    private string _descript;

    public MarryProcessorAttribute(byte code, string description)
    {
      this._code = code;
      this._descript = description;
    }

    public byte Code
    {
      get
      {
        return this._code;
      }
    }

    public string Description
    {
      get
      {
        return this._descript;
      }
    }
  }
}
