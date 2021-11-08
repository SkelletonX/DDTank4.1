// Decompiled with JetBrains decompiler
// Type: Center.Server.ServerData
// Assembly: Center.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4391F35A-9CAD-44A9-AFE0-208E0547A0DD
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Center\Center.Server.dll

using System.Runtime.Serialization;

namespace Center.Server
{
  [DataContract]
  public class ServerData
  {
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Ip { get; set; }

    [DataMember]
    public int LowestLevel { get; set; }

    [DataMember]
    public int MustLevel { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int Online { get; set; }

    [DataMember]
    public int Port { get; set; }

    [DataMember]
    public int State { get; set; }
  }
}
