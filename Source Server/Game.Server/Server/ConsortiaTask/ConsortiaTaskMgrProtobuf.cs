// Decompiled with JetBrains decompiler
// Type: Game.Server.ConsortiaTask.ConsortiaTaskMgrProtobuf
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using Game.Server.ConsortiaTask.Data;
using ProtoBuf;
using System.Collections.Generic;

namespace Game.Server.ConsortiaTask
{
  [ProtoContract]
  public class ConsortiaTaskMgrProtobuf
  {
    [ProtoMember(1)]
    public List<ConsortiaTaskUserTempInfo> tempUsers { get; set; }
  }
}
