// Decompiled with JetBrains decompiler
// Type: Game.Server.WebLogin.ChenckValidateRequestBody
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7994645F-6854-4AAC-A332-C61842D2DD9F
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\Game.Server.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Game.Server.WebLogin
{
  [DataContract(Namespace = "dandantang")]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class ChenckValidateRequestBody
  {
    [DataMember(EmitDefaultValue = false, Order = 0)]
    public string applicationname;
    [DataMember(EmitDefaultValue = false, Order = 1)]
    public string username;
    [DataMember(EmitDefaultValue = false, Order = 2)]
    public string password;

    public ChenckValidateRequestBody()
    {
    }

    public ChenckValidateRequestBody(string applicationname, string username, string password)
    {
      this.applicationname = applicationname;
      this.username = username;
      this.password = password;
    }
  }
}
