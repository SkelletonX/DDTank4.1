// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.Data.ConsortiaTaskUserTempInfo
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using ProtoBuf;

namespace Game.Server.ConsortiaTask.Data
{
  [ProtoContract]
  public class ConsortiaTaskUserTempInfo
  {
    [ProtoMember(1)]
    public int UserID { get; set; }

    [ProtoMember(2)]
    public int Total { get; set; }

    [ProtoMember(3)]
    public int Exp { get; set; }

    [ProtoMember(4)]
    public int Offer { get; set; }

    [ProtoMember(5)]
    public int Riches { get; set; }
  }
}
